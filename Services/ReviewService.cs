using CommerceSystemAPI.Models;
using CommerceSystemAPI.Repositories;
using CommerceSystemAPI.DTOs;
using CommerceSystemAPI.Models;
namespace CommerceSystemAPI.Services
{
    public class ReviewService
    {
        private readonly ReviewRepository _reviewRepository;

        public ReviewService(ReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }
        public List<Review> GetAllReviews()
        {
            return _reviewRepository.GetAllReviews();
        }
        public Review? GetReviewById(int id)
        {
            return _reviewRepository.GetById(id);
        }
        public string AddReview(
    ReviewCreateDTO dto,
    int userId)
        {
            if (!_reviewRepository.HasPurchasedProduct(
                userId,
                dto.ProductId))
            {
                return "You can only review products you have purchased";
            }

            if (_reviewRepository.HasReviewedProduct(
                userId,
                dto.ProductId))
            {
                return "You have already reviewed this product";
            }

            Review review = new Review()
            {
                UserId = userId,
                ProductId = dto.ProductId,
                Rating = dto.Rating,
                Comment = dto.Comment,
                ReviewDate = DateTime.Now
            };

            _reviewRepository.AddReview(review);

            var product =
                _reviewRepository.GetProduct(dto.ProductId);

            product!.OverallRating =
                _reviewRepository.CalculateAverageRating(
                    dto.ProductId);

            _reviewRepository.UpdateProductRating(product);

            return "Review Added Successfully";
        }
        public string UpdateReview(
        int id,
        ReviewUpdateDTO dto,
        int userId)
        {
            var review = _reviewRepository.GetById(id);

            if (review == null)
            {
                return "Review Not Found";
            }

            if (review.UserId != userId)
            {
                return "Forbidden";
            }

            review.Rating = dto.Rating;
            review.Comment = dto.Comment;
            review.ReviewDate = DateTime.Now;

            _reviewRepository.UpdateReview(review);

            var product =
                _reviewRepository.GetProduct(review.ProductId);

            product!.OverallRating =
                _reviewRepository.CalculateAverageRating(
                    review.ProductId);

            _reviewRepository.UpdateProductRating(product);

            return "Review Updated Successfully";
        }
        public string DeleteReview(
    int id,
    int userId)
        {
            var review = _reviewRepository.GetById(id);

            if (review == null)
            {
                return "Review Not Found";
            }

            if (review.UserId != userId)
            {
                return "Forbidden";
            }

            int productId = review.ProductId;

            _reviewRepository.DeleteReview(review);

            var product =
                _reviewRepository.GetProduct(productId);

            var reviews = _reviewRepository
                .GetReviewsQuery()
                .Where(r => r.ProductId == productId);

            if (reviews.Any())
            {
                product!.OverallRating =
                    (decimal)reviews.Average(r => r.Rating);
            }
            else
            {
                product!.OverallRating = 0;
            }

            _reviewRepository.UpdateProductRating(product);

            return "Review Deleted Successfully";
        }
    }
}

