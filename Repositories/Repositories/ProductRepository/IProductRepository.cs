using BookStoreWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repositories.ProductRepository
{
    public interface IProductRepository
    {
        public IEnumerable<Product> GetAll();
        public Product? GetById(int id);
        public Product Add(Product product);

        public void Delete(Product product);

        public void Update(Product updatedProductInfo);
    }
    }
