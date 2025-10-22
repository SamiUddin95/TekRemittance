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
    public class AgentFileTemplateFieldsController : ControllerBase
    {
        private readonly IAgentFileTemplateFieldService _service;
        public AgentFileTemplateFieldsController(IAgentFileTemplateFieldService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetByTemplateId([FromQuery] Guid templateId)
        {
            try
            {
                var items = await _service.GetByTemplateIdAsync(templateId);
                return Ok(ApiResponse<object>.Success(items, 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] agentFileTemplateFieldDTO dto)
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
        public async Task<IActionResult> Update([FromBody] agentFileTemplateFieldDTO dto)
        {
            try
            {
                var updated = await _service.UpdateAsync(dto);
                if (updated == null) return NotFound(ApiResponse<string>.Error("Field not found", 404));
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

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var ok = await _service.DeleteAsync(id);
                if (!ok) return NotFound(ApiResponse<string>.Error("Field not found", 404));
                return Ok(ApiResponse<string>.Success("Field deleted", 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }
    }
}
