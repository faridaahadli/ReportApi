using Report.Core.Entities;
using Report.Core.Enums;
using Report.Data.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Seeders;

internal class SeedConstant
{
    public static void SeedPackageStatus(ModelBuilder builder)
    {
        builder.Entity<PackageStatus>()
            .HasData(
                Enum.GetValues<PackageStatusEnum>().Select(x => new PackageStatus
                {
                    Id = (int)x,
                    Name = x.GetEnumDescription()
                }));
    }
    public static void SeedPackageType(ModelBuilder builder)
    {
        builder.Entity<PackageType>()
            .HasData(
                Enum.GetValues<PackageTypeEnum>().Select(x => new PackageType
                {
                    Id = (int)x,
                    Name = x.GetEnumDescription()
                }));
    }
    public static void SeedPaymentStatus(ModelBuilder builder)
    {
        builder.Entity<PaymentStatus>()
            .HasData(
                Enum.GetValues<PaymentTypeEnum>().Select(x => new PaymentStatus
                {
                    Id = (int)x,
                    Name = x.GetEnumDescription()
                }));
    }
}
