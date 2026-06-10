
using CommerceSystemAPI.Models;
namespace CommerceSystemAPI.Repositories
{
    public class ProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context)

        {
            _context = context;
        }

        public List<Product> GetAllProducts()
        {
            return _context.Products.ToList();
        }

        public Product? GetById(int id)
        {
            return _context.Products.Find(id);
        }

        public void AddProduct(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
        }

        public void UpdateProduct(Product product)
        {
            _context.Products.Update(product);
            _context.SaveChanges();
        }

        public void DeleteProduct(Product product)
        {
            _context.Products.Remove(product);
            _context.SaveChanges();
        }
        public IQueryable<Product> GetProductsQuery()
        {
            return _context.Products;
        }
    }

}

