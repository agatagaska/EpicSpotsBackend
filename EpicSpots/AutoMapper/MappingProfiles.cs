using System;
using AutoMapper;
using EpicSpots.DTO;
using EpicSpots.Models;
using Microsoft.EntityFrameworkCore;

namespace EpicSpots.AutoMapper
{
	public class MappingProfiles : Profile
	{
		public MappingProfiles()
		{
			CreateMap<Campsite, CampsiteDTO>();
            CreateMap<CampsiteDTO, Campsite>();

            CreateMap<Amenity, AmenityDTO>();
            CreateMap<AmenityDTO, Amenity>();

            CreateMap<Campsite, CampsiteDTO>()
                .ForMember(dest => dest.Amenities, opt => opt.Ignore())
                .ForMember(dest => dest.ImageBase64, opt => opt.MapFrom(src => src.Images != null ? Convert.ToBase64String(src.Images) : null));

            CreateMap<CampsiteDTO, Campsite>()
                .ForMember(dest => dest.Images, opt => opt.MapFrom(src => !string.IsNullOrEmpty(src.ImageBase64) ? Convert.FromBase64String(src.ImageBase64) : null));

            CreateMap<Booking, BookingDTO>();
            CreateMap<BookingDTO, Booking>();

            CreateMap<User, UserDTO>();
            CreateMap<UserDTO, User>();

            // Mapping for retrieving bookings with campsite details
            CreateMap<Booking, BookingDTO>()
                .ForMember(dest => dest.Campsite, opt => opt.MapFrom(src => src.Campsite));
            CreateMap<Campsite, CampsiteDTO>();

            // Mapping for creating/updating bookings
            CreateMap<BookingCreateDTO, Booking>();

            // Mapping for creating/updating campsites
            CreateMap<CampsiteCreateDTO, Campsite>()
                .ForMember(dest => dest.Images, opt => opt.MapFrom(src => !string.IsNullOrEmpty(src.ImageBase64) ? Convert.FromBase64String(src.ImageBase64) : null));
        }
    }
}

