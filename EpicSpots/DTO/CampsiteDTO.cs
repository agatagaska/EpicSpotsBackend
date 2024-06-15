using System;

namespace EpicSpots.DTO
{
	public class CampsiteDTO
	{
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Location { get; set; }

        public decimal PricePerNight { get; set; }

        public string Amenities { get; set; }

        public string ImageBase64 { get; set; }

    }
}

