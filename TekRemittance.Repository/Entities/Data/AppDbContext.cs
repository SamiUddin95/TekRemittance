using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TekRemittance.Repository.Entities;

namespace TekRemittance.Repository.Entities.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Country> Countries { get; set; }
        public DbSet<Province> Provinces { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Bank> Banks { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<RevokedToken> RevokedTokens { get; set; }

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

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);

                entity.Property(u => u.Name)
                      .IsRequired()
                      .HasMaxLength(150);

                entity.Property(u => u.Email)
                      .HasMaxLength(200);

                entity.Property(u => u.Phone)
                      .HasMaxLength(30);

                entity.Property(u => u.EmployeeId)
                      .HasMaxLength(50);

                entity.Property(u => u.Limit)
                      .HasColumnType("decimal(18,2)");

                entity.Property(u => u.LoginName)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(u => u.PasswordHash)
                      .IsRequired()
                      .HasMaxLength(256);

                entity.Property(u => u.IsActive)
                      .HasDefaultValue(true);

                entity.Property(u => u.CreatedBy)
                      .HasMaxLength(100);

                entity.Property(u => u.UpdatedBy)
                      .HasMaxLength(100);

                entity.Property(u => u.CreatedOn)
                      .HasDefaultValueSql("GETUTCDATE()");

                entity.HasIndex(u => u.LoginName)
                      .IsUnique();

                entity.HasIndex(u => u.Email)
                      .IsUnique()
                      .HasFilter("[Email] IS NOT NULL");
            });

            modelBuilder.Entity<RevokedToken>(entity =>
            {
                entity.HasKey(r => r.Id);
                entity.Property(r => r.Jti)
                      .IsRequired()
                      .HasMaxLength(100);
                entity.HasIndex(r => r.Jti).IsUnique();
                entity.Property(r => r.RevokedAt).IsRequired();
                entity.Property(r => r.ExpiresAt).IsRequired();
            });
        }
    }
}
