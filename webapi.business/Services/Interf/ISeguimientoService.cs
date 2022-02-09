﻿using System.Collections.Generic;
using System.Threading.Tasks;
using webapi.business.Pagination;
using webapi.core.Models;

namespace webapi.business.Services.Interf
{
    public interface ISeguimientoService
    {
        IEnumerable<Seguimiento> GetAll();
        Task<PaginationList<Seguimiento>> GetAllSeguimiento(SeguimientoParametros parametros);
        IEnumerable<User> GetAllVoluntarios();
        Task<PaginationList<Seguimiento>> GetAllSeguimientoVoluntario(int idUser, SeguimientoParametros parametros);
        Task<Seguimiento> GetById(int id);
        void CreateSeguimiento(int idAdopcion);
        Task<bool> AsignarSeguimiento(int id, int idUser);
        Task<bool> QuitarAsignacion(int id, int idUser);
        Task<bool> DeleteAllSeguimiento(int idUser);
    }
}
