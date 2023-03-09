using BookStoreWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Models.DTO
{
    public class ProductForAdminDTO : Product
    {
        public int QuantitySold { get; set; } = 0;
        public ProductForAdminDTO(Product product, int quantitySold)
        {
            this.Id = product.Id;
            this.Page = product.Page;
            this.Price = product.Price;
            this.Category = product.Category;
            this.Reviews = product.Reviews;
            this.Author = product.Author;
            this.CategoryId = product.CategoryId;
            this.CreatedAt = product.CreatedAt;
            this.Description = product.Description;
            this.Name = product.Name;
            this.Quantity = product.Quantity;
            this.ProductImages = product.ProductImages;
            this.QuantitySold = quantitySold;
        }
    }
}
