using Library.Contracts;
using Library.Data;
using Library.Data.Entities;
using Library.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.Services
{
    public class BookService : IBookService
    {
        private readonly LibraryDbContext context;

        public BookService(LibraryDbContext _context)
        {
            context = _context;
        }

        public async Task<IEnumerable<BookViewModel>> GetAllAsync()
        {
            var entities = await context.Books
                .Include(m => m.Category)
                .ToListAsync();

            return entities
                .Select(m => new BookViewModel()
                {
                    Id = m.Id,
                    Title = m.Title,
                    Author = m.Author,
                    Description = m.Description,
                    ImageUrl = m.ImageUrl,
                    Rating = m.Rating,
                    Category = m.Category.Name
                });
        }

        public async Task<IEnumerable<Category>> GetCategoryesAsync()
        {
            return await context.Categories.ToListAsync();
        }

        public async Task AddBookAsync(AddBookViewModel model)
        {
            var entity = new Book()
            {
                Title = model.Title,
                Author = model.Author,
                Description = model.Description,
                ImageUrl = model.ImageUrl,
                Rating = model.Rating,
                CategoryId = model.CategoryId
            };

            await context.Books.AddAsync(entity);
            await context.SaveChangesAsync();
        }

        public async Task AddBookToCollectionAsync(int bookId, string applicationUserId)
        {
            var user = await context.Users
                .Where(u => u.Id == applicationUserId)
                .Include(u => u.ApplicationUsersBooks)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                throw new ArgumentException("Invalid user ID");
            }

            var book = await context.Books.FirstOrDefaultAsync(u => u.Id == bookId);

            if (book == null)
            {
                throw new ArgumentException("Invalid Movie ID");
            }

            if (!user.ApplicationUsersBooks.Any(m => m.BookId == bookId))
            {
                user.ApplicationUsersBooks.Add(new ApplicationUserBook()
                {
                    BookId = book.Id,
                    ApplicationUserId = user.Id,
                    Book = book,
                    ApplicationUser = user
                });

                await context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<BookViewModel>> GetMyBooksAsync(string applicationUserId)
        {
            var user = await context.Users
                .Where(u => u.Id == applicationUserId)
                .Include(u => u.ApplicationUsersBooks)
                .ThenInclude(ub => ub.Book)
                .ThenInclude(b => b.Category)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                throw new ArgumentException("Invalid user ID");
            }

            return user.ApplicationUsersBooks
                .Select(m => new BookViewModel()
                {
                    Id = m.BookId,
                    Title = m.Book.Title,
                    Author = m.Book.Author,
                    Description = m.Book.Description,
                    ImageUrl = m.Book.ImageUrl,
                    Rating = m.Book.Rating,
                    Category = m.Book.Category.Name
                });
        }

        public async Task RemoveBookFromCollectionAsync(int bookId, string applicationUserId)
        {
            var user = await context.Users
                .Where(u => u.Id == applicationUserId)
                .Include(u => u.ApplicationUsersBooks)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                throw new ArgumentException("Invalid user ID");
            }

            var book = user.ApplicationUsersBooks.FirstOrDefault(b => b.BookId == bookId);

            if (book != null)
            {
                user.ApplicationUsersBooks.Remove(book);

                await context.SaveChangesAsync();
            }
        }
    }
}
