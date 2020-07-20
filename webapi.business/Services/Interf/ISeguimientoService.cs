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
        Task<Seguimiento> GetByIdContrato(int id);
        Task<bool> CreateSeguimiento(Seguimiento seguimiento);
        Task<bool> DeleteSeguimiento(Seguimiento seguimiento);
    }
}
