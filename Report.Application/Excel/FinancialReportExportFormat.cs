using Report.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Report.Application.Excel;

public class FinancialReportExportFormat
{
   [ Description("Şəxsiyyət vəs. fin kodu")]
    public string Pin { get; set; }=String.Empty;
    [Description("Ümumi adı")]
    public string Fullname { get; set; } = String.Empty;
    [Description("VÖEN")]
    public string Voen { get; set; } = String.Empty;
    [Description("Təşkilat")]
    public string OrganisationName { get; set; } = String.Empty;
    [Description("Müqavilə nömrəsi")]
    public string PackageNumber { get; set; } = String.Empty;
    [Description("Ərizə sayı")]
    public int ApplicationCount { get; set; } = 0;
    [Description("Qaimə nömrəsi")]
    public string OverheadNumber { get; set; } = String.Empty;
    [Description("Hesablanan məbləğ")]
    public decimal PackageAmount { get; set; }
    [Description("Ödənilmiş Məbləğ")]
    public decimal PaidAmount { get; set; } =0;
    [Description("Ödəniş  tarixi")]
    public string PaymentDate { get; set; } 
    [Description("Müqavilə  tarixi")]
    public string PackageCreateDate { get; set; }
    [Description("Müraciət formatı")]
    public string IsOnline { get; set; }=String.Empty;

    [Description("Ödəniş növü")]
    public string PaymentType { get; set; } = String.Empty;
    [Description("Müraciət statusu")]
    public string PackageStatus { get; set; } = String.Empty;
    [Description("Müraciət növü")]
    public string PackageType { get; set; } = String.Empty;
    [Description("Səbəb")]
    public string Reason { get; set; } = String.Empty;
    [Description("Səbəb qeydi")]
    public string Comment { get; set; }=String.Empty;
}
