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

        [HttpGet("dashboard/disbursementTotalAmount")]
        public async Task<IActionResult> GetDisbursementAmount([FromQuery] string dateRange)
        {
            try
            {
                var result = await _service.GetDisbursementDashboardAsync(dateRange);

                var response = new
                {
                    totalAmount = result.TotalAmount
                };

                return Ok(ApiResponse<object>.Success(response, 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }


        [HttpGet("dashboard/disbursement/count")]
        public async Task<IActionResult> GetDisbursementCount([FromQuery] string dateRange)
        {
            try
            {
                var result = await _service.GetDisbursementCountAsync(dateRange);

                var response = new
                {
                    totalCount = result.TotalCount
                };

                return Ok(ApiResponse<object>.Success(response, 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }

        [HttpGet("dashboard/disbursement/Successcount")]
        public async Task<IActionResult> GetDisbursementSuccessCount([FromQuery] string dateRange)
        {
            try
            {
                var result = await _service.GetDisbursementSuccessCountAsync(dateRange);

                var response = new
                {
                    totalCount = result.TotalCount
                };

                return Ok(ApiResponse<object>.Success(response, 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }


        [HttpGet("dashboard/disbursementSuccessTotalAmount")]
        public async Task<IActionResult> GetDisbursementSuccessAmount([FromQuery] string dateRange)
        {
            try
            {
                var result = await _service.GetDisbursementDashboardSuccessAsync(dateRange);

                var response = new
                {
                    totalAmount = result.TotalAmount
                };

                return Ok(ApiResponse<object>.Success(response, 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }

        [HttpGet("dashboard/disbursement/SuccessPercentage")]
        public async Task<IActionResult> GetDisbursementSuccessPercentage([FromQuery] string? dateRange)
        {
            try
            {
                var result = await _service.GetDisbursementSuccessPercentageAsync(dateRange);

                var response = new
                {
                    successCount = result.SuccessCount,
                    totalCount = result.TotalCount,
                    successPercentage = result.SuccessPercentage
                };

                return Ok(ApiResponse<object>.Success(response, 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }





    }
}
