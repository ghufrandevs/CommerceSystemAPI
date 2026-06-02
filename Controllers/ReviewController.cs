using CommerceSystemAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace CommerceSystemAPI.Controllers
{
    [ApiController]
    [Route("api/Review")]
    public class ReviewController : ControllerBase
    {
        AppDbContext contex = new AppDbContext();

        [HttpPost("AddReview")]
        public IActionResult AddReview(Review review)
        {
            contex.Reviews.Add(review);
            contex.SaveChanges();
            return Ok("Review Added Successfully");
        }

        [HttpGet("GetAllReview")]
        public IActionResult GetAllReview()
        {
            var reviews = contex.Reviews.ToList();

            return Ok(reviews);
        }

        [HttpGet("GetReviewById")]
        public IActionResult GetReviewById(int id)
        {
            var review = contex.Reviews.Find(id);

            if (review == null)
            {
                return NotFound("Review Not Found");
            }

            return Ok(review);
        }

        [HttpGet("ViewProductReviews")]
        public IActionResult ViewProductReviews(int productId)
        {
            var reviews = contex.Reviews
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
            var rev = contex.Reviews.Find(id);

            if (rev == null)
            {
                return NotFound("Review Not Found");
            }

            rev.Rating = review.Rating;
            rev.Comment = review.Comment;
            rev.ReviewDate = review.ReviewDate;

            contex.Reviews.Update(rev);
            contex.SaveChanges();

            return Ok("Review Updated Successfully");

        }

        [HttpDelete("DeleteReview")]

        public IActionResult DeleteReview(int id)
        {
            var review = contex.Reviews.Find(id);

            if (review == null)
            {
                return NotFound("Review Not Found");
            }

            contex.Reviews.Remove(review);
            contex.SaveChanges();

            return Ok("Review Deleted Successfully");
        }








    }
}
