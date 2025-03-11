using Report.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Report.Application;

public static class ApplicationDependencyInjection
{
    public static void AddApplicationDependecyInjection(this IServiceCollection services)
    {
        services.AddServices();
    }
    private static void AddServices(this IServiceCollection services)
    {
        Assembly.GetExecutingAssembly().GetExportedTypes()
        .Where(e =>
                    e.IsClass
                    && !e.IsAbstract
                    && e.GetInterfaces().Contains(typeof(IBaseService)))
                .ToList()
                .ForEach(type =>
                {
                    var nestedInterface = type.GetInterfaces().First(x => x.GetInterfaces().Contains(typeof(IBaseService)));
                    services.AddScoped(nestedInterface, type);
                });
    }
    }
