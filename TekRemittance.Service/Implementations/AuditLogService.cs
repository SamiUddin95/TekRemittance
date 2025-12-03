using System.Threading.Tasks;
using TekRemittance.Repository.Entities;
using TekRemittance.Repository.Interfaces;
using TekRemittance.Service.Interfaces;
using TekRemittance.Web.Models.dto;
using System;
using TekRemittance.Repository.Models.dto;

namespace TekRemittance.Service.Implementations
{
    public class AuditLogService : IAuditLogService
    {
        private readonly IAuditLogRepository _repository;
        public AuditLogService(IAuditLogRepository repository)
        {
            _repository = repository;
        }
        public async Task AddAsync(AuditLog log)
        {
            await _repository.AddAsync(log);
        }

        public Task<PagedResult<AuditLog>> QueryAsync(string? entityName, Guid? entityId, string? action, string? performedBy, DateTime? from, DateTime? to, int pageNumber = 1, int pageSize = 10)
        {
            return _repository.QueryAsync(entityName, entityId, action, performedBy, from, to, pageNumber, pageSize);
        }

        public async Task<PagedResult<AuditLogDTO>> GetAllAuditLogs(int pageNumber = 1, int pageSize = 10, string? action = null, string? performedby = null, string? entityName = null)
        {
            return await _repository.GetAllAuditLogsAsync(pageNumber, pageSize,action,performedby,entityName);
        }
    }
}
