using CommerceSystemAPI.DTOs;
using CommerceSystemAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace CommerceSystemAPI.Controllers
{
    [ApiController]
    [Route("api/Product")]
    public class ProductController:ControllerBase
    {
        AppDbContext contex = new AppDbContext();

        [HttpPost("AddProduct")]
        public IActionResult AddProduct(ProductCreateDTO dto)
        {
            Product product = new Product()
            {
                ProductName = dto.ProductName,
                Description = dto.Description,
                Price = dto.Price,
                Stock = dto.Stock
            };
            contex.Products.Add(product);
            contex.SaveChanges();
            return Ok("Product Added Successfully");
        }

        [HttpGet("GetAllProducts")]
        public IActionResult GetAllProducts()
        {
            var products = contex.Products

                .Select(p => new ProductOutputDTO
                {
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    Description = p.Description,
                    Price = p.Price,
                    Stock = p.Stock,
                    OverallRating = p.OverallRating
                })
             .ToList();

            
            return Ok(products);


        }
        [HttpGet("GetProductById")]
        public IActionResult GetProductById(int id)
        {
            var product = contex.Products.Find(id);
            if(product == null)

            {
                return NotFound("Product Not Found");

            }
            ProductOutputDTO productOutput = new ProductOutputDTO()
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock,
                OverallRating = product.OverallRating
            };
            return Ok(productOutput);
        }

        [HttpGet("FilterProducts")]
        public IActionResult FilterProducts(string search, decimal minPrice, decimal maxPrice, int pageNumber, int pageSize)

        {
            var products = contex.Products
           .Where(p => p.ProductName.Contains(search) &&
            p.Price >= minPrice &&
            p.Price <= maxPrice)
            .Skip((pageNumber - 1) * pageSize)
           .Take(pageSize)
           .ToList();
            return Ok(products);
           
        }

        [HttpPut("UpdateProduct")]
        public IActionResult UpdateProduct(int id , ProductUpdateDTO dto)
        {
            var prod = contex.Products.Find(id);
            if (prod == null)
            {
                return NotFound("product not found");

            }
            prod.ProductName=dto.ProductName;
            prod.Description= dto.Description;
            prod.Price = dto.Price;
            prod.Stock = dto.Stock;

            contex.Update(prod);
            contex.SaveChanges();
            return Ok("Product Updated Successfully");
        }

    }

    
}
