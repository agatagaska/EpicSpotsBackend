using System;
using EpicSpots.Models;

namespace EpicSpots.DTO
{
	public class AmenityDTO
	{
        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<CampsiteAmenity> CampsiteAmenities { get; set; }
    }
}

