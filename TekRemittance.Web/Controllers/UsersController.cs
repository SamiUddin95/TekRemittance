using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TekRemittance.Service.Interfaces;
using TekRemittance.Web.Models;
using TekRemittance.Web.Models.dto;
using System;
using System.Threading.Tasks;
using TekRemittance.Repository.Models.dto;

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
        [AllowAnonymous]
        public async Task<IActionResult> Create([FromBody] CreateUserRequest req)
        {
            try
            {
                var created = await _service.CreateAsync(req.User, req.Password);
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
        public async Task<IActionResult> Update([FromBody] userDTO dto)
        {
            try
            {
                var updated = await _service.UpdateAsync(dto);
                if (updated == null) return NotFound(ApiResponse<string>.Error("User not found", 404));
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
                if (!ok) return NotFound(ApiResponse<string>.Error("User not found", 404));
                return Ok(ApiResponse<string>.Success("User deleted successfully", 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }

        [HttpPut("{id:guid}/supervise")]
        public async Task<IActionResult> UpdateSupervise(Guid id, [FromQuery] bool isSupervise)
        {
            try
            {
                var ok = await _service.UpdateIsSuperviseAsync(id, isSupervise);
                if (!ok) return NotFound(ApiResponse<string>.Error("User not found", 404));
                return Ok(ApiResponse<string>.Success("IsSupervise updated", 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }

        public class UpdateNamePasswordRequest
        {
            public string Name { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
        }

        [HttpPut("{id:guid}/name-password")]
        public async Task<IActionResult> UpdateNameAndPassword(Guid id, [FromBody] UpdateNamePasswordRequest req)
        {
            try
            {
                var ok = await _service.UpdateNameAndPasswordAsync(id, req.Name, req.Password);
                if (!ok) return NotFound(ApiResponse<string>.Error("User not found", 404));
                return Ok(ApiResponse<string>.Success("Name and password updated", 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }

        [HttpPost("forgetpassword")]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordDTO dto)
        {

            try
            {
                var result = await _service.ForgetPassword(dto);

                if (!result.Success)
                    return BadRequest(ApiResponse<string>.Error("Invalid Username or Email", 400));

                return Ok(new { message = result.Message, newPassword = result.NewPassword });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }

        [HttpGet("UnAuthorizeUsers")]
        public async Task<IActionResult> GetAllUnAuthorize(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var result = await _service.GetAllUnAuthorize(pageNumber, pageSize);
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
