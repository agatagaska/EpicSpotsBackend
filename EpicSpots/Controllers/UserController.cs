using System;
using Microsoft.AspNetCore.Mvc;
using EpicSpots.Interfaces;
using EpicSpots.Models;
using AutoMapper;
using EpicSpots.DTO;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Text.RegularExpressions;
using EpicSpots.Repository;

namespace EpicSpots.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;
        private readonly ICampsiteRepository _campsiteRepository;

        public UserController(IUserRepository userRepository, IRoleRepository roleRepository, IMapper mapper, ICampsiteRepository campsiteRepository)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _roleRepository = roleRepository;
            _campsiteRepository = campsiteRepository;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<UserDTO>))]
        public IActionResult GetUsers()
        {
            var users = _mapper.Map<List<UserDTO>>(_userRepository.GetUsers());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(users);
        }

        [HttpGet("{userId}")]
        [ProducesResponseType(200, Type = typeof(User))]
        [ProducesResponseType(400)]
        public IActionResult GetUser(int userId)
        {
            if (!_userRepository.UserExist(userId))
                return NotFound();

            var user = _mapper.Map<UserDTO>(_userRepository.GetUser(userId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(user);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateUser([FromQuery] int roleId, [FromBody] UserDTO userCreate)
        {
            if (userCreate == null)
                return BadRequest(ModelState);

            var users = _userRepository.GetUsers()
                .Where(u => u.Username.Trim().ToLower() == userCreate.Username.TrimEnd().ToLower())
                .FirstOrDefault();

            if (users != null)
            {
                ModelState.AddModelError("", $"User {users} already exists");
                return StatusCode(422, ModelState);
            }

            var existingEmail = _userRepository.GetUsers()
                .Where(u => u.Email.Trim().ToLower() == userCreate.Email.Trim().ToLower())
                .FirstOrDefault();

            if (existingEmail != null)
            {
                ModelState.AddModelError("", $"Email {userCreate.Email} is already in use");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userMap = _mapper.Map<User>(userCreate);

            userMap.Role = _roleRepository.GetRole(roleId);

            if (!_userRepository.CreateUser(userMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok($"Successfully created user {userMap}");
        }

        [Authorize]
        [HttpPut("{userId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdateUser(int userId, [FromBody] UpdateUserDTO userUpdate)
        {
            if (userUpdate == null || userId != userUpdate.Id)
            {
                return BadRequest(ModelState);
            }

            if (!_userRepository.UserExist(userId))
            {
                return NotFound();
            }

            if (!Regex.IsMatch(userUpdate.PhoneNumber.ToString(), @"^\d{9}$"))
            {
                ModelState.AddModelError("PhoneNumber", "Phone number must be 9 digits.");
                return BadRequest(ModelState);
            }

            var user = _userRepository.GetUser(userId);

            // update user properties if they have new values
            user.Username = !string.IsNullOrWhiteSpace(userUpdate.Username) ? userUpdate.Username : user.Username;
            user.Email = !string.IsNullOrWhiteSpace(userUpdate.Email) ? userUpdate.Email : user.Email;
            user.FirstName = !string.IsNullOrWhiteSpace(userUpdate.FirstName) ? userUpdate.FirstName : user.FirstName;
            user.LastName = !string.IsNullOrWhiteSpace(userUpdate.LastName) ? userUpdate.LastName : user.LastName;
            user.PhoneNumber = userUpdate.PhoneNumber != 0 ? userUpdate.PhoneNumber : user.PhoneNumber;

            if (!_userRepository.UpdateUser(user))
            {
                ModelState.AddModelError("", "Something went wrong updating the user");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [Authorize]
        [HttpPut("{userId}/password")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdateUserPassword(int userId, [FromBody] UpdatePasswordDTO passwordUpdate)
        {
            if (passwordUpdate == null)
            {
                return BadRequest(ModelState);
            }

            if (!_userRepository.UserExist(userId))
            {
                return NotFound();
            }

            var user = _userRepository.GetUser(userId);

            // Verify current password
            if (user.PasswordHash != passwordUpdate.CurrentPassword) // For practice, we are comparing plain text
            {
                ModelState.AddModelError("CurrentPassword", "Current password is incorrect.");
                return BadRequest(ModelState);
            }
            user.PasswordHash = passwordUpdate.NewPassword; // Update to new password

            if (!_userRepository.UpdateUser(user))
            {
                ModelState.AddModelError("", "Something went wrong updating the user");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [Authorize]
        [HttpDelete("{userId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult DeleteUser(int userId)
        {
            if (!_userRepository.UserExist(userId))
            {
                return NotFound();
            }

            var user = _userRepository.GetUser(userId);

            if (!_userRepository.DeleteUser(user))
            {
                ModelState.AddModelError("", "Something went wrong deleting the user");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [Authorize]
        [HttpGet("{userId}/bookings")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<BookingDTO>))]
        [ProducesResponseType(400)]
        public IActionResult GetUserBookings(int userId)
        {
            if (!_userRepository.UserExist(userId))
            {
                return NotFound("User not found");
            }

            var bookings = _mapper.Map<List<BookingDTO>>(_userRepository.GetUserBookings(userId));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(bookings);
        }

        [Authorize]
        [HttpGet("{userId}/campsites")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<CampsiteDTO>))]
        [ProducesResponseType(400)]
        public async Task<ActionResult> GetOwnerCampsites(int userId)
        {
            if (!_userRepository.UserExist(userId))
            {
                return NotFound("User not found");
            }

            var campsiteDTOs = await _campsiteRepository.GetCampsitesByOwnerWithBase64ImagesAsync(userId);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(campsiteDTOs);
        }
    }
}
