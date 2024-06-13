using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace EpicSpots.Models
{
	public class User
	{
        public int Id { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public string PasswordHash { get; set; } 

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public long PhoneNumber { get; set; }

        public int RoleId { get; set; }
        
        public ICollection<Review> Reviews { get; set; }

        public Role Role { get; set; }

        public ICollection<Campsite> Campsites { get; set; }
    }
}

