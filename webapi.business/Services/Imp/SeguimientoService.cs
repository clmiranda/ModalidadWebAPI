using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            else if (parametros.Filter == "Asignado")
                resul = resul.Where(x => x.Estado.Equals("Asignado"));
            var pagination = await PaginationList<Seguimiento>.ToPagedList(resul, parametros.PageNumber, parametros.PageSize);
            return pagination;
        }
        public async Task<PaginationList<Seguimiento>> GetAllSeguimientoVoluntario(int idUser, SeguimientoParametros parametros)
        {
            var lista = _unitOfWork.SeguimientoRepository.GetAll();
            lista = lista.Where(x => x.UserId == idUser);
            var pagination = await PaginationList<Seguimiento>.ToPagedList(lista, parametros.PageNumber, parametros.PageSize);
            return pagination;
        }
        public async Task<Seguimiento> GetById(int id)
        {
            return await _unitOfWork.SeguimientoRepository.GetById(id);
        }
        public void CreateSeguimiento(int idAdopcion)
        {
            Seguimiento seguimiento = new Seguimiento
            {
                SolicitudAdopcionId = idAdopcion,
                FechaInicio = DateTime.Now,
                FechaFin = DateTime.Now,
                Estado = "Activo"
            };
            _unitOfWork.SeguimientoRepository.Insert(seguimiento);
        }
        public IEnumerable<User> GetAllVoluntarios() {
            var lista = _unitOfWork.UserRepository.FindByCondition(x => x.UserRoles.All(y => !y.Role.Name.Equals("SuperAdministrador") && y.Role.Name.Equals("Voluntario"))).ToList();
            return lista;
        }
        public async Task<bool> DeleteAllSeguimientoFromUser(int idUser)
        {
            var seguimiento = await _unitOfWork.SeguimientoRepository.FindByCondition(x => x.UserId == idUser).ToListAsync();
            foreach (var item in seguimiento)
            {
                item.User = null;
                item.Estado = "Activo";
            }
            return await _unitOfWork.SaveAll();
        }
        public async Task<bool> AsignarSeguimiento(int id, int idUser) {
            var seguimiento = await _unitOfWork.SeguimientoRepository.GetById(id);
            var user = await _unitOfWork.UserRepository.GetById(idUser);
            seguimiento.UserId = idUser;
            seguimiento.Estado = "Asignado";
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
    }
}