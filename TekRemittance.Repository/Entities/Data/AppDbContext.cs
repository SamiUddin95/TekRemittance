using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using TekRemittance.Repository.Entities;
using TekRemittance.Repository.Models.dto;

namespace TekRemittance.Repository.Entities.Data
{
    public class AppDbContext : DbContext
    {
        private readonly IHttpContextAccessor? _httpContextAccessor;

        public AppDbContext(DbContextOptions<AppDbContext> options, IHttpContextAccessor? httpContextAccessor = null) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public DbSet<Country> Countries { get; set; }
        public DbSet<Province> Provinces { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Bank> Banks { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<RevokedToken> RevokedTokens { get; set; }
        public DbSet<AcquisitionAgents> AcquisitionAgents { get; set; }
        public DbSet<AgentFileTemplate> AgentFileTemplates { get; set; }
        public DbSet<AgentFileTemplateField> AgentFileTemplateFields { get; set; }
        public DbSet<AgentFileUpload> AgentFileUploads { get; set; }
        public DbSet<AgentAccount> AgentAccounts { get; set; }
        public DbSet<RemittanceInfo> RemittanceInfos { get; set; }
        public DbSet<Branches> Branches { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<PasswordPolicy> PasswordPolicy { get; set; } 
        public DbSet<ClearingStatus> ClearingStatus { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<UserGroup> UserGroups { get; set; }
        public DbSet<GroupPermission> GroupPermissions { get; set; }
        public DbSet<barGraphDto> barGraphDtos { get; set; } = null!;
        public DbSet<TransactionDetail> TransactionDetail { get; set; }
        public DbSet<Channels> Channels { get; set; }
        public DbSet<EPRC> EPRC { get; set; }
        public DbSet<AmlData> AmlData { get; set; }





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

            modelBuilder.Entity<AgentFileTemplate>(entity =>
            {
                entity.HasKey(t => t.Id);
                entity.Property(t => t.Name).IsRequired().HasMaxLength(150);
                entity.Property(t => t.SheetName).HasMaxLength(100);
                entity.Property(t => t.Delimiter).HasMaxLength(10);
                entity.Property(t => t.CreatedBy).HasMaxLength(100);
                entity.Property(t => t.UpdatedBy).HasMaxLength(100);

                // One template per agent
                entity.HasIndex(t => t.AgentId).IsUnique();
                entity.HasOne(t => t.Agent)
                      .WithOne()
                      .HasForeignKey<AgentFileTemplate>(t => t.AgentId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Store enums as strings
                entity.Property(t => t.Format).HasConversion<string>().HasMaxLength(20);
            });

            modelBuilder.Entity<AgentFileTemplateField>(entity =>
            {
                entity.HasKey(f => f.Id);
                entity.Property(f => f.FieldName).IsRequired().HasMaxLength(100);
                entity.Property(f => f.StartIndex);
                entity.Property(f => f.Length);

                entity.HasOne(f => f.Template)
                      .WithMany()
                      .HasForeignKey(f => f.TemplateId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Unique order per template
                entity.HasIndex(f => new { f.TemplateId, f.FieldOrder }).IsUnique();

                // Store enums as strings
                entity.Property(f => f.FieldType).HasConversion<string>().HasMaxLength(20);
            });

            modelBuilder.Entity<AgentFileUpload>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.FileName).IsRequired().HasMaxLength(260);
                entity.Property(u => u.StoragePath).HasMaxLength(500);
                entity.Property(u => u.ErrorMessage).HasMaxLength(1000);

                entity.HasOne(u => u.Agent)
                      .WithMany()
                      .HasForeignKey(u => u.AgentId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(u => u.Template)
                      .WithMany()
                      .HasForeignKey(u => u.TemplateId)
                      .OnDelete(DeleteBehavior.Restrict);

                // Store enums as strings
                entity.Property(u => u.Status).HasConversion<string>().HasMaxLength(20);
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
                      .HasMaxLength(20);

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

            modelBuilder.Entity<AcquisitionAgents>(entity =>
            {
                entity.HasKey(a => a.Id);
                entity.Property(a => a.Code).IsRequired().HasMaxLength(50);
                entity.HasIndex(a => a.Code).IsUnique();
                entity.Property(a => a.AgentName).IsRequired().HasMaxLength(150);
                entity.Property(a => a.Phone1).HasMaxLength(30);
                entity.Property(a => a.Phone2).HasMaxLength(30);
                entity.Property(a => a.Fax).HasMaxLength(30);
                entity.Property(a => a.Email).HasMaxLength(200);
                entity.Property(a => a.LogoUrl).HasMaxLength(400);
                entity.Property(a => a.Address).HasMaxLength(500);
                entity.Property(a => a.InquiryURL).HasMaxLength(400);
                entity.Property(a => a.PaymentURL).HasMaxLength(400);
                entity.Property(a => a.UnlockURL).HasMaxLength(400);
                entity.Property(a => a.CreatedBy).HasMaxLength(100);
                entity.Property(a => a.UpdatedBy).HasMaxLength(100);

                // Store enums as strings
                entity.Property(a => a.RIN)
                      .HasConversion<string>()
                      .HasMaxLength(50);
                entity.Property(a => a.Process)
                      .HasConversion<string>()
                      .HasMaxLength(50);
                entity.Property(a => a.AcquisitionModes)
                      .HasConversion<string>()
                      .HasMaxLength(200);
                entity.Property(a => a.DisbursementModes)
                      .HasConversion<string>()
                      .HasMaxLength(200);

                entity.HasOne(a => a.Country)
                      .WithMany()
                      .HasForeignKey(a => a.CountryId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(a => a.Province)
                      .WithMany()
                      .HasForeignKey(a => a.ProvinceId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(a => a.City)
                      .WithMany()
                      .HasForeignKey(a => a.CityId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.Property(a => a.IsActive).HasDefaultValue(true);
                entity.Property(a => a.IsDeleted).HasDefaultValue(false);
      
                entity.Property(a => a.CreatedOn).HasDefaultValueSql("GETUTCDATE()");
            }); 

            modelBuilder.Entity<AgentAccount>(entity =>
            {
                entity.HasKey(a => a.Id);
                entity.Property(a => a.AccountNumber);
                entity.HasOne(a => a.AcquisitionAgents)
                      .WithMany()
                      .HasForeignKey(a => a.AgentId)
                      .OnDelete(DeleteBehavior.Restrict);
                entity.Property(a => a.Approve);
                entity.Property(a => a.AccountTitle).HasMaxLength(150);
                entity.Property(a => a.AccountType).HasMaxLength(50);
                entity.Property(a => a.IsActive).HasDefaultValue(true);
                entity.Property(b => b.CreatedBy)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(b => b.UpdatedBy)
                      .HasMaxLength(100);

                entity.Property(b => b.CreatedOn)
                      .HasDefaultValueSql("GETUTCDATE()");

                entity.Property(b => b.UpdatedOn);
            });

            modelBuilder.Entity<RemittanceInfo>(entity =>
            {
                entity.HasKey(r => r.Id);
                entity.Property(r => r.DataJson).IsRequired();
                entity.Property(r => r.Error).HasMaxLength(1000);
                entity.Property(r => r.CreatedOn).HasDefaultValueSql("GETUTCDATE()");
                entity.HasIndex(r => r.AgentId);
                entity.HasIndex(r => r.UploadId);
                entity.HasIndex(r => r.TemplateId);
                entity.HasIndex(r => r.AccountNumber);
                entity.HasIndex(r => r.AccountTitle);
                entity.HasIndex(r => r.Xpin);
                entity.HasIndex(r => r.LimitType);
                entity.HasIndex(r => r.Date);
                entity.HasIndex(r => r.DataJson);

                entity.Property(a => a.ModeOfTransaction)
                      .HasConversion<string>()
                      .HasMaxLength(200);
            });

            modelBuilder.Entity<Branches>(entity =>
            {
                entity.HasKey(b => b.Id);

                entity.Property(b => b.Code)
                      .IsRequired()
                      .HasMaxLength(50);

                entity.HasIndex(b => b.Code)
                      .IsUnique();

                entity.Property(b => b.AgentBranchName)
                      .IsRequired()
                      .HasMaxLength(150);

                entity.Property(b => b.Phone1)
                      .HasMaxLength(30);

                entity.Property(b => b.Phone2)
                      .HasMaxLength(30);

                entity.Property(b => b.Fax)
                      .HasMaxLength(30);

                entity.Property(b => b.Email)
                      .HasMaxLength(200);

                entity.Property(b => b.Address)
                      .HasMaxLength(500);

                entity.Property(b => b.CreatedBy)
                      .HasMaxLength(100);

                entity.Property(b => b.UpdatedBy)
                      .HasMaxLength(100);

                //entity.Property(b => b.AcquisitionModes)
                //      .HasConversion<string>()
                //      .HasMaxLength(200);

                //entity.Property(b => b.DisbursementModes)
                //      .HasConversion<string>()
                //      .HasMaxLength(200);

                entity.HasOne(b => b.Agent)
                      .WithMany()
                      .HasForeignKey(b => b.AgentId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(b => b.Country)
                      .WithMany()
                      .HasForeignKey(b => b.CountryId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(b => b.Province)
                      .WithMany()
                      .HasForeignKey(b => b.ProvinceId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(b => b.City)
                      .WithMany()
                      .HasForeignKey(b => b.CityId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.Property(b => b.CreatedOn)
                      .HasDefaultValueSql("GETUTCDATE()");
            });

            modelBuilder.Entity<AuditLog>(entity =>
            {
                entity.HasKey(a => a.Id);
                entity.Property(a => a.EntityName)
                      .IsRequired()
                      .HasMaxLength(100);
                entity.Property(a => a.Action)
                      .IsRequired()
                      .HasMaxLength(20);
                entity.Property(a => a.OldValues);
                entity.Property(a => a.NewValues);
                entity.Property(a => a.PerformedBy)
                      .HasMaxLength(100);
                entity.Property(a => a.PerformedOn)
                      .HasDefaultValueSql("GETUTCDATE()");
                entity.HasIndex(a => new { a.EntityName, a.EntityId });
            });
            modelBuilder.Entity<Group>(entity =>
            {
                entity.HasKey(g => g.Id);

                entity.Property(g => g.Name)
                      .IsRequired()
                      .HasMaxLength(150);

                entity.Property(g => g.Description)
                      .HasMaxLength(500);

                entity.Property(g => g.IsActive)
                      .HasDefaultValue(true);

                entity.Property(g => g.CreatedBy)
                      .HasMaxLength(100);

                entity.Property(g => g.UpdatedBy)
                      .HasMaxLength(100);

                entity.Property(g => g.CreatedOn)
                      .HasDefaultValueSql("GETUTCDATE()");

                entity.HasIndex(g => g.Name)
                      .IsUnique();
            });

            modelBuilder.Entity<Permission>(entity =>
            {
                entity.HasKey(p => p.Id);

                entity.Property(p => p.Name)
                      .IsRequired()
                      .HasMaxLength(150);

                entity.Property(p => p.Description)
                      .HasMaxLength(500);

                entity.Property(p => p.IsActive)
                      .HasDefaultValue(true);

                entity.Property(p => p.CreatedBy)
                      .HasMaxLength(100);

                entity.Property(p => p.UpdatedBy)
                      .HasMaxLength(100);

                entity.Property(p => p.CreatedOn)
                      .HasDefaultValueSql("GETUTCDATE()");

                entity.HasIndex(p => p.Name)
                      .IsUnique();
            });

            modelBuilder.Entity<UserGroup>(entity =>
            {
                entity.HasKey(ug => ug.Id);

                entity.HasOne(ug => ug.User)
                      .WithMany()
                      .HasForeignKey(ug => ug.UserId)
                      .OnDelete(DeleteBehavior.Cascade);    

                entity.HasOne(ug => ug.Group)
                      .WithMany(g => g.UserGroups)
                      .HasForeignKey(ug => ug.GroupId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.Property(ug => ug.CreatedBy)
                      .HasMaxLength(100);

                entity.Property(ug => ug.CreatedOn)
                      .HasDefaultValueSql("GETUTCDATE()");

                entity.HasIndex(ug => new { ug.UserId, ug.GroupId })
                      .IsUnique();
            });
            modelBuilder.Entity<AmlData>(entity =>
            {
                entity.HasKey(a => a.Id);

                entity.Property(a => a.CNIC)
                      .HasMaxLength(15);

                entity.Property(a => a.AccountName)
                      .HasMaxLength(200);

                entity.Property(a => a.Address)
                      .HasMaxLength(500);

                entity.Property(a => a.CreatedBy)
                      .HasMaxLength(100);

                entity.Property(a => a.CreatedOn)
                      .HasDefaultValueSql("GETUTCDATE()");

                entity.Property(a => a.UpdatedBy)
                      .HasMaxLength(100);

                entity.Property(a => a.UpdatedOn);
            });

                modelBuilder.Entity<GroupPermission>(entity =>
            {
                entity.HasKey(gp => gp.Id);

                entity.HasOne(gp => gp.Group)
                      .WithMany(g => g.GroupPermissions)
                      .HasForeignKey(gp => gp.GroupId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(gp => gp.Permission)
                      .WithMany(p => p.GroupPermissions)
                      .HasForeignKey(gp => gp.PermissionId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.Property(gp => gp.CreatedBy)
                      .HasMaxLength(100);

                entity.Property(gp => gp.CreatedOn)
                      .HasDefaultValueSql("GETUTCDATE()");

                entity.HasIndex(gp => new { gp.GroupId, gp.PermissionId })
                      .IsUnique();
            });
            modelBuilder.Entity<barGraphDto>().HasNoKey();

            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            AddAuditLogs();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            AddAuditLogs();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void AddAuditLogs()
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is not AuditLog &&
                            (e.State == EntityState.Added || e.State == EntityState.Modified || e.State == EntityState.Deleted))
                .ToList();

            if (entries.Count == 0) return;

            var performedBy = GetCurrentUserName();
            foreach (var entry in entries)
            {
                var entityType = entry.Entity.GetType();
                var entityName = entityType.Name;
                var idProp = entityType.GetProperty("Id");
                var entityId = idProp != null && idProp.PropertyType == typeof(Guid)
                    ? (Guid)(idProp.GetValue(entry.Entity) ?? Guid.Empty)
                    : Guid.Empty;

                string? oldValues = null;
                string? newValues = null;

                if (entry.State == EntityState.Added)
                {
                    var currentDict = entry.CurrentValues.Properties.ToDictionary(p => p.Name, p => entry.CurrentValues[p.Name]);
                    newValues = JsonSerializer.Serialize(currentDict);
                }
                else if (entry.State == EntityState.Modified)
                {
                    var originalDict = entry.OriginalValues.Properties.ToDictionary(p => p.Name, p => entry.OriginalValues[p.Name]);
                    var currentDict = entry.CurrentValues.Properties.ToDictionary(p => p.Name, p => entry.CurrentValues[p.Name]);
                    oldValues = JsonSerializer.Serialize(originalDict);
                    newValues = JsonSerializer.Serialize(currentDict);
                }
                else if (entry.State == EntityState.Deleted)
                {
                    var originalDict = entry.OriginalValues.Properties.ToDictionary(p => p.Name, p => entry.OriginalValues[p.Name]);
                    oldValues = JsonSerializer.Serialize(originalDict);
                }

                AuditLogs.Add(new AuditLog
                {
                    Id = Guid.NewGuid(),
                    EntityName = entityName,
                    EntityId = entityId,
                    Action = entry.State.ToString(),
                    OldValues = oldValues ?? "{}",
                    NewValues = newValues ?? "{}",
                    PerformedBy = performedBy,
                    PerformedOn = DateTime.UtcNow
                });
            }
        }

        private string GetCurrentUserName()
        {
            try
            {
                var user = _httpContextAccessor?.HttpContext?.User;
                if (user == null || !user.Identity?.IsAuthenticated == true)
                    return "system";
                return user.Identity?.Name
                       ?? user.FindFirst("name")?.Value
                       ?? user.FindFirst("unique_name")?.Value
                       ?? "system";
            }
            catch
            {
                return "system";
            }
        }
    }
}
