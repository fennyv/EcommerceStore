using EcommerceStoreMVC.Models;

namespace EcommerceStoreMVC.Repository
{
    public interface IOrderRepository
    {
        Task<Order> CreateOrderAsync(Order order);

        Task<Order?> GetOrderByIdAsync(int orderId);
        Task<Order?> GetOrderDetailsByIdAsync(int orderId);

        Task<IEnumerable<Order>> GetOrdersByCustomerIdAsync(string customerId);

        Task<IEnumerable<Order>> GetAllOrdersAsync();

        Task<Order> UpdateOrderAsync(Order order);

        Task DeleteOrderAsync(Order order);

        Task DeleteOrderByIdAsync(int orderId);
    }
}
