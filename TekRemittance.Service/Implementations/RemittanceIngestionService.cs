using ClosedXML.Excel;
using ExcelDataReader;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TekRemittance.Repository.Entities;
using TekRemittance.Repository.Entities.Data;
using TekRemittance.Repository.Interfaces;
using TekRemittance.Service.Interfaces;
using TekRemittance.Web.Models.dto;

namespace TekRemittance.Service.Implementations
{
    public class RemittanceIngestionService : IRemittanceIngestionService
    {
        private readonly IAgentFileTemplateRepository _templateRepo;
        private readonly IAgentFileTemplateFieldRepository _fieldRepo;
        private readonly IRemittanceInfoRepository _remitRepo;
        private readonly AppDbContext _context;
        public RemittanceIngestionService(
            IAgentFileTemplateRepository templateRepo,
            IAgentFileTemplateFieldRepository fieldRepo,
            IRemittanceInfoRepository remitRepo,
            AppDbContext context)
        {
            _templateRepo = templateRepo;
            _fieldRepo = fieldRepo;
            _remitRepo = remitRepo;
            _context = context;
        }

        public async Task<(Guid UploadId, int RowCount)> IngestAsync(Guid agentId, Guid? templateId, IFormFile file, bool hasHeader)
        {
            if (file == null || file.Length == 0) throw new ArgumentException("File is empty");

            var baseName = Path.GetFileNameWithoutExtension(file.FileName)?.Trim();
            var getTemplateId = _context.AgentFileTemplates.Where(x => x.SheetName == baseName).FirstOrDefault();
            if (getTemplateId == null)
            {
                throw new InvalidOperationException("File template Not Found.");
            }
            var template = await _templateRepo.GetByAgentIdAsync(getTemplateId.AgentId) 
                          ?? throw new InvalidOperationException("Template not found for agent");
            if (templateId.HasValue && template.Id != templateId.Value)
            {
                throw new InvalidOperationException("Provided templateId does not belong to the agent's active template.");
            }

            // Filename vs SheetName check
            agentId = getTemplateId.AgentId;
             var templateSheetName = template.SheetName?.Trim();
            if (!string.IsNullOrEmpty(templateSheetName))
            {
                if (!string.Equals(baseName, templateSheetName, StringComparison.OrdinalIgnoreCase))
                    throw new InvalidOperationException($"Uploaded file name '{baseName}' does not match template SheetName '{templateSheetName}'.");
            }

            var fields = (await _fieldRepo.GetByTemplateIdAsync(template.Id))
                .Where(f => f.Enabled)
                .OrderBy(f => f.FieldOrder)
                .ToList();
            if (fields.Count == 0) throw new InvalidOperationException("Template has no enabled fields");

            var uploadId = await _remitRepo.CreateUploadAsync(agentId, template.Id, baseName);

            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            var rows = new List<RemittanceInfo>(capacity: 512);
            int rowNo = 0;
            try
            {
                if (ext == ".txt")
                {
                    using var stream = file.OpenReadStream();
                    using var reader = new StreamReader(stream, Encoding.UTF8, detectEncodingFromByteOrderMarks: true);
                    string? line; bool first = true;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (first && hasHeader) { first = false; continue; }
                        first = false;
                        rowNo++;
                        var values = SplitPipe(line);
                        var accountNumberIndex = fields.FindIndex(f =>
                        {
                            if (string.IsNullOrWhiteSpace(f.FieldName))
                                return false;

                            var normalized = f.FieldName
                                .ToLower()
                                .Replace("_", "")
                                .Replace(" ", "");

                            return normalized.Contains("account")
                                   && (normalized.Contains("no") || normalized.Contains("number"));
                        });

                        string? accountNumber = null;
                        if (accountNumberIndex >= 0 && accountNumberIndex < values.Count)
                        {
                            accountNumber = values[accountNumberIndex];
                        }
                        var (json, error) = MapToJson(values, fields);
                        rows.Add(BuildRow(agentId, template.Id, uploadId, rowNo, json, error, accountNumber));
                    }
                }
                //else if (ext == ".csv")
                //{
                //    using var stream = file.OpenReadStream();
                //    using var reader = new StreamReader(stream, Encoding.UTF8, detectEncodingFromByteOrderMarks: true);
                //    string? line; bool first = true;
                //    while ((line = reader.ReadLine()) != null)
                //    {
                //        if (first && hasHeader) { first = false; continue; }
                //        first = false;
                //        rowNo++;
                //        var values = ParseCsvLine(line);
                //        var accountNumber = values[2];
                //        var (json, error) = MapToJson(values, fields);
                //        rows.Add(BuildRow(agentId, template.Id, uploadId, rowNo, json, error, accountNumber));
                //    }
                //}
                //else if (ext == ".xlsx")
                //{
                //    using var stream = file.OpenReadStream();
                //    using var wb = new XLWorkbook(stream);
                //    var ws = !string.IsNullOrEmpty(template.SheetName) ? wb.Worksheet(template.SheetName) : wb.Worksheets.FirstOrDefault();
                //    if (ws == null) throw new InvalidOperationException("Worksheet not found");
                //    var table = ws.RangeUsed();
                //    if (table == null) throw new InvalidOperationException("Worksheet is empty");
                //    int startRow = table.RangeAddress.FirstAddress.RowNumber;
                //    int endRow = table.RangeAddress.LastAddress.RowNumber;
                //    int colCount = fields.Count;
                //    bool skippedHeader = false;
                //    for (int r = startRow; r <= endRow; r++)
                //    {
                //        if (hasHeader && !skippedHeader) { skippedHeader = true; continue; }
                //        rowNo++;
                //        var values = new List<string>(colCount);
                //        var accountNumber = values[2];
                //        for (int i = 0; i < colCount; i++)
                //        {
                //            var cell = ws.Cell(r, i + 1);
                //            values.Add(cell?.GetString() ?? string.Empty);
                //        }
                //        var (json, error) = MapToJson(values, fields);
                //        rows.Add(BuildRow(agentId, template.Id, uploadId, rowNo, json, error, accountNumber));
                //    }
                //}
                //else if (ext == ".xls")
                //{
                //    System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                //    using var stream = file.OpenReadStream();
                //    using var excel = ExcelReaderFactory.CreateBinaryReader(stream);
                //    var ds = excel.AsDataSet(new ExcelDataSetConfiguration
                //    {
                //        UseColumnDataType = false,
                //        ConfigureDataTable = _ => new ExcelDataTableConfiguration { UseHeaderRow = hasHeader }
                //    });
                //    var sheet = !string.IsNullOrEmpty(template.SheetName) ? ds.Tables[template.SheetName] : ds.Tables[0];
                //    if (sheet == null) throw new InvalidOperationException("Worksheet not found");
                //    foreach (DataRow row in sheet.Rows)
                //    {
                //        rowNo++;
                //        var values = new List<string>(fields.Count);
                //        var accountNumber = values[2];
                //        for (int i = 0; i < fields.Count; i++)
                //        {
                //            var val = row.ItemArray.Length > i ? row[i]?.ToString() ?? string.Empty : string.Empty;
                //            values.Add(val);
                //        }
                //        var (json, error) = MapToJson(values, fields);
                //        rows.Add(BuildRow(agentId, template.Id, uploadId, rowNo, json, error, accountNumber));
                //    }
                //}
                else
                {
                    throw new NotSupportedException($"Unsupported file extension: {ext}");
                }

                if (rows.Count > 0)
                {
                    await _remitRepo.AddRangeAsync(rows);
                }
                await _remitRepo.UpdateUploadAsync(uploadId, rowNo, success: true, errorMessage: null);
                return (uploadId, rowNo);
            }
            catch (Exception ex)
            {
                await _remitRepo.UpdateUploadAsync(uploadId, rowNo, success: false, errorMessage: ex.Message);
                throw;
            }
        }

        private static RemittanceInfo BuildRow(Guid agentId, Guid templateId, Guid uploadId, int rowNo, string json, string? error,string accountNumber)
        {
            return new RemittanceInfo
            {
                Id = Guid.NewGuid(),
                AgentId = agentId,
                TemplateId = templateId,
                UploadId = uploadId,
                RowNumber = rowNo,
                DataJson = json,
                Error = error,
                CreatedOn = DateTime.Now,
                AccountNumber=accountNumber,
                Date = DateTime.Now,
                Status = "P"
                
            };
        }
        private static (string Json, string? Error) MapToJson(IReadOnlyList<string> values, List<agentFileTemplateFieldDTO> fields)
        {
            var dict = new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase);
            string? error = null;
            for (int i = 0; i < fields.Count; i++)
            {
                var f = fields[i];
                var raw = values.Count > i ? values[i] : string.Empty;
                if (string.IsNullOrWhiteSpace(raw))
                {
                    if (f.Required) error = AppendError(error, $"Missing required field {f.FieldName}");
                    dict[f.FieldName] = null;
                    continue;
                }
                try
                {
                    object? parsed = f.FieldType switch
                    {
                        FieldType.Number => int.TryParse(raw, NumberStyles.Integer, CultureInfo.InvariantCulture, out var n) ? n : throw new FormatException($"Invalid number: {raw}"),
                        FieldType.Decimal => decimal.TryParse(raw, NumberStyles.Number, CultureInfo.InvariantCulture, out var d) ? d : throw new FormatException($"Invalid decimal: {raw}"),
                        FieldType.Date => DateTime.TryParse(raw, CultureInfo.InvariantCulture, DateTimeStyles.None, out var dt) ? dt : throw new FormatException($"Invalid date: {raw}"),
                        FieldType.Bool => bool.TryParse(raw, out var b) ? b : throw new FormatException($"Invalid bool: {raw}"),
                        _ => raw
                    };
                    dict[f.FieldName] = parsed;
                }
                catch (Exception ex)
                {
                    error = AppendError(error, $"{f.FieldName}: {ex.Message}");
                    dict[f.FieldName] = raw;
                }
            }
            var json = JsonSerializer.Serialize(dict);
            return (json, error);
        }

        private static List<string> SplitPipe(string line)
        {
            // Simple split for pipe-delimited
            return (line ?? string.Empty).Split('|').Select(s => s).ToList();
        }

        private static List<string> ParseCsvLine(string line)
        {
            // Minimal CSV parser for common cases (quotes and commas)
            var result = new List<string>();
            if (line == null) return result;
            var sb = new StringBuilder();
            bool inQuotes = false;
            for (int i = 0; i < line.Length; i++)
            {
                var c = line[i];
                if (c == '"')
                {
                    if (inQuotes && i + 1 < line.Length && line[i + 1] == '"') { sb.Append('"'); i++; }
                    else inQuotes = !inQuotes;
                }
                else if (c == ',' && !inQuotes)
                {
                    result.Add(sb.ToString()); sb.Clear();
                }
                else
                {
                    sb.Append(c);
                }
            }
            result.Add(sb.ToString());
            return result;
        }

        

        private static string? AppendError(string? existing, string add)
        {
            if (string.IsNullOrEmpty(existing)) return add;
            return existing + "; " + add;
        }
    }
}
