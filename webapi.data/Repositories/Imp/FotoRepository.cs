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
    public class FotoRepository: Repository<Foto>, IFotoRepository
    {
        public FotoRepository(BDSpatContext context) : base(context) { }

        public async Task<Foto> GetPhotoPrincipalMascota(int id)
        {
            var foto = await FindByCondition(x => x.Mascota.Id == id).ToListAsync();
            return foto.FirstOrDefault(x => x.IsPrincipal);
        }
    }
}
