using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using webapi.core.Models;

namespace webapi.data.Repositories.Interf
{
    public interface ISeguimientoRepository: IRepository<Seguimiento>
    {
        Task<Seguimiento> GetByIdContrato(int id);
        //IEnumerable<Seguimiento> GetAllSeguimiento();
    }
}