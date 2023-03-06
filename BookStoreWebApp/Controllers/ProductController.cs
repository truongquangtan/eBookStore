using BookStoreWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Hosting;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Threading;
using System.Threading.Tasks;
using BookStoreWebApp.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using BookStoreWebApp.Supporters.Constants;

namespace BookStoreWebApp.Controllers
{
    [Authorize(Roles = RoleName.ADMIN)]
    public class ProductController : Controller
    {
        private readonly eBookStore5Context context;
        private readonly IWebHostEnvironment webHostEnvironment;

        public ProductController(eBookStore5Context context, IWebHostEnvironment webHostEnvironment)
        {
            this.context = context;
            this.webHostEnvironment = webHostEnvironment;
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

        //--------------------------------------- PRODUCT ----------------------------------------
        public IActionResult Index()
        {
            List<Product> products = context.Products
                .Where(p => p.IsDeleted == false && p.OnStock == true)
                .Include(p => p.Category)
                .Include(p => p.ProductImages)
                .ToList();
            return View(products);
        }

        //GET create page
        [Route("product/create")]
        public IActionResult Create()
        {
            List<Category> categories = context.Categories.Where(cate => cate.IsDeleted == false).ToList();

            TempData["Categories"] = JsonConvert.SerializeObject(categories);

            return View();
        }

        //POST product to create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("product/create")]
        public async Task<IActionResult> Create([Bind("Name,Author,CategoryId,Quantity,Page,Price,ImageFile,Description")] Product product)
        {
            if (ModelState.IsValid)
            {
                // Save file img
                string wwwRootPath = webHostEnvironment.WebRootPath;
                string extension = Path.GetExtension(product.ImageFile.FileName);
                string persistentName = Guid.NewGuid().ToString() + extension;
                string path = Path.Combine(wwwRootPath + "/images/", persistentName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await product.ImageFile.CopyToAsync(fileStream);
                }

                //Create product entity
                Product productEntity = new()
                {
                    Name = product.Name,
                    CategoryId = product.CategoryId,
                    Price = product.Price,
                    Quantity = product.Quantity,
                    Page = product.Page,
                    Author = product.Author,
                    Description = product.Description,
                };

                //Save product to db
                context.Add(productEntity);
                context.SaveChanges();

                //Create image entity
                ProductImage productImage = new()
                {
                    ProductId = productEntity.Id,
                    Link = persistentName
                };

                //Save image to db
                context.ProductImages.Add(productImage);
                context.SaveChanges();

                //Pass message to index page
                SetMessage(MessageType.Success, "Create product successfully");
                return RedirectToAction("Index");
            }
            return View(product);
        }

        // GET edit product page
        [HttpGet]
        public IActionResult EditProduct(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = context.Products
                .Include(p => p.Category)
                .Include(p => p.ProductImages)
                .FirstOrDefault(p => p.Id == id);

            if(product == null)
            { 
                return NotFound();
            }

            // Get all categories
            List<Category> categories = context.Categories.Where(cate => cate.IsDeleted == false).ToList();

            //Pass to view
            TempData["Categories"] = JsonConvert.SerializeObject(categories);

            return View(product);
        }

        // POST new product
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProduct ([Bind("Id,Name,Author,CategoryId,Quantity,Page,Price,ImageFile,Description")] Product product)
        {
            if (ModelState.IsValid)
            {
                var inDbProduct = context.Products.Where(p => p.Id == product.Id).FirstOrDefault();

                inDbProduct.Name = product.Name;
                inDbProduct.CategoryId = product.CategoryId;
                inDbProduct.Price = product.Price;
                inDbProduct.Quantity = product.Quantity;
                inDbProduct.Page = product.Page;
                inDbProduct.Author = product.Author;
                inDbProduct.Description = product.Description;

                // Save file img
                string wwwRootPath = webHostEnvironment.WebRootPath;
                string extension = Path.GetExtension(product.ImageFile.FileName);
                string persistentName = Guid.NewGuid().ToString() + extension;
                string path = Path.Combine(wwwRootPath + "/images/", persistentName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await product.ImageFile.CopyToAsync(fileStream);
                }

                //Delete old image
                var productImg = context.ProductImages.Where(p => p.ProductId == inDbProduct.Id).FirstOrDefault();
                context.ProductImages.Remove(productImg);
                context.SaveChanges();
                //Create image entity
                ProductImage productImage = new()
                {
                    ProductId = inDbProduct.Id,
                    Link = persistentName
                };

                //Save image to db
                context.ProductImages.Add(productImage);
                context.SaveChanges();

                context.Products.Update(inDbProduct);
                context.SaveChanges();

                SetMessage(MessageType.Success, $"Edit product with product id [${product.Id}] successfully");
                return RedirectToAction("Index");
            }
            // Get all categories
            List<Category> categories = context.Categories.Where(cate => cate.IsDeleted == false).ToList();

            //Pass to view
            TempData["Categories"] = JsonConvert.SerializeObject(categories);
            return View(product);
        }

        [Route("Product/DeleteProduct/{id}")]
        [HttpGet]
        public async Task<IActionResult> DeleteProductConfirmed(int id)
        {
            try
            {
                var product = await context.Products.FindAsync(id);
                product.IsDeleted = true;
                await context.SaveChangesAsync();

                SetMessage(MessageType.Success, $"Delete product with product id [${id}] successfully");
            } catch (Exception ex)
            {
                SetMessage(MessageType.Error, $"Cannot delete product with product id [${id}].");
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
