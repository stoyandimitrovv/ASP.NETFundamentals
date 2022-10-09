using ForumDemoApp.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace ForumDemoApp.Data
{
    public class ForumAppDbContext : DbContext
    {
        //Initialise db context
        public ForumAppDbContext
            (DbContextOptions<ForumAppDbContext> options)
            : base(options)
        {
            this.Database.Migrate();
        }

        //Create dbSet for Post entity
        public DbSet<Post> Posts { get; set; }

        private Post FirstPost { get; set; }
        private Post SecondPost { get; set; }
        private Post ThirdPost { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //Create a method to insert some data in the table
            SeedPosts();
            builder
                .Entity<Post>()
                .HasData(this.FirstPost,
                    this.SecondPost,
                    this.ThirdPost);

            //At the end call base.OnModelCreating
            base.OnModelCreating(builder);
        }

        private void SeedPosts()
        {
            this.FirstPost = new Post()
            {
                Id = 1,
                Title = "My first post",
                Content = "My first post will be about performing CRUD operations in MVC. It's so much fun!"
            };

            this.SecondPost = new Post()
            {
                Id = 2,
                Title = "My second post",
                Content = "This is my second post. It's getting more and more interesting!"
            };

            this.ThirdPost = new Post()
            {
                Id = 3,
                Title = "My third post",
                Content = "Hello there! I'm getting better and better with the CRUD operations!"
            };
        }
    }
}
