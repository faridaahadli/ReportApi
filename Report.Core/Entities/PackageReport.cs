using Report.Core.Enums;
using Report.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Report.Core.Entities;

public class PackageReport:BaseEntity
{
    public string Pin { get; set; }
    public string Fullname { get; set; }
    public string? Voen { get; set; }
    public string? OrganisationName { get; set; }
    public string? Reason { get; set; } 
    public string? Comment { get; set; } 
    public string PackageNumber { get; set; }
    public int? ApplicationCount { get; set; }    

    [Column(TypeName = "decimal(8,2)")]
    public decimal PackageAmount { get; set; }
    [Column(TypeName = "decimal(8,2)")]
    public decimal? PaidAmount { get; set; }
    public DateTime? PaymentDate { get; set; }
    public DateTime? PackageCreateDate { get; set; }
    public bool IsOnline { get; set; }
    public DateTime? UpdateDate { get; set; }
    public PaymentTypeEnum  PaymentType { get; set; }
    public PackageStatusEnum PackageStatus { get; set; }
    public PackageTypeEnum PackageType{ get; set; }
}
