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
        private readonly IDisbursementService  _service;

        public DisbursementController(IDisbursementService service)
        {
            _service = service; 
        }

        [HttpGet("{agentId:guid}")]
        public async Task<IActionResult> GetDisbursementData(Guid agentId, int pageNumber = 1, int pageSize = 50)
        {
            try
            {
                var result = await _service.GetDataByAgentIdAsync( agentId, pageNumber, pageSize);

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

    }
}
