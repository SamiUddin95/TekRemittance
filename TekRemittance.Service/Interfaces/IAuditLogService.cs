using System;
using System.Threading.Tasks;
using TekRemittance.Repository.Entities;
using TekRemittance.Repository.Models.dto;
using TekRemittance.Web.Models.dto;

namespace TekRemittance.Service.Interfaces
{
    public interface IAuditLogService
    {
        Task AddAsync(AuditLog log);
        Task<PagedResult<AuditLog>> QueryAsync(
            string? entityName,
            Guid? entityId,
            string? action,
            string? performedBy,
            DateTime? from,
            DateTime? to,
            int pageNumber = 1,
            int pageSize = 10);

        Task<PagedResult<AuditLogDTO>> GetAllAuditLogs(int pageNumber = 1, int pageSize = 10, string? action = null, string? performedby = null, string? entityName = null);


    }
}
