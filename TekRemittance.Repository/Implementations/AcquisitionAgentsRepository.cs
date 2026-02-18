using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using TekRemittance.Repository.Entities;
using TekRemittance.Repository.Entities.Data;
using TekRemittance.Repository.Enums;
using TekRemittance.Repository.Interfaces;
using TekRemittance.Web.Models.dto;
using static System.Collections.Specialized.BitVector32;

namespace TekRemittance.Repository.Implementations
{
    public class AcquisitionAgentsRepository : IAcquisitionAgentsRepository
    {
        private readonly AppDbContext _context;
        public AcquisitionAgentsRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<acquisitionAgentDTO>> GetAllAsync(int pageNumber = 1, int pageSize = 10, string? code = null, string? agentname = null, StatusesEnums? status = null)
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;

            var query = _context.AcquisitionAgents.AsNoTracking().Where(a => a.IsDeleted == false);

            if (!string.IsNullOrWhiteSpace(code))
                query = query.Where(a => a.Code.Contains(code.Trim()));
            if (!string.IsNullOrWhiteSpace(agentname))
                query = query.Where(a => a.AgentName.Contains(agentname.Trim()));
            if (status == StatusesEnums.Active)
                query = query.Where(x => x.IsActive == true);
            if (status == StatusesEnums.Inactive)
                query = query.Where(x => x.IsActive == false);


            var totalCount = await query.CountAsync();

            var items = await query
                .OrderByDescending(a => a.UpdatedOn??a.CreatedOn)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(a => new acquisitionAgentDTO
                {
                    Id = a.Id,
                    Code = a.Code,
                    AgentName = a.AgentName,
                    Phone1 = a.Phone1,
                    Phone2 = a.Phone2,
                    Fax = a.Fax,
                    Email = a.Email,
                    LogoUrl = a.LogoUrl,
                    Address = a.Address,
                    CountryId = a.CountryId,
                    ProvinceId = a.ProvinceId,
                    CityId = a.CityId,
                    CutOffTimeStart = a.CutOffTimeStart,
                    CutOffTimeEnd = a.CutOffTimeEnd,
                    RIN = a.RIN,
                    Process = a.Process,
                    AcquisitionModes = a.AcquisitionModes,
                    DisbursementModes = a.DisbursementModes,
                    DirectIntegration = a.DirectIntegration,
                    IsActive = a.IsActive,
                    InquiryURL = a.InquiryURL,
                    PaymentURL = a.PaymentURL,
                    UnlockURL = a.UnlockURL,
                    CreatedBy = a.CreatedBy,
                    CreatedOn = a.CreatedOn,
                    UpdatedBy = a.UpdatedBy,
                    UpdatedOn = a.UpdatedOn
                })
                .ToListAsync();

            return new PagedResult<acquisitionAgentDTO>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<acquisitionAgentDTO?> GetByIdAsync(Guid id)
        {
            return await _context.AcquisitionAgents.AsNoTracking()
                .Where(a => a.Id == id)
                .Select(a => new acquisitionAgentDTO
                {
                    Id = a.Id,
                    Code = a.Code,
                    AgentName = a.AgentName,
                    Phone1 = a.Phone1,
                    Phone2 = a.Phone2,
                    Fax = a.Fax,
                    Email = a.Email,
                    LogoUrl = a.LogoUrl,
                    Address = a.Address,
                    CountryId = a.CountryId,
                    ProvinceId = a.ProvinceId,
                    CityId = a.CityId,
                    CutOffTimeStart = a.CutOffTimeStart,
                    CutOffTimeEnd = a.CutOffTimeEnd,
                    RIN = a.RIN,
                    Process = a.Process,
                    AcquisitionModes = a.AcquisitionModes,
                    DisbursementModes = a.DisbursementModes,
                    DirectIntegration = a.DirectIntegration,
                    IsActive = a.IsActive,
                    InquiryURL = a.InquiryURL,
                    PaymentURL = a.PaymentURL,
                    UnlockURL = a.UnlockURL,
                    CreatedBy = a.CreatedBy,
                    CreatedOn = a.CreatedOn,
                    UpdatedBy = a.UpdatedBy,
                    UpdatedOn = a.UpdatedOn
                })
                .FirstOrDefaultAsync();
        }

        public async Task<acquisitionAgentDTO> AddAsync(acquisitionAgentDTO dto)
        {
            var code = dto.Code?.Trim() ?? string.Empty;
            if (string.IsNullOrEmpty(code)) throw new ArgumentException("Code is required.");
            if (await _context.AcquisitionAgents.AnyAsync(a => a.Code.ToLower() == code.ToLower()))
            {
                throw new ArgumentException("Acquisition Agent code already exists.");
            }
            var agentName = dto.AgentName?.Trim() ?? string.Empty;
            if (string.IsNullOrEmpty(agentName)) throw new ArgumentException("Agent name is required.");
            if (await _context.AcquisitionAgents.AnyAsync(a => a.AgentName.ToLower() == agentName.ToLower()))
            {
                throw new ArgumentException("Acquisition Agent name already exists.");
            }

            var entity = new AcquisitionAgents
            {
                Id = Guid.NewGuid(),
                Code = code,
                AgentName = agentName,
                Phone1 = dto.Phone1,
                Phone2 = dto.Phone2,
                Fax = dto.Fax,
                Email = dto.Email,
                LogoUrl = dto.LogoUrl,
                Address = dto.Address,
                CountryId = dto.CountryId,
                ProvinceId = dto.ProvinceId,
                CityId = dto.CityId,
                CutOffTimeStart = dto.CutOffTimeStart,
                CutOffTimeEnd = dto.CutOffTimeEnd,
                RIN = dto.RIN,
                Process = dto.Process,
                AcquisitionModes = dto.AcquisitionModes,
                DisbursementModes = dto.DisbursementModes,
                DirectIntegration = dto.DirectIntegration,
                IsActive = dto.IsActive,
                InquiryURL = dto.InquiryURL,
                PaymentURL = dto.PaymentURL,
                UnlockURL = dto.UnlockURL,
                CreatedBy = dto.CreatedBy ?? "system",
                CreatedOn = DateTime.UtcNow,
                UpdatedBy = dto.UpdatedBy ?? "system",
                UpdatedOn = DateTime.UtcNow
            };

            await _context.AcquisitionAgents.AddAsync(entity);
            await _context.SaveChangesAsync();

            dto.Id = entity.Id;
            dto.CreatedBy = entity.CreatedBy;
            dto.CreatedOn = entity.CreatedOn;
            dto.UpdatedBy = entity.UpdatedBy;
            dto.UpdatedOn = entity.UpdatedOn;
            dto.Code = entity.Code;
            return dto;
        }

        public async Task<acquisitionAgentDTO?> UpdateAsync(acquisitionAgentDTO dto)
        {
            var existing = await _context.AcquisitionAgents.FirstOrDefaultAsync(a => a.Id == dto.Id);
            if (existing == null) return null;

            var code = dto.Code?.Trim() ?? string.Empty;
            if (string.IsNullOrEmpty(code)) throw new ArgumentException("Code is required.");
            if (await _context.AcquisitionAgents.AnyAsync(a => a.Id != dto.Id && a.Code.ToLower() == code.ToLower()))
            {
                throw new ArgumentException("Acquisition Agent code already exists.");
            }
            var agentName = dto.AgentName?.Trim() ?? string.Empty;
            if (string.IsNullOrEmpty(agentName)) throw new ArgumentException("Agent name is required.");
            if (await _context.AcquisitionAgents.AnyAsync(a => a.Id != dto.Id && a.AgentName.ToLower() == agentName.ToLower()))
            {
                throw new ArgumentException("Acquisition Agent name already exists.");
            }

            existing.Code = code;
            existing.AgentName = agentName;
            existing.Phone1 = dto.Phone1;
            existing.Phone2 = dto.Phone2;
            existing.Fax = dto.Fax;
            existing.Email = dto.Email;
            existing.LogoUrl = dto.LogoUrl;
            existing.Address = dto.Address;
            existing.CountryId = dto.CountryId;
            existing.ProvinceId = dto.ProvinceId;
            existing.CityId = dto.CityId;
            existing.CutOffTimeStart = dto.CutOffTimeStart;
            existing.CutOffTimeEnd = dto.CutOffTimeEnd;
            existing.RIN = dto.RIN;
            existing.Process = dto.Process;
            existing.AcquisitionModes = dto.AcquisitionModes;
            existing.DisbursementModes = dto.DisbursementModes;
            existing.DirectIntegration = dto.DirectIntegration;
            existing.IsActive = dto.IsActive;
            existing.InquiryURL = dto.InquiryURL;
            existing.PaymentURL = dto.PaymentURL;
            existing.UnlockURL = dto.UnlockURL;
            existing.UpdatedBy = dto.UpdatedBy;
            existing.UpdatedOn = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return new acquisitionAgentDTO
            {
                Id = existing.Id,
                Code = existing.Code,
                AgentName = existing.AgentName,
                Phone1 = existing.Phone1,
                Phone2 = existing.Phone2,
                Fax = existing.Fax,
                Email = existing.Email,
                LogoUrl = existing.LogoUrl,
                Address = existing.Address,
                CountryId = existing.CountryId,
                ProvinceId = existing.ProvinceId,
                CityId = existing.CityId,
                CutOffTimeStart = existing.CutOffTimeStart,
                CutOffTimeEnd = existing.CutOffTimeEnd,
                RIN = existing.RIN,
                Process = existing.Process,
                AcquisitionModes = existing.AcquisitionModes,
                DisbursementModes = existing.DisbursementModes,
                DirectIntegration = existing.DirectIntegration,
                IsActive = existing.IsActive,
                InquiryURL = existing.InquiryURL,
                PaymentURL = existing.PaymentURL,
                UnlockURL = existing.UnlockURL,
                CreatedBy = existing.CreatedBy,
                CreatedOn = existing.CreatedOn,
                UpdatedBy = existing.UpdatedBy,
                UpdatedOn = existing.UpdatedOn
            };
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var existing = await _context.AcquisitionAgents.FirstOrDefaultAsync(a => a.Id == id && !a.IsDeleted);

            if (existing == null) return false;
            existing.IsDeleted = true;

            _context.AcquisitionAgents.Update(existing);
            await _context.SaveChangesAsync();
            return true;
        }

      

    }
}
