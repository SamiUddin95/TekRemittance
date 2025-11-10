using Microsoft.AspNetCore.Mvc;
using TekRemittance.Repository.DTOs;
using TekRemittance.Service.Interfaces;
using System;
using System.Threading.Tasks;
using TekRemittance.Web.Models;
using Microsoft.AspNetCore.Cors;

namespace TekRemittance.API.Controllers
{
    [EnableCors("AllowFrontend")]
    [Route("api/[controller]")]
    [ApiController]
    public class PasswordPolicyController : ControllerBase
    {
        private readonly IPasswordPolicyService _service;

        public PasswordPolicyController(IPasswordPolicyService service)
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
                var policy = await _service.GetByIdAsync(id);
                if (policy == null)
                    return NotFound(ApiResponse<string>.Error("PasswordPolicy not found", 404));

                return Ok(ApiResponse<PasswordPolicyDto>.Success(policy, 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PasswordPolicyDto policy)
        {
            try
            {
                var created = await _service.CreateAsync(policy);
                return Ok(ApiResponse<PasswordPolicyDto>.Success(created, 201));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] PasswordPolicyDto policy)
        {
            try
            {
                var updated = await _service.UpdateAsync(policy);
                if (updated == null)
                    return NotFound(ApiResponse<string>.Error("PasswordPolicy not found", 404));

                return Ok(ApiResponse<PasswordPolicyDto>.Success(updated, 200));
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
                var result = await _service.DeleteAsync(id);
                if (!result)
                    return NotFound(ApiResponse<string>.Error("PasswordPolicy not found", 404));

                return Ok(ApiResponse<string>.Success("PasswordPolicy deleted successfully", 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }
    }
}
