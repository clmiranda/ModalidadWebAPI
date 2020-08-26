using System;
using System.Collections.Generic;
using System.Text;
using webapi.core.Models;
using webapi.data.Repositories.Interf;

namespace webapi.data.Repositories.Imp
{
    public class ReporteSeguimientoRepository: Repository<ReporteSeguimiento>, IReporteSeguimientoRepository
    {
        public ReporteSeguimientoRepository(BDSpatContext context) : base(context) { }


    }
}
