using CommerceSystemAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CommerceSystemAPI.Controllers
{
    [ApiController]
    [Route("api/OrderProducts")]
    public class OrderProductsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public OrderProductsController(AppDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("AddOrderProduct")]
        public IActionResult AddOrderProduct(OrderProducts orderProducts)
        {
            _context.OrderProductss.Add(orderProducts);
            _context.SaveChanges();

            return Ok("Order Product Added Successfully");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetAllOrderProducts")]
        public IActionResult GetAllOrderProducts()
        {
            var orderProducts = _context.OrderProductss.ToList();

            return Ok(orderProducts);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("GetOrderProduct")]
        public IActionResult GetOrderProduct(int orderId, int productId)
        {
            var orderProduct = _context.OrderProductss
                .FirstOrDefault(op =>
                    op.OrderId == orderId &&
                    op.ProductId == productId);

            if (orderProduct == null)
            {
                return NotFound("Order Product Not Found");
            }

            return Ok(orderProduct);
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("UpdateOrderProduct")]
        public IActionResult UpdateOrderProduct(int orderId, int productId, OrderProducts orderProduct)
        {
            var op = _context.OrderProductss
                .FirstOrDefault(x =>
                    x.OrderId == orderId &&
                    x.ProductId == productId);

            if (op == null)
            {
                return NotFound("Order Product Not Found");
            }

            op.Quantity = orderProduct.Quantity;

            _context.OrderProductss.Update(op);
            _context.SaveChanges();

            return Ok("Order Product Updated Successfully");
        }
    }
}
