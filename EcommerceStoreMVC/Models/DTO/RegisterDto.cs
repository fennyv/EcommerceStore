using System.ComponentModel.DataAnnotations;

namespace EcommerceStoreMVC.Models.DTO
{
    public class RegisterDto
    {
        [Required, MaxLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string LastName { get; set; } = string.Empty;

        [Required, EmailAddress, MaxLength(100)]
        public string Email { get; set; } = string.Empty;

        [Phone, MaxLength(20)]
        public string? PhoneNumber { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string Address { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string Password { get; set; } = string.Empty;

        [Required, Compare("Password"), MaxLength(100)]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
