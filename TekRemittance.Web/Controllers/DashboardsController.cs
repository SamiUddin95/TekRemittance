using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TekRemittance.Repository.Entities;
using TekRemittance.Repository.Models.dto;
using TekRemittance.Service.Interfaces;
using TekRemittance.Web.Models;

namespace TekRemittance.Web.Controllers
{
    [EnableCors("AllowFrontend")]
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardsController : ControllerBase
    {
        private readonly IDashboardsService _service;
        public DashboardsController(IDashboardsService service)
        {
            _service = service;
        }

      //  [HttpGet("dashboard/disbursement")]
      //  public async Task<IActionResult> GetDisbursementDashboard(
      //[FromQuery] string dateRange)
      //  {
      //      try
      //      {
      //          var result = await _service.GetDashboardDataAsync(dateRange);
      //          return Ok(ApiResponse<object>.Success(result, 200));
      //      }
      //      catch (ArgumentException ex)
      //      {
      //          return BadRequest(ApiResponse<string>.Error(ex.Message));
      //      }
      //      catch (Exception ex)
      //      {
      //          return StatusCode(500, ApiResponse<string>.Error(ex.Message));
      //      }
      //  }

      //  [HttpGet("barGraphDashboard")]
      //  public async Task<IActionResult> barGraphDashboard([FromQuery] string dateRange)
      //  {
      //      try
      //      {
      //          var result = await _service.GetbarChartDataAsync(dateRange);
      //          return Ok(ApiResponse<object>.Success(result, 200));
      //      }
      //      catch (ArgumentException ex)
      //      {
      //          return BadRequest(ApiResponse<string>.Error(ex.Message));
      //      }
      //      catch (Exception ex)
      //      {
      //          return StatusCode(500, ApiResponse<string>.Error(ex.Message));
      //      }
      //  }

      //  [HttpGet("dashboard/transactionModeCount")]
      //  public async Task<IActionResult> GetTransactionModeCounts([FromQuery] string dateRange)
      //  {
      //      try
      //      {
      //          var result = await _service.GetTransactionModeCountsAsync(dateRange);
      //          return Ok(ApiResponse<object>.Success(result, 200));
      //      }
      //      catch (ArgumentException ex)
      //      {
      //          return BadRequest(ApiResponse<string>.Error(ex.Message));
      //      }
      //  }

      //  [HttpGet("dashboard/RecentTransactions")]
      //  public async Task<IActionResult> GetLast10Remittances()
      //  {
      //      try
      //      {
      //          var result = await _service.GetLast10RemittancesAsync();
      //          return Ok(ApiResponse<List<RecentTransactionDTO>>.Success(result, 200));
      //      }
      //      catch (Exception ex)
      //      {
      //          return StatusCode(500, ApiResponse<string>.Error(ex.Message));
      //      }
      //  }


      //  [HttpGet("dashboard/transactionModeList")]
      //  public async Task<IActionResult> GetTransactionModeList([FromQuery] string dateRange, [FromQuery] string mode)
      //  {
      //      try
      //      {
      //          var result = await _service.GetTransactionModeListAsync(dateRange, mode);
      //          return Ok(ApiResponse<List<RecentTransactionDTO>>.Success(result, 200));
      //      }
      //      catch (ArgumentException ex)
      //      {
      //          return BadRequest(ApiResponse<string>.Error(ex.Message));
      //      }
      //  }
        [HttpGet("dashboard/agent-performance")]
        public async Task<IActionResult> GetAgentPerformance()
        {
            try
            {
                var result = await _service.GetAgentPerformanceAsync();
                return Ok(ApiResponse<List<AgentPerformanceDTO>>.Success(result, 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }
        [HttpGet("dashboard/top-bank-transaction")]
        public async Task<IActionResult> GetTopBankTransaction()
        {
            try
            {
                var result = await _service.GetTopBankTransactionAsync();
                return Ok(ApiResponse<List<TopBankTransactionDTO>>.Success(result, 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }
        [HttpGet("dashboard/transaction-status-by-channel")]
        public async Task<IActionResult> GetTransactionStatusByChannel()
        {
            try
            {
                var result = await _service.GetTransactionStatusByChannelAsync();
                return Ok(ApiResponse<List<TransactionStatusByChannelDTO>>.Success(result, 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }
        [HttpGet("dashboard/incoming")]
        public async Task<IActionResult> GetIncomingSummary()
        {
            try
            {
                var result = await _service.GetIncomingSummaryAsync();
                return Ok(ApiResponse<SummaryDTO>.Success(result, 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }
        [HttpGet("dashboard/outgoing")]
        public async Task<IActionResult> GetOutgoingSummary()
        {
            try
            {
                var result = await _service.GetOutgoingSummaryAsync();
                return Ok(ApiResponse<SummaryDTO>.Success(result, 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }
        [HttpGet("dashboard/eprc")]
        public async Task<IActionResult> GetEPRC()
        {
            try
            {
                var result = await _service.GetEPRCAsync();
                return Ok(ApiResponse<List<EPRCDTO>>.Success(result, 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }
        [HttpGet("dashboard/channels")]
        public async Task<IActionResult> GetChannels()
        {
            try
            {
                var result = await _service.GetChannelsAsync();
                return Ok(ApiResponse<List<ChannelsDTO>>.Success(result, 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }





    }
}
