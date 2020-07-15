using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using webapi.core.Models;
using webapi.data.Repositories.Interf;

namespace webapi.data.Repositories.Imp
{
    public class ContratoAdopcionRepository : Repository<ContratoAdopcion>, IContratoAdopcionRepository
    {
        public ContratoAdopcionRepository(BDSpatContext context) : base(context) { }
        public IEnumerable<ContratoAdopcion> GetAllAdopcionesPendientes() {
            return context.ContratoAdopcion.Where(x=>x.Estado.Equals("En Proceso")).ToList();
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
    }
}