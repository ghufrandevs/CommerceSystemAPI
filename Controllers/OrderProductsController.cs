using CommerceSystemAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace CommerceSystemAPI.Controllers
{
    [ApiController]
    [Route("api/OrderProducts")]
    public class OrderProductsController : ControllerBase
    {
        AppDbContext contex = new AppDbContext();

        [HttpPost("AddOrderProduct")]
        public IActionResult AddOrderProduct(OrderProducts orderProducts)
        {
            contex.OrderProductss.Add(orderProducts);
            contex.SaveChanges();

            return Ok("Order Product Added Successfully");
        }

        [HttpGet("GetAllOrderProducts")]
        public IActionResult GetAllOrderProducts()
        {
            var orderProducts = contex.OrderProductss.ToList();

            return Ok(orderProducts);
        }
        [HttpGet("GetOrderProduct")]
        public IActionResult GetOrderProduct(int orderId, int productId)
        {
            var orderProduct = contex.OrderProductss
                .FirstOrDefault(op =>
                    op.OrderId == orderId &&
                    op.ProductId == productId);

            if (orderProduct == null)
            {
                return NotFound("Order Product Not Found");
            }

            return Ok(orderProduct);
        }
        [HttpPut("UpdateOrderProduct")]
        public IActionResult UpdateOrderProduct(int orderId, int productId, OrderProducts orderProduct)
        {
            var op = contex.OrderProductss
                .FirstOrDefault(x =>
                    x.OrderId == orderId &&
                    x.ProductId == productId);

            if (op == null)
            {
                return NotFound("Order Product Not Found");
            }

            op.Quantity = orderProduct.Quantity;

            contex.OrderProductss.Update(op);
            contex.SaveChanges();

            return Ok("Order Product Updated Successfully");
        }
    }
}
