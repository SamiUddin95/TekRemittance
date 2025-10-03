using Microsoft.AspNetCore.Mvc;
using TekRemittance.Repository.Entities;
using TekRemittance.Service.Implementations;
using TekRemittance.Service.Interfaces;
using TekRemittance.Web.Models;

namespace TekRemittance.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BasicSetupController : ControllerBase
    {
        private readonly IBasicSetupService _service;

        public BasicSetupController(IBasicSetupService service)
        {
            _service = service;
        }

        [HttpGet("countries")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var countries = await _service.GetAllCountriesAsync();
                return Ok(ApiResponse<IEnumerable<Country>>.Success(countries, 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }

        // ✅ GET: api/country/{id}
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var country = await _service.GetCountryByIdAsync(id);
                if (country == null)
                    return NotFound(ApiResponse<string>.Error("Country not found", 404));

                return Ok(ApiResponse<Country>.Success(country, 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }

        // ✅ POST: api/country/create
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] Country country)
        {
            try
            {
                var created = await _service.CreateCountryAsync(country);
                return Ok(ApiResponse<Country>.Success(created, 201));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }

        // ✅ PUT: api/country/update/{id}
        [HttpPut("update/{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] Country country)
        {
            try
            {
                country.Id = id;
                var updated = await _service.UpdateCountryAsync(country);

                if (updated == null)
                    return NotFound(ApiResponse<string>.Error("Country not found", 404));

                return Ok(ApiResponse<Country>.Success(updated, 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }

        // ✅ DELETE: api/country/delete/{id}
        [HttpDelete("delete/{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var result = await _service.DeleteCountryAsync(id);
                if (!result)
                    return NotFound(ApiResponse<string>.Error("Country not found", 404));

                return Ok(ApiResponse<string>.Success("Country deleted successfully", 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }
    }
}
