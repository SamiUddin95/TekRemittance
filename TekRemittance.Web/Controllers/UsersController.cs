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
    public class UsersController : ControllerBase
    {
        private readonly IUserService _service;
        public UsersController(IUserService service)
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

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var user = await _service.GetByIdAsync(id);
                if (user == null) return NotFound(ApiResponse<string>.Error("User not found", 404));
                return Ok(ApiResponse<object>.Success(user, 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }

        public class CreateUserRequest
        {
            public userDTO User { get; set; } = new userDTO();
            public string Password { get; set; } = string.Empty;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserRequest req)
        {
            try
            {
                var created = await _service.CreateAsync(req.User, req.Password);
                return Ok(ApiResponse<object>.Success(created, 201));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] userDTO dto)
        {
            try
            {
                var updated = await _service.UpdateAsync(dto);
                if (updated == null) return NotFound(ApiResponse<string>.Error("User not found", 404));
                return Ok(ApiResponse<object>.Success(updated, 200));
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
                if (!ok) return NotFound(ApiResponse<string>.Error("User not found", 404));
                return Ok(ApiResponse<string>.Success("User deleted successfully", 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }
    }
}
