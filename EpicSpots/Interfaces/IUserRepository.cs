using System;
using EpicSpots.DTO;
using EpicSpots.Models;
using Microsoft.AspNetCore.Mvc;

namespace EpicSpots.Interfaces
{
	public interface IUserRepository
	{
		ICollection<User> GetUsers();
		User GetUser(int userId);
		bool UserExist(int userId);
        bool CreateUser(User user);
        bool UpdateUser(User user);
        bool DeleteUser(User user);
        bool Save();
        User GetUserByUsername(string username);
        int GetUserIdByUsername(string username);
        IEnumerable<Booking> GetUserBookings(int userId);
        IEnumerable<Campsite> GetCampsitesByOwner(int ownerId);
    }
}

