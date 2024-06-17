using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EpicSpots.DTO;
using EpicSpots.Models;

namespace EpicSpots.Interfaces
{
    public interface ICampsiteRepository
    {
        Campsite GetCampsite(int id);

        bool CampsiteExist(int campId);

        string GetCampsiteAmenities(int campId);

        Task<bool> CreateCampsiteAsync(int ownerId, List<int> amenityIds, Campsite campsite);

        Task<bool> DeleteCampsiteAsync(int campId);

        Task<IEnumerable<CampsiteDTO>> SearchCampsitesWithBase64ImagesAsync(string? location, DateTime? checkin, DateTime? checkout, decimal? maxPrice);

        Task<IEnumerable<CampsiteDTO>> GetCampsitesWithBase64ImagesAsync();

        Task<IEnumerable<CampsiteDTO>> GetCampsitesByOwnerWithBase64ImagesAsync(int ownerId);
    }
}
