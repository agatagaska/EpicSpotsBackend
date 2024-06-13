using System;
using Mysqlx.Crud;
using System.Diagnostics.Metrics;
using EpicSpots.Models;

namespace EpicSpots.Interfaces
{
	public interface IRoleRepository
	{
        ICollection<Role> GetRoles();

        Role GetRole(int id);

        Role GetRoleByUser(int userId);

        ICollection<User> GetUsersFromARole(int roleId);

        bool RoleExists(int id);

        bool CreateRole(Role role);

        bool UpdateRole(Role role);

        bool DeleteRole(Role role);

        bool Save();
    }
}

