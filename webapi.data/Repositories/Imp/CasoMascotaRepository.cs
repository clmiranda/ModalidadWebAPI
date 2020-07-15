using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using webapi.core.Models;
using webapi.data.Repositories.Interf;

namespace webapi.data.Repositories.Imp
{
    public class CasoMascotaRepository : Repository<CasoMascota>, ICasoMascotaRepository
    {
        public CasoMascotaRepository(BDSpatContext context) : base(context) { }
        public Task<CasoMascota> GetCasoMascotaByIdDenuncia(int id)
        {
            return context.CasoMascota.FirstOrDefaultAsync(x=>x.DenunciaId==id);
        }
        public int GetIdLastCasoMascota()
        {
            return int.Parse(context.CasoMascota.OrderByDescending(p=>p.Id)
                .Select(s=>s.Id)
                .FirstOrDefault().ToString());
        }
    }
}
