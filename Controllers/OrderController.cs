using CommerceSystemAPI.DTOs;
using CommerceSystemAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;

namespace CommerceSystemAPI.Controllers
{
    [ApiController]
    [Route("api/Order")]
    public class OrderController : ControllerBase
    {
        private readonly AppDbContext _context;

        public OrderController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet("GetAllOrders")]
        public IActionResult GetAllOrders()
        {
            var orders = _context.Orders.ToList();

            return Ok(orders);
        }

        [HttpGet("GetOrderById")]
        public IActionResult GetOrderById(int id)
        {
            var order = _context.Orders.Find(id);

            if (order == null)
            {
                return NotFound("Order Not Found");
            }

            return Ok(order);
        }

        [HttpGet("ViewMyOrders")]
        public IActionResult ViewMyOrders(int userId)
        {
            var orders = _context.Orders
                .Where(o => o.UserId == userId)
                .ToList();

            if (orders.Count == 0)
            {
                return NotFound("No Orders Found");
            }

            return Ok(orders);
        }

        [HttpPost("PlaceOrder")]
        public IActionResult PlaceOrder(PlaceOrderDTO dto)
        {
            var user = _context.Users.Find(dto.UserId);

            if (user == null)
            {
                return BadRequest("User Not Found");
            }
            decimal totalAmount = 0;

            Order order = new Order()
            {
                UserId = dto.UserId,
                OrderDate = DateTime.Now
            };

            _context.Orders.Add(order);
            _context.SaveChanges();

            foreach (var item in dto.Items)
            {
                var product = _context.Products.Find(item.ProductId);

                if (product == null)
                {
                    return BadRequest("Product Not Found");
                }

                if (product.Stock < item.Quantity)
                {
                    return BadRequest($"Insufficient stock for product {product.ProductName}");
                }

                totalAmount += product.Price * item.Quantity;

                product.Stock -= item.Quantity;

                OrderProducts orderProduct = new OrderProducts()
                {
                    OrderId = order.OrderId,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity
                };

                _context.OrderProductss.Add(orderProduct);
            }

            order.TotalAmount = totalAmount;

            _context.SaveChanges();

            return Ok(new
            {
                Message = "Order Placed Successfully",
                OrderId = order.OrderId,
                TotalAmount = order.TotalAmount
            });
        }

        [HttpGet("GetOrderDetails")]
        public IActionResult GetOrderDetails(int orderId)
        {
            var details = _context.OrderProductss
                .Include(op => op.Product)
                .Where(op => op.OrderId == orderId)
                .Select(op => new
                {
                    op.ProductId,
                    op.Product.ProductName,
                    op.Product.Price,
                    op.Quantity
                })
                .ToList();

            if (details.Count == 0)
            {
                return NotFound("No Order Details Found");
            }

            return Ok(details);
        }

        
    }
}
