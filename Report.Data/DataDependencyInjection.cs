using Report.Data.Persistance;
using Report.Data.Repositories;
using Report.Data.RepositoryManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Report.Data.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Report.Data.Repositories.Concrete;

namespace Report.Data;

public static class DataDependencyInjection
{
    public static IServiceCollection AddDataDependecyInjection(this IServiceCollection services,IConfiguration configuration)
    {
        services.AddDatabase(configuration);
        services.AddRepositories();  
        return services;
    }

    private static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped(typeof(IQueryRepository<>), typeof(QueryRepository<>));
        services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
       
        services.AddScoped<IRepositoryManager, RepositoryManager>();
        services.AddScoped<IPackageReportRepository, PackageReportRepository>();
        

        Assembly.GetExecutingAssembly().GetExportedTypes()
            .Where(type => type.IsClass
                && !type.IsAbstract
                && type != typeof(BaseRepository<>)
                && type.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IBaseRepository<>)))
            .ToList()
            .ForEach(type =>
            {
                var nestedInterface = type.GetInterfaces().First(i => !i.IsGenericType && i.GetInterfaces().Any(e => e.IsGenericType && e.GetGenericTypeDefinition() == typeof(IBaseRepository<>)));
                services.AddScoped(nestedInterface, type);
            });

    }
    private static void AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var reportDb=EnvironmentHelper.GetReportDb();
        var connectionString = $"Host={reportDb.Host};Port={reportDb.Port};Database={reportDb.Name};Username={reportDb.Username};Password={reportDb.Password}";
       services.AddDbContext<ReportContext>(options =>
              options.UseNpgsql(connectionString,
                  opt => opt.MigrationsAssembly(typeof(ReportContext).Assembly.FullName)));
    }
}
