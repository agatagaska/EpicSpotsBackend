using System;
using EpicSpots.Data;
using EpicSpots.DTO;
using EpicSpots.Interfaces;
using EpicSpots.Models;
using Microsoft.EntityFrameworkCore;

namespace EpicSpots.Repository
{
	public class BookingRepository : IBookingRepository
	{
        private readonly CampingDataContext _context;

        public BookingRepository(CampingDataContext context)
        {
            _context = context;
        }

        public bool BookingExist(int id)
        {
            return _context.Bookings.Any(p => p.Id == id);
        }

        public Booking GetBooking(int id)
        {
            return _context.Bookings.Where(p => p.Id == id).FirstOrDefault();
        }

        public ICollection<Booking> GetBookings()
        {
            return _context.Bookings.ToList();
        }

        public bool CreateBooking(Booking booking)
        {
            _context.Add(booking);
            return Save();
        }

        public bool UpdateBooking(Booking booking)
        {
            _context.Update(booking);
            return Save();
        }

        public IEnumerable<Booking> GetUserBookings(int userId)
        {
            return _context.Bookings.Where(b => b.UserId == userId).ToList();
        }


        public bool DeleteBooking(Booking booking)
        {
            try
            {
                _context.Bookings.Remove(booking);
                return Save();
            }
            catch (Exception ex)
            {
                // Log the exception details
                Console.WriteLine($"Exception occurred while deleting booking: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
                return false;
            }
        }


        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}

