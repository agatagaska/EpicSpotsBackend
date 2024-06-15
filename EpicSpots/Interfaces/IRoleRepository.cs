using System;
using Mysqlx.Crud;
using System.Diagnostics.Metrics;
using EpicSpots.Models;

namespace EpicSpots.Interfaces
{
	public interface IRoleRepository
	{
        Role GetRole(int id);

        Role GetRoleByUser(int userId);

        bool RoleExists(int id);
    }
}

