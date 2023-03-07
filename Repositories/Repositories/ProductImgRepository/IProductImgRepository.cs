using BookStoreWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repositories.ProductImgRepository
{
    public interface IProductImgRepository
    {
        public ProductImage? GetById(int id);
        public ProductImage Add(ProductImage productImage);
        public void Delete(ProductImage productImage);
        public void Update(ProductImage updatedProductImgInfo);
    }
}
