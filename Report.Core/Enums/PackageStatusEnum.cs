using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Report.Core.Enums;

public enum PackageStatusEnum:byte
{
    [Description("Status-1")] Status1 = 1,
    [Description("Status-2")] Status2 = 2,
    [Description("Status-3")] Status3 = 3,
    [Description("Status-4")] Status4 = 4,
    [Description("Status-5")] Status5 = 5,
    [Description("Status-6")] Status6 = 6,
    [Description("Status-7")] Status7 = 7,
    [Description("Status-8")] Status8 = 8,
    [Description("Status-9")] Status9 = 9
}
