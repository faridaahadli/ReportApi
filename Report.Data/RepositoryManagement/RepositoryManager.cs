using Report.Data.Persistance;
using Report.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Report.Data.RepositoryManagement;

public class RepositoryManager:IRepositoryManager
{
    private readonly ReportContext _context;
    public RepositoryManager(ReportContext context)
    {
            _context = context;
    }


    private Lazy<IOverheadRepository> OverheadRepository =>
        CreateLazy<IOverheadRepository>(ctx => new OverheadRepository(ctx));

    

    private Lazy<IPackageReportRepository> PackageReportRepository =>
        CreateLazy<IPackageReportRepository>(ctx => new PackageReportRepository(ctx));
    private Lazy<T> CreateLazy<T>(Func<ReportContext, T> repositoryFactory)
    {
       return new Lazy<T>(()=>repositoryFactory(_context));
    }

    public IOverheadRepository Overhead => OverheadRepository.Value;
    public IPackageReportRepository PackageReport => PackageReportRepository.Value;
}
