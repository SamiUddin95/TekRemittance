using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TekRemittance.Repository.Interfaces;
using TekRemittance.Service.Interfaces;
using TekRemittance.Web.Models;

namespace TekRemittance.Web.Controllers
{
    [EnableCors("AllowFrontend")]
    [ApiController]
    [Route("api/[controller]")]
    public class AgentFileUploadsController : ControllerBase
    {
        private readonly IRemittanceIngestionService _ingestion;
        private readonly IRemittanceInfoRepository _repo;

        public AgentFileUploadsController(IRemittanceIngestionService ingestion, IRemittanceInfoRepository repo)
        {
            _ingestion = ingestion;
            _repo = repo;
        }

        [AllowAnonymous]
        [HttpPost]
        [RequestSizeLimit(long.MaxValue)]
        public async Task<IActionResult> Upload([FromForm] Guid agentId, [FromForm] bool hasHeader, [FromForm] IFormFile file, [FromForm] Guid? templateId)
        {
            if (file == null)
            {
                return BadRequest(ApiResponse<string>.Error("File is required", 400));
            }
            try
            {
                var result = await _ingestion.IngestAsync(agentId, templateId, file, hasHeader);
                return Ok(ApiResponse<object>.Success(new { uploadId = result.UploadId, rowCount = result.RowCount }, 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> List([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 50, string? templatename = null, string? filename = null)
        {
            try
             {
                var (items, total) = await _repo.GetByUploadAsync(pageNumber, pageSize,templatename,filename);
                return Ok(ApiResponse<object>.Success(new { total, items }, 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }
    }
}
