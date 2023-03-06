using BookStoreWebApp.Models;
using BookStoreWebApp.Models.DTO;
using BookStoreWebApp.Supporters.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace BookStoreWebApp.Controllers
{
    [Authorize(Roles = RoleName.ADMIN)]
    public class CategoryController : Controller
    {
        private readonly eBookStore5Context context;

        public CategoryController(eBookStore5Context context)
        {
            this.context = context;
        }

        private void SetMessage(MessageType type, string text)
        {
            var message = new MessageDTO()
            {
                MessageType = type,
                Message = text,
            };
            TempData["Message"] = JsonConvert.SerializeObject(message);
        }

        public IActionResult Index()
        {
            List<Category> categories = context.Categories.Where(c => c.IsDeleted == false).ToList();
            return View(categories);
        }

        [HttpGet]
        [Route("category/create")]
        public IActionResult CreateCategory()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddCategory([Bind("Name")] Category category)
        {
            if (ModelState.IsValid)
            {
                /// Call db to save
                Category categoryEntity = new()
                {
                    Name = category.Name
                };
                context.Categories.Add(categoryEntity);
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }
    }
}
