using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using webapi.business.Dtos.Adopciones;
using webapi.business.Dtos.ContratoRechazo;
using webapi.business.Pagination;
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
        [HttpGet("GetInformeContrato/{id}")]
        public async Task<ActionResult> GetInformeContrato(int id)
        {
            var resul = await _contratoAdopcionService.GetById(id);
            if (resul==null)
                return BadRequest("Id de contrato no existe.");
            var modelo = _mapper.Map<ContratoAdopcionForDetailDto>(resul);
            return Ok(modelo);
        }
        [HttpGet("GetAllAdopcionesPendientes")]
        public async Task<PaginationContratoAdopcion> GetAllAdopcionesPendientes([FromQuery] ContratoAdopcionParametros parametros) {
            //var lista = await _contratoAdopcionService.FindByCondition(x=> x.Estado.Equals("Pendiente") || x.Estado.Equals("Aprobado")).ToListAsync();
            //var resul = _mapper.Map<IEnumerable<ContratoAdopcionReturnDto>>(lista);
            var lista = await _contratoAdopcionService.GetAll(parametros);
            return lista;
        }
        [HttpGet("GetContratoByIdMascota/{id}")]
        public async Task<ActionResult<ContratoAdopcionReturnDto>> GetContratoByIdMascota(int id)
        {
            var mascota = await _mascotaService.GetMascotaById(id);
            if (mascota==null)
                return NotFound();
            var resul = await _contratoAdopcionService.FindByCondition(x=>x.Mascota.Id==id).FirstOrDefaultAsync();
            if (resul==null)
                return Ok(null);

            var modelo = _mapper.Map<ContratoAdopcionReturnDto>(resul);
            return Ok(modelo);
        }
        [HttpPost("GenerarContrato")]
        public async Task<IActionResult> GenerarContrato([FromBody] ContratoAdopcionForCreate contrato) {
            //var modelo = _mapper.Map<ContratoAdopcion>(contrato);
            var c = await _contratoAdopcionService.CreateContratoAdopcion(contrato);
            if (c != null) {
                var mapeado = _mapper.Map<ContratoAdopcionReturnDto>(c);
                return Ok(mapeado);
            }
                //var mascota = await _mascotaService.GetMascotaById(modelo.MascotaId);
                //if(await _contratoAdopcionService.ContratoEstadoMascota(mascota))
                //return Ok();
                //return BadRequest("El contrato fue enviado, pero hubo un conflicto guardando algunos datos.");
            return BadRequest("Ha ocurrido un error guardando los datos.");
        }
        [HttpPut("UpdateFecha")]
        public async Task<IActionResult> UpdateFecha([FromBody] ContratoAdopcion contrato) {
            var modelo =await _contratoAdopcionService.GetById(contrato.Id);
            if (modelo != null) {
                if (await _contratoAdopcionService.UpdateContratoAdopcion(contrato))
                {
                    var res = _mapper.Map<ContratoAdopcionReturnDto>(modelo);
                    return Ok(res);
                }
                return BadRequest("Ha ocurrido un error actualizando los datos.");
            }
            return BadRequest("No se pudo encontrar el Contrato.");
        }
        [HttpGet("DetailAdopcion/{id}")]
        public async Task<IActionResult> DetailAdopcion(int id) {
            var resul = _mapper.Map<ContratoAdopcionReturnDto>(await _contratoAdopcionService.GetById(id));
            if (resul==null)
                return BadRequest("No se ha encontrado el Contrato.");
            return Ok(resul);
        }
        [HttpPut("{id}/AprobarAdopcion")]
        public async Task<IActionResult> AprobarAdopcion(int id) {
            var modelo = await _contratoAdopcionService.GetById(id);
            if (modelo!=null)
            {
                if (await _contratoAdopcionService.AprobarAdopcion(id)) {
                    //otro dto a retornar para incluir todas las preguntas respondidas por el adoptante
                    var contrato = _mapper.Map<ContratoAdopcionReturnDto>(modelo);
                        return Ok(contrato);
                }
                else
                    return BadRequest("Hubo problemas al guardar los datos.");
            }
            return BadRequest("No se ha encontrado el Contrato.");
        }
        [HttpPut("RechazarAdopcion")]
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
        [HttpPut("{id}/CancelarAdopcion")]
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