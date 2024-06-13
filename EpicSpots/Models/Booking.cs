using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EpicSpots.Models
{
	public class Booking
	{
        public int Id { get; set; }

        public int UserId { get; set; }

        public int CampsiteId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int NumberOfGuests { get; set; }

        public decimal TotalPrice { get; set; }

        public string BookingStatus { get; set; } = "Pending"; // Pending, Confirmed, Cancelled

        public Campsite Campsite { get; set; }
 
    }
}

