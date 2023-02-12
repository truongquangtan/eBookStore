using System;
using System.Collections.Generic;

#nullable disable

namespace BookStoreWebApp.Models
{
    public partial class Product
    {
        public Product()
        {
            Orders = new HashSet<Order>();
            ProductImages = new HashSet<ProductImage>();
            Reviews = new HashSet<Review>();
        }

        public int Id { get; set; }
        public int? CategoryId { get; set; }
        public string Name { get; set; }
        public int? Quantity { get; set; }
        public int? Page { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? OnStock { get; set; }
        public DateTime? CreatedAt { get; set; }
        public decimal? Price { get; set; }

        public virtual Category Category { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<ProductImage> ProductImages { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
    }
}
