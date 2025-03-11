using Report.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Report.Core.Entities;

public class BaseEntity : IIdentity
{
    public int Id { get ; set ; }
}
