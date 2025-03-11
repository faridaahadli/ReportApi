using Report.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Report.Core.Entities;

public class Overhead:BaseEntity
{
    public string PackageNumber{ get; set; }
    public string OverheadNumber { get; set; }
    public DateTime CreateDate { get; set; }= DateTime.Now; 
  
}
