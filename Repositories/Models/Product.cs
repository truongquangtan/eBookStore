using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

#nullable disable

namespace BookStoreWebApp.Models
{
    public partial class Product
    {
        public Product()
        {
            ProductImages = new HashSet<ProductImage>();
            OrderDetails = new HashSet<OrderDetail>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public int? CategoryId { get; set; }
        [Required]
        [DisplayName("Name*")]
        public string Name { get; set; }
        public string Author { get; set; }
        [Required]
        [DisplayName("Quantity*")]
        public int? Quantity { get; set; }
        public int? Page { get; set; }
        public bool? IsDeleted { get; set; } = false;
        public bool? OnStock { get; set; } = true;
        public string Description { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
        [Required]
        [DisplayName("Price*")]
        public decimal? Price { get; set; }
        [NotMapped]
        [DisplayName("Upload File*")]
        [Required]
        public IFormFile ImageFile { get; set; }
        public virtual Category Category { get; set; }

        [JsonIgnore]
        public virtual ICollection<Cart> Carts { get; set; }
        public virtual ICollection<ProductImage> ProductImages { get; set; }
        [JsonIgnore]
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}