using EcommerceStoreMVC.Models;
using EcommerceStoreMVC.Models.DTO;
using Riok.Mapperly.Abstractions;


namespace EcommerceStoreMVC.Mapping
{

    [Mapper]
    public partial class ProfileMapper 
    {
        public partial ProfileDto ApplicationUserToProfileDto(ApplicationUser appUser);
    }
}
