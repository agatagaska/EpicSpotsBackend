using System;
using System.Diagnostics.Metrics;
using AutoMapper;
using EpicSpots.Data;
using EpicSpots.Interfaces;
using EpicSpots.Models;

namespace EpicSpots.Repository
{
	public class RoleRepository : IRoleRepository
	{
        private readonly CampingDataContext _context;
        private readonly IMapper _mapper;

        public RoleRepository(CampingDataContext context, IMapper mapper)
		{
            _context = context;
            _mapper = mapper;
        }

        public bool CreateRole(Role role)
        {
            _context.Add(role);
            return Save();
        }

        public bool DeleteRole(Role role)
        {
            _context.Remove(role);
            return Save();
        }

        public Role GetRole(int id)
        {
            return _context.Roles.Where(r => r.Id == id).FirstOrDefault();
        }

        public Role GetRoleByUser(int userId)
        {
            return _context.Users.Where(u => u.Id ==userId).Select(r => r.Role).FirstOrDefault();
        }

        public ICollection<Role> GetRoles()
        {
            return _context.Roles.ToList();
        }

        public ICollection<User> GetUsersFromARole(int roleId)
        {
            throw new NotImplementedException();
        }

        public bool RoleExists(int id)
        {
            return _context.Roles.Any(r => r.Id == id);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateRole(Role role)
        {
            _context.Update(role);
            return Save();
        }
    }
}

