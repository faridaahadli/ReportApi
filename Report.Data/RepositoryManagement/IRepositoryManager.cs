using Report.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Report.Data.RepositoryManagement;

public interface IRepositoryManager
{
    IOverheadRepository Overhead { get; }
    IPackageReportRepository PackageReport { get; }
}
