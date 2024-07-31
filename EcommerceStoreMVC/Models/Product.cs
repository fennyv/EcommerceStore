using Microsoft.EntityFrameworkCore;
using Riok.Mapperly.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace EcommerceStoreMVC.Models
{
    public class Product
    {
      
        public int ProductId { get; set; }
        [MaxLength(100)]
        public string ProductName { get; set; } = string.Empty;
        public string ProductDescription { get; set; } = string.Empty;
        [Precision(16, 2)]
        public decimal ProductPrice { get; set; }
        [MaxLength(100)]
        public string ProductImage { get; set; } = string.Empty;
        [MaxLength(100)]
        public string Brand { get; set; }   = string.Empty;
        [MaxLength(100)]
        public string Category { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }

    }
}
