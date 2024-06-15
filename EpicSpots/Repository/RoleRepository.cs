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
        public Role GetRole(int id)
        {
            return _context.Roles.Where(r => r.Id == id).FirstOrDefault();
        }

        public Role GetRoleByUser(int userId)
        {
            return _context.Users.Where(u => u.Id ==userId).Select(r => r.Role).FirstOrDefault();
        }

        public bool RoleExists(int id)
        {
            return _context.Roles.Any(r => r.Id == id);
        }

    }
}

