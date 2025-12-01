using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using TekRemittance.Repository.Entities;
using TekRemittance.Service.Implementations;
using TekRemittance.Service.Interfaces;
using TekRemittance.Web.Models;
using TekRemittance.Web.Models.dto;
using TekRemittance.Web.Attributes;
using TekRemittance.Repository.Enums;

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

        //[RequirePermission("BasicSetup.Countries.Read")]
        [HttpGet("countries")]
        public async Task<IActionResult> GetAllCountries(int pageNumber = 1, int pageSize = 10, string? countryCode = null, string? countryName = null, StatusesEnums? status = null)
        {
            try
            {
                var result = await _service.GetAllCountriesAsync(pageNumber, pageSize,countryCode,countryName,status);
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

        [RequirePermission("BasicSetup.Countries.Read")]
        [HttpGet("countrybyId/{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var country = await _service.GetCountryByIdAsync(id);
                if (country == null)
                    return NotFound(ApiResponse<string>.Error("Country not found", 404));

                return Ok(ApiResponse<countryDTO>.Success(country, 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }

        //[RequirePermission("BasicSetup.Countries.Create")]
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] countryDTO country)
        {
            try
            {
                var created = await _service.CreateCountryAsync(country);
                return Ok(ApiResponse<countryDTO>.Success(created, 201));
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

        //[RequirePermission("BasicSetup.Countries.Edit")]
        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] countryDTO country)
        {
            try
            {
                var updated = await _service.UpdateCountryAsync(country);

                if (updated == null)
                    return NotFound(ApiResponse<string>.Error("Country not found", 404));

                return Ok(ApiResponse<countryDTO>.Success(updated, 200));
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

        //[RequirePermission("BasicSetup.Countries.Delete")]
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

        //[RequirePermission("BasicSetup.Provinces.Read")]
        [HttpGet("provinces")]
        public async Task<IActionResult> GetAllProvinces(int pageNumber = 1, int pageSize = 10, string? provinceCode = null, string? provinceName = null, StatusesEnums? status = null)
        {
            try
            {
                var result = await _service.GetAllProvinceAsync(pageNumber, pageSize,provinceCode,provinceName,status);
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

        //[RequirePermission("BasicSetup.Provinces.Read")]
        [HttpGet("provincebyId/{Id:guid}")]
        public async Task<IActionResult> GetProvinceById(Guid Id)
        {
            try
            {
                var province = await _service.GetProvinceByIdAsync(Id);
                if (province == null)
                    return NotFound(ApiResponse<string>.Error("Province not found", 404));

                return Ok(ApiResponse<provinceDTO>.Success(province, 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }

        [HttpPost("CreateProvince")]
        public async Task<IActionResult> CreateProvince([FromBody] provinceDTO dto)
        {
            try
            {
                var created = await _service.CreateProvinceAsync(dto);
                return Ok(ApiResponse<provinceDTO>.Success(created, 201));
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

        [HttpPut("updateProvince")]
        public async Task<IActionResult> UpdateProvince([FromBody] provinceDTO province)
        {
            try
            {
                var updated = await _service.UpdateProvinceAsync(province);

                if (updated == null)
                    return NotFound(ApiResponse<string>.Error("Province not found", 404));

                return Ok(ApiResponse<provinceDTO>.Success(updated, 200));
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

        [HttpGet("cities")]
        public async Task<IActionResult> GetAllCities(int pageNumber = 1, int pageSize = 10, string? cityCode = null, string? cityName = null, StatusesEnums? status = null)
        {
            try
            {
                var result = await _service.GetAllCityAsync(pageNumber, pageSize,cityCode,cityName,status);
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

        [HttpGet("CitybyId/{Id:guid}")]
        public async Task<IActionResult> GetCityById([FromRoute] Guid Id)
        {
            try
            {
                var city = await _service.GetCityByIdAsync(Id);
                if (city == null)
                    return NotFound(ApiResponse<string>.Error("City not found", 404));

                return Ok(ApiResponse<cityDTO>.Success(city, 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }

        [HttpPost("CreateCity")]
        public async Task<IActionResult> CreateCity([FromBody] cityDTO city)
        {
            try
            {
                var created = await _service.CreateCityAsync(city);
                return Ok(ApiResponse<cityDTO>.Success(created, 201));
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

        [HttpPut("updateCity")]
        public async Task<IActionResult> UpdateCity([FromBody] cityDTO city)
        {
            try
            {
                var updated = await _service.UpdateCityAsync(city);

                if (updated == null)
                    return NotFound(ApiResponse<string>.Error("City not found", 404));

                return Ok(ApiResponse<cityDTO>.Success(updated, 200));
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

        #region Bank

        [HttpGet("banks")]
        public async Task<IActionResult> GetAllBanks(int pageNumber = 1, int pageSize = 10, string? bankCode = null, string? bankName = null, StatusesEnums? status = null)
        {
            try
            {
                var result = await _service.GetAllBankAsync(pageNumber, pageSize,bankCode,bankName,status);
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

        [HttpGet("BankbyId/{bankId:guid}")]
        public async Task<IActionResult> GetBankById(Guid bankId)
        {
            try
            {
                var bank = await _service.GetBankByIdAsync(bankId);
                if (bank == null)
                    return NotFound(ApiResponse<string>.Error("Bank not found", 404));

                return Ok(ApiResponse<bankDTO>.Success(bank, 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }

        [HttpPost("CreateBank")]
        public async Task<IActionResult> CreateBank([FromBody] bankDTO bank)
        {
            try
            {
                var created = await _service.CreateBankAsync(bank);
                return Ok(ApiResponse<bankDTO>.Success(created, 201));
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

        [HttpPut("updateBank")]
        public async Task<IActionResult> UpdateBank([FromBody] bankDTO bank)
        {
            try
            {
                var updated = await _service.UpdateBankAsync(bank);

                if (updated == null)
                    return NotFound(ApiResponse<string>.Error("Bank not found", 404));

                return Ok(ApiResponse<bankDTO>.Success(updated, 200));
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

        [HttpDelete("deleteBank/{id:guid}")]
        public async Task<IActionResult> DeleteBank(Guid id)
        {
            try
            {
                var result = await _service.DeleteBankAsync(id);
                if (!result)
                    return NotFound(ApiResponse<string>.Error("Bank not found", 404));

                return Ok(ApiResponse<string>.Success("Bank deleted successfully", 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }

        #endregion

    }
}
