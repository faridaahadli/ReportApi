using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Report.Application.Excel;

public class FinancialReportInvalidDataFormat
{
    [Description("Müqavilə nömrəsi")]
    public string PackageNumber { get; set; } = String.Empty;
    [Description("Qaimə nömrəsi")]
    public string OverheadNumber { get; set; } = String.Empty;
    [Description("Yüklənməmə səbəbi")]
    public string Reason { get; set; } = String.Empty;
}
