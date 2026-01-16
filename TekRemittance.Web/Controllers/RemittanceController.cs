using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TekRemittance.Repository.Entities.Data;
using TekRemittance.Web.Models;

namespace TekRemittance.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class RemittanceController : ControllerBase
    {
        private readonly AppDbContext _context;
        public RemittanceController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("data")]
        public async Task<ActionResult<PagedResult<RemittanceRowDto>>> GetData([FromQuery] string? accountNumber, [FromQuery] int page = 1, [FromQuery] int pageSize = 50)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 50;

            var query = _context.RemittanceInfos.AsNoTracking().AsQueryable();
            if (!string.IsNullOrWhiteSpace(accountNumber))
            {
                var acc = accountNumber.Trim();
                query = query.Where(r => r.AccountNumber == acc);
            }

            var total = await query.CountAsync();

            var items = await query
                .OrderByDescending(r => r.CreatedOn)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(r => new RemittanceRowDto
                {
                    RowNumber = r.RowNumber,
                    Error = r.Error,
                    Status = r.Status,
                    AccountNumber = r.AccountNumber,
                    AccountTitle = r.AccountTitle,
                    Xpin = r.Xpin,
                    CreatedOn = r.CreatedOn
                })
                .ToListAsync();

            var result = new PagedResult<RemittanceRowDto>
            {
                Items = items,
                Total = total
            };
            return Ok(result);
        }
    }
}
