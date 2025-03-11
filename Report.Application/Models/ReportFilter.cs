using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Report.Application.Models;

public class ReportFilter
{
    //public List<int> PackageTypeId { get; init; }
    public List<int> PackageStatusId { get; set; }
    public string? Pin { get; set; }
    public List<int> PaymentTypeId { get; set; }
    public string? CompanyName { get; set; } 
    public string? OverheadNumber { get; set; } 
    public string? PackageNumber { get; set; } 
    public string? FullName { get; set; } 
    public string? Voen { get; set; }
    public DateTime? PaymentDateStart { get; set; }
    public DateTime? PaymentDateEnd { get; set; }
    public DateTime? PackageCreateDateStart { get; set; }
    public DateTime? PackageCreateDateEnd { get; set; }
    public bool? IsOnline { get; set; }
}
