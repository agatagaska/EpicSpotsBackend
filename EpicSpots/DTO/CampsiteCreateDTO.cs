using System.Collections.Generic;

namespace EpicSpots.DTO
{
    public class CampsiteCreateDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public decimal PricePerNight { get; set; }
        public string ImageBase64 { get; set; }
        public List<int> Amenities { get; set; }
    }
}
