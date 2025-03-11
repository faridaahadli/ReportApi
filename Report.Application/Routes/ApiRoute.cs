using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Report.Application.Routes;

public struct ApiRoute
{
    private const string Root = "api";
    private const string Version = "v1";
    private const string Base = Root + "/" + Version;


    public struct Report
    {
        public const string  GetReports= Base + "/report";
        public const string  ReportImport= Base + "/excel/import";
        public const string  ReportExport= Base + "/excel/export";
        public const string  Update= Base + "/report";
    }
}
