using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

#nullable disable

namespace BookStoreWebApp.Models
{
    public class ProductRequest
    {
        public int Id { get; set; }
        public int? CategoryId { get; set; }
        [Required]
        public string Name { get; set; }
        public string Author { get; set; }
        public int? Quantity { get; set; }
        public int? Page { get; set; }
        public bool? IsDeleted { get; set; } = false;
        public bool? OnStock { get; set; } = true;
        public string Description { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
        public decimal? Price { get; set; }
        [NotMapped]
        [DisplayName("Upload File")]
        public IFormFile ImageFile { get; set; }
        public virtual ICollection<ProductImage> ProductImages { get; set; }
        public virtual Category Category { get; set; }

        public static ProductRequest FromProduct(Product product)
        {
            return new ProductRequest()
            {
                Id = product.Id,
                CategoryId = product.CategoryId,
                Name = product.Name,
                Author = product.Author,
                Quantity = product.Quantity,
                Page = product.Page,
                Description = product.Description,
                CreatedAt = product.CreatedAt,
                Price = product.Price,
                ImageFile = product.ImageFile,
                ProductImages = product.ProductImages,
                Category = product.Category,
            };
        }
    }
}