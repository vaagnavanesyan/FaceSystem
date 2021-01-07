using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Ordering.OrdersApi.Models;

namespace Ordering.OrdersApi.Persistence
{
    public class OrderRepository : IOrderRepository
    {
        private OrdersContext _context;

        public OrderRepository(OrdersContext context)
        {
            _context = context;
        }

        public Order GetOrder(Guid id)
        {
            return _context.Orders
            .Include(e => e.OrderDetails)
            .FirstOrDefault(e => e.OrderId == id);
        }

        public async Task<Order> GetOrderAsync(Guid id)
        {
            return await _context.Orders
            .Include(e => e.OrderDetails)
            .FirstOrDefaultAsync(e => e.OrderId == id);
        }

        public async Task<IEnumerable<Order>> GetOrdersAsync()
        {
            return await _context.Orders.ToListAsync();
        }

        public async Task RegisterOrder(Order order)
        {
            order.Status = Status.Registered;
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
        }

        public void UpdateOrder(Order order)
        {
            _context.Entry(order).State = EntityState.Modified;
            _context.SaveChanges();
        }
    }
}