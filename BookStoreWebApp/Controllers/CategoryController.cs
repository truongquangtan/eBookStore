using BookStoreWebApp.Models;
using BookStoreWebApp.Models.DTO;
using BookStoreWebApp.Supporters.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Repositories.Repositories.CategoryRepository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BookStoreWebApp.Controllers
{
    [Authorize(Roles = RoleName.ADMIN)]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        public IActionResult Index()
        {
            IEnumerable<Category> categories = categoryRepository.GetAll();
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
                categoryRepository.Add(categoryEntity);
                return RedirectToAction("Index");
            }
            return View(category);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var category = categoryRepository.GetById(id);
            try
            {
                categoryRepository.Delete(category);
            } catch (Exception)
            {
                TempData["Message"] = "Cannot delete this category";
                TempData["IsSuccess"] = "false";
            }

            return RedirectToAction("Index");
        }
    }
}
