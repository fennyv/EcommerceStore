using EcommerceStoreMVC.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public  class ApplicationDbContext : IdentityDbContext<ApplicationUser>

{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<Order> Orders { get; set; }

    //public DbSet<Category> Categories { get; set; }
    //public DbSet<Cart> Carts { get; set; }
    //public DbSet<CartItem> CartItems { get; set; }
    //public DbSet<Order> Orders { get; set; }
    //public DbSet<OrderItem> OrderItems { get; set; }
    //public DbSet<ShippingDetail> ShippingDetails { get; set; }
    //public DbSet<Review> Reviews { get; set; }
    //public DbSet<Rating> Ratings { get; set; }
    //public DbSet<Payment> Payments { get; set; }
    //public DbSet<PaymentDetail> PaymentDetails { get; set; }
    //public DbSet<Shipping> Shippings { get; set; }
    //public DbSet<ShippingMethod> ShippingMethods { get; set; }
    //public DbSet<ShippingProvider> ShippingProviders { get; set; }
    //public DbSet<ShippingRate> ShippingRates { get; set; }
    //public DbSet<ShippingRateTier> ShippingRateTiers { get; set; }
    //public DbSet<ShippingRateTierRange> ShippingRateTierRanges { get; set; }
    //public DbSet<ShippingRateTierWeight> ShippingRateTierWeights { get; set; }
    //public DbSet<ShippingRateTierPrice> ShippingRateTierPrices { get; set; }
    //public DbSet<ShippingRateTierQuantity> ShippingRateTierQuantities { get; set; }
    //public DbSet<ShippingRateTierDestination> ShippingRateTierDestinations { get; set; }
    //public DbSet<ShippingRateTierDestinationCountry> ShippingRateTierDestinationCountries { get; set; }
    //public DbSet<ShippingRateTierDestinationState> ShippingRateTierDestinationStates { get; set; }
    //public DbSet<ShippingRateTierDestinationCity> ShippingRateTierDestinationCities { get; set; }
    //public DbSet<ShippingRateTierDestinationZipCode> ShippingRateTierDestinationZipCodes { get; set; }
    //public DbSet<ShippingRateTierDestinationAddress> ShippingRateTierDestinationAddresses { get; set; }
    //public DbSet<ShippingRateTierDestinationLatitude> ShippingRateTierDestinationLatitudes { get; set; }
    //public DbSet<ShippingRateTierDestinationLongitude> ShippingRateTierDestinationLongitudes { get; set; }
    //public DbSet<ShippingRateTierDestination



}