using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webapi.business.Dtos.Seguimientos;
using webapi.business.Pagination;
using webapi.business.Services.Interf;
using webapi.core.Models;
using webapi.data.Repositories.Interf;

namespace webapi.business.Services.Imp
{
    public class SeguimientoService : ISeguimientoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly List<string> listaEstado;
        public SeguimientoService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            listaEstado = new List<string> { "Activo", "Pendiente", "Asignado" };
        }
        public IEnumerable<Seguimiento> GetAll() {
            var lista= _unitOfWork.SeguimientoRepository.GetAll().ToList();
            return lista;
        }

        public async Task<PaginationList<Seguimiento>> GetAllSeguimiento(SeguimientoParametros parametros)
        {
            if (string.IsNullOrEmpty(parametros.Filter) || !listaEstado.Contains(parametros.Filter))
                return null;
            var resul = _unitOfWork.SeguimientoRepository.GetAll();
            if (parametros.Filter == "Activo")
                resul = resul.Where(x => x.Estado.Equals("Activo"));
            else if (parametros.Filter == "Pendiente")
                resul = resul.Where(x => x.Estado.Equals("Pendiente"));
            else if (parametros.Filter == "Asignado")
                resul = resul.Where(x => x.Estado.Equals("Asignado"));
            var pagination = await PaginationList<Seguimiento>.ToPagedList(resul, parametros.PageNumber, parametros.PageSize);
            return pagination;
        }
        public async Task<Seguimiento> GetById(int id)
        {
            return await _unitOfWork.SeguimientoRepository.GetById(id);
        }
        public void CreateSeguimiento(int idContrato)
        {
            Seguimiento seguimiento = new Seguimiento
            {
                ContratoAdopcionId = idContrato,
                FechaInicio = DateTime.Now,
                FechaConclusion = DateTime.Now,
                Estado = "Activo"
            };
            _unitOfWork.SeguimientoRepository.Insert(seguimiento);
        }
        public IEnumerable<User> GetAllVoluntarios() {
            var lista = _unitOfWork.UserRepository.FindByCondition(x => x.UserRoles.Any(y => y.Role.Name.Equals("Voluntario"))).ToList();
            return lista;
        }
        public async Task<Seguimiento> UpdateFecha(FechaReporteDto dto) {
            var seguimiento = await _unitOfWork.SeguimientoRepository.GetById(dto.Id);
            seguimiento.FechaInicio = Convert.ToDateTime(dto.RangoFechas[0]);
            seguimiento.FechaConclusion = Convert.ToDateTime(dto.RangoFechas[1]);
            _unitOfWork.SeguimientoRepository.Update(seguimiento);
            if (await _unitOfWork.SaveAll())
                return seguimiento;
            return null;
        }
        public async Task<bool> DeleteSeguimiento(Seguimiento seguimiento)
        {
            _unitOfWork.SeguimientoRepository.Delete(seguimiento);
            return await _unitOfWork.SaveAll();
        }
        public async Task<bool> AsignarSeguimiento(int id, int idUser) {
            var seguimiento = await _unitOfWork.SeguimientoRepository.GetById(id);
            var user = await _unitOfWork.UserRepository.GetById(idUser);
            seguimiento.UserId = idUser;
            seguimiento.Estado = "Pendiente";
            _unitOfWork.SeguimientoRepository.Update(seguimiento);
            user.Seguimientos.Add(seguimiento);
            return await _unitOfWork.SaveAll();
        }
        public async Task<bool> QuitarAsignacion(int id, int idUser)
        {
            var seguimiento = await _unitOfWork.SeguimientoRepository.GetById(id);
            var user = await _unitOfWork.UserRepository.GetById(idUser);
            seguimiento.User = null;
            seguimiento.Estado = "Activo";
            _unitOfWork.SeguimientoRepository.Update(seguimiento);
            user.Seguimientos.Remove(seguimiento);
            return await _unitOfWork.SaveAll();
        }
        public async Task<bool> AceptarSeguimientoVoluntario(int id)
        {
            var resul = await _unitOfWork.SeguimientoRepository.GetById(id);
            resul.Estado = "Asignado";
            _unitOfWork.SeguimientoRepository.Update(resul);
            return await _unitOfWork.SaveAll();
        }
        public async Task<bool> RechazarSeguimientoVoluntario(int id)
        {
            var seguimiento = await _unitOfWork.SeguimientoRepository.GetById(id);
            seguimiento.User = null;
            seguimiento.Estado = "Activo";
            _unitOfWork.SeguimientoRepository.Update(seguimiento);
            return await _unitOfWork.SaveAll();
        }
    }
}