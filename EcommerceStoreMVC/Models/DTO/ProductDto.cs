
using Riok.Mapperly.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace EcommerceStoreMVC.Models.DTO
{
    public class ProductDto
    {
        [Required, MaxLength(100)]
        public string ProductName { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string ProductDescription { get; set; } = string.Empty;

        [Required]
        public decimal ProductPrice { get; set; }

        [MapperIgnore]
        public IFormFile? ProductImageFile { get; set; }

        public string ProductImage { get; set; } = string.Empty;

        [Required,  MaxLength(100)]
        public string Brand { get; set; } = string.Empty;
        [Required,  MaxLength(100)]
        public string Category { get; set; } =  string.Empty;
    }
}
