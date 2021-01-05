﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using webapi.core.Models;

namespace webapi.business.Services.Interf
{
    public interface ISeguimientoService
    {
        IEnumerable<Seguimiento> GetAll();
        Task<Seguimiento> GetById(int id);
        Task<Seguimiento> GetByIdContrato(int id);
        void CreateSeguimiento(ContratoAdopcion contrato);
        Task<bool> UpdateSeguimiento(Seguimiento seguimiento);
        Task<bool> DeleteSeguimiento(Seguimiento seguimiento);
        Task<bool> CheckedVoluntarioAsignado(Seguimiento seguimiento, User user);
        Task<bool> AsignarSeguimiento(int id);
        Task<bool> RechazarSeguimiento(int id);
        IEnumerable<Seguimiento> GetSeguimientoForVoluntario(int userId);
    }
}
