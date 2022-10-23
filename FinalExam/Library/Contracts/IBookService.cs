using Library.Data.Entities;
using Library.Models;

namespace Library.Contracts
{
    public interface IBookService
    {
        Task<IEnumerable<BookViewModel>> GetAllAsync();

        Task<IEnumerable<Category>> GetCategoryesAsync();

        Task AddBookAsync(AddBookViewModel model);

        Task AddBookToCollectionAsync(int bookId, string applicationUserId);

        Task<IEnumerable<BookViewModel>> GetMyBooksAsync(string applicationUserId);

        Task RemoveBookFromCollectionAsync(int bookId, string applicationUserId);
    }
}
