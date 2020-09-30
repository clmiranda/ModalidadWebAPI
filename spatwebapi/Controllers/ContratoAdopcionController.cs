using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using webapi.business.Dtos.Adopciones;
using webapi.business.Dtos.ContratoRechazo;
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
        public async Task<IEnumerable<ContratoAdopcionReturnDto>> GetAllAdopcionesPendientes() {
            var resul = _mapper.Map<IEnumerable<ContratoAdopcionReturnDto>>(await _contratoAdopcionService.GetAllAdopcionesPendientes());
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
        [HttpPost("GenerarContrato")]
        public async Task<IActionResult> GenerarContrato([FromBody] ContratoAdopcionReturnDto contrato) {
            var modelo = _mapper.Map<ContratoAdopcion>(contrato);
            if (await _contratoAdopcionService.CreateContratoAdopcion(modelo))
            {
                var mascota = await _mascotaService.GetMascotaById(modelo.MascotaId);
                if(await _contratoAdopcionService.ContratoEstadoMascota(mascota))
                return Ok();
                return BadRequest("El contrato fue enviado, pero hubo un conflicto guardando algunos datos.");
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
        [HttpPost("{id}/AprobarAdopcion")]
        public async Task<IActionResult> AprobarAdopcion(int id) {
            var modelo = await _contratoAdopcionService.GetById(id);
            if (modelo!=null)
            {
                if (await _contratoAdopcionService.AprobarAdopcion(id)) {
                        return Ok("El contrato se ha aprobado correctamente, se ha creado el seguimiento y los reportes respectivos.");
                }
                else
                    return BadRequest("Se aprobo el contrato, pero ocurrio un problema al generar los demas datos.");
            }
            return BadRequest("No se ha encontrado el Contrato.");
        }
        [HttpPost("{id}/RechazarAdopcion")]
        public async Task<IActionResult> RechazarAdopcion(ContratoRechazoForCreateDto contratoRechazo) {
            var modelo = await _contratoAdopcionService.GetById(contratoRechazo.ContratoAdopcionId);
            if (modelo!=null)
            {
                if (await _contratoAdopcionService.RechazarAdopcion(contratoRechazo.ContratoAdopcionId)) {
                    if (await _contratoAdopcionService.CreateContratoRechazo(contratoRechazo))
                    {
                        return Ok("Contrato rechazado, se ha creado un informe de la razon del Rechazo");
                    }
                    return BadRequest("Ha ocurrido un problema al tratar de generar el informe de Rechazo");
                }

                return BadRequest("Ha ocurrido un problema al tratar de rechazar el Contrato.");
            }
            return BadRequest("No se ha encontrado el Contrato.");
        }
        [HttpPost("{id}/CancelarAdopcion")]
        public async Task<IActionResult> CancelarAdopcion(ContratoRechazoForCreateDto contratoRechazo) {
            var modelo = await _contratoAdopcionService.GetById(contratoRechazo.ContratoAdopcionId);
            if (modelo!=null)
            {
                if (await _contratoAdopcionService.CancelarAdopcion(contratoRechazo.ContratoAdopcionId))
                {
                    if (await _contratoAdopcionService.CreateContratoRechazo(contratoRechazo))
                        return Ok("Se ha cancelado el Contrato y eliminado el Seguimiento correspondiente.");

                    return BadRequest("Ha ocurrido un error al cancelar el Contrato");
                }
                return BadRequest("Ha ocurrido un error al cancelar el Contrato");
            }
            return BadRequest("No se ha encontrado el Contrato.");
        }
    }
}