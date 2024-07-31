using EcommerceStoreMVC.Models;
using EcommerceStoreMVC.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Linq;

namespace EcommerceStoreMVC.Controllers
{
    public class StoreController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly int pageSize = 8;

        public StoreController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IActionResult> Index(int pageIndex, string? search, string brand , string category, string sortOrder)
        {
            if (pageIndex < 1)
            {
                pageIndex = 1;
            }

            var products = await _productRepository.GetAllProductsAsync();

            if (search != "" && search != null)
            {
                search = search.ToLower().Trim();
                products = products.AsQueryable().Where(p => p.ProductName.ToLower().Contains(search) || p.Brand.ToLower().Contains(search));
            }
            if(!brand.IsNullOrEmpty())
            {
                products = products.AsQueryable().Where(p => p.Brand == brand);
            

            }
            if(!category.IsNullOrEmpty())
            {
                products = products.AsQueryable().Where(p => p.Category == category);
            }


            IQueryable<Product> query = products.AsQueryable();

            switch (sortOrder)
            {
                case "newest":
                    query = query.OrderByDescending(p => p.CreatedDate);
                    break;
                case "price_desc":
                    query = query.OrderByDescending(p => p.ProductPrice);
                    break;
                case "price_asc":
                    query = query.OrderBy(p => p.ProductPrice);
                    break;
                default:
                    query = query.OrderBy(p => p.ProductId);
                    break;
            }
            
            var count = query.Count();
            var totalPages = (int)Math.Ceiling(count / (double)pageSize);
            if (pageIndex > totalPages)
            {
                pageIndex = totalPages;
            }

            var paginatedList = query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();



            ViewBag.Products = paginatedList;
            ViewBag.PageIndex = pageIndex;
            ViewBag.TotalPages = totalPages;

            var storeSearchModel = new StoreSearchModel
            {
                Search = search,
                Brand = brand,
                Category = category,
                SortOrder = sortOrder
            };

            return View(storeSearchModel);
        }

        public async Task<IActionResult> Details(int productId)
        {
            var product = await _productRepository.GetProductByIdAsync(productId);
            if(product == null)
            {
                return RedirectToAction("Index");
            }
            return View(product);
        }
    }
}
