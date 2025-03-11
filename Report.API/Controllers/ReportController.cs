using Report.Application.Excel;
using Report.Application.Models;
using Report.Application.Models.Pagination;
using Report.Application.Routes;
using Report.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Report.API.Attributes;
using Report.Application.Models.Report;
using Report.Core.Enums;

namespace Report.API.Controllers;

public class ReportController : ControllerBase
{
    private readonly IReportService _reportService;

    public ReportController(IReportService reportService) => _reportService = reportService;

    [HttpGet(ApiRoute.Report.GetReports)]
    public async Task<IActionResult> GetAll([FromQuery] PaginationFilter filter,
        [FromQuery] ReportFilter? reportFilter) =>
        Ok(await _reportService.GetReportsAsync(filter, reportFilter));

    [HttpPost(ApiRoute.Report.ReportImport)]
    [Permission(ClaimEnum.Reporter)]
    public async Task<IActionResult> Import(IFormFile file)
    {
        return Ok(await _reportService.ExcelImport(file));
    }
    [HttpPut(ApiRoute.Report.Update)]
    [Permission(ClaimEnum.Reporter)]
    public async Task<IActionResult> Update([FromBody] OverheadUpdateDto overheadDto)
    {
        return Ok(await _reportService.OverheadUpdate(overheadDto));
    }


    [HttpGet(ApiRoute.Report.ReportExport)]
    public async Task<IActionResult> Export([FromQuery] ReportFilter? reportFilter)
    {
        return Ok(await _reportService.ExcelExport(reportFilter));
    }


}
