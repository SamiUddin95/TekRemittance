using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TekRemittance.Service.Interfaces;
using TekRemittance.Web.Models;
using TekRemittance.Web.Models.dto;

namespace TekRemittance.Web.Controllers
{
    [EnableCors("AllowFrontend")]
    [ApiController]
    [Route("api/[controller]")]
    public class AgentFileTemplatesController : ControllerBase
    {
        private readonly IAgentFileTemplateService _service;
        public AgentFileTemplatesController(IAgentFileTemplateService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var result = await _service.GetAllAsync(pageNumber, pageSize);
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

        [HttpGet("{agentId:guid}")]
        public async Task<IActionResult> GetByAgentId(Guid agentId)
        {
            try
            {
                var result = await _service.GetByAgentIdAsync(agentId);
                if (result == null) return NotFound(ApiResponse<string>.Error("Template not found", 404));
                return Ok(ApiResponse<object>.Success(result, 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] agentFileTemplateDTO dto)
        {
            try
            {
                var created = await _service.CreateAsync(dto);
                return Ok(ApiResponse<object>.Success(created, 201));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ApiResponse<string>.Error(ex.Message, 400));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] agentFileTemplateDTO dto)
        {
            try
            {
                var updated = await _service.UpdateAsync(dto);
                if (updated == null) return NotFound(ApiResponse<string>.Error("Template not found", 404));
                return Ok(ApiResponse<object>.Success(updated, 200));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ApiResponse<string>.Error(ex.Message, 400));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }

        [HttpDelete("{agentId:guid}")]
        public async Task<IActionResult> Delete(Guid agentId)
        {
            try
            {
                var ok = await _service.DeleteByAgentIdAsync(agentId);
                if (!ok) return NotFound(ApiResponse<string>.Error("Template not found", 404));
                return Ok(ApiResponse<string>.Success("Template deleted", 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }
    }
}
