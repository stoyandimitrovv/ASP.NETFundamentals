﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TaskBoardApp.Data.Entities;
using Task = TaskBoardApp.Data.Entities.Task;

namespace TaskBoardApp.Data
{
    public class TaskBoardAppDbContext : IdentityDbContext<User>
    {
        public TaskBoardAppDbContext(DbContextOptions<TaskBoardAppDbContext> options)
            : base(options)
        {
            this.Database.Migrate();
        }

        public DbSet<Board> Boards { get; set; }
        public DbSet<Task> Tasks { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder
                .Entity<Task>()
                .HasOne(t => t.Board)
                .WithMany(b => b.Tasks)
                .HasForeignKey(t => t.BoardId)
                .OnDelete(DeleteBehavior.Restrict);

            SeedUsers();
            builder
                .Entity<User>()
                .HasData(this.GuestUser);

            SeedBoards();
            builder
                .Entity<Board>()
                .HasData(this.OpenBoard, this.InProgressBoard, this.DoneBoard);

            builder
                .Entity<Task>()
                .HasData(new Task()
                {
                    Id = 1,
                    Title = "Prepare for ASP.Net Fundamentals Exam",
                    Description = "Learn using EF and MS Studio",
                    CreatedOn = DateTime.Now.AddMonths(-1),
                    OwnerId = this.GuestUser.Id,
                    BoardId = this.OpenBoard.Id
                },
                    new Task()
                    {
                        Id = 2,
                        Title = "Improve EF Core skills",
                        Description = "Learn using ASP.Net Core Identity",
                        CreatedOn = DateTime.Now.AddMonths(-5),
                        OwnerId = this.GuestUser.Id,
                        BoardId = this.DoneBoard.Id
                    },
                    new Task()
                    {
                        Id = 3,
                        Title = "Improve ASP.Net Core skills",
                        Description = "Learn using ASP.Net Core Identity",
                        CreatedOn = DateTime.Now.AddMonths(-10),
                        OwnerId = this.GuestUser.Id,
                        BoardId = this.InProgressBoard.Id
                    }, new Task()
                    {
                        Id = 4,
                        Title = "Prepare for C# Fundamentals Exam",
                        Description = "Prepare for solving old Mid and Final exams",
                        CreatedOn = DateTime.Now.AddMonths(-1),
                        OwnerId = this.GuestUser.Id,
                        BoardId = this.DoneBoard.Id
                    }
                    );

            base.OnModelCreating(builder);
        }

        private void SeedUsers()
        {
            var hasher = new PasswordHasher<IdentityUser>();

            this.GuestUser = new User()
            {
                UserName = "guest",
                NormalizedUserName = "GUEST",
                Email = "guest@mail.com",
                NormalizedEmail = "GUEST@MAIL.COM",
                FirstName = "Guest",
                LastName = "User"
            };

            this.GuestUser.PasswordHash = hasher.HashPassword(this.GuestUser, "guest");
        }

        private void SeedBoards()
        {
            this.OpenBoard = new Board()
            {
                Id = 1,
                Name = "Open"
            };
            this.InProgressBoard = new Board()
            {
                Id = 2,
                Name = "In Progress"
            };
            this.DoneBoard = new Board()
            {
                Id = 3,
                Name = "Done"
            };
        }

        private User GuestUser { get; set; }
        private Board OpenBoard { get; set; }
        private Board InProgressBoard { get; set; }
        private Board DoneBoard { get; set; }

    }
}