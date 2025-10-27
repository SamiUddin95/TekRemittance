using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using TekRemittance.Repository.Entities;
using TekRemittance.Repository.Models.dto;
using TekRemittance.Service.Interfaces;
using TekRemittance.Web.Models;
using TekRemittance.Web.Models.dto;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace TekRemittance.Web.Controllers
{
    [EnableCors("AllowFrontend")]
    [ApiController]
    [Route("api/[controller]")]
    public class AcquisitionAgentAccountController : ControllerBase
    {
        private readonly IAcquisitionAgentAccountService _service;

        public AcquisitionAgentAccountController(IAcquisitionAgentAccountService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var result = await _service.GetAllAccounts(pageNumber, pageSize);
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

        [HttpGet("byname/{name}")]
       
        public async Task<IActionResult> GetByName(string name)
        {
            try
            {
                var account = await _service.GetAccountByName(name);
                if (account == null)
                    return NotFound(ApiResponse<string>.Error("Account not found", 404));

                return Ok(ApiResponse<object>.Success(account, 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }



        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AcquisitionAgentAccountDTO model)
        {
            try
            {
                if (model == null)
                    return BadRequest(ApiResponse<string>.Error("Invalid request data."));

                var created = await _service.CreateAccount(model);
                return Ok(ApiResponse<object>.Success(created, 201));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }

        [HttpPut("updatebyname")]
        public async Task<IActionResult> UpdateByName([FromBody] AcquisitionAgentAccountDTO model)
        {
            try
            {
                if (model == null || string.IsNullOrWhiteSpace(model.AgentAccountName))
                    return BadRequest(ApiResponse<string>.Error("AgentAccountName is required."));

                var updated = await _service.UpdateAccount(model);

                if (updated == null)
                    return NotFound(ApiResponse<string>.Error("Record not found for the given AgentAccountName."));

                return Ok(ApiResponse<object>.Success(updated, 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }



        [HttpDelete("deletebyname/{agentAccountName}")]
        public async Task<IActionResult> DeleteByName(string agentAccountName)
        {
            var deleted = await _service.DeleteAccount(agentAccountName);

            if (!deleted)
                return NotFound(ApiResponse<string>.Error("Record not found for the given AgentAccountName."));

            return Ok(ApiResponse<string>.Success($"Record '{agentAccountName}' deleted successfully.", 200));
        }



    }
}
