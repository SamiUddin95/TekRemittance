using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TekRemittance.Web.Models;
using TekRemittance.Service.Interfaces;


namespace TekRemittance.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DisbursementController : ControllerBase
    {
        private readonly IDisbursementService _service;

        public DisbursementController(IDisbursementService service)
        {
            _service = service;
        }

        [HttpGet("{agentId:guid}")]
        public async Task<IActionResult> GetDisbursementData(Guid agentId, int pageNumber = 1, int pageSize = 50)
        {
            try
            {
                var result = await _service.GetDataByAgentIdAsync(agentId, pageNumber, pageSize);

                if (result == null || !result.Items.Any())
                    return NotFound(ApiResponse<string>.Error("No data found for this agent", 404));

                return Ok(ApiResponse<object>.Success(new
                {
                    items = result.Items,
                    totalCount = result.TotalCount,
                    pageNumber = result.PageNumber,
                    pageSize = result.PageSize,
                    totalPages = result.TotalPages
                }, 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }
        [HttpGet("GetDataByAgent/{agentId:guid}")]
        public async Task<IActionResult> GetByAgentIdP(Guid agentId, int pageNumber = 1, int pageSize = 50)
        {
            try
            {
                var result = await _service.GetByAgentIdWithStatusPAsync(agentId, pageNumber, pageSize);

                if (result == null || !result.Items.Any())
                    return NotFound(ApiResponse<string>.Error("No remittance info found for this AgentId With Status P.", 404));

                return Ok(ApiResponse<object>.Success(new
                {
                    items = result.Items,
                    totalCount = result.TotalCount,
                    pageNumber = result.PageNumber,
                    pageSize = result.PageSize,
                    totalPages = result.TotalPages
                }, 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }
        [HttpGet("GetDataByAuthorize/{agentId:guid}")]
        public async Task<IActionResult> GetDataByAgentId(Guid agentId, int pageNumber = 1, int pageSize = 50)
        {
            try
            {
                var data = await _service.GetByAgentIdWithStatusUAsync(agentId, pageNumber , pageSize);
                if (data == null || !data.Items.Any())
                    return NotFound(ApiResponse<string>.Error("No remittance info found for this AgentId With Status U.", 404));
                return Ok(ApiResponse<object>.Success(new
                {
                    items = data.Items,
                    totalCount = data.TotalCount,
                    pageNumber = data.PageNumber,
                    pageSize = data.PageSize,
                    totalPages = data.TotalPages
                }, 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }

        }
        [HttpGet("GetDataByReject/{agentId:guid}")]
        public async Task<IActionResult> GetDataREByAgentId(Guid agentId, int pageNumber = 1, int pageSize = 50)
        {
            try
            {
                var data = await _service.GetByAgentIdWithStatusREAsync(agentId, pageNumber, pageSize);
                if (data == null || !data.Items.Any())
                    return NotFound(ApiResponse<string>.Error("No remittance info found for this AgentId With Status RE.", 404));
                return Ok(ApiResponse<object>.Success(new
                {
                    items = data.Items,
                    totalCount = data.TotalCount,
                    pageNumber = data.PageNumber,
                    pageSize = data.PageSize,
                    totalPages = data.TotalPages
                }, 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }

        }
        [HttpGet("GetDataByRepair/{agentId:guid}")]
        public async Task<IActionResult> GetDataRByAgentId(Guid agentId, int pageNumber = 1, int pageSize = 50)
        {
            try
            {
                var data = await _service.GetByAgentIdWithStatusRAsync(agentId, pageNumber, pageSize);

                if (data == null || !data.Items.Any())
                    return NotFound(ApiResponse<string>.Error("No remittance info found for this AgentId with status R.", 404));

                return Ok(ApiResponse<object>.Success(new
                {
                    items = data.Items,
                    totalCount = data.TotalCount,
                    pageNumber = data.PageNumber,
                    pageSize = data.PageSize,
                    totalPages = data.TotalPages
                }, 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }
    }
}
