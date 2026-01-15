using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TekRemittance.Service.Interfaces;
using TekRemittance.Web.Models;

namespace TekRemittance.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ReportsController : ControllerBase
    {
        private readonly ISsrsRenderService _renderService;

        public ReportsController(ISsrsRenderService renderService)
        {
            _renderService = renderService;
        }

        [HttpPost("render")]
        public async Task<IActionResult> Render([FromBody] ReportRenderRequest request, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            var (bytes, contentType, ext) = await _renderService.RenderAsync(
                request.ReportPath,
                request.Format ?? "PDF",
                request.Parameters,
                ct);

            var safeName = string.IsNullOrWhiteSpace(request.FileName)
                ? SlugFromPath(request.ReportPath) + ext
                : request.FileName.EndsWith(ext, StringComparison.OrdinalIgnoreCase)
                    ? request.FileName
                    : request.FileName + ext;

            return File(bytes, contentType, safeName);
        }

        private static string SlugFromPath(string path)
        {
            var name = path?.TrimEnd('/') ?? "report";
            var last = name.LastIndexOf('/') >= 0 ? name[(name.LastIndexOf('/') + 1)..] : name;
            foreach (var c in System.IO.Path.GetInvalidFileNameChars())
            {
                last = last.Replace(c, '_');
            }
            return string.IsNullOrWhiteSpace(last) ? "report" : last;
        }
    }
}
