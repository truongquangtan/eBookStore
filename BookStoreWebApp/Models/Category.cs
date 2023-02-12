using System;
using System.Collections.Generic;

#nullable disable

namespace BookStoreWebApp.Models
{
    public partial class Category
    {
        public Category()
        {
            Products = new HashSet<Product>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? CreatedAt { get; set; }
        public bool? IsDeleted { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
