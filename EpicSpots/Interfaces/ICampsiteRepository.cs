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

        bool UpdateCampsite(int ownerId, List<int> amenitiesIds, Campsite campsite);

        bool DeleteCampsite(Campsite campsite);

        bool Save();

        Task<IEnumerable<CampsiteDTO>> SearchCampsitesWithBase64ImagesAsync(string? location, DateTime? checkin, DateTime? checkout);

        // Image handling
        Task<byte[]> GetCampsiteImageAsync(int campId);

        Task<bool> UpdateCampsiteImageAsync(int campId, string base64Image);

        Task<bool> DeleteCampsiteImageAsync(int campId);

        Task<IEnumerable<CampsiteDTO>> GetCampsitesWithBase64ImagesAsync();
    }
}
