using Report.Core.Entities;
using Report.Data.Persistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Report.Data.Repositories;

public class PackageReportRepository : BaseRepository<PackageReport>,IPackageReportRepository
{
    public PackageReportRepository(ReportContext context) : base(context)
    {
    }

}
