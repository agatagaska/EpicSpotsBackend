using System;
using Microsoft.AspNetCore.Mvc;
using EpicSpots.Interfaces;
using EpicSpots.Models;
using AutoMapper;
using EpicSpots.DTO;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using EpicSpots.Repository;

namespace EpicSpots.Controllers
{
	[Route("api/[controller]")]
	[ApiController]

	public class BookingController : Controller
	{
		private readonly IBookingRepository _bookingRepository;
		private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

		public BookingController(IBookingRepository bookingRepository, IMapper mapper, IUserRepository userRepository)
		{
			_bookingRepository = bookingRepository;
			_mapper = mapper;
            _userRepository = userRepository;
		}

		[HttpGet]
		[ProducesResponseType(200, Type = typeof(IEnumerable<Booking>))]
        [ProducesResponseType(400)]
        public IActionResult GetBookings()
		{
			var bookings = _mapper.Map<List<BookingDTO>>(_bookingRepository.GetBookings());
			 
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			return Ok(bookings);
		}

		[HttpGet("{bookingId}")]
		[ProducesResponseType(200, Type = typeof(Booking))]
        [ProducesResponseType(400)]
        public IActionResult GetBooking(int bookingId)
		{
			if (!_bookingRepository.BookingExist(bookingId))
				return NotFound();

            var booking = _mapper.Map<BookingDTO>(_bookingRepository.GetBooking(bookingId));

			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			return Ok(booking);
		}

        [Authorize]
        [HttpPost]
        public IActionResult CreateBooking([FromBody] BookingCreateDTO bookingCreate)
        {
            if (bookingCreate == null)
                return BadRequest(ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var claims = User.Claims.Select(c => new { c.Type, c.Value }).ToList();

            // Extract userId from token claims
            var userIdClaim = User.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier && int.TryParse(c.Value, out _));

            if (userIdClaim == null)
            {
                Console.WriteLine("Valid User ID not found in claims");
                return Unauthorized("User ID not found");
            }

            if (!int.TryParse(userIdClaim.Value, out int userId))
            {
                Console.WriteLine($"Invalid User ID format: {userIdClaim.Value}");
                return BadRequest("Invalid User ID format");
            }

            Console.WriteLine($"Extracted User ID: {userId}");

            // Assign the extracted userId to the booking object
            var booking = _mapper.Map<Booking>(bookingCreate);
            booking.UserId = userId;

            if (!_bookingRepository.IsCampsiteAvailable(booking.CampsiteId, booking.StartDate, booking.EndDate))
            {
                ModelState.AddModelError("", "Campsite is already booked for the selected dates");
                return Conflict(ModelState);
            }

            if (!_bookingRepository.CreateBooking(booking))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return CreatedAtAction(nameof(GetBooking), new { bookingId = booking.Id }, booking);
        }


        [Authorize]
        [HttpGet("user/{userId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<BookingDTO>))]
        [ProducesResponseType(400)]
        public IActionResult GetUserBookings(int userId)
        {
            if (!_userRepository.UserExist(userId))
            {
                return NotFound("User not found");
            }

            var bookings = _mapper.Map<List<BookingDTO>>(_bookingRepository.GetUserBookings(userId));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(bookings);
        }


        [Authorize]
        [HttpDelete("{bookingId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult DeleteBooking(int bookingId)
        {
            if (!_bookingRepository.BookingExist(bookingId))
            {
                return NotFound();
            }

            var bookingToDelete = _bookingRepository.GetBooking(bookingId);
            if (!_bookingRepository.DeleteBooking(bookingToDelete))
            {
                ModelState.AddModelError("", $"Something went wrong while deleting booking {bookingId}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

    }
}

