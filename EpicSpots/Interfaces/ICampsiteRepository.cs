using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EpicSpots.DTO;
using EpicSpots.Models;

namespace EpicSpots.Interfaces
{
    public interface ICampsiteRepository
    {
        ICollection<Campsite> GetCampsites();

        Campsite GetCampsite(int id);

        Campsite GetCampsite(string name);

        decimal GetCampsiteRating(int campId);

        decimal GetCampsitePricing(int campId);

        bool CampsiteExist(int campId);

        string GetCampsiteAmenities(int campId);

        Campsite GetCampsiteTrimToUpper(CampsiteDTO campsiteCreate);

        IEnumerable<Campsite> GetCampsitesByOwner(int ownerId);

        Campsite GetCampsiteTrimToUpper(CampsiteCreateDTO campsiteCreate);

        Task<bool> CreateCampsiteAsync(int ownerId, List<int> amenityIds, Campsite campsite);

        bool AddCampsite(Campsite campsite);

        bool Save();

        Task<IEnumerable<CampsiteDTO>> SearchCampsitesWithBase64ImagesAsync(string? location, DateTime? checkin, DateTime? checkout, decimal? maxPrice);

        // Image handling
        Task<byte[]> GetCampsiteImageAsync(int campId);

        Task<IEnumerable<CampsiteDTO>> GetCampsitesWithBase64ImagesAsync();

        Task<IEnumerable<CampsiteDTO>> GetCampsitesByOwnerWithBase64ImagesAsync(int ownerId);
    }
}
