using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EpicSpots.Models
{
	public class Review
	{
        public int Id { get; set; }

        public int UserId { get; set; }

        public int CampsiteId { get; set; }

        public int Rating { get; set; }

        public string? Comment { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Campsite Campsite { get; set; }

        public User User { get; set; }
    }
}

