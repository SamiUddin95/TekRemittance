using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TekRemittance.Service.Interfaces;
using TekRemittance.Web.Models;
using TekRemittance.Web.Models.dto;
using System;
using System.Threading.Tasks;

namespace TekRemittance.Web.Controllers
{
    [EnableCors("AllowFrontend")]
    [ApiController]
    [Route("api/[controller]")]
    public class PermissionsController : ControllerBase
    {
        private readonly IPermissionService _service;

        public PermissionsController(IPermissionService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(int pageNumber = 1, int pageSize = 10, string? name = null)
        {
            try
            {
                var result = await _service.GetAllAsync(pageNumber, pageSize, name);

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

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var permission = await _service.GetByIdAsync(id);
                if (permission == null) return NotFound(ApiResponse<string>.Error("Permission not found", 404));
                return Ok(ApiResponse<object>.Success(permission, 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PermissionDTO dto)
        {
            try
            {
                dto.CreatedBy = User.FindFirst("unique_name")?.Value ?? User.FindFirst("name")?.Value ?? "system";
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
        public async Task<IActionResult> Update([FromBody] PermissionDTO dto)
        {
            try
            {
                dto.UpdatedBy = User.FindFirst("unique_name")?.Value ?? User.FindFirst("name")?.Value ?? "system";
                var updated = await _service.UpdateAsync(dto);
                if (updated == null) return NotFound(ApiResponse<string>.Error("Permission not found", 404));
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
                if (!ok) return NotFound(ApiResponse<string>.Error("Permission not found", 404));
                return Ok(ApiResponse<string>.Success("Permission deleted successfully", 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }
    }
}
