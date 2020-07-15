using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        private ISeguimientoService _seguimientoService;
        public ContratoAdopcionController(IContratoAdopcionService contratoAdopcionService, IMapper mapper,
            ISeguimientoService seguimientoService)
        {
            _contratoAdopcionService = contratoAdopcionService;
            _mapper = mapper;
            _seguimientoService = seguimientoService;
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
        [HttpPost("SendDataContratoAdopcion")]
        public async Task<IActionResult> SendDataContratoAdopcion([FromBody] ContratoAdopcionReturnDto contrato) {
            var modelo = _mapper.Map<ContratoAdopcion>(contrato);
            _contratoAdopcionService.ModifyStateMascota(contrato.MascotaId);
            if (await _contratoAdopcionService.CreateContratoAdopcion(modelo))
            {
                return Ok(_contratoAdopcionService.GetLast());
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
                    Seguimiento seguimiento = new Seguimiento { 
                    ContratoAdopcionId= modelo.Id
                    };
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
    }
}