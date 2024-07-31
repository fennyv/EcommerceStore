using EcommerceStoreMVC.Repository;
using Microsoft.AspNetCore.Mvc;
using EcommerceStoreMVC.Models;
using EcommerceStoreMVC.Mapping;
using System;
using EcommerceStoreMVC.Models.DTO;
using Microsoft.AspNetCore.Authorization;

namespace EcommerceStoreMVC.Controllers
{
    [Authorize(Roles = "admin")]
    [Route("/Admin/[Controller]/{action=Index}/{id?}")]
    public class ProductsController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly IWebHostEnvironment environment;
        private readonly int _pageSize = 5;

        public ProductsController(IProductRepository productRepository, IWebHostEnvironment environment)
        {
            _productRepository = productRepository;
            this.environment = environment;
        }
        public async Task<IActionResult> Index(int pageIndex, string? search, string? column, string? orderBy )
        {
            string[] validColumns = { "ProductId",  "ProductName", "Brand", "Category", "ProductPrice", "CreatedAt" };
            string[] validOrderBy = ["asc", "desc"];

            if(pageIndex < 1)
            {
                pageIndex = 1;
            }
            if(!validColumns.Contains(column))
            {
                column = "ProductId";
            }
            if(!validOrderBy.Contains(orderBy))
            {
                orderBy = "asc";
            }
            //IQueryable queryable = await _productRepository.GetAllProductsAsync();
            //var products = await PaginatedList<Product>.CreateAsync(queryable, pageIndex, _pageSize);

            var products = await _productRepository.GetAllProductsAsync();
            IQueryable<Product> queryable = products.AsQueryable();
            if(search != "" && search != null)
            {
                search = search.ToLower().Trim();
                queryable = products.AsQueryable().Where(p => p.ProductName.ToLower().Contains(search) || p.Brand.ToLower().Contains(search) );
            }
            else
            {
                queryable = products.AsQueryable();
            }
            if(orderBy == "asc")
            {
                if(column == "CreatedAt" ) {
                    queryable = queryable.OrderBy(prod => prod.CreatedDate.ToString());
                }
                else
                queryable = queryable.OrderBy(prod => prod.GetType().GetProperty(column).GetValue(prod, null));

            }
            else
            {
                if (column == "CreatedAt")
                {
                    queryable = queryable.OrderByDescending(prod => prod.CreatedDate.ToString());
                }
                else
                    queryable = queryable.OrderByDescending(prod => prod.GetType().GetProperty(column).GetValue(prod, null));
            }
           // IQueryable<Product> queryable = products.AsQueryable().Where(p => p.ProductName.Contains(search) || p.Brand.Contains(search) ).OrderBy(p => p.ProductId);
            var count = queryable.Count();
            var totalPages = (int)Math.Ceiling(count / (double)_pageSize);
            var paginatedProducts = queryable.Skip((_pageSize * pageIndex) - _pageSize).Take(_pageSize).ToList();
            ViewData["TotalPages"] = totalPages;
            ViewData["PageIndex"] = pageIndex;
            ViewData["Search"] = search?? "";
            ViewData["Column"] = column?? "ProductId";
            ViewData["OrderBy"] = orderBy?? "asc";

            return View(paginatedProducts);
            
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create( ProductDto productDto)
        {
            if (productDto.ProductImageFile == null)
            {
                ModelState.AddModelError("ProductImageFile", "The image file is required");
            }

            if (ModelState.IsValid)
            {
                var mapper = new ProductMapper();
                var product = mapper.ProductDtoToProduct(productDto);

                // save the image file
                string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                newFileName += Path.GetExtension(productDto.ProductImageFile!.FileName);

                string imageFullPath = environment.WebRootPath + "/products/" + newFileName;
                using (var stream = System.IO.File.Create(imageFullPath))
                {
                    productDto.ProductImageFile.CopyTo(stream);
                }
                product.ProductImage = newFileName;

                await _productRepository.AddProductAsync(product);
                return RedirectToAction(nameof(Index));
            }
            return View(productDto);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var product = await _productRepository.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            var mapper = new ProductMapper();
            var productDto = mapper.ProductToProductDto(product);

            ViewData["ProductImage"] = product.ProductImage;
            ViewData["ProductId"] = product.ProductId;
            ViewData["CreatedDate"] = product.CreatedDate.ToShortDateString();
            return View(productDto);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, ProductDto productDto)
        {
            if (!ModelState.IsValid)
            {
                return View(productDto);
            }
            var mapper = new ProductMapper();
            var product = mapper.ProductDtoToProduct(productDto);
            product.ProductId = id;

            if (productDto.ProductImageFile != null)
            {
                if (productDto.ProductImageFile.Length > 0)
                {
                    string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                    newFileName += Path.GetExtension(productDto.ProductImageFile.FileName);

                    string imageFullPath = environment.WebRootPath + "/products/" + newFileName;
                    using (var stream = System.IO.File.Create(imageFullPath))
                    {
                        productDto.ProductImageFile.CopyTo(stream);
                    }
                    string oldImagePath = environment.WebRootPath + "/products/" + product.ProductImage;
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                    product.ProductImage = newFileName;
                }
            }
            
            await _productRepository.UpdateProductAsync(product);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var product = await _productRepository.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            if(product.ProductImage != null)
            {
                string oldImagePath = environment.WebRootPath + "/products/" + product.ProductImage;
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }

            await _productRepository.DeleteProductAsync(product);
            return RedirectToAction(nameof(Index), "Products");
        }
    }
}
