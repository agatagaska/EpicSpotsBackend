using System;
using Microsoft.AspNetCore.Mvc;
using EpicSpots.Interfaces;
using EpicSpots.Models;
using AutoMapper;
using EpicSpots.DTO;
using EpicSpots.Repository;
using System.Diagnostics.Metrics;

namespace EpicSpots.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class RoleController : Controller
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;

        public RoleController(IRoleRepository roleRepository, IMapper mapper)
        {
            _roleRepository = roleRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Role>))]
        public IActionResult GetCountries()
        {
            var roles = _mapper.Map<List<RoleDTO>>(_roleRepository.GetRoles());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(roles);
        }

        [HttpGet("{roleId}")]
        [ProducesResponseType(200, Type = typeof(Role))]
        [ProducesResponseType(400)]
        public IActionResult GetRole(int roleId)
        {
            if (!_roleRepository.RoleExists(roleId))
                return NotFound();

            var role = _mapper.Map<RoleDTO>(_roleRepository.GetRole(roleId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(role);
        }

        [HttpGet("/roles/{roleId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200, Type = typeof(Role))]
        public IActionResult GetRoleOfAnUser(int userId)
        {
            var role = _mapper.Map<RoleDTO>(
                _roleRepository.GetRoleByUser(userId));

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(role);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateRole([FromBody] RoleDTO roleCreate)
        {
            if (roleCreate == null)
                return BadRequest(ModelState);

            var role = _roleRepository.GetRoles()
                .Where(r => r.RoleName.Trim().ToUpper() == roleCreate.RoleName.TrimEnd().ToUpper())
                .FirstOrDefault();

            if (role != null)
            {
                ModelState.AddModelError("", $"Role {role} already exists!");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var roleMap = _mapper.Map<Role>(roleCreate);

            if (!_roleRepository.CreateRole(roleMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok($"Successfully created role {role}");
        }

        [HttpPut("{roleId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateRole(int roleId, [FromBody] RoleDTO updatedRole)
        {
            if (updatedRole == null)
                return BadRequest(ModelState);

            if (roleId != updatedRole.Id)
                return BadRequest(ModelState);

            if (!_roleRepository.RoleExists(roleId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var roleMap = _mapper.Map<Role>(updatedRole);

            if (!_roleRepository.UpdateRole(roleMap))
            {
                ModelState.AddModelError("", "Something went wrong updating role");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{roleId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteRole(int roleId)
        {
            if (!_roleRepository.RoleExists(roleId))
            {
                return NotFound();
            }

            var roleToDelete = _roleRepository.GetRole(roleId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_roleRepository.DeleteRole(roleToDelete))
            {
                ModelState.AddModelError("", $"Something went wrong deleting role {roleToDelete}");
            }

            return NoContent();
        }
    }

}