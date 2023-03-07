using BookStoreWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repositories.CategoryRepository
{
    public interface ICategoryRepository
    {
        public IEnumerable<Category> GetAll();
        public Category? GetById(int id);
        public Category Add(Category category);

        public void Delete(Category category);

        public void Update(Category updateCategoryInfo);
    }
}
