using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using webapi.core.Models;

namespace webapi.business.Services.Interf
{
    public interface ISeguimientoService
    {
        Task<Seguimiento> GetById(int id);
        Task<bool> CreateSeguimiento(Seguimiento seguimiento);
    }
}
