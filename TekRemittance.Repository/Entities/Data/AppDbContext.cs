using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TekRemittance.Repository.Entities.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Country> Countries { get; set; }
        public DbSet<Province> Provinces { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Bank> Banks { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Country>(entity =>
            {
                entity.HasKey(c => c.Id);

                entity.Property(c => c.CountryCode)
                      .IsRequired()
                      .HasMaxLength(10);

                entity.Property(c => c.CountryName)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(c => c.CreatedOn)
                      .HasDefaultValueSql("GETUTCDATE()");
            });

            // Province configuration
            modelBuilder.Entity<Province>(entity =>
            {
                entity.HasKey(p => p.Id);

                entity.Property(p => p.ProvinceCode)
                      .IsRequired()
                      .HasMaxLength(10);

                entity.Property(p => p.ProvinceName)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(p => p.CreatedOn)
                      .HasDefaultValueSql("GETUTCDATE()");

                entity.HasOne(p => p.Country)
                      .WithMany()
                      .HasForeignKey(p => p.CountryId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<City>(entity =>
            {
                entity.HasKey(c => c.Id);

                entity.Property(c => c.CityCode)
                      .IsRequired()
                      .HasMaxLength(10);

                entity.Property(c => c.CityName)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(c => c.CreatedOn)
                      .HasDefaultValueSql("GETUTCDATE()");

                // Relationships
                entity.HasOne(c => c.Country)
                      .WithMany()
                      .HasForeignKey(c => c.CountryId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(c => c.Province)
                      .WithMany()
                      .HasForeignKey(c => c.ProvinceId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
            modelBuilder.Entity<Bank>(entity =>
            {
                entity.HasKey(b => b.Id);

                entity.Property(b => b.BankCode)
                      .IsRequired()
                      .HasMaxLength(20);

                entity.Property(b => b.BankName)
                      .IsRequired()
                      .HasMaxLength(150);

                entity.Property(b => b.IMD)
                      .HasMaxLength(50);

                entity.Property(b => b.Website)
                      .HasMaxLength(200);

                entity.Property(b => b.Allases)
                      .HasMaxLength(100);

                entity.Property(b => b.PhoneNo)
                      .HasColumnType("int");

                entity.Property(b => b.Description)
                      .HasMaxLength(500);

                entity.Property(b => b.IsActive)
                      .HasDefaultValue(true);

                entity.Property(b => b.CreatedBy)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(b => b.UpdatedBy)
                      .HasMaxLength(100);

                entity.Property(b => b.CreatedOn)
                      .HasDefaultValueSql("GETUTCDATE()");

                entity.Property(b => b.UpdatedOn);
            });

        }
    }
}
