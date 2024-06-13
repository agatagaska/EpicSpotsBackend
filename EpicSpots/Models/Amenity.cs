using System;
using System.ComponentModel.DataAnnotations;

namespace EpicSpots.Models
{
	public class Amenity
	{
        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<CampsiteAmenity> CampsiteAmenities { get; set; }
    }
}

