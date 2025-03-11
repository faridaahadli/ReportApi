using Report.Core.Entities;
using Data.Seeders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Report.Data.Persistance;

public class ReportContext : DbContext
{
    public ReportContext(DbContextOptions<ReportContext> options)
        : base(options) { }

    public DbSet<PackageReport> PackageReports { get; set; }
    public DbSet<Overhead> Overheads { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        SeedDatabase(modelBuilder);
    }

    private void SeedDatabase(ModelBuilder modelBuilder)
    {
        SeedConstant.SeedPackageStatus(modelBuilder);
        SeedConstant.SeedPackageType(modelBuilder);
        SeedConstant.SeedPaymentStatus(modelBuilder);
    }
}
