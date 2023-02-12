using System;
using System.Collections.Generic;

#nullable disable

namespace BookStoreWebApp.Models
{
    public partial class ReviewImage
    {
        public int Id { get; set; }
        public int? ReviewId { get; set; }
        public DateTime? CreateAt { get; set; }
        public bool? IsDeleted { get; set; }

        public virtual Review Review { get; set; }
    }
}
