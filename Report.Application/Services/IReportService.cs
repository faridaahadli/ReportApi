using Report.Application.Models.Pagination;
using Report.Application.Models.Report;
using Report.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Report.Application.Models.Report;

namespace Report.Application.Services;

public interface IReportService:IBaseService
{
    Task<ReportGetAllResponse> GetReportsAsync(PaginationFilter paginationFilter,
            ReportFilter reportFilter);
    Task<byte[]> ExcelExport(ReportFilter reportFilter);
    Task<byte[]> ExcelImport(IFormFile file);
    Task<string> OverheadUpdate(OverheadUpdateDto overhead);
}
