using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EpicSpots.Data;
using EpicSpots.DTO;
using EpicSpots.Interfaces;
using EpicSpots.Models;
using Microsoft.EntityFrameworkCore;

namespace EpicSpots.Repository
{
    public class CampsiteRepository : ICampsiteRepository
    {
        private readonly CampingDataContext _context;

        public CampsiteRepository(CampingDataContext context)
        {
            _context = context;
        }

        public Campsite GetCampsite(int id)
        {
            return _context.Campsites.FirstOrDefault(p => p.Id == id);
        }

        public Campsite GetCampsite(string name)
        {
            return _context.Campsites.FirstOrDefault(p => p.Name == name);
        }

        public decimal GetCampsitePricing(int campId)
        {
            return _context.Campsites.Where(p => p.Id == campId).Select(p => p.PricePerNight).FirstOrDefault();
        }

        public IEnumerable<Campsite> GetCampsitesByOwner(int ownerId)
        {
            return _context.Campsites
                           .Where(c => c.OwnerId == ownerId)
                           .Include(c => c.CampsiteAmenities) // Include related entities if necessary
                           .ThenInclude(ca => ca.Amenity)
                           .ToList();
        }

        public decimal GetCampsiteRating(int campId)
        {
            var review = _context.Reviews.Where(p => p.CampsiteId == campId);

            if (!review.Any())
                return 0;

            return Math.Round((decimal)review.Sum(r => r.Rating) / review.Count(), 2);
        }

        public ICollection<Campsite> GetCampsites()
        {
            return _context.Campsites.OrderBy(p => p.Id).ToList();
        }
        public bool CampsiteExist(int campId)
        {
            return _context.Campsites.Any(p => p.Id == campId);
        }

        public string GetCampsiteAmenities(int campId)
        {
            var amenityNames = _context.CampsiteAmenities
                .Where(ca => ca.CampsiteId == campId)
                .Include(ca => ca.Amenity)
                .Select(ca => ca.Amenity.Name)
                .ToList();

            return amenityNames.Any() ? string.Join(", ", amenityNames) : "No amenities";
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0;
        }

        public Campsite GetCampsiteTrimToUpper(CampsiteDTO campsiteCreate)
        {
            return GetCampsites().FirstOrDefault(c => c.Name.Trim().ToUpper() == campsiteCreate.Name.TrimEnd().ToUpper());
        }

        public Campsite GetCampsiteTrimToUpper(CampsiteCreateDTO campsiteCreate)
        {
            return GetCampsites().FirstOrDefault(c => c.Name.Trim().ToUpper() == campsiteCreate.Name.TrimEnd().ToUpper());
        }

        public async Task<bool> CreateCampsiteAsync(int ownerId, List<int> amenitiesIds, Campsite campsite)
        {
            foreach (var amenityId in amenitiesIds)
            {
                if (!await _context.Amenities.AnyAsync(a => a.Id == amenityId))
                {
                    return false;
                }
            }

            campsite.OwnerId = ownerId;
            campsite.CampsiteAmenities = amenitiesIds.Select(id => new CampsiteAmenity
            {
                AmenityId = id,
                Campsite = campsite
            }).ToList();

            _context.Campsites.Add(campsite);
            return await _context.SaveChangesAsync() > 0;
        }


        public async Task<byte[]> GetCampsiteImageAsync(int campId)
        {
            var campsite = await _context.Campsites.FindAsync(campId);
            return campsite?.Images;
        }

        private async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() >= 0;
        }

        public async Task<IEnumerable<CampsiteDTO>> GetCampsitesWithBase64ImagesAsync()
        {
            var campsites = await _context.Campsites.ToListAsync();
            return campsites.Select(campsite => new CampsiteDTO
            {
                Id = campsite.Id,
                Name = campsite.Name,
                Location = campsite.Location,
                Description = campsite.Description,
                PricePerNight = campsite.PricePerNight,
                AverageRating = Math.Round(GetCampsiteRating(campsite.Id), 2),
                ImageBase64 = campsite.Images != null ? Convert.ToBase64String(campsite.Images) : null,
                Amenities = GetCampsiteAmenities(campsite.Id)
            }).ToList();
        }

        public async Task<IEnumerable<CampsiteDTO>> GetCampsitesByOwnerWithBase64ImagesAsync(int ownerId)
        {
            var campsites = await _context.Campsites
                .Where(c => c.OwnerId == ownerId)
                .Include(c => c.CampsiteAmenities)
                    .ThenInclude(ca => ca.Amenity)
                .ToListAsync();

            return campsites.Select(campsite => new CampsiteDTO
            {
                Id = campsite.Id,
                Name = campsite.Name,
                Location = campsite.Location,
                Description = campsite.Description,
                PricePerNight = campsite.PricePerNight,
                AverageRating = Math.Round(GetCampsiteRating(campsite.Id), 2),
                ImageBase64 = campsite.Images != null ? Convert.ToBase64String(campsite.Images) : null,
                Amenities = GetCampsiteAmenities(campsite.Id)
            }).ToList();
        }


        public async Task<IEnumerable<CampsiteDTO>> SearchCampsitesWithBase64ImagesAsync(string? location, DateTime? checkin, DateTime? checkout, decimal? maxPrice)
        {
            var query = _context.Campsites.AsQueryable();

            if (!string.IsNullOrEmpty(location))
            {
                query = query.Where(c => c.Location.Contains(location, StringComparison.OrdinalIgnoreCase));
            }

            if (checkin.HasValue && checkout.HasValue)
            {
                var checkinDate = checkin.Value.Date;
                var checkoutDate = checkout.Value.Date;

                var overlappingBookings = _context.Bookings
                    .Where(b => b.StartDate < checkoutDate && b.EndDate > checkinDate);

                var bookedCampsiteIds = overlappingBookings
                    .Select(b => b.CampsiteId)
                    .Distinct()
                    .ToList();

                Console.WriteLine("Booked Campsite IDs: " + string.Join(", ", bookedCampsiteIds));

                query = query.Where(c => !bookedCampsiteIds.Contains(c.Id));
            }

            if (maxPrice.HasValue)
            {
                query = query.Where(c => c.PricePerNight <= maxPrice.Value);
            }

            var campsites = await query
                .Include(c => c.CampsiteAmenities)
                .ToListAsync();

            return campsites.Select(campsite => new CampsiteDTO
            {
                Id = campsite.Id,
                Name = campsite.Name,
                Location = campsite.Location,
                Description = campsite.Description,
                PricePerNight = campsite.PricePerNight,
                AverageRating = Math.Round(GetCampsiteRating(campsite.Id), 2),
                ImageBase64 = campsite.Images != null ? Convert.ToBase64String(campsite.Images) : null,
                Amenities = GetCampsiteAmenities(campsite.Id)
            }).ToList();
        }


        public async Task<bool> UploadCampsiteImageAsync(int campId, string base64Image)
        {
            var campsite = await _context.Campsites.FindAsync(campId);
            if (campsite == null) return false;

            // Data URL Handling (if you expect Data URLs)
            if (base64Image.StartsWith("data:"))
            {
                int commaIndex = base64Image.IndexOf(',');
                base64Image = base64Image.Substring(commaIndex + 1);
            }

            campsite.Images = Convert.FromBase64String(base64Image);
            _context.Campsites.Update(campsite);
            return await SaveAsync();
        }

        public bool AddCampsite(Campsite campsite)
        {
            _context.Campsites.Add(campsite);
            return Save();
        }
    }
}

