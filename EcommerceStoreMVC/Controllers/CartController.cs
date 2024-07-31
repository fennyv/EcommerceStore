using EcommerceStoreMVC.Models;
using EcommerceStoreMVC.Models.DTO;
using EcommerceStoreMVC.Repository;
using EcommerceStoreMVC.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace EcommerceStoreMVC.Controllers
{
    public class CartController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly decimal shippingFee;

        public CartController(IProductRepository productRepository, IOrderRepository orderRepository, IConfiguration configuration
            , UserManager<ApplicationUser> userManager)
        {
            _productRepository = productRepository;
            _orderRepository = orderRepository;
            this.userManager = userManager;
            shippingFee = configuration.GetValue<decimal>("CartSettings:ShippingFee");
        }

        public async Task<IActionResult> Index()
        {
            var address = TempData["DeliveryAddress"] as string ?? string.Empty;
            CheckoutDto model = new CheckoutDto { DeliveryAddress = address };
            List<OrderItem> cartItems = await CartHelper.GetCartItems(Request, Response, _productRepository);
            decimal subtotal = CartHelper.GetSubtotal(cartItems);
 

            ViewBag.CartItems = cartItems;
            ViewBag.ShippingFee = shippingFee;
            ViewBag.Subtotal = subtotal;
            ViewBag.Total = subtotal + shippingFee;
            TempData["DeliveryAddress"] = address;
            return View(model);
        }
        
        [HttpPost]
        public async Task<IActionResult> Index(CheckoutDto model )
        {
            List<OrderItem> cartItems = await  CartHelper.GetCartItems(Request, Response, _productRepository);
            decimal subtotal = CartHelper.GetSubtotal(cartItems);


            ViewBag.CartItems = cartItems;
            ViewBag.ShippingFee = shippingFee;
            ViewBag.Subtotal = subtotal;
            ViewBag.Total = subtotal + shippingFee;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Check if shopping cart is empty or not
            if (cartItems.Count == 0)
            {
                ViewBag.ErrorMessage = "Your cart is empty";
                return View(model);
            }


            TempData["DeliveryAddress"] = model.DeliveryAddress;
            TempData["PaymentMethod"] = model.PaymentMethod;


            return RedirectToAction("Confirm");
        }

        public async Task<IActionResult> Confirm()
        {
            List<OrderItem> cartItems = await CartHelper.GetCartItems(Request, Response, _productRepository);
            decimal total = CartHelper.GetSubtotal(cartItems) + shippingFee;
            int cartSize = 0;
            foreach (var item in cartItems)
            {
                cartSize += item.Quantity;
            }


            string deliveryAddress = TempData["DeliveryAddress"] as string ?? "";
            string paymentMethod = TempData["PaymentMethod"] as string ?? "";
            TempData.Keep();


            if (cartSize == 0 || deliveryAddress.Length == 0 || paymentMethod.Length == 0)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.DeliveryAddress = deliveryAddress;
            ViewBag.PaymentMethod = paymentMethod;
            ViewBag.Total = total;
            ViewBag.CartSize = cartSize;

            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Confirm(int any)
        {
            var cartItems = await CartHelper.GetCartItems(Request, Response, _productRepository);

            string deliveryAddress = TempData["DeliveryAddress"] as string ?? "";
            string paymentMethod = TempData["PaymentMethod"] as string ?? "";
            TempData.Keep();

            if (cartItems.Count == 0 || deliveryAddress.Length == 0 || paymentMethod.Length == 0)
            {
                return RedirectToAction("Index", "Home");
            }

            var appUser = await userManager.GetUserAsync(User);
            if (appUser == null)
            {
                return RedirectToAction("Index", "Home");
            }

            // save the order
            var order = new Order
            {
                CustomerId = appUser.Id,
              //  Customer = appUser,
                Items = cartItems,
                ShippingFee = shippingFee,
                DeliveryAddress = deliveryAddress,
                PaymentMethod = paymentMethod,
                PaymentStatus = "pending",
                PaymentDetails = "",
                OrderStatus = "created",
                CreatedAt = DateTime.Now,
            };

            await _orderRepository.CreateOrderAsync(order);
            
            // delete the shopping cart cookie
            Response.Cookies.Delete("shopping_cart");

            ViewBag.SuccessMessage = "Order created successfully";

            return View();
        }

        //[Authorize]
        //public async Task<IActionResult> AddToCart(int productId )
        //{
        //    var product = await _productRepository.GetProductByIdAsync(productId);
        //    if (product == null)
        //    {
        //        return NotFound();
        //    }

        //    var cart = CartHelper.GetShoppingCartCookie(Request, Response);
        //    if (cart.ContainsKey(productId))
        //    {
        //        cart[productId] ++;
        //    }
        //    else
        //    {
        //        cart[productId] = 1;
        //    }

        //    string cartJson = JsonSerializer.Serialize(cart);
        //    string cartBase64 = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(cartJson));
        //    Response.Cookies.Append("shopping_cart", cartBase64, new CookieOptions
        //    {
        //        Expires = DateTimeOffset.Now.AddDays(30)
        //    });

        //    return RedirectToAction("Index", "Home");
        //}
    }
}
