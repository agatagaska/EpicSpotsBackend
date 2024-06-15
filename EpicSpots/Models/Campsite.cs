using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EpicSpots.Models
{
	public class Campsite
	{
        public int Id { get; set; }

        public string Name { get; set; }

        public string Location { get; set; }

        public string Description { get; set; }

        public decimal PricePerNight { get; set; }

        // relationships

        public ICollection<Booking> Bookings { get; set; }

        public byte[] Images { get; set; }

        public ICollection<CampsiteAmenity> CampsiteAmenities { get; set; }

        public int OwnerId { get; set; }

        public User Owner { get; set; }

    }
}

