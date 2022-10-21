using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using TaskBoardApp.Data;
using TaskBoardApp.Models.Task;
using Task = TaskBoardApp.Data.Entities.Task;

namespace TaskBoardApp.Controllers
{
    public class TaskController : Controller
    {
        private readonly TaskBoardAppDbContext data;

        public TaskController(TaskBoardAppDbContext context)
            => this.data = context;

        public IActionResult Create()
        {
            TaskFormModel taskModel = new TaskFormModel()
            {
                Boards = GetBoards()
            };

            return View(taskModel);
        }

        [HttpPost]
        public IActionResult Create(TaskFormModel taskModel)
        {
            if (!GetBoards().Any(b => b.Id == taskModel.BoardId))
            {
                this.ModelState.AddModelError(nameof(taskModel.BoardId), "Board does not exist");
            }

            string currentUserId = GetUserId();
            Task task = new Task()
            {
                Title = taskModel.Title,
                Description = taskModel.Description,
                CreatedOn = DateTime.Now,
                BoardId = taskModel.BoardId,
                OwnerId = currentUserId
            };
            this.data.Tasks.Add(task);
            this.data.SaveChanges();

            var boards = this.data.Boards;

            return RedirectToAction("All", "Boards");
        }

        private IEnumerable<TaskBoardModel> GetBoards()
            => this.data.Boards.Select(x => new TaskBoardModel()
            {
                Id = x.Id,
                Name = x.Name
            });

        private string GetUserId()
            => this.User.FindFirstValue(ClaimTypes.NameIdentifier);

        public IActionResult Details(int id)
        {
            var task = this.data
                .Tasks
                .Where(t => t.Id == id)
                .Select(t => new TaskDetailsViewModel()
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    CreatedOn = t.CreatedOn.ToString("dd/MM/yyyy HH:mm"),
                    Board = t.Board.Name,
                    Owner = t.Owner.UserName
                })
                .FirstOrDefault();

            if (task == null)
            {
                return BadRequest();
            }

            return View(task);
        }

        public IActionResult Edit(int id)
        {
            Task task = data.Tasks.Find(id);
            if (task == null)
            {
                return BadRequest();
            }

            string currentUserId = GetUserId();
            if (currentUserId != task.OwnerId)
            {
                return Unauthorized();
            }

            TaskFormModel taskModel = new TaskFormModel()
            {
                Title = task.Title,
                Description = task.Description,
                BoardId = task.BoardId,
                Boards = GetBoards()
            };

            return View(taskModel);
        }

        [HttpPost]
        public IActionResult Edit(int id, TaskFormModel taskModel)
        {
            Task task = data.Tasks.Find(id);
            if (task == null)
            {
                return BadRequest();
            }

            string currentUserId = GetUserId();
            if (currentUserId != task.OwnerId)
            {
                return Unauthorized();
            }

            if (!GetBoards().Any(b => b.Id == taskModel.BoardId))
            {
                this.ModelState.AddModelError(nameof(taskModel.BoardId), "Board does not exist.");
            }

            task.Title = taskModel.Title;
            task.Description = taskModel.Description;
            task.BoardId = taskModel.BoardId;

            this.data.SaveChanges();
            return RedirectToAction("All", "Boards");
        }


        public IActionResult Delete(int id)
        {
            Task task = data.Tasks.Find(id);
            if (task == null)
            {
                return BadRequest();
            }

            string currentUserId = GetUserId();
            if (currentUserId != task.OwnerId)
            {
                return Unauthorized();
            }

            TaskViewModel taskModel = new TaskViewModel()
            {
                Id = task.Id, 
                Title = task.Title,
                Description = task.Description,
            };

            return View(taskModel);
        }

        [HttpPost]
        public IActionResult Delete(TaskViewModel taskModel)
        {
            Task task = data.Tasks.Find(taskModel.Id);
            if (task == null)
            {
                return BadRequest();
            }

            string currentUserId = GetUserId();
            if (currentUserId != task.OwnerId)
            {
                return Unauthorized();
            }

            this.data.Tasks.Remove(task);
            this.data.SaveChanges();

            return RedirectToAction("All", "Boards");
        }
    }
}
