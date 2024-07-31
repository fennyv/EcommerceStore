using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EcommerceStoreMVC.Models.DTO
{
    public class LoginDto
    {
        [Required, EmailAddress, MaxLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string Password { get; set; } = string.Empty;
        
        public  bool RememberMe { get; set; }




    }
}