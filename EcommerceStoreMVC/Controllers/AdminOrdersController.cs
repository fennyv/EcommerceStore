using EcommerceStoreMVC.Repository;
using EcommerceStoreMVC.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceStoreMVC.Controllers
{
    [Authorize(Roles = "admin")]
    [Route("/Admin/Orders/{action=Index}/{id?}")]
    public class AdminOrdersController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly IOrderRepository _orderRepository;

        public AdminOrdersController(IProductRepository productRepository, IOrderRepository orderRepository )
        {
            _productRepository = productRepository;
            _orderRepository = orderRepository;
        }
        public async Task<IActionResult> Index()
        {
          var orders =  await _orderRepository.GetAllOrdersAsync();
            ViewBag.Orders = orders;
            return View();
        }
    

    public async Task<IActionResult> Details(int id)
    {
            var order = await _orderRepository.GetOrderDetailsByIdAsync(id); 


        if (order == null)
        {
            return RedirectToAction("Index");
        }


        var ordersByCustomer = await _orderRepository.GetOrdersByCustomerIdAsync(order.CustomerId);
            
        ViewBag.NumOrders = ordersByCustomer.Count();

        return View(order);
    }


    public async Task<IActionResult> Edit(int id, string? payment_status, string? order_status)
    {
        var order = await _orderRepository.GetOrderByIdAsync(id);
        if (order == null)
        {
            return RedirectToAction("Index");
        }


        if (payment_status == null && order_status == null)
        {
            return RedirectToAction("Details", new { id });
        }

        if (payment_status != null)
        {
            order.PaymentStatus = payment_status;
        }

        if (order_status != null)
        {
            order.OrderStatus = order_status;
        }

        await _orderRepository.UpdateOrderAsync(order);  


        return RedirectToAction("Details", new { id });
    }
}
}
