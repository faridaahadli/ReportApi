using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Report.Data.RepositoryManagement;
using Report.Application.Excel;
using Report.Application.Models.Pagination;
using Report.Application.Models.Report;
using Report.Core.Entities;
using Report.Application.Models;
using Report.Application.Helpers;
using Report.Application.MappingProfiles;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Report.Application.ExceptionHandle.Models;
using Report.Application.Excel;
using Report.Application.Models.Report;
using static Dapper.SqlMapper;
using System.Text.RegularExpressions;
using Report.Application.Helpers;


namespace Report.Application.Services;

public class ReportService : IBaseService, IReportService
{
    private readonly IRepositoryManager _manager;
    public ReportService(IRepositoryManager manager)
    {
        _manager = manager;
    }

    public async Task<ReportGetAllResponse> GetReportsAsync(PaginationFilter paginationFilter,
    ReportFilter reportFilter)
    {
        var reportPredicate = PredicateBuilder.New<PackageReport>();
        reportPredicate = CreateReportFilterQuery(reportPredicate, reportFilter);

        var query = _manager.PackageReport
            .GetQuery(reportPredicate)
            .OrderByDescending(x => x.PackageCreateDate);
            
        var allReports = ReportMapper.MapPackageToPackageGetAllDto(query);


        var pagedReports = await PagedList<ReportGetAllDto>.CreateAsync(allReports, paginationFilter.PageNumber, paginationFilter.PageSize);

        
        var packageNumbers = pagedReports.Items.Select(r => r.PackageNumber).ToList();

        
        var overheadNumbers = await _manager.Overhead
            .GetQuery(x => packageNumbers.Contains(x.PackageNumber))
            .ToDictionaryAsync(x => x.PackageNumber, x => x.OverheadNumber);

        pagedReports.Items.ForEach(item =>
        {
            item.OverheadNumber = overheadNumbers.ContainsKey(item.PackageNumber)
                ? overheadNumbers[item.PackageNumber]
                : null;
        });

        if (reportFilter.OverheadNumber is not null)
        {
            pagedReports.Items = pagedReports.Items
                .Where(x => x.OverheadNumber == reportFilter.OverheadNumber)
                .ToList();
        }

        return new ReportGetAllResponse()
        {
            Reports = pagedReports
        };
    }

    public async Task<byte[]> ExcelExport(ReportFilter? reportFilter)
    {

        var reportPredicate = PredicateBuilder.New<PackageReport>();
        reportPredicate = CreateReportFilterQuery(reportPredicate, reportFilter);
        var reports = await _manager.PackageReport.GetAllAsync(reportPredicate);
        var excelDatas = ReportMapper.MapToExcelFormat(reports.OrderByDescending(p=>p.PackageCreateDate).ToList());
        excelDatas.ForEach(item =>
        {
            var overhead = _manager.Overhead
            .GetQuery(x => x.PackageNumber == item.PackageNumber).FirstOrDefault();
            item.OverheadNumber = overhead is null ? String.Empty : overhead.OverheadNumber;
        });
        if (reportFilter?.OverheadNumber is not null)
            excelDatas = excelDatas.Where(x => x.OverheadNumber == reportFilter.OverheadNumber).ToList();
        return ExcelOperation.Export(excelDatas);

    }

    public async Task<string> OverheadUpdate(OverheadUpdateDto overheadDto)
    {
        
        if (!await _manager.PackageReport.AnyAsync(x => x.PackageNumber == overheadDto.PackageNumber))
            throw new NotFoundException("Qeyd olunan müqavilə nömrəsi sistemdə mövcud deyil.");
        var overhead=await _manager.Overhead.GetFirstOrDefaultAsync(x=>x.PackageNumber== overheadDto.PackageNumber)
            ?? throw new BadRequestException("Yalnız sistemə öncədən yüklənən qaimələrdə düzəliş edilə bilər.");
        if (String.IsNullOrEmpty(overheadDto.OverheadNumber))
            throw new BadRequestException("Qaimə nömrəsi mütləq qeyd edilməlidir.");
        if (await _manager.Overhead.AnyAsync(x => x.OverheadNumber == overheadDto.OverheadNumber))
            throw new BadRequestException("Bu qaimə nömrəsi sistemdə digər müqavilə üçün aşkar edilmişdir.");
        if(!RegexChecker.DigitsAndLettersCheck(overheadDto.OverheadNumber))
            throw new BadRequestException("Qaimə nömrəsi yalnız hərf və rəqəmlərdən ibarət ola bilər.");
        overhead.OverheadNumber= overheadDto.OverheadNumber;
        await _manager.Overhead.UpdateAsync(overhead);
        await _manager.Overhead.SaveChangesAsync();
        return overhead.PackageNumber;

    }

    public async Task<byte[]> ExcelImport(IFormFile file)
    {
        CheckFileFormat(file);
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "import.xlsx");
        using (FileStream stream = new FileStream(filePath, FileMode.Create))
        {
            file.CopyTo(stream);
        };
        List<string> properties = new List<string>() { "OverheadNumber", "PackageNumber" };
        var result = ExcelOperation.Import<FinancialReportExportFormat>(filePath, properties);
        if (result.Any(x => String.IsNullOrEmpty(x.PackageNumber)))
            throw new BadRequestException("Müqavilə nömrəsi olmayan xana aşkar edilmişdir.");
        result = result.Where(x => !String.IsNullOrEmpty(x.OverheadNumber)).ToList();
        if (result.Count == 0)
            throw new BadRequestException("Əlavə edilən faylda heç bir qaimə aşkar edilməmişdir.");
        if (File.Exists(filePath))
            File.Delete(filePath);
        HashSet<FinancialReportInvalidDataFormat> existingOverheads = new();
        var overheads = ReportMapper.MapFinancialReportFormatToOverhead(result);     
        foreach (var item in overheads)
        {
            if (!await _manager.PackageReport
                .AnyAsync(x => x.PackageNumber == item.PackageNumber))
            {
                var existingOverhead = new FinancialReportInvalidDataFormat()
                {
                    PackageNumber = item.PackageNumber,
                    OverheadNumber = item.OverheadNumber,
                    Reason = "Qeyd olunan müqavilə nömrəsi sistemdə mövcud deyil."
                };
                existingOverheads.Add(existingOverhead);
                continue;
            }
            else if (await _manager.Overhead
             .AnyAsync(x => x.PackageNumber == item.PackageNumber))
            {
                var overHeadEntity = await _manager.Overhead.GetFirstAsync(x => x.PackageNumber == item.PackageNumber);
                var existingOverhead = ReportMapper.MapOverheadToFinancialReportFormat(overHeadEntity);
                existingOverhead.Reason = "Göndərilən müqavilə nömrəsi sistemdə qeyd olunan " +
                   "qaimə nömrəsi ilə artıq mövcuddur.";
                existingOverheads.Add(existingOverhead);
                continue;
            }
            else if (await _manager.Overhead
             .AnyAsync(x => x.OverheadNumber == item.OverheadNumber))
            {
                var overHeadEntity = await _manager.Overhead.GetFirstAsync(x => x.OverheadNumber == item.OverheadNumber);
                var existingOverhead = ReportMapper.MapOverheadToFinancialReportFormat(overHeadEntity);
                existingOverhead.Reason = "Göndərilən qaimə nömrəsi sistemdə qeyd olunan müqavilə" +
                    "nömrəsi ilə artıq mövcuddur.";
                existingOverheads.Add(existingOverhead);
                continue;
            }
            else if (!RegexChecker.DigitsAndLettersCheck(item.OverheadNumber))
            {
                var overHeadEntity = new Overhead() {PackageNumber=item.PackageNumber,OverheadNumber=item.OverheadNumber};
                var existingOverhead = ReportMapper.MapOverheadToFinancialReportFormat(overHeadEntity);
                existingOverhead.Reason = "Qaimə nömrəsi yalnız hərf və rəqəmlərdən ibarət ola bilər.";
                existingOverheads.Add(existingOverhead);
                continue;
            }
            await _manager.Overhead.AddAsync(new Overhead()
            {
                OverheadNumber = item.OverheadNumber,
                PackageNumber = item.PackageNumber,

            });
        }

        await _manager.Overhead.SaveChangesAsync();
        if (existingOverheads.Count > 0)
            return ExcelOperation.Export(existingOverheads.ToList());
        return null;
    }
    private void CheckFileFormat(IFormFile file)
    {
        if (file == null || file.Length <= 0) throw new BadRequestException("File əlavə olunmayıb");
        var validMimeTypes = new List<string>
        {
            "application/vnd.ms-excel", // for .xls
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" // for .xlsx
        };

        if (!validMimeTypes.Contains(file.ContentType))
            throw new BadRequestException("Fayl formatı yalnışdır.");

    }

    private static Expression<Func<PackageReport, bool>>? CreateReportFilterQuery(Expression<Func<PackageReport, bool>>? predicate, ReportFilter? reportFilter)
    {
        if (reportFilter is null || predicate is null) return predicate;

        if (reportFilter.PackageCreateDateStart is not null &&
           reportFilter.PackageCreateDateEnd is not null &&
           reportFilter.PackageCreateDateEnd.Value.Date < reportFilter.PackageCreateDateStart.Value.Date)
            throw new BadRequestException("Sifariş tarixində başlanğıc tarixi son tarixdən böyük seçilə bilməz");

        if (reportFilter.PaymentDateStart is not null &&
         reportFilter.PaymentDateEnd is not null &&
         reportFilter.PaymentDateEnd.Value.Date < reportFilter.PaymentDateStart.Value.Date)
            throw new BadRequestException("Ödənilmə tarixində başlanğıc tarixi son tarixdən böyük seçilə bilməz");


        predicate = reportFilter.PackageStatusId is not null
            ? predicate.And(x => reportFilter.PackageStatusId.Contains((int)x.PackageStatus))
            : predicate;

        predicate = reportFilter.Pin is not null
         ? predicate.And(x => x.Pin.ToLower().Trim().Contains(reportFilter.Pin.ToLower().Trim()))
         : predicate;

        predicate = reportFilter.PaymentTypeId is not null
            ? predicate.And(x => reportFilter.PaymentTypeId.Contains((int)x.PaymentType))
            : predicate;

        predicate = reportFilter.PaymentDateStart is not null
             ? predicate.And(x => x.PaymentDate.Value.Date >= reportFilter.PaymentDateStart.Value.Date)
             : predicate;

        predicate = reportFilter.PaymentDateEnd is not null
           ? predicate.And(x => x.PaymentDate.Value.Date <= reportFilter.PaymentDateEnd.Value.Date)
           : predicate;

        predicate = reportFilter.PackageCreateDateStart is not null
           ? predicate.And(x => x.PackageCreateDate.Value.Date >= reportFilter.PackageCreateDateStart.Value.Date)
           : predicate;
        predicate = reportFilter.PackageCreateDateEnd is not null
           ? predicate.And(x => x.PackageCreateDate.Value.Date <= reportFilter.PackageCreateDateEnd.Value.Date)
           : predicate;

        predicate = reportFilter.CompanyName is not null
            ? predicate.And(x =>
                x.OrganisationName.ToLower().Trim().Contains(reportFilter.CompanyName.ToLower().Trim()))
            : predicate;

        predicate = reportFilter.FullName is not null
           ? predicate.And(x =>
           x.Fullname.ToLower().Trim().Contains(reportFilter.FullName.ToLower().Trim()))
           : predicate;

        predicate = reportFilter.Voen is not null
            ? predicate.And(x => x.Voen.Trim().Contains(reportFilter.Voen.Trim()))
            : predicate;


        predicate = reportFilter.PackageNumber is not null
            ? predicate.And(
                x => x.PackageNumber.ToLower().Trim().Contains(reportFilter.PackageNumber.ToLower().Trim())
            )
            : predicate;

        predicate = reportFilter.IsOnline is not null
           ? predicate.And(
               x => x.IsOnline == reportFilter.IsOnline
           )
           : predicate;
        return predicate;
    }



}
