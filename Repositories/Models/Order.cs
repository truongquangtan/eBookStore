using System;
using System.Collections.Generic;

#nullable disable

namespace BookStoreWebApp.Models
{
    public partial class OrderSum
    {
        public OrderSum()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int Id { get; set; }
        public string UserId { get; set; }
        public string DeliveryAddress { get; set; }
        public string Method { get; set; }
        public string Status { get; set; } = "CREATED";
        public DateTime? OrderAt { get; set; } = DateTime.Now;
        public DateTime? UpdateAt { get; set; } = DateTime.Now;
        public decimal? Total { get; set; }
        public string Phone { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
