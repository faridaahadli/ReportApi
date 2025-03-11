using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Report.Core.Entities;

public class BaseKeyValueEntity
{
    [StringLength(500)] public string Name { get; set; }
}
