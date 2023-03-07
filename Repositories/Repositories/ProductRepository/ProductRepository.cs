using BookStoreWebApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repositories.ProductRepository
{
    public class ProductRepository : IProductRepository
    {
        private readonly eBookStore5Context context;

        public ProductRepository(eBookStore5Context context)
        {
            this.context = context;
        }

        public Product Add(Product p)
        {
            var entityEntry = context.Products.Add(p);
            context.SaveChanges();
            return entityEntry.Entity;
        }

        public void Delete(Product product)
        {
            context.Products.Remove(product);
            context.SaveChanges();
        }

        public IEnumerable<Product> GetAll()
        {
            List<Product> products = context.Products
                .Where(p => p.IsDeleted == false)
                .Include(p => p.Category)
                .Include(p => p.ProductImages).ToList();
            return products;
        }

        public Product GetById(int id)
        {
            Product product = context.Products
                .Where(p => p.Id == id && p.IsDeleted == false)
                .Include(p => p.ProductImages)
                .Include(p => p.Category).FirstOrDefault();
            return product;
        }

        public void Update(Product updatedProductInfo)
        {
            context.Products.Update(updatedProductInfo);
            context.SaveChanges();
        }
    }
}
