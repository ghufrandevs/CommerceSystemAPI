using CommerceSystemAPI.Models;

namespace CommerceSystemAPI.Repositories
{
    public class ReviewRepository
    {
        private readonly AppDbContext _context;

        public ReviewRepository(AppDbContext context)
        {
            _context = context;
        }

        public List<Review> GetAllReviews()
        {
            return _context.Reviews.ToList();
        }

        public Review? GetById(int id)
        {
            return _context.Reviews.Find(id);
        }

        public void AddReview(Review review)
        {
            _context.Reviews.Add(review);
            _context.SaveChanges();
        }

        public void UpdateReview(Review review)
        {
            _context.Reviews.Update(review);
            _context.SaveChanges();
        }

        public void DeleteReview(Review review)
        {
            _context.Reviews.Remove(review);
            _context.SaveChanges();
      
        }
        public IQueryable<Review> GetReviewsQuery()
        {
            return _context.Reviews;
        }
        public bool HasPurchasedProduct(int userId, int productId)
        {
            return _context.OrderProductss.Any(op =>
                op.ProductId == productId &&
                op.Order.UserId == userId);
        }

        public bool HasReviewedProduct(int userId, int productId)
        {
            return _context.Reviews.Any(r =>
                r.UserId == userId &&
                r.ProductId == productId);
        }

        public Product? GetProduct(int productId)
        {
            return _context.Products.Find(productId);
        }

        public decimal CalculateAverageRating(int productId)
        {
            return (decimal)_context.Reviews
                .Where(r => r.ProductId == productId)
                .Average(r => r.Rating);
        }
        public void UpdateProductRating(Product product)
        {
            _context.Products.Update(product);
            _context.SaveChanges();
        }
    }
}

