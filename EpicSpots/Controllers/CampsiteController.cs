using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EpicSpots.Interfaces;
using EpicSpots.Models;
using AutoMapper;
using EpicSpots.DTO;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using EpicSpots.Data;
using Microsoft.AspNetCore.Http;

namespace EpicSpots.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CampsiteController : Controller
    {
        private readonly ICampsiteRepository _campsiteRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly CampingDataContext _context;


        public CampsiteController(ICampsiteRepository campsiteRepository, IMapper mapper, IConfiguration configuration, IUserRepository userRepository, CampingDataContext context)
        {
            _campsiteRepository = campsiteRepository;
            _mapper = mapper;
            _configuration = configuration;
            _userRepository = userRepository;
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<CampsiteDTO>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetCampsites()
        {
            var campsiteDTOs = await _campsiteRepository.GetCampsitesWithBase64ImagesAsync();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(campsiteDTOs);
        }

        [HttpGet("{campId}")]
        [ProducesResponseType(200, Type = typeof(CampsiteDTO))]
        [ProducesResponseType(400)]
        public IActionResult GetCampsite(int campId)
        {
            if (!_campsiteRepository.CampsiteExist(campId))
                return NotFound();

            var campsite = _campsiteRepository.GetCampsite(campId);
            var campsiteDTO = _mapper.Map<CampsiteDTO>(campsite);
            campsiteDTO.Amenities = _campsiteRepository.GetCampsiteAmenities(campId);

            if (campsite.Images != null)
            {
                campsiteDTO.ImageBase64 = Convert.ToBase64String(campsite.Images);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(campsiteDTO);
        }

        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> CreateCampsite([FromBody] CampsiteCreateDTO campsiteCreateDTO)
        {
            if (campsiteCreateDTO == null)
                return BadRequest(ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier && int.TryParse(c.Value, out _));
            if (userIdClaim == null)
            {
                return Unauthorized("User ID not found");
            }

            if (!int.TryParse(userIdClaim.Value, out int userId))
            {
                return BadRequest("Invalid User ID format");
            }

            foreach (var amenityId in campsiteCreateDTO.Amenities)
            {
                if (!await _context.Amenities.AnyAsync(a => a.Id == amenityId))
                {
                    ModelState.AddModelError("", $"Amenity with ID {amenityId} does not exist.");
                    return BadRequest(ModelState);
                }
            }


            byte[] fileBytes;
            try
            {
                fileBytes = Convert.FromBase64String(campsiteCreateDTO.ImageBase64);
            }
            catch (FormatException)
            {
                return BadRequest("Invalid image format");
            }

            var campsite = _mapper.Map<Campsite>(campsiteCreateDTO);
            campsite.Images = fileBytes;

            var result = await _campsiteRepository.CreateCampsiteAsync(userId, campsiteCreateDTO.Amenities, campsite);

            if (!result)
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return CreatedAtAction(nameof(GetCampsite), new { campId = campsite.Id }, campsite);
        }

        [Authorize]
        [HttpDelete("{campId}")]
        public async Task<IActionResult> DeleteCampsite(int campId)
        {
            if (!_campsiteRepository.CampsiteExist(campId))
            {
                return NotFound();
            }

            var result = await _campsiteRepository.DeleteCampsiteAsync(campId);

            if (!result)
            {
                ModelState.AddModelError("", "Something went wrong while deleting");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }


        [HttpGet("search")]
        public async Task<IActionResult> SearchCampsites([FromQuery] string? location, [FromQuery] DateTime? checkin, [FromQuery] DateTime? checkout, [FromQuery] decimal? maxPrice)
        {
            var campsites = await _campsiteRepository.SearchCampsitesWithBase64ImagesAsync(location, checkin, checkout, maxPrice);
            return Ok(campsites);
        }

    }
}
