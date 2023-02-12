using System.ComponentModel.DataAnnotations;

namespace BookStoreWebApp.Models.DTO
{
    public class RegisterDTO
    {
        [Required]
        public string DisplayName { get; set; }
        public string? Phone { get; set; }
        [Required]
        public string Mail { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string ConfirmPassword { get; set; }
        public string Address { get; set; }
        public string Avatar { get; set; }
    }
}
