using System;
using System.Collections.Generic;

#nullable disable

namespace BookStoreWebApp.Models
{
    public partial class Order
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int? ProductId { get; set; }
        public int? Quantity { get; set; }
        public decimal? Price { get; set; }
        public string DeliveryAddress { get; set; }
        public string Method { get; set; }
        public string Status { get; set; }
        public DateTime? OrderAt { get; set; }
        public DateTime? UpdateAt { get; set; }
        public decimal? Total { get; set; }

        public virtual Product Product { get; set; }
        public virtual User User { get; set; }
    }
}
