using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace EpicSpots.Models
{
    public class UserLoginRequest
    {
        public string Username { get; set; }
        public string PasswordHash { get; set; }
    }
}
