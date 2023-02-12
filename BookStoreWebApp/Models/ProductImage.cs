using System;
using System.Collections.Generic;

#nullable disable

namespace BookStoreWebApp.Models
{
    public partial class ProductImage
    {
        public int Id { get; set; }
        public int? ProductId { get; set; }
        public string Link { get; set; }

        public virtual Product Product { get; set; }
    }
}
