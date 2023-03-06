using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace BookStoreWebApp.Models
{
    public partial class ProductImage
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int? ProductId { get; set; }
        public string Link { get; set; }

        [JsonIgnore]
        public virtual Product Product { get; set; }
    }
}
