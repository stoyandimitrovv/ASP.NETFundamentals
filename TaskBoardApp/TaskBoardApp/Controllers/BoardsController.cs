using Microsoft.AspNetCore.Mvc;
using TaskBoardApp.Data;

namespace TaskBoardApp.Controllers
{
    public class BoardsController : Controller
    {
        private readonly TaskBoardAppDbContext data;

        public BoardsController(TaskBoardAppDbContext context) => this.data = context;

        public IActionResult All()
        {


            return View();
        }
    }
}
