using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EpicSpots.Models
{
	public class Role
	{
        public int Id { get; set; }

        public string RoleName { get; set; }

        public ICollection<User> Users { get; set; }
    }
}

