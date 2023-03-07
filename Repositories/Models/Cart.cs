using System;
using System.Collections.Generic;

#nullable disable

namespace BookStoreWebApp.Models
{
    public partial class Cart
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public int ProductId { get; set; }
        public string UserId { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
        public int? Quantity { get; set; }

        public virtual Product Product { get; set; }
        public virtual User User { get; set; }
    }
}

