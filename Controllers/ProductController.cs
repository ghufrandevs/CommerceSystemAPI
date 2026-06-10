using CommerceSystemAPI.DTOs;
using CommerceSystemAPI.Models;
using CommerceSystemAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CommerceSystemAPI.Controllers
{
    [ApiController]
    [Route("api/Product")]
    public class ProductController:ControllerBase
    {
        private readonly AppDbContext _context;

        private readonly ProductService _productService;

        public ProductController(AppDbContext context, ProductService productService)
        {
            _context = context;
            _productService = productService;
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("AddProduct")]
        public IActionResult AddProduct(ProductCreateDTO dto)
        {
            var result = _productService.AddProduct(dto);

            return Ok(result);
        }
        [AllowAnonymous]
        [HttpGet("GetAllProducts")]
       
            public IActionResult GetAllProducts()
        {
            var products = _productService.GetAllProducts();

            return Ok(products);
        }
        [AllowAnonymous]
        [HttpGet("GetProductById")]
        public IActionResult GetProductById(int id)
        {
            var product = _productService.GetProductById(id);

            if (product == null)
            {
                return NotFound("Product Not Found");
            }

            return Ok(product);
        }
        [AllowAnonymous]
        [HttpGet("FilterProducts")]
        public IActionResult FilterProducts(
        string? search,
        decimal minPrice,
        decimal maxPrice,
        int pageNumber,
        int pageSize)
        {
            var result = _productService.FilterProducts(
                search,
                minPrice,
                maxPrice,
                pageNumber,
                pageSize);

            if (result is string)
            {
                return NotFound(result);
            }

            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("UpdateProduct")]
        public IActionResult UpdateProduct(int id , ProductUpdateDTO dto)
        {
            var result = _productService.UpdateProduct(id, dto);

            if (result == "Product Not Found")
            {
                return NotFound(result);
            }

            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("DeleteProduct")]
        public IActionResult DeleteProduct(int id)
        {
            var result = _productService.DeleteProduct(id);

            if (result == "Product Not Found")
            {
                return NotFound(result);
            }

            return Ok(result);
        }

    }

    
}
