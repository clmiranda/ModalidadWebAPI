using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
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
        private readonly IMapper _mapper;
        public GraficaService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<List<DataGraficaDto>> DatosAdopciones(string filtro)
        {
            //int a = 0, b = 0, c = 0;
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
            //a = datos.Count(x => x.Estado.Equals("Aprobado"));
            //b = datos.Count(x => x.Estado.Equals("Rechazado"));
            //c = datos.Count(x => x.Estado.Equals("Cancelado"));
            //var lista = new List<DataGraficaDto>
            //{
            //    new DataGraficaDto
            //    {
            //        Nombre = "Aprobado",
            //        Cantidad = a
            //    },
            //    new DataGraficaDto
            //    {
            //        Nombre = "Rechazado",
            //        Cantidad = b
            //    },
            //    new DataGraficaDto
            //    {
            //        Nombre = "Cancelado",
            //        Cantidad = c
            //    }
            //};
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
    }
}
