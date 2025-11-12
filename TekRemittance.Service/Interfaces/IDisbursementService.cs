using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TekRemittance.Repository.Entities;
using TekRemittance.Repository.Models.dto;
using TekRemittance.Web.Models.dto;

namespace TekRemittance.Service.Interfaces
{
    public interface IDisbursementService
    {
        Task<PagedResult<KeyValuePair<string, List<string>>>> GetDataByAgentIdAsync(Guid agentId, int pageNumber = 1, int pageSize = 10);
        Task<PagedResult<RemitttanceInfosStatusDTO>> GetByAgentIdWithStatusPAsync(Guid agentId, int pageNumber = 1, int pageSize = 10);
        Task<PagedResult<RemitttanceInfosStatusDTO>> GetByAgentIdWithStatusUAsync(Guid agentId, int pageNumber = 1, int pageSize = 10);
        Task<PagedResult<RemitttanceInfosStatusDTO>> GetByAgentIdWithStatusREAsync(Guid agentId, int pageNumber = 1, int pageSize = 10);
        Task<PagedResult<RemitttanceInfosStatusDTO>> GetByAgentIdWithStatusRAsync(Guid agentId, int pageNumber = 1, int pageSize = 10);
        Task<PagedResult<RemitttanceInfosStatusDTO>> GetByAgentIdWithStatusAAsync(Guid agentId, int pageNumber = 1, int pageSize = 10);
        Task<(bool isSuccess, string message,string Xpin)> RemitApproveAsync(string xpin, Guid? userId);

        Task<RemittanceInfoModelDTO> RemitRejectAsync(string xpin, Guid? userId);

        Task<RemittanceInfoModelDTO> RemitAuthorizeAsync(string xpin, Guid? userId);
        Task<RemittanceInfoModelDTO> RemitRepairAsync(string xpin, Guid? userId);

        Task<RemittanceInfoModelDTO> RemitReverseAsync(string xpin, Guid? userId);



    }

}
