using System;
using System.ComponentModel.DataAnnotations;

namespace EpicSpots.DTO
{
	public class UserDTO
	{
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public long PhoneNumber { get; set; }
        public int RoleID { get; set; }
    }
}

