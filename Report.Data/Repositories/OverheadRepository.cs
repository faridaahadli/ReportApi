using Report.Core.Entities;
using Report.Data.Persistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Report.Data.Repositories;

public class OverheadRepository : BaseRepository<Overhead>,IOverheadRepository
{
    public OverheadRepository(ReportContext context) : base(context)
    {
    }
}
