using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Report.Core.Entities;

public class PaymentStatus:BaseKeyValueEntity, IIdentity,ISoftDeleted
{
    public int Id { get; set; }
    public bool Deleted { get; set; }
}
