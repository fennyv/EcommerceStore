using System.ComponentModel.DataAnnotations;

namespace EcommerceStoreMVC.Models.DTO
{
    public class PasswordDto
    {
        [Required(ErrorMessage = "The Current Password field is required"), MaxLength(100)]
        public string CurrentPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "The New Password field is required"), MaxLength(100)]
        public string NewPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "The Confirm Password field is required")] 
        [Compare("NewPassword"), MaxLength(100)]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
