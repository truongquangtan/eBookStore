using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace BookStoreWebApp.Models
{
    public partial class Category
    {
        public Category()
        {
            Products = new HashSet<Product>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
        public bool? IsDeleted { get; set; } = false;

        [JsonIgnore]
        public virtual ICollection<Product> Products { get; set; }
    }
}
