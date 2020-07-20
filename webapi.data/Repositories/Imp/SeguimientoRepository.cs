using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using webapi.core.Models;
using webapi.data.Repositories.Interf;

namespace webapi.data.Repositories.Imp
{
    public class SeguimientoRepository: Repository<Seguimiento>, ISeguimientoRepository
    {
        public SeguimientoRepository(BDSpatContext context) : base(context) { }

        public async Task<Seguimiento> GetByIdContrato(int id) {
            return await context.Seguimiento.FirstAsync(x => x.ContratoAdopcionId == id);
        }
    }
}
