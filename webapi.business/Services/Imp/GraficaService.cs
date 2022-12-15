using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webapi.business.Dtos.Graficas;
using webapi.business.Services.Interf;
using webapi.core.Models;
using webapi.data.Repositories.Interf;

namespace webapi.business.Services.Imp
{
    public class GraficaService : IGraficaService
    {
        private readonly IUnitOfWork _unitOfWork;
        public GraficaService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<List<DataGraficaDto>> DatosAdopciones(string[] fechas)
        {
            var datos = new List<SolicitudAdopcion>();
            datos = await _unitOfWork.SolicitudAdopcionRepository.FindByCondition(x => x.FechaSolicitudAdopcion.Date >= Convert.ToDateTime(fechas[0]) && x.FechaSolicitudAdopcion.Date <= Convert.ToDateTime(fechas[1])).ToListAsync();

            var lista = datos.OrderBy(x => x.FechaSolicitudAdopcion.Date)
                .GroupBy(x => x.FechaSolicitudAdopcion.Date.ToString("MMMM yyyy"))
    .Select(x => new DataGraficaDto()
    {
        Nombre = x.Key,
        Cantidad = x.Count()
    }).ToList();
            return lista;
        }
        public async Task<List<DataGraficaDto>> DatosMascotas(string[] fechas)
        {
            var datos = new List<Mascota>();
            datos = await _unitOfWork.MascotaRepository.FindByCondition(x => x.FechaCreacion.Date >= Convert.ToDateTime(fechas[0]) && x.FechaCreacion.Date <= Convert.ToDateTime(fechas[1])).ToListAsync();
            var lista = datos.OrderBy(x => x.FechaCreacion.Date)
                .GroupBy(x => x.FechaCreacion.Date.ToString("MMMM yyyy"))
                .Select(x => new DataGraficaDto()
                {
                    Nombre = x.Key,
                    Cantidad = x.Count()
                }).ToList();
            return lista;
        }
        public async Task<List<DataGraficaDto>> DatosReporteSeguimientos(string[] fechas)
        {
            var datos = new List<ReporteSeguimiento>();
            datos = await _unitOfWork.ReporteSeguimientoRepository.FindByCondition(x => x.FechaCreacion.Date >= Convert.ToDateTime(fechas[0]) && x.FechaCreacion.Date <= Convert.ToDateTime(fechas[1])).ToListAsync();
            var lista = datos.Where(x => x.Estado.Equals("Enviado"))
                .OrderBy(x => x.FechaReporte.Date)
                .GroupBy(x => x.FechaReporte.Date.ToString("MMMM yyyy"))
                .Select(x => new DataGraficaDto()
                {
                    Nombre = x.Key,
                    Cantidad = x.Count()
                }).ToList();
            return lista;
        }
        public async Task<DataForDashboardDto> GetDataForDashboard()
        {
            var dataForDashboardDto = new DataForDashboardDto
            {
                ContadorMascotasRegistradas = await _unitOfWork.MascotaRepository.GetAll().CountAsync(),

                ContadorAdopcionesAprobadas = await _unitOfWork.SolicitudAdopcionRepository.FindByCondition(x => x.Estado.Equals("Aprobado")).CountAsync(),

                ContadorAdopcionesRechazadas = await _unitOfWork.SolicitudAdopcionRepository.FindByCondition(x => x.Estado.Equals("Rechazado")).CountAsync(),

                ContadorAdopcionesCanceladas = await _unitOfWork.SolicitudAdopcionRepository.FindByCondition(x => x.Estado.Equals("Cancelado")).CountAsync(),

                ContadorSeguimientosActuales = await _unitOfWork.SeguimientoRepository.GetAll().CountAsync(),

                ContadorVoluntariosRegistrados = await _unitOfWork.UserRepository.FindByCondition(x => x.UserRoles.Any(y => y.Role.Name.Equals("Voluntario"))).CountAsync(),

                ContadorReportesSemana = await _unitOfWork.ReporteSeguimientoRepository.FindByCondition(x => x.Estado.Equals("Enviado") && x.FechaReporte.Date >= DateTime.Today.AddDays(-(int)DayOfWeek.Monday) && x.FechaReporte.Date <= DateTime.Today.AddDays((int)DayOfWeek.Sunday)).CountAsync(),

                ContadorDenunciasRegistradas = await _unitOfWork.DenunciaRepository.GetAll().CountAsync()
            };

            var listaMascotas = await _unitOfWork.MascotaRepository.GetAll().ToListAsync();
            dataForDashboardDto.DataGraficaMascota = listaMascotas
                 .OrderBy(x => x.Estado)
                 .GroupBy(x => x.Estado)
                 .Select(x => new DataGraficaDto()
                 {
                     Nombre = x.Key,
                     Cantidad = x.Count()
                 }).ToList();

            var listaSolicitudesAdopcion = await _unitOfWork.SolicitudAdopcionRepository.GetAll().ToListAsync();
            dataForDashboardDto.DataGraficaSolicitudAdopcion = listaSolicitudesAdopcion
                 .OrderBy(x => x.Estado)
                 .GroupBy(x => x.Estado)
                 .Select(x => new DataGraficaDto()
                 {
                     Nombre = x.Key,
                     Cantidad = x.Count()
                 }).ToList();
            return dataForDashboardDto;
        }
    }
}
