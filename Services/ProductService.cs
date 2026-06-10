using CommerceSystemAPI.Repositories;
namespace CommerceSystemAPI.Services;
using CommerceSystemAPI.DTOs;
using CommerceSystemAPI.Models;

public class ProductService
{

    private readonly ProductRepository _productRepository;

    public ProductService(ProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public string AddProduct(ProductCreateDTO dto)
    {
        Product product = new Product()
        {
            ProductName = dto.ProductName,
            Description = dto.Description,
            Price = dto.Price,
            Stock = dto.Stock
        };

        _productRepository.AddProduct(product);

        return "Product Added Successfully";
    }

    public List<ProductOutputDTO> GetAllProducts()
    {
        return _productRepository.GetAllProducts()
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
    }

    public ProductOutputDTO? GetProductById(int id)
    {
        var product = _productRepository.GetById(id);

        if (product == null)
        {
            return null;
        }

        return new ProductOutputDTO()
        {
            ProductId = product.ProductId,
            ProductName = product.ProductName,
            Description = product.Description,
            Price = product.Price,
            Stock = product.Stock,
            OverallRating = product.OverallRating
        };
    }
    public string UpdateProduct(int id, ProductUpdateDTO dto)
    {
        var product = _productRepository.GetById(id);

        if (product == null)
        {
            return "Product Not Found";
        }

        product.ProductName = dto.ProductName;
        product.Description = dto.Description;
        product.Price = dto.Price;
        product.Stock = dto.Stock;

        _productRepository.UpdateProduct(product);

        return "Product Updated Successfully";
    }

    public string DeleteProduct(int id)
    {
        var product = _productRepository.GetById(id);

        if (product == null)
        {
            return "Product Not Found";
        }

        _productRepository.DeleteProduct(product);

        return "Product Deleted Successfully";
    }

    public object FilterProducts(
    string? search,
    decimal minPrice,
    decimal maxPrice,
    int pageNumber,
    int pageSize)
    {
        if (pageNumber < 1)
        {
            pageNumber = 1;
        }

        if (pageSize < 1)
        {
            pageSize = 10;
        }

        var products = _productRepository
            .GetProductsQuery()
            .Where(p =>
                (string.IsNullOrEmpty(search) ||
                 p.ProductName.Contains(search))
                &&
                p.Price >= minPrice
                &&
                p.Price <= maxPrice)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        if (!products.Any())
        {
            return "No Products Found";
        }

        return products;
    }
}






