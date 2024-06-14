using System;
using System.Security.Claims;
using EpicSpots.Data;
using EpicSpots.Interfaces;
using EpicSpots.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mysqlx.Crud;
using Org.BouncyCastle.Crypto.Generators;

namespace EpicSpots.Repository
{
	public class UserRepository : IUserRepository
	{
        private readonly CampingDataContext _context;

		public UserRepository(CampingDataContext context)
		{
            _context = context;
		}

        public ICollection<User> GetUsers()
        {
            return _context.Users.ToList();
        }

        public User GetUser(int userId)
        {
            return _context.Users.Where(u => u.Id == userId).FirstOrDefault();
        }

        public bool UserExist(int userId)
        {
            return _context.Users.Any(u => u.Id == userId);
        }

        public bool CreateUser(User user)
        {
            _context.Add(user);
            return Save();
        }
        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateUser(User user)
        {
            _context.Users.Update(user);
            return Save();
        }

        public bool DeleteUser(User user)
        {
            _context.Users.Remove(user);
            return Save();
        }

        public User GetUserByUsername(string username)
        {
            return _context.Users.FirstOrDefault(u => u.Username == username);
        }

        public int GetUserIdByUsername(string username)
        {
            var user = _context.Users.SingleOrDefault(u => u.Username == username);
            return user != null ? user.Id : 0;
        }

        public IEnumerable<Booking> GetUserBookings(int userId)
        {
            return _context.Bookings
                           .Include(b => b.Campsite) 
                           .Where(b => b.UserId == userId)
                           .ToList();
        }

        public IEnumerable<Campsite> GetCampsitesByOwner(int ownerId)
        {
            return _context.Campsites
                           .Include(c => c.CampsiteAmenities)
                           .ThenInclude(ca => ca.Amenity)
                           .Where(c => c.OwnerId == ownerId)
                           .ToList();
        }


    }
}

