using BookStoreWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repositories.ProductImgRepository
{
    public class ProductImgRepository : IProductImgRepository
    {
        private readonly eBookStore5Context context;

        public ProductImgRepository(eBookStore5Context context)
        {
            this.context = context;
        }

        public ProductImage Add(ProductImage productImage)
        {
            var entityEntry = context.ProductImages.Add(productImage);
            context.SaveChanges();
            return entityEntry.Entity;

        }

        public void Delete(ProductImage productImage)
        {
            context.ProductImages.Remove(productImage);
            context.SaveChanges();
        }

        public ProductImage GetById(int id)
        {
            var productImg = context.ProductImages.Where(p => p.ProductId == id).FirstOrDefault();
            return productImg;
        }

        public void Update(ProductImage updatedProductImgInfo)
        {
            context.ProductImages.Update(updatedProductImgInfo);
            context.SaveChanges();
        }

        public ProductImage GetFirstByProductId(int productId)
        {
            return context.ProductImages.Where(p => p.ProductId == productId).FirstOrDefault();
        }
    }
}
