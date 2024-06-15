using System;
using EpicSpots.Models;
using Microsoft.EntityFrameworkCore;

namespace EpicSpots.Data
{
	public class CampingDataContext : DbContext
	{
		public CampingDataContext(DbContextOptions<CampingDataContext> options): base(options)
		{

		}

        public DbSet<Amenity> Amenities { get; set; }

		public DbSet<CampsiteAmenity> CampsiteAmenities { get; set; }

		public DbSet<Booking> Bookings { get; set; }

		public DbSet<Campsite> Campsites { get; set; }

		public DbSet<User> Users { get; set; }

		public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
            // campsite amenities
			modelBuilder.Entity<CampsiteAmenity>()
				.HasKey(ac => new { ac.CampsiteId, ac.AmenityId });

            modelBuilder.Entity<CampsiteAmenity>()
                .HasOne(ac => ac.Campsite)
                .WithMany(c => c.CampsiteAmenities)
                .HasForeignKey(ac => ac.CampsiteId);

            modelBuilder.Entity<CampsiteAmenity>()
               .HasOne(ac => ac.Amenity)
               .WithMany(a => a.CampsiteAmenities)
               .HasForeignKey(ac => ac.AmenityId);

            // user role
            modelBuilder.Entity<User>()
                .HasOne(ur => ur.Role)
				.WithMany(u => u.Users)
                .HasForeignKey(r => r.RoleId);

            // campsite owner
            modelBuilder.Entity<Campsite>()
                 .HasOne(c => c.Owner)
                 .WithMany(u => u.Campsites)
                 .HasForeignKey(c => c.OwnerId);

            // campsite - bookings
            modelBuilder.Entity<Campsite>()
                    .HasMany(c => c.Bookings)
                    .WithOne(b => b.Campsite)
                    .HasForeignKey(b => b.CampsiteId);


            base.OnModelCreating(modelBuilder);

        }

    }
}

