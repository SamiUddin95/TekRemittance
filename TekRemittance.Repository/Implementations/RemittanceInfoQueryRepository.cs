using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TekRemittance.Repository.Entities.Data;
using TekRemittance.Repository.Interfaces;
using TekRemittance.Repository.Models.dto;

namespace TekRemittance.Repository.Implementations
{
    public class RemittanceInfoQueryRepository : IRemittanceInfoQueryRepository
    {
        private readonly AppDbContext _context;
        public RemittanceInfoQueryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<(IEnumerable<RemittanceInfoListItemDTO> Items, int TotalCount)> GetRemittanceInfosAsync(string? accountNumber, int pageNumber = 1, int pageSize = 50)
        {
            if (pageNumber < 1) pageNumber = 1;
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
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(r => new RemittanceInfoListItemDTO
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

            return (items, total);
        }
    }
}
