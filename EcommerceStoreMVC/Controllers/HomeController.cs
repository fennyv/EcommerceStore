using EcommerceStoreMVC.Models;
using EcommerceStoreMVC.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace EcommerceStoreMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductRepository _productRepository;

        public HomeController( IProductRepository productRepository )
        {
            _productRepository = productRepository;

        }

        public async Task<IActionResult> Index()
        {
            var products = await _productRepository.GetAllProductsAsync();
            products = products.OrderByDescending(p => p.ProductId).Take(4).ToList();
            return View(products);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
