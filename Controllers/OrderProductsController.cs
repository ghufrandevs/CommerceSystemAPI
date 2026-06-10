using CommerceSystemAPI.Models;
using CommerceSystemAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CommerceSystemAPI.Controllers
{
    [ApiController]
    [Route("api/OrderProducts")]
    public class OrderProductsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly OrderProductsService  _orderProductsService;

        public OrderProductsController(AppDbContext context, OrderProductsService orderProductsService)
        {
            _context = context;
            _orderProductsService = orderProductsService;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("AddOrderProduct")]
        public IActionResult AddOrderProduct(OrderProducts orderProducts)
        {
            return Ok(
                _orderProductsService
                    .AddOrderProduct(orderProducts));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetAllOrderProducts")]
        public IActionResult GetAllOrderProducts()
        {
            return Ok(
                _orderProductsService
                    .GetAllOrderProducts());
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("GetOrderProduct")]
        public IActionResult GetOrderProduct( int orderId, int productId)
        {
            var result =
                _orderProductsService.GetOrderProduct(orderId,  productId);

            if (result == null)
            {
                return NotFound(
                    "Order Product Not Found");
            }

            return Ok(result);
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("UpdateOrderProduct")]
        public IActionResult UpdateOrderProduct(int orderId,int productId,OrderProducts orderProduct)
        {
            var result =
                _orderProductsService.UpdateOrderProduct(orderId,productId,orderProduct);

            if (result == "Order Product Not Found")
            {
                return NotFound(result);
            }

            return Ok(result);
        }
    }

    }

