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
using Repositories.Repositories.ProductRepository;
using Repositories.Repositories.CategoryRepository;
using Repositories.Repositories.ProductImgRepository;
using Repositories.Models.DTO;
using Repositories.Repositories.OrderDetailRepository;

namespace BookStoreWebApp.Controllers
{
    [Authorize(Roles = RoleName.ADMIN)]
    public class ProductController : Controller
    {
        private readonly IProductRepository productRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly IProductImgRepository productImgRepository;
        private readonly IOrderDetailRepository orderDetailRepository;
        private readonly IWebHostEnvironment webHostEnvironment;

        public ProductController(IWebHostEnvironment webHostEnvironment, IProductRepository productRepository, ICategoryRepository categoryRepository, IProductImgRepository productImgRepository, IOrderDetailRepository orderDetailRepository)
        {
            this.orderDetailRepository = orderDetailRepository;
            this.webHostEnvironment = webHostEnvironment;
            this.productRepository = productRepository;
            this.categoryRepository = categoryRepository;
            this.productImgRepository = productImgRepository;
        }
        private void SetMessage(MessageType type, string text)
        {
            var message = new MessageDTO()
            {
                MessageType = type,
                Message = text,
            };
            TempData["Message"] = message.Message;
            TempData["IsSuccess"] = type == MessageType.Success ? "true" : "false";
        }

        //--------------------------------------- PRODUCT ----------------------------------------
        public IActionResult Index()
        {
            var products = productRepository.GetAll();
            var productDTOs = new List<ProductForAdminDTO>();
            foreach(var product in products)
            {
                int quantitySold = orderDetailRepository.CountProductQuantity(product.Id);
                productDTOs.Add(new ProductForAdminDTO(product, quantitySold));
            }
            return View(productDTOs);
        }

        //GET create page
        [Route("product/create")]
        public IActionResult Create()
        {
            var categories = categoryRepository.GetAll();

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
                productRepository.Add(productEntity);

                //Create image entity
                ProductImage productImage = new()
                {
                    ProductId = productEntity.Id,
                    Link = persistentName
                };

                //Save image to db
                productImgRepository.Add(productImage);

                //Pass message to index page
                SetMessage(MessageType.Success, "Create product successfully");
                return RedirectToAction("Index");
            }
            var categories = categoryRepository.GetAll();

            TempData["Categories"] = JsonConvert.SerializeObject(categories);
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

            var product = productRepository.GetById(id.Value);

            if(product == null)
            { 
                return NotFound();
            }

            // Get all categories
            var categories = categoryRepository.GetAll();

            //Pass to view
            TempData["Categories"] = JsonConvert.SerializeObject(categories);

            return View(ProductRequest.FromProduct(product));
        }

        // POST new product
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProduct ([Bind("Id,Name,Author,CategoryId,Quantity,Page,Price,ImageFile,Description")] ProductRequest product)
        {
            if (ModelState.IsValid)
            {
                var inDbProduct = productRepository.GetById(product.Id);

                inDbProduct.Name = product.Name;
                inDbProduct.CategoryId = product.CategoryId;
                inDbProduct.Price = product.Price;
                inDbProduct.Quantity = product.Quantity;
                inDbProduct.Page = product.Page;
                inDbProduct.Author = product.Author;
                inDbProduct.Description = product.Description;

                if(product.ImageFile != null)
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

                    //Delete old image
                    var productImg = productImgRepository.GetFirstByProductId(product.Id);
                    productImgRepository.Delete(productImg);
                    //Create image entity
                    ProductImage productImage = new()
                    {
                        ProductId = inDbProduct.Id,
                        Link = persistentName
                    };

                    //Save image to db
                    productImgRepository.Add(productImage);
                }

                productRepository.Update(inDbProduct);

                SetMessage(MessageType.Success, $"Edit product with product id [${product.Id}] successfully");
                return RedirectToAction("Index");
            }
            // Get all categories
            var categories = categoryRepository.GetAll();

            //Pass to view
            TempData["Categories"] = JsonConvert.SerializeObject(categories);

            var includedImgProduct = productRepository.GetById(product.Id);

            return View(includedImgProduct);
        }

        [Route("Product/DeleteProduct/{id}")]
        [HttpGet]
        public IActionResult DeleteProductConfirmed(int id)
        {
            try
            {
                var product = productRepository.GetById(id);
                product.IsDeleted = true;

                productRepository.Update(product);

                SetMessage(MessageType.Success, $"Delete product with product id [${id}] successfully");
            } catch (Exception ex)
            {
                SetMessage(MessageType.Error, $"Cannot delete product with product id [${id}].");
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
