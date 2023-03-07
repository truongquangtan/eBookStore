using System;
using System.Collections.Generic;

#nullable disable

namespace BookStoreWebApp.Models
{
    public partial class OrderDetail
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public int? OrderId { get; set; }
        public int? ProductId { get; set; }
        public decimal? Price { get; set; }
        public int? Quantity { get; set; }

        public virtual OrderSum Order { get; set; }
        public virtual Product Product { get; set; }
    }
}
