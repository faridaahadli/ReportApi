using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Report.Application.Models.Report;

public class ReportGetAllDto
{
    public int Id { get; set; }
    public string Pin { get; set; } 
    public string Fullname { get; set; }
    public string Voen { get; set; } 
    public string? OrganisationName { get; set; } 
    public string PackageNumber { get; set; } 
    public int? ApplicationCount { get; set; }
    public string? OverheadNumber { get; set; } 
    public decimal PackageAmount { get; set; }
    public decimal? PaidAmount { get; set; } 
    public DateTime? PaymentDate { get; set; }
    public DateTime? PackageCreateDate { get; set; } 
    public bool IsOnline { get; set; }
    public string PaymentType { get; set; }
    public int PaymentTypeId { get; set; }
    public string PackageStatus { get; set; } 
    public int PackageStatusId { get; set; } 
}
