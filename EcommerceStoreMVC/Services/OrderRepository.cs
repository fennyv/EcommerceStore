using EcommerceStoreMVC.Models;
using EcommerceStoreMVC.Repository;
using Microsoft.EntityFrameworkCore;

namespace EcommerceStoreMVC.Services
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public OrderRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Order> CreateOrderAsync(Order order)
        {
            await _dbContext.Orders.AddAsync(order);
            await _dbContext.SaveChangesAsync();
            return order;
        }

        public async Task DeleteOrderAsync(Order order)
        {
            _dbContext.Orders.Remove(order);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteOrderByIdAsync(int orderId)
        {
            var order = await GetOrderByIdAsync(orderId);
            if (order != null)
            {
                await DeleteOrderAsync(order);
            }
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            var orders  = await _dbContext.Orders.OrderBy(o => o.Id).Include("Customer").ToListAsync();
            return orders;
        }

        public async Task<Order?> GetOrderByIdAsync(int orderId)
        {
            return await _dbContext.Orders.FindAsync(orderId);
        }

        public async Task<Order?> GetOrderDetailsByIdAsync(int orderId)
        {
            var order = await _dbContext.Orders.Include(o => o.Customer).Include(o => o.Items).ThenInclude(oi => oi.Product).FirstOrDefaultAsync(o => o.Id == orderId);
            return order;
        }

        public async Task<IEnumerable<Order>> GetOrdersByCustomerIdAsync(string customerId)
        {
            return await _dbContext.Orders.Where(o => o.CustomerId == customerId).ToListAsync();
        }

        public async Task<Order> UpdateOrderAsync(Order order)
        {
            _dbContext.Orders.Update(order);
            await _dbContext.SaveChangesAsync();
            return order;
        }
    }
}
