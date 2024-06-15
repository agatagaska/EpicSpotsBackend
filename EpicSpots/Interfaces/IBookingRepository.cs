using System;
using EpicSpots.Models;
using Microsoft.EntityFrameworkCore;

namespace EpicSpots.Interfaces
{
	public interface IBookingRepository
	{
		ICollection<Booking> GetBookings();
		Booking GetBooking(int id);
        bool BookingExist(int id);
        IEnumerable<Booking> GetUserBookings(int userId);
        bool CreateBooking(Booking booking);
        bool DeleteBooking(Booking booking);
        bool Save();
    }

}

