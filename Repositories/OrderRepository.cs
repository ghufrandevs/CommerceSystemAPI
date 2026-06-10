using CommerceSystemAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CommerceSystemAPI.Repositories
{
    public class OrderRepository
    {
        private readonly AppDbContext _context;

        public OrderRepository(AppDbContext context)
        {
            _context = context;
        }

        public List<Order> GetAllOrders()
        {
            return _context.Orders.ToList();
        }

        public Order? GetById(int id)
        {
            return _context.Orders.Find(id);
        }

        public List<Order> GetUserOrders(int userId)
        {
            return _context.Orders
                .Where(o => o.UserId == userId)
                .ToList();
        }

        public List<object> GetOrderDetails(int orderId)
        {
            return _context.OrderProductss
                .Include(op => op.Product)
                .Where(op => op.OrderId == orderId)
                .Select(op => new
                {
                    op.ProductId,
                    op.Product.ProductName,
                    op.Product.Price,
                    op.Quantity
                })
                .Cast<object>()
                .ToList();
        }

    }
}

