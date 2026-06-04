using CommerceSystemAPI.DTOs;
using CommerceSystemAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace CommerceSystemAPI.Controllers
{
    [ApiController]
    [Route("api/Review")]
    public class ReviewController : ControllerBase
    {
        private readonly AppDbContext _context;
        public ReviewController(AppDbContext context)
        {
            _context = context;
        }
        [HttpPost("AddReview")]
        public IActionResult AddReview(ReviewCreateDTO dto)
        {
            var hasPurchased = _context.OrderProductss
                .Any(op =>
                    op.ProductId == dto.ProductId &&
                    op.Order.UserId == dto.UserId);

            if (!hasPurchased)
            {
                return BadRequest("You can only review products you have purchased");
            }

            var existingReview = _context.Reviews
                .Any(r =>
                    r.UserId == dto.UserId &&
                    r.ProductId == dto.ProductId);

            if (existingReview)
            {
                return BadRequest("You have already reviewed this product");
            }

            Review review = new Review()
            {
                UserId = dto.UserId,
                ProductId = dto.ProductId,
                Rating = dto.Rating,
                Comment = dto.Comment,
                ReviewDate = DateTime.Now
            };

            _context.Reviews.Add(review);
            _context.SaveChanges();

            var product = _context.Products.Find(dto.ProductId);

            product.OverallRating = (decimal)_context.Reviews
                .Where(r => r.ProductId == dto.ProductId)
                .Average(r => r.Rating);

            _context.SaveChanges();

            return Ok("Review Added Successfully");
        }

            [HttpGet("GetAllReview")]
        public IActionResult GetAllReview()
        {
            var reviews = _context.Reviews.ToList();

            return Ok(reviews);
        }

        [HttpGet("GetReviewById")]
        public IActionResult GetReviewById(int id)
        {
            var review = _context.Reviews.Find(id);

            if (review == null)
            {
                return NotFound("Review Not Found");
            }

            return Ok(review);
        }

        [HttpGet("ViewProductReviews")]
        public IActionResult ViewProductReviews(int productId)
        {
            var reviews = _context.Reviews
                .Where(r => r.ProductId == productId)
                .Take(10)
                .ToList();

            if (reviews.Count == 0)
            {
                return NotFound("No Reviews Found");
            }

            return Ok(reviews);
        }

        [HttpPut("UpdateReview")]
        public IActionResult UpdateReview(int id, Review review)
        {
            var rev = _context.Reviews.Find(id);

            if (rev == null)
            {
                return NotFound("Review Not Found");
            }

            rev.Rating = review.Rating;
            rev.Comment = review.Comment;
            rev.ReviewDate = review.ReviewDate;

            _context.Reviews.Update(rev);
            _context.SaveChanges();

            return Ok("Review Updated Successfully");

        }

        [HttpDelete("DeleteReview")]

        public IActionResult DeleteReview(int id)
        {
            var review = _context.Reviews.Find(id);

            if (review == null)
            {
                return NotFound("Review Not Found");
            }

            _context.Reviews.Remove(review);
            _context.SaveChanges();

            return Ok("Review Deleted Successfully");
        }








    }
}
