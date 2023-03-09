using System;
using System.Collections.Generic;

#nullable disable

namespace BookStoreWebApp.Models
{
    public partial class User
    {
        public User()
        {
            Carts = new HashSet<Cart>();
            Orders = new HashSet<OrderSum>();
            UserRoles = new HashSet<UserRole>();
        }

        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Phone { get; set; }
        public string Mail { get; set; }
        public string Password { get; set; }
        public string Address { get; set; }
        public string Avatar { get; set; }
        public string DisplayName { get; set; }
        public DateTime? CreatedAt { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? IsActive { get; set; }

        public virtual ICollection<Cart> Carts { get; set; }
        public virtual ICollection<OrderSum> Orders { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
