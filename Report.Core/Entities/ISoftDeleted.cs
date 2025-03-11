using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Report.Core.Entities;

public interface ISoftDeleted
{
    public bool Deleted { get; set; }
}
