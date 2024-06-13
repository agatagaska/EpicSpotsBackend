using System;
using EpicSpots.Models;

namespace EpicSpots.Interfaces
{
	public interface IReviewRepository
	{
        ICollection<Review> GetReviews();

        Review GetReview(int reviewId);

        ICollection<Review> GetReviewsOfACampsite(int campId);

        bool ReviewExist(int reviewId);

        bool CreateReview(Review review);

        bool UpdateReview(Review review);

        bool DeleteReview(Review review);

        bool DeleteReviews(List<Review> reviews);

        bool Save();
    }
}

