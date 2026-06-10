using CommerceSystemAPI.DTOs;
using CommerceSystemAPI.Models;
using CommerceSystemAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CommerceSystemAPI.Controllers
{
    [ApiController]
    [Route("api/Review")]
    public class ReviewController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ReviewService _reviewService;
        public ReviewController(AppDbContext context , ReviewService reviewService)
        {
            _context = context;
            _reviewService = reviewService;
        }
        [Authorize]
        [HttpPost("AddReview")]
        public IActionResult AddReview(ReviewCreateDTO dto)
        {
            int userId = int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier)!
                .Value);

            var result =
                _reviewService.AddReview(dto, userId);

            if (result ==
                "You can only review products you have purchased"
                ||
                result ==
                "You have already reviewed this product")
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetAllReview")]
        public IActionResult GetAllReview()
        {
            return Ok(_reviewService.GetAllReviews());
        }

        [AllowAnonymous]

        [HttpGet("GetReviewById")]
        public IActionResult GetReviewById(int id)
        {
            var review = _reviewService.GetReviewById(id);

            if (review == null)
            {
                return NotFound("Review Not Found");
            }

            return Ok(review);
        }

        [AllowAnonymous]
        [HttpGet("ViewProductReviews")]
        public IActionResult ViewProductReviews( int productId, int pageNumber, int pageSize)
        {
            if (pageNumber < 1)
            {
                pageNumber = 1;
            }

            if (pageSize < 1)
            {
                pageSize = 10;
            }
            var reviews = _context.Reviews
          .Where(r => r.ProductId == productId)
          .Skip((pageNumber - 1) * pageSize)
          .Take(pageSize)
          .ToList();

            if (reviews.Count == 0)
            {
                return NotFound("No Reviews Found");
            }

            return Ok(reviews);
          }
          [Authorize]
          [HttpPut("UpdateReview")]
          public IActionResult UpdateReview(
          int id,
           ReviewUpdateDTO dto)
          {
            int userId = int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var result = _reviewService.UpdateReview(id,dto, userId);
                   
            if (result == "Review Not Found")
            {
                return NotFound(result);
            }

            if (result == "Forbidden")
            {
                return Forbid();
            }

            return Ok(result);
        }
        [Authorize]
        [HttpDelete("DeleteReview")]
        public IActionResult DeleteReview(int id)
        {
            int userId = int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier)!
                .Value);

            var result =
                _reviewService.DeleteReview(
                    id,
                    userId);

            if (result == "Review Not Found")
            {
                return NotFound(result);
            }

            if (result == "Forbidden")
            {
                return Forbid();
            }

            return Ok(result);
        }
    }

    }

