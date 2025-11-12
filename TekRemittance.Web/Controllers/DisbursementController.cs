using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TekRemittance.Web.Models;
using TekRemittance.Service.Interfaces;
using Microsoft.AspNetCore.Cors;
using TekRemittance.Repository.Models.dto;
using Microsoft.AspNetCore.Http.HttpResults;


namespace TekRemittance.Web.Controllers
{
    [EnableCors("AllowFrontend")]
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
        public async Task<IActionResult> GetDisbursementData(Guid agentId, int pageNumber = 1, int pageSize = 10)
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
      
        [HttpGet("GetDataByAgent")]
        public async Task<IActionResult> GetByAgentIdP(Guid agentId, int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var result = await _service.GetByAgentIdWithStatusPAsync(agentId, pageNumber, pageSize);

                return Ok(ApiResponse<object>.Success(new
                {
                    items = result.Items ,
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
        public async Task<IActionResult> GetDataByAgentId(Guid agentId, int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var data = await _service.GetByAgentIdWithStatusUAsync(agentId, pageNumber, pageSize);

                return Ok(ApiResponse<object>.Success(new
                {
                    items = data.Items ,
                    totalCount = data.TotalCount ,
                    pageNumber = data.PageNumber ,
                    pageSize = data.PageSize ,
                    totalPages = data.TotalPages 
                }, 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }

        [HttpGet("GetDataByReject/{agentId:guid}")]
        public async Task<IActionResult> GetDataREByAgentId(Guid agentId, int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var data = await _service.GetByAgentIdWithStatusREAsync(agentId, pageNumber, pageSize);

                return Ok(ApiResponse<object>.Success(new
                {
                    items = data.Items ,
                    totalCount = data.TotalCount ,
                    pageNumber = data.PageNumber,
                    pageSize = data.PageSize ,
                    totalPages = data.TotalPages 
                }, 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }

        [HttpGet("GetDataByRepair/{agentId:guid}")]
        public async Task<IActionResult> GetDataRByAgentId(Guid agentId, int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var data = await _service.GetByAgentIdWithStatusRAsync(agentId, pageNumber, pageSize);

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

        [HttpGet("GetDataByApproved/{agentId:guid}")]
        public async Task<IActionResult> GetDataAByAgentId(Guid agentId, int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var data = await _service.GetByAgentIdWithStatusAAsync(agentId, pageNumber, pageSize);

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
        [HttpPost("RemitApprove")]
        public async Task<IActionResult> Approve([FromBody] RemittanceInfoModelDTO dto)
        {
            try
            {
                if (dto == null)
                    return BadRequest(ApiResponse<string>.Error("Request body cannot be null", 400));

                var result = await _service.RemitApproveAsync(dto.Xpin, dto.UserId);
                return Ok(new { isSuccess = result.isSuccess, message= result.message, Xpin=result.Xpin });
            }

            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }
        [HttpPost("RemitReject")]
        public async Task<IActionResult> Reject([FromBody] RemittanceInfoModelDTO dto)
        {
            try
            {
                if (dto == null)
                    return BadRequest(ApiResponse<string>.Error("Request body cannot be null", 400));

                var result = await _service.RemitRejectAsync(dto.Xpin, dto.UserId);

                return Ok(ApiResponse<RemittanceInfoModelDTO>.Success(result, 200));
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ApiResponse<string>.Error(ex.Message, 400));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponse<string>.Error(ex.Message, 400));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }

        [HttpPost("RemitAuthorize")]
        public async Task<IActionResult> Authorize([FromBody] RemittanceInfoModelDTO dto)
        {
            try
            {
                if (dto == null)
                    return BadRequest(ApiResponse<string>.Error("Request body cannot be null", 400));

                var result = await _service.RemitAuthorizeAsync(dto.Xpin, dto.UserId);

                return Ok(ApiResponse<RemittanceInfoModelDTO>.Success(result, 200));
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ApiResponse<string>.Error(ex.Message, 400));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponse<string>.Error(ex.Message, 400));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }

        [HttpPost("RemitRepair")]
        public async Task<IActionResult> Repair([FromBody] RemittanceInfoModelDTO dto)
        {
            try
            {
                if (dto == null)
                    return BadRequest(ApiResponse<string>.Error("Request body cannot be null", 400));

                var result = await _service.RemitRepairAsync(dto.Xpin, dto.UserId);

                return Ok(ApiResponse<RemittanceInfoModelDTO>.Success(result, 200));
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ApiResponse<string>.Error(ex.Message, 400));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponse<string>.Error(ex.Message, 400));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }

        [HttpPost("RemitReverse")]
        public async Task<IActionResult> Reverse([FromBody] RemittanceInfoModelDTO dto)
        {
            try
            {
                if (dto == null)
                    return BadRequest(ApiResponse<string>.Error("Request body cannot be null", 400));

                var result = await _service.RemitReverseAsync(dto.Xpin, dto.UserId);

                return Ok(ApiResponse<RemittanceInfoModelDTO>.Success(result, 200));
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ApiResponse<string>.Error(ex.Message, 400));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponse<string>.Error(ex.Message, 400));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }


    }
}
