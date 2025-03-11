using Report.Application.Models.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Report.Application.Models.Report;

public class ReportGetAllResponse
{
    public PagedList<ReportGetAllDto> Reports { get; set; } = null!;
}
