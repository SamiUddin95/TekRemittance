using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TekRemittance.Repository.Models.dto;
using TekRemittance.Service.Interfaces;
using TekRemittance.Web.Models;

namespace TekRemittance.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExchangeRebateController : ControllerBase
    {
        private readonly IExchangeRebateService _service;

        public ExchangeRebateController(IExchangeRebateService service)
        {
            _service = service;
        }

        [HttpGet("ExchangeRebateCalcualtion")]
        public async Task<IActionResult> GetExchangeRebate([FromQuery] ExchangeRebateRequestDTO request)
        {
            try
            {
                var result = await _service.GetExchangeRebateAsync(request);
                return Ok(ApiResponse<ExchangeRebateResultDto>.Success(result, 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }
    }
}
