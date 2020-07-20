using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using webapi.business.Dtos.Adopciones;
using webapi.business.Services.Interf;
using webapi.core.Models;

namespace spatwebapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContratoAdopcionController : ControllerBase
    {
        private readonly IMapper _mapper;
        private IContratoAdopcionService _contratoAdopcionService;
        private IMascotaService _mascotaService;
        private ISeguimientoService _seguimientoService;
        private static Seguimiento seguimiento;
        public ContratoAdopcionController(IContratoAdopcionService contratoAdopcionService, IMapper mapper,
            ISeguimientoService seguimientoService, IMascotaService mascotaService)
        {
            _contratoAdopcionService = contratoAdopcionService;
            _mapper = mapper;
            _seguimientoService = seguimientoService;
            _mascotaService = mascotaService;
        }
        [HttpGet("GetById/{id}")]
        public async Task<ContratoAdopcionReturnDto> GetById(int id) {
            var resul =await _contratoAdopcionService.GetById(id);
            var modelo = _mapper.Map<ContratoAdopcionReturnDto>(resul);
            return modelo;
        }
        [HttpGet("GetAllAdopcionesPendientes")]
        public IEnumerable<ContratoAdopcionReturnDto> GetAllAdopcionesPendientes() {
            var resul = _mapper.Map<IEnumerable<ContratoAdopcionReturnDto>>( _contratoAdopcionService.GetAllAdopcionesPendientes());
            return resul;
        }
        [HttpGet("GetContratoByIdMascota/{id}")]
        public ContratoAdopcionReturnDto GetContratoByIdMascota(int id)
        {
            var resul = _contratoAdopcionService.GetContratoByIdMascota(id);
            if (resul==null)
                return null;

            var modelo = _mapper.Map<ContratoAdopcionReturnDto>(resul);
            return modelo;
        }
        //[HttpGet("ExistFotosForContrato/{idContrato}")]
        //public bool ExistFotosForContrato(int idContrato) {
        //    return _contratoAdopcionService.ExistFotosForContrato(idContrato);
        //}
        [HttpPost("GenerarContrato")]
        public async Task<IActionResult> GenerarContrato([FromBody] ContratoAdopcionReturnDto contrato) {
            var modelo = _mapper.Map<ContratoAdopcion>(contrato);
            if (await _contratoAdopcionService.CreateContratoAdopcion(modelo))
            {
                var mascota = await _mascotaService.GetMascotaById(modelo.Id);
                _contratoAdopcionService.ContratoEstadoMascota(mascota);
                return Ok();
            }
            return BadRequest("Ha ocurrido un error guardando los datos.");
        }
        [HttpPut("ModifyFechaContrato")]
        public async Task<IActionResult> ModifyFechaContrato([FromBody] ContratoAdopcionReturnDto contrato) {
            var modelo =await _contratoAdopcionService.GetById(contrato.Id);
            modelo.FechaAdopcion = contrato.FechaAdopcion;
            if (await _contratoAdopcionService.UpdateContratoAdopcion(modelo))
            {
                var res = _mapper.Map<ContratoAdopcionReturnDto>(modelo);
                return Ok(res);
            }
            return BadRequest("Ha ocurrido un error actualizando los datos.");
        }
        [HttpGet("DetailAdopcion/{id}")]
        public async Task<IActionResult> DetailAdopcion(int id) {
            var resul = _mapper.Map<ContratoAdopcionReturnDto>(await _contratoAdopcionService.GetById(id));
            if (resul==null)
                return BadRequest("No se ha encontrado el Contrato.");
            return Ok(resul);
        }
        [HttpPost("AprobarAdopcion")]
        public async Task<IActionResult> AprobarAdopcion([FromBody] ContratoAdopcionReturnDto contrato) {
            var modelo = await _contratoAdopcionService.GetById(contrato.Id);
            if (modelo!=null)
            {
                if (await _contratoAdopcionService.AprobarAdopcion(modelo)) {
                    seguimiento = new Seguimiento { ContratoAdopcionId= modelo.Id };
                    var resul= await _seguimientoService.CreateSeguimiento(seguimiento);
                    if (resul)
                        return Ok("El contrato se ha aprobado correctamente, se ha creado el seguimiento correspondiente.");
                    else
                        return BadRequest("Se aprobo el contrato, pero ocurrio un problema al generar el seguimiento de la adopcion.");
                }
                return BadRequest("Ha ocurrido un problema al tratar de aprobar el Contrato.");
            }
            return BadRequest("No se ha encontrado el Contrato.");
        }
        [HttpPost("RechazarAdopcion")]
        public async Task<IActionResult> RechazarAdopcion([FromBody] ContratoAdopcionReturnDto contrato) {
            var modelo = await _contratoAdopcionService.GetById(contrato.Id);
            if (modelo!=null)
            {
                if (await _contratoAdopcionService.RechazarAdopcion(modelo))
                    return Ok("El contrato se ha rechazado.");

                return BadRequest("Ha ocurrido un problema al tratar de rechazar el Contrato.");
            }
            return BadRequest("No se ha encontrado el Contrato.");
        }
        [HttpPost("CancelarAdopcion")]
        public async Task<IActionResult> CancelarAdopcion([FromBody] ContratoAdopcionReturnDto contrato) {
            var modelo = await _contratoAdopcionService.GetById(contrato.Id);
            if (modelo!=null)
            {
                if (await _contratoAdopcionService.CancelarAdopcion(modelo))
                {
                    var resul = await _seguimientoService.GetByIdContrato(contrato.Id);
                    if (await _seguimientoService.DeleteSeguimiento(resul))
                        return Ok("Se ha cancelado el Contrato y eliminado el Seguimiento correspondiente.");

                    return BadRequest("Se ha cancelado el Contrato, pero hubo problemas al eliminar el Seguimiento.");
                }
                return BadRequest("Ha ocurrido un error al tratar de cancelar el Contrato");
            }
            return BadRequest("No se ha encontrado el Contrato.");
        }
    }
}