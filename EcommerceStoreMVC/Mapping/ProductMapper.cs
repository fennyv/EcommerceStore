using EcommerceStoreMVC.Models;
using EcommerceStoreMVC.Models.DTO;
using Riok.Mapperly.Abstractions;
using System;

namespace EcommerceStoreMVC.Mapping
{
    [Mapper]
    public partial class ProductMapper
    {

       
        public partial Product ProductDtoToProduct(ProductDto productDto);

        public partial ProductDto ProductToProductDto(Product product);

        

    }
}
