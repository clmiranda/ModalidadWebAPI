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
    public class MascotaRepository : Repository<Mascota>, IMascotaRepository
    {
        public MascotaRepository(BDSpatContext context) : base(context) { }
        public IEnumerable<Foto> GetAllFotosMascota(int id)
        {
            var listaFotos = context.Foto.Where(x=>x.MascotaId==id).ToList();
            return listaFotos;
        }
        public IEnumerable<Mascota> GetAllMascotaAdopcion()
        {
            var listaAdopcion = context.Mascota.Where(x => x.EstadoSituacion.Equals("En Adopcion")).ToList();
            return listaAdopcion;
        }
        public async Task<Foto> GetFoto(int id)
        {
            var foto = await context.Foto.FirstOrDefaultAsync(x => x.Id == id);
            return foto;
        }

        public int GetIdLastMascota()
        {
            return int.Parse(context.Mascota.OrderByDescending(p => p.Id)
                .Select(s => s.Id)
                .FirstOrDefault().ToString());
        }

        public Task<Mascota> GetMascotaByIdCaso(int id)
        {
            return context.Mascota.FirstOrDefaultAsync(x=>x.CasoMascotaId==id);
        }
        public async Task<bool> SaveAll()
        { return await context.SaveChangesAsync() > 0; }
        public void AprobarAdopcion(Mascota mascota)
        {
            mascota.EstadoSituacion = "Adoptada";
            Update(mascota);
        }

        public void RechazarAdopcion(Mascota mascota)
        {
            mascota.EstadoSituacion = "En Adopcion";
            Update(mascota);
        }

        public void CancelarAdopcion(Mascota mascota)
        {
            mascota.EstadoSituacion = "Registrado";
            Update(mascota);
        }

        public void ContratoEstadoMascota(Mascota mascota)
        {
            mascota.EstadoSituacion = "Adopcion en Proceso";
            Update(mascota);
        }
    }
}