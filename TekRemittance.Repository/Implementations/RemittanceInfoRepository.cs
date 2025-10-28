using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TekRemittance.Repository.Entities;
using TekRemittance.Repository.Entities.Data;
using TekRemittance.Repository.Interfaces;

namespace TekRemittance.Repository.Implementations
{
    public class RemittanceInfoRepository : IRemittanceInfoRepository
    {
        private readonly AppDbContext _context;
        public RemittanceInfoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> CreateUploadAsync(Guid agentId, Guid templateId, string fileName)
        {
            var upload = new AgentFileUpload
            {
                Id = Guid.NewGuid(),
                AgentId = agentId,
                TemplateId = templateId,
                FileName = fileName,
                Status = UploadStatus.Pending,
                RowCount = 0,
                CreatedOn = DateTime.UtcNow
            };
            _context.AgentFileUploads.Add(upload);
            await _context.SaveChangesAsync();
            return upload.Id;
        }

        public async Task UpdateUploadAsync(Guid uploadId, int rowCount, bool success, string? errorMessage)
        {
            var upload = await _context.AgentFileUploads.FirstOrDefaultAsync(u => u.Id == uploadId);
            if (upload == null) return;
            upload.RowCount = rowCount;
            upload.ProcessedAt = DateTime.Now;
            upload.Status = success ? UploadStatus.Parsed : UploadStatus.Failed;
            upload.ErrorMessage = errorMessage;
            await _context.SaveChangesAsync();
        }

        public async Task AddRangeAsync(IEnumerable<RemittanceInfo> rows)
        {
            await _context.RemittanceInfos.AddRangeAsync(rows);
            await _context.SaveChangesAsync();
        }

        public async Task<(IEnumerable<AgentFileUpload> Items, int TotalCount)> GetByUploadAsync(int pageNumber = 1, int pageSize = 50)
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 50;
            var query = _context.AgentFileUploads
                .AsNoTracking()
                .OrderByDescending(u => u.CreatedOn);
            var total = await query.CountAsync();
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return (items, total);
        }
    }
}
