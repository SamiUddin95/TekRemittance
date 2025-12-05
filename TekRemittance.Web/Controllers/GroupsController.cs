using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TekRemittance.Service.Interfaces;
using TekRemittance.Web.Models;
using TekRemittance.Web.Models.dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TekRemittance.Web.Controllers
{
    [EnableCors("AllowFrontend")]
    [ApiController]
    [Route("api/[controller]")]
    public class GroupsController : ControllerBase
    {
        private readonly IGroupService _service;

        public GroupsController(IGroupService service)
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
                var group = await _service.GetByIdAsync(id);
                if (group == null) return NotFound(ApiResponse<string>.Error("Group not found", 404));
                return Ok(ApiResponse<object>.Success(group, 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] GroupDTO dto)
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

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update([FromBody] GroupDTO dto)
        {
            try
            {
                dto.UpdatedBy = User.FindFirst("unique_name")?.Value ?? User.FindFirst("name")?.Value ?? "system";
                var updated = await _service.UpdateAsync(dto);
                if (updated == null) return NotFound(ApiResponse<string>.Error("Group not found", 404));
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
                if (!ok) return NotFound(ApiResponse<string>.Error("Group not found", 404));
                return Ok(ApiResponse<string>.Success("Group deleted successfully", 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }

        [HttpPut("{id:guid}/users")]
        public async Task<IActionResult> SetUsers(Guid id, [FromBody] List<Guid> userIds)
        {
            try
            {
                var ok = await _service.SetUsersAsync(id, userIds);
                if (!ok) return NotFound(ApiResponse<string>.Error("Group not found", 404));
                return Ok(ApiResponse<string>.Success("Users assigned to group successfully", 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }

        [HttpPut("{id:guid}/permissions")]
        public async Task<IActionResult> SetPermissions(Guid id, [FromBody] List<Guid> permissionIds)
        {
            try
            {
                var ok = await _service.SetPermissionsAsync(id, permissionIds);
                if (!ok) return NotFound(ApiResponse<string>.Error("Group not found", 404));
                return Ok(ApiResponse<string>.Success("Permissions assigned to group successfully", 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }

        [HttpGet("{id:guid}/users")]
        public async Task<IActionResult> GetUsers(Guid id)
        {
            try
            {
                var userIds = await _service.GetUsersByGroupIdAsync(id);
                return Ok(ApiResponse<object>.Success(userIds, 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }

        [HttpGet("{id:guid}/permissions")]
        public async Task<IActionResult> GetPermissions(Guid id)
        {
            try
            {
                var permissionIds = await _service.GetPermissionsByGroupIdAsync(id);
                return Ok(ApiResponse<object>.Success(permissionIds, 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }
    }
}
