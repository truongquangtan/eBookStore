using BookStoreWebApp.Models;
using BookStoreWebApp.Supporters.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;

namespace BookStoreWebApp.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly eBookStore5Context context;

        public HomeController(ILogger<HomeController> logger, eBookStore5Context context)
        {
            _logger = logger;
            this.context = context;
        }

        public IActionResult Index()
        {
            var books = context.Products.Where(p => p.IsDeleted == false).Include(p => p.Category).Include(p => p.ProductImages).ToList();
            return View(books);
        }

        [Route("Product/{id}")]
        public IActionResult BookDetail(int id)
        {
            var book = context.Products.Where(p => p.Id == id && p.IsDeleted == false).Include(p => p.ProductImages).Include(p => p.Category).FirstOrDefault();
            return View(book);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
