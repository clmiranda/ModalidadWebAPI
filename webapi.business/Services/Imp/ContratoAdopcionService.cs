using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using webapi.business.Dtos.ContratoRechazo;
using webapi.business.Services.Interf;
using webapi.core.Models;
using webapi.data.Repositories.Interf;

namespace webapi.business.Services.Imp
{
    public class ContratoAdopcionService : IContratoAdopcionService
    {
        private IUnitOfWork _unitOfWork;
        private ISeguimientoService _seguimientoService;
        public ContratoAdopcionService(IUnitOfWork unitOfWork, ISeguimientoService seguimientoService)
        {
            _unitOfWork = unitOfWork;
            _seguimientoService = seguimientoService;
        }
        public async Task<ContratoAdopcion> GetById(int id)
        {
            return await _unitOfWork.ContratoAdopcionRepository.GetById(id);
        }
        public async Task<IEnumerable<ContratoAdopcion>> GetAllAdopcionesPendientes() {
            return await _unitOfWork.ContratoAdopcionRepository.GetAllAdopcionesPendientes();
        }
        public async Task<bool> CreateContratoAdopcion(ContratoAdopcion contrato)
        {
            _unitOfWork.ContratoAdopcionRepository.Insert(contrato);
            return await _unitOfWork.SaveAll();
        }
        public async Task<bool> UpdateContratoAdopcion(ContratoAdopcion contrato) {
            _unitOfWork.ContratoAdopcionRepository.Update(contrato);
            return await _unitOfWork.SaveAll();
        }
        public async Task<bool> ContratoEstadoMascota(Mascota mascota)
        {
            _unitOfWork.MascotaRepository.ContratoEstadoMascota(mascota);
            return await _unitOfWork.SaveAll();
        }
        public int GetLast() {
            return _unitOfWork.ContratoAdopcionRepository.GetLast().Id;
        }
        public ContratoAdopcion GetContratoByIdMascota(int id)
        {
            return _unitOfWork.ContratoAdopcionRepository.GetContratoByIdMascota(id);
        }
        public async Task<bool> CreateContratoRechazo(ContratoRechazoForCreateDto contratoRechazo) {
            var contrato= await _unitOfWork.ContratoAdopcionRepository.GetById(contratoRechazo.ContratoAdopcionId);
            ContratoRechazo c = new ContratoRechazo();
            c.RazonRechazo = contratoRechazo.RazonRechazo;
            c.ContratoAdopcion = contrato;
            c.ContratoAdopcionId = contrato.Id;
            _unitOfWork.ContratoRechazoRepository.Insert(c);
            return await _unitOfWork.SaveAll();
        }
        public async Task<bool> AprobarAdopcion(int id) {
            var contrato = await _unitOfWork.ContratoAdopcionRepository.GetById(id);
            contrato.Estado = "Aprobado";
            _unitOfWork.ContratoAdopcionRepository.Update(contrato);
            var mascota = await _unitOfWork.MascotaRepository.GetById(contrato.MascotaId);
            mascota.EstadoSituacion = "Adoptada";
            _unitOfWork.MascotaRepository.Update(mascota);
            if (await _seguimientoService.CreateSeguimiento(contrato))
            {
                var seguimiento = _unitOfWork.SeguimientoRepository.FindByCondition(x=>x.Estado.Equals("Activo")).LastOrDefault();
                for (int i = 0; i < 3; i++)
                {
                    ReporteSeguimiento reporteSeguimiento = new ReporteSeguimiento();
                    //FechaReporte = DateTime.Now.Date,
                    reporteSeguimiento.Seguimiento = seguimiento;
                    reporteSeguimiento.SeguimientoId = seguimiento.Id;
                    reporteSeguimiento.Estado = "Activo";
                    seguimiento.ReporteSeguimientos.Add(reporteSeguimiento);
                }
                return await _unitOfWork.SaveAll();
            }
            else
                return false;
        }

        public async Task<bool> RechazarAdopcion(int id)
        {
            var contrato = await _unitOfWork.ContratoAdopcionRepository.GetById(id);
            contrato.Estado = "Rechazado";
            _unitOfWork.ContratoAdopcionRepository.Update(contrato);
            var mascota = await _unitOfWork.MascotaRepository.GetById(contrato.MascotaId);
            mascota.EstadoSituacion = "En Adopcion";
            _unitOfWork.MascotaRepository.Update(mascota);
            _unitOfWork.SeguimientoRepository.Delete(contrato.Seguimiento);
            return await _unitOfWork.SaveAll();
        }

        public async Task<bool> CancelarAdopcion(int id)
        {
            var contrato = await _unitOfWork.ContratoAdopcionRepository.GetById(id);
            contrato.Estado = "Cancelado";
            _unitOfWork.ContratoAdopcionRepository.Update(contrato);
            var mascota = await _unitOfWork.MascotaRepository.GetById(contrato.MascotaId);
            mascota.EstadoSituacion = "Registrado";
            _unitOfWork.MascotaRepository.Update(mascota);
            _unitOfWork.SeguimientoRepository.Delete(contrato.Seguimiento);
            return await _unitOfWork.SaveAll();
        }
    }
}