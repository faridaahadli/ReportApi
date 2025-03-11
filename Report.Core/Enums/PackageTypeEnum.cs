using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Report.Core.Enums;

 public enum PackageTypeEnum:byte
{
    [Description("Type-1")] Citizens = 1,
    [Description("Type-2")] LegalEntities,
    [Description("Type-3")] Owner,
    [Description("Type-4")] GovernmentAgency
}
