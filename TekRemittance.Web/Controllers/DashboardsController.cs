using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TekRemittance.Repository.Models.dto;
using TekRemittance.Service.Interfaces;
using TekRemittance.Web.Models;

namespace TekRemittance.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardsController : ControllerBase
    {
        private readonly IDashboardsService _service;
        public DashboardsController(IDashboardsService service)
        {
            _service = service;
        }

        [HttpGet("dashboard/disbursement")]
        public async Task<IActionResult> GetDisbursementDashboard(
      [FromQuery] string dateRange)
        {
            try
            {
                var result = await _service.GetDashboardDataAsync(dateRange);
                return Ok(ApiResponse<object>.Success(result, 200));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ApiResponse<string>.Error(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }

        [HttpGet("barGraphDashboard")]
        public async Task<IActionResult> barGraphDashboard([FromQuery] string dateRange)
        {
            try
            {
                var result = await _service.GetbarChartDataAsync(dateRange);
                return Ok(ApiResponse<object>.Success(result, 200));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ApiResponse<string>.Error(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }

        [HttpGet("dashboard/transactionModeCount")]
        public async Task<IActionResult> GetTransactionModeCounts([FromQuery] string dateRange)
        {
            try
            {
                var result = await _service.GetTransactionModeCountsAsync(dateRange);
                return Ok(ApiResponse<object>.Success(result, 200));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ApiResponse<string>.Error(ex.Message));
            }
        }






    }
}
