using ForumDemoApp.Data;
using ForumDemoApp.Data.Entities;
using ForumDemoApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace ForumDemoApp.Controllers
{
    public class PostsController : Controller
    {
        //inject the ForumAppDbContext through the constructor and assign it to a variable to use i
        private readonly ForumAppDbContext data;

        public PostsController(ForumAppDbContext data)
        {
            this.data = data;
        }

        //Extract the products from the database to a model collection, which will be passed to a view
        public IActionResult All()
        {
            var posts = this.data
                .Posts
                .Select(p => new PostViewModel()
                {
                    Id = p.Id,
                    Title = p.Title,
                    Content = p.Content
                })
                .ToList();//Remember to cast ToList

            return View(posts);
        }

        //First create the view for the adding method
        public IActionResult Add() => View();

        //Then implement the method- adding post to the database and redirect to All
        [HttpPost]
        public IActionResult Add(PostFormModel model)
        {
            var post = new Post()
            {
                Title = model.Title,
                Content = model.Content
            };

            this.data.Posts.Add(post);
            this.data.SaveChanges();

            return RedirectToAction("All");
        }

        //Create the view for the edit method
        public IActionResult Edit(int id)
        {
            var post = this.data.Posts.Find(id);

            return View(new PostFormModel()
            {
                Title = post.Title,
                Content = post.Content
            });
        }

        //Then implement the method- edit and redirect to All
        [HttpPost]
        public IActionResult Edit(int id, PostFormModel model)
        {
            var post = this.data.Posts.Find(id);
            post.Title = post.Title;
            post.Content = post.Content;

            this.data.SaveChanges();

            return RedirectToAction("All");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var post = this.data.Posts.Find(id);

            this.data.Posts.Remove(post);
            this.data.SaveChanges();

            return RedirectToAction("All");
        }
    }
}
