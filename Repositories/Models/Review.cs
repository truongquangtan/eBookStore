using System;
using System.Collections.Generic;
using Newtonsoft.Json;

#nullable disable

namespace BookStoreWebApp.Models
{
    public partial class Review
    {
        public Review()
        {
            ReviewImages = new HashSet<ReviewImage>();
        }

        public int Id { get; set; }
        public int? ProductId { get; set; }
        public string UserId { get; set; }
        public string Comment { get; set; }
        public int? Star { get; set; }
        public DateTime? CreateAt { get; set; }
        public bool? IsDeleted { get; set; }
        [JsonIgnore]
        public virtual Product Product { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<ReviewImage> ReviewImages { get; set; }
    }
}
