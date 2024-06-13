using System;
using AutoMapper;
using EpicSpots.Data;
using EpicSpots.Interfaces;
using EpicSpots.Models;
using Microsoft.EntityFrameworkCore;

namespace EpicSpots.Repository
{
	public class ReviewRepository : IReviewRepository
	{
        private readonly CampingDataContext _context;
        private readonly IMapper _mapper;

		public ReviewRepository(CampingDataContext context, IMapper mapper)
		{
            _context = context;
            _mapper = mapper;
		}

        public bool CreateReview(Review review)
        {
            _context.Add(review);
            return Save();
        }

        public bool DeleteReview(Review review)
        {
            _context.Remove(review);
            return Save();
        }

        public bool DeleteReviews(List<Review> reviews)
        {
            _context.RemoveRange(reviews);
            return Save();
        }

        public Review GetReview(int reviewId)
        {
            return _context.Reviews.Where(r => r.Id == reviewId).FirstOrDefault();
        }

        public ICollection<Review> GetReviews()
        {
            return _context.Reviews.ToList();
        }

        public ICollection<Review> GetReviewsOfACampsite(int campId)
        {
            return _context.Reviews.Where(r => r.Campsite.Id == campId).ToList();
        }

        public bool ReviewExist(int reviewId)
        {
            return _context.Reviews.Any(r => r.Id == reviewId);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateReview(Review review)
        {
            _context.Update(review);
            return Save();
        }
    }
}

