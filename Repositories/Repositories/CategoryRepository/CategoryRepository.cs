using BookStoreWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repositories.CategoryRepository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly eBookStore5Context context;

        public CategoryRepository(eBookStore5Context context)
        {
            this.context = context;
        }

        public Category Add(Category category)
        {
            var entityEntry = context.Categories.Add(category);
            context.SaveChanges();
            return entityEntry.Entity;
        }

        public void Delete(Category category)
        {
            context.Categories.Remove(category);
            context.SaveChanges();
        }

        public IEnumerable<Category> GetAll()
        {
            List<Category> categories = context.Categories.Where(c => c.IsDeleted == false).ToList();
            return categories;
        }

        public Category GetById(int id)
        {
            var category = context.Categories.Where(cate => cate.Id == id).FirstOrDefault();
            return category;
        }

        public void Update(Category updateCategoryInfo)
        {
            context.Categories.Update(updateCategoryInfo);
            context.SaveChanges();
        }
    }
}
