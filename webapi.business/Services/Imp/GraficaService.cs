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
        private readonly List<string> listaFiltro;
        public GraficaService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            listaFiltro = new List<string> { "3 meses", "6 meses", "9 meses", "12 meses" };
        }
        public async Task<List<DataGraficaDto>> DatosAdopciones(string filtro)
        {
            if (string.IsNullOrEmpty(filtro) || !listaFiltro.Contains(filtro))
                return null;
            var datos = new List<ContratoAdopcion>();
            switch (filtro)
            {
                case "3 meses":
                    datos = await _unitOfWork.ContratoAdopcionRepository.FindByCondition(x => x.FechaSolicitudAdopcion.Date >= DateTime.Today.AddMonths(-3) && x.FechaSolicitudAdopcion.Date <= DateTime.Now.Date).ToListAsync();
                    break;
                case "6 meses":
                    datos = await _unitOfWork.ContratoAdopcionRepository.FindByCondition(x => x.FechaSolicitudAdopcion.Date >= DateTime.Today.AddMonths(-6) && x.FechaSolicitudAdopcion.Date <= DateTime.Now.Date).ToListAsync();
                    break;
                case "9 meses":
                    datos = await _unitOfWork.ContratoAdopcionRepository.FindByCondition(x => x.FechaSolicitudAdopcion.Date >= DateTime.Today.AddMonths(-9) && x.FechaSolicitudAdopcion.Date <= DateTime.Now.Date).ToListAsync();
                    break;
                case "12 meses":
                    datos = await _unitOfWork.ContratoAdopcionRepository.FindByCondition(x => x.FechaSolicitudAdopcion.Date >= DateTime.Today.AddMonths(-12) && x.FechaSolicitudAdopcion.Date <= DateTime.Now.Date).ToListAsync();
                    break;
            }
            var lista = datos.OrderBy(x => x.FechaSolicitudAdopcion.Date)
                .GroupBy(x => x.FechaSolicitudAdopcion.Date.ToString("MMMM yyyy"))
    .Select(x => new DataGraficaDto()
    {
        Nombre = x.Key,
        Cantidad = x.Count()
    }).ToList();
            return lista;
        }
        public async Task<List<DataGraficaDto>> DatosMascotas(string filtro)
        {
            if (string.IsNullOrEmpty(filtro) || !listaFiltro.Contains(filtro))
                return null;
            var datos = new List<Mascota>();
            switch (filtro)
            {
                case "3 meses":
                    datos = await _unitOfWork.MascotaRepository.FindByCondition(x => x.FechaCreacion.Date >= DateTime.Today.AddMonths(-3) && x.FechaCreacion.Date <= DateTime.Now.Date).ToListAsync();
                    break;
                case "6 meses":
                    datos = await _unitOfWork.MascotaRepository.FindByCondition(x => x.FechaCreacion.Date >= DateTime.Today.AddMonths(-6) && x.FechaCreacion.Date <= DateTime.Now.Date).ToListAsync();
                    break;
                case "9 meses":
                    datos = await _unitOfWork.MascotaRepository.FindByCondition(x => x.FechaCreacion.Date >= DateTime.Today.AddMonths(-9) && x.FechaCreacion.Date <= DateTime.Now.Date).ToListAsync();
                    break;
                case "12 meses":
                    datos = await _unitOfWork.MascotaRepository.FindByCondition(x => x.FechaCreacion.Date >= DateTime.Today.AddMonths(-12) && x.FechaCreacion.Date <= DateTime.Now.Date).ToListAsync();
                    break;
            }
            var lista = datos.OrderBy(x => x.FechaCreacion.Date)
                .GroupBy(x => x.FechaCreacion.Date.ToString("MMMM yyyy"))
                .Select(x => new DataGraficaDto()
                {
                    Nombre = x.Key,
                    Cantidad = x.Count()
                }).ToList();
            return lista;
        }
        public async Task<List<DataGraficaDto>> DatosReporteSeguimientos(string filtro)
        {
            if (string.IsNullOrEmpty(filtro) || !listaFiltro.Contains(filtro))
                return null;
            var datos = new List<ReporteSeguimiento>();
            switch (filtro)
            {
                case "3 meses":
                    datos = await _unitOfWork.ReporteSeguimientoRepository.FindByCondition(x => x.Fecha.Date >= DateTime.Today.AddMonths(-3) && x.Fecha.Date <= DateTime.Now.Date).ToListAsync();
                    break;
                case "6 meses":
                    datos = await _unitOfWork.ReporteSeguimientoRepository.FindByCondition(x => x.Fecha.Date >= DateTime.Today.AddMonths(-6) && x.Fecha.Date <= DateTime.Now.Date).ToListAsync();
                    break;
                case "9 meses":
                    datos = await _unitOfWork.ReporteSeguimientoRepository.FindByCondition(x => x.Fecha.Date >= DateTime.Today.AddMonths(-9) && x.Fecha.Date <= DateTime.Now.Date).ToListAsync();
                    break;
                case "12 meses":
                    datos = await _unitOfWork.ReporteSeguimientoRepository.FindByCondition(x => x.Fecha.Date >= DateTime.Today.AddMonths(-12) && x.Fecha.Date <= DateTime.Now.Date).ToListAsync();
                    break;
            }
            var lista = datos.Where(x => x.Estado.Equals("Enviado"))
                .OrderBy(x => x.Fecha.Date)
                .GroupBy(x => x.Fecha.Date.ToString("MMMM yyyy"))
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

                ContadorAdopcionesAprobadas = await _unitOfWork.ContratoAdopcionRepository.FindByCondition(x => x.Estado.Equals("Aprobado")).CountAsync(),

                ContadorAdopcionesRechazadas = await _unitOfWork.ContratoAdopcionRepository.FindByCondition(x => x.Estado.Equals("Rechazado")).CountAsync(),

                ContadorAdopcionesCanceladas = await _unitOfWork.ContratoAdopcionRepository.FindByCondition(x => x.Estado.Equals("Cancelado")).CountAsync(),

                ContadorSeguimientosActuales = await _unitOfWork.SeguimientoRepository.GetAll().CountAsync(),

                ContadorVoluntariosRegistrados = await _unitOfWork.UserRepository.FindByCondition(x => x.UserRoles.Any(y => y.Role.Name.Equals("Voluntario"))).CountAsync(),

                ContadorReportesSemana = await _unitOfWork.ReporteSeguimientoRepository.FindByCondition(x => x.Estado.Equals("Enviado") && x.Fecha.Date >= DateTime.Today.AddDays(-(int)DayOfWeek.Monday) && x.Fecha.Date <= DateTime.Today.AddDays((int)DayOfWeek.Sunday)).CountAsync(),

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

            var listaSeguimientos = await _unitOfWork.SeguimientoRepository.GetAll().ToListAsync();
            dataForDashboardDto.DataGraficaSeguimiento = listaSeguimientos
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
