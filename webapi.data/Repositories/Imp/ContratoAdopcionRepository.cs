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
    public class ContratoAdopcionRepository : Repository<ContratoAdopcion>, IContratoAdopcionRepository
    {
        public ContratoAdopcionRepository(BDSpatContext context) : base(context) { }
        public async Task<IEnumerable<ContratoAdopcion>> GetAllAdopcionesPendientes() {
            return await context.ContratoAdopcion.Where(x=>x.Estado.Equals("Pendiente")||x.Estado.Equals("Aprobado")).ToListAsync();
        }
        public ContratoAdopcion GetContratoByIdMascota(int id)
        {
            return context.ContratoAdopcion.FirstOrDefault(x=>x.MascotaId==id);
        }
        public void ModifyStateMascota(int id)
        {
            var resul= context.Mascota.FirstOrDefault(x=>x.Id==id);
            resul.EstadoSituacion = "Adopcion en Proceso";
        }
        public void AprobarAdopcion(ContratoAdopcion contrato) {
            contrato.Estado = "Aprobado";
            Update(contrato);
        }

        public void RechazarAdopcion(ContratoAdopcion contrato)
        {
            contrato.Estado = "Rechazado";
            Update(contrato);
        }

        public void CancelarAdopcion(ContratoAdopcion contrato)
        {
            contrato.Estado = "Cancelado";
            Update(contrato);
        }

        public void InsertContratoRechazo(ContratoRechazo contrato)
        {
            context.ContratoRechazo.AddAsync(contrato);
        }
    }
}