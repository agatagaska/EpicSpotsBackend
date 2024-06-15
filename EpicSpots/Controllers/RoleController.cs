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
    }

}