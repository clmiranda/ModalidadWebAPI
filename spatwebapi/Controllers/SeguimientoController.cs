using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using webapi.business.Dtos.Seguimientos;
using webapi.business.Services.Interf;
using webapi.core.Models;
using webapi.data.Repositories.Interf;

namespace spatwebapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeguimientoController : ControllerBase
    {
        private ISeguimientoService _seguimientoService;
        private IReporteSeguimientoService _reporteSeguimientoService;
        private IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public SeguimientoController(ISeguimientoService seguimientoService, IMapper mapper,
            IUserRepository userRepository, IReporteSeguimientoService reporteSeguimientoService)
        {
            _seguimientoService = seguimientoService;
            _userRepository = userRepository;
            _reporteSeguimientoService = reporteSeguimientoService;
            _mapper = mapper;
        }
        [AllowAnonymous]
        [HttpGet("ListSeguimiento")]
        public async Task<IEnumerable<SeguimientoForReturnDto>> ListSeguimiento() {
            return _mapper.Map<IEnumerable<SeguimientoForReturnDto>>(await _seguimientoService.GetAll());
        }
        [AllowAnonymous]
        [HttpGet("GetSeguimiento/{id}")]
        public async Task<ActionResult<SeguimientoForReturnDto>> GetSeguimiento(int id)
        {
            var aux = await _seguimientoService.GetById(id);
            if (aux==null)
                return NotFound("Seguimiento no encontrado.");
            return Ok( _mapper.Map<SeguimientoForReturnDto>(aux));
        }
        [AllowAnonymous]
        [HttpPut("SaveSeguimiento")]
        public async Task<IActionResult> SaveSeguimiento([FromBody] Seguimiento modelo)
        {
            var objeto = await _seguimientoService.GetById(modelo.Id);
            objeto.CantidadVisitas = modelo.CantidadVisitas;
            if (await _seguimientoService.UpdateSeguimiento(objeto)) {
                return Ok("Datos actualizados correctamente, se generaron los reportes respectivos.");
            }
            return BadRequest("Ocurrio un problema al actualizar los datos.");
        }
        [AllowAnonymous]
        [HttpPut("{id}/User/{idUser}")]
        public async Task<ActionResult<SeguimientoForReturnDto>> CheckedVoluntarioAsignado(int id, int idUser)
        {
            var seguimiento = await _seguimientoService.GetById(id);
            var user = await _userRepository.GetById(idUser);
            if (await _seguimientoService.CheckedVoluntarioAsignado(seguimiento,user))
                return Ok("Voluntario Asignado");
            return BadRequest("Ocurrio un problema.");
        }
    }
}