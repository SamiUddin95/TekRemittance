using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using TekRemittance.Repository.Entities;
using TekRemittance.Service.Implementations;
using TekRemittance.Service.Interfaces;
using TekRemittance.Web.Models;

namespace TekRemittance.Web.Controllers
{
    [EnableCors("AllowFrontend")]
    [ApiController]
    [Route("api/[controller]")]
    public class BasicSetupController : ControllerBase
    {
        private readonly IBasicSetupService _service;

        public BasicSetupController(IBasicSetupService service)
        {
            _service = service;
        }

        #region Country

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
        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] Country country)
        {
            try
            {
                country.Id = country.Id;
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

        #endregion

        #region Province

        [HttpGet("Province")]
        public async Task<IActionResult> GetAllProvince()
        {
            try
            {
                var province = await _service.GetAllProvinceAsync();
                return Ok(ApiResponse<IEnumerable<Province>>.Success(province, 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }

        // ✅ GET: api/country/{id}
        [HttpGet("{provinceId:guid}")]
        public async Task<IActionResult> GetProvinceById(Guid provinceId)
        {
            try
            {
                var province = await _service.GetProvinceByIdAsync(provinceId);
                if (province == null)
                    return NotFound(ApiResponse<string>.Error("Province not found", 404));

                return Ok(ApiResponse<Province>.Success(province, 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }

        // ✅ POST: api/country/create
        [HttpPost("CreateProvince")]
        public async Task<IActionResult> CreateProvince([FromBody] Province province)
        {
            try
            {
                var created = await _service.CreateProvinceAsync(province);
                return Ok(ApiResponse<Province>.Success(created, 201));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }

        // ✅ PUT: api/country/update/{id}
        [HttpPut("updateProvince")]
        public async Task<IActionResult> UpdateProvince([FromBody] Province province)
        {
            try
            {
                province.Id = province.Id;
                var updated = await _service.UpdateProvinceAsync(province);

                if (updated == null)
                    return NotFound(ApiResponse<string>.Error("Province not found", 404));

                return Ok(ApiResponse<Province>.Success(updated, 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }

        // ✅ DELETE: api/country/delete/{id}
        [HttpDelete("deleteProvince/{id:guid}")]
        public async Task<IActionResult> DeleteProvince(Guid id)
        {
            try
            {
                var result = await _service.DeleteProvinceAsync(id);
                if (!result)
                    return NotFound(ApiResponse<string>.Error("Province not found", 404));

                return Ok(ApiResponse<string>.Success("Province deleted successfully", 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }

        #endregion

        #region City

        [HttpGet("City")]
        public async Task<IActionResult> GetAllCity()
        {
            try
            {
                var city = await _service.GetAllCityAsync();
                return Ok(ApiResponse<IEnumerable<City>>.Success(city, 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }

        // ✅ GET: api/country/{id}
        [HttpGet("{cityId:guid}")]
        public async Task<IActionResult> GetCityById(Guid CityId)
        {
            try
            {
                var city = await _service.GetCityByIdAsync(CityId);
                if (city == null)
                    return NotFound(ApiResponse<string>.Error("City not found", 404));

                return Ok(ApiResponse<City>.Success(city, 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }

        // ✅ POST: api/country/create
        [HttpPost("CreateCity")]
        public async Task<IActionResult> CreateCity([FromBody] City city)
        {
            try
            {
                var created = await _service.CreateCityAsync(city);
                return Ok(ApiResponse<City>.Success(created, 201));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }

        // ✅ PUT: api/country/update/{id}
        [HttpPut("updateCity")]
        public async Task<IActionResult> UpdateCity([FromBody] City city)
        {
            try
            {
                city.Id = city.Id;
                var updated = await _service.UpdateCityAsync(city);

                if (updated == null)
                    return NotFound(ApiResponse<string>.Error("City not found", 404));

                return Ok(ApiResponse<City>.Success(updated, 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }

        // ✅ DELETE: api/country/delete/{id}
        [HttpDelete("deleteCity/{id:guid}")]
        public async Task<IActionResult> DeleteCity(Guid id)
        {
            try
            {
                var result = await _service.DeleteCityAsync(id);
                if (!result)
                    return NotFound(ApiResponse<string>.Error("City not found", 404));

                return Ok(ApiResponse<string>.Success("City deleted successfully", 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }

        #endregion

    }
}
