using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Report.Core.Enums;

public enum PaymentTypeEnum : byte
{
    [Description("Transfer")] Transfer = 1,
    [Description("Cash")] Cash = 2,
    [Description("Free")] Free = 3,
    [Description("Online")] Online = 4,
}
