using Report.Core.Entities;
using Report.Core.Enums;
using Report.Data.Helpers;
using Report.Application.Excel;
using Report.Application.Models.Report;
using Report.Data.Models;
using Report.Application.Excel;
using Report.Application.Models.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Report.Application.MappingProfiles;

public static class ReportMapper
{
    public static List<FinancialReportExportFormat> MapToExcelFormat(List<PackageReport> packageReports)
    {
        return packageReports.Select(pck => new FinancialReportExportFormat()
        {
            Pin = pck.Pin,
            Fullname = pck.Fullname,
            OrganisationName = pck?.OrganisationName??String.Empty,
            Reason=pck?.Reason?? String.Empty,
            Comment=pck?.Comment?? String.Empty,
            PackageNumber = pck.PackageNumber,
            ApplicationCount = pck.ApplicationCount ?? 0,
            PackageAmount = pck.PackageAmount,
            PaidAmount = pck.PaidAmount ?? 0,
            PaymentDate = pck.PaymentDate?.ToString("yyyy-MM-dd") ?? String.Empty,
            PackageCreateDate = pck.PackageCreateDate?.ToString()?? String.Empty,
            IsOnline=pck.IsOnline?"Online":"Offline",
            PaymentType=pck.PaymentType.GetEnumDescription(),
            PackageStatus = pck.PackageStatus.GetEnumDescription(),
            PackageType = pck.PackageType.GetEnumDescription(),        
            Voen=pck.Voen??String.Empty
        }).ToList();
    }

    public static IQueryable<ReportGetAllDto> MapPackageToPackageGetAllDto(IQueryable<PackageReport> reports)
    {
        return reports.Select(rp => new ReportGetAllDto
        {
           Id=rp.Id,
           Pin=rp.Pin,
           Fullname=rp.Fullname,
           Voen=rp.Voen,
           OrganisationName=rp.OrganisationName,
           PackageNumber=rp.PackageNumber,
           ApplicationCount =rp.ApplicationCount,
           PackageAmount=rp.PackageAmount,
           PaidAmount=rp.PaidAmount,
           PaymentDate=rp.PaymentDate,
           PackageCreateDate=rp.PackageCreateDate,
           IsOnline=rp.IsOnline,
           PaymentType = rp.PaymentType.GetEnumDescription(),
           PaymentTypeId = (int)rp.PaymentType,
           PackageStatus=rp.PackageStatus.GetEnumDescription(),
           PackageStatusId=(int)rp.PackageStatus
        });
    }

    public static List<Overhead> MapFinancialReportFormatToOverhead(IEnumerable<FinancialReportExportFormat> reports)
    {
        return reports.Select(rp => new Overhead
        {
            PackageNumber = rp.PackageNumber,
            OverheadNumber =rp.OverheadNumber            
        }).ToList();
    }

    public static FinancialReportInvalidDataFormat MapOverheadToFinancialReportFormat(Overhead overhead)
    {
        return new FinancialReportInvalidDataFormat
        {
            PackageNumber = overhead.PackageNumber,
            OverheadNumber = overhead.OverheadNumber
        };
    }
}
