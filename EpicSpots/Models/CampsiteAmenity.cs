using System;
using System.ComponentModel.DataAnnotations;

namespace EpicSpots.Models
{
	public class CampsiteAmenity
	{
        public int CampsiteId { get; set; }

        public Campsite Campsite { get; set; }

        public int AmenityId { get; set; }

        public Amenity Amenity { get; set; }
    }
}

