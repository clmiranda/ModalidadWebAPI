﻿using System;
using System.Collections.Generic;
using System.Text;
using webapi.core.Models;

namespace webapi.data.Repositories.Interf
{
    public interface IContratoAdopcionRepository: IRepository<ContratoAdopcion>
    {
        IEnumerable<ContratoAdopcion> GetAllAdopcionesPendientes();
        ContratoAdopcion GetContratoByIdMascota(int id);
        void ModifyStateMascota(int id);
        void AprobarAdopcion(ContratoAdopcion contrato);
        void RechazarAdopcion(ContratoAdopcion contrato);
        void CancelarAdopcion(ContratoAdopcion contrato);
    }
}
