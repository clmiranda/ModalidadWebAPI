﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using webapi.business.Dtos.Adopciones;
using webapi.business.Dtos.ContratoRechazo;
using webapi.business.Helpers;
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
        private readonly IContratoAdopcionService _contratoAdopcionService;
        private readonly IMascotaService _mascotaService;
        public ContratoAdopcionController(IContratoAdopcionService contratoAdopcionService,
            IMapper mapper, IMascotaService mascotaService)
        {
            _contratoAdopcionService = contratoAdopcionService;
            _mapper = mapper;
            _mascotaService = mascotaService;
        }
        [HttpGet("GetAllContratos")]
        public async Task<ActionResult> GetAllContratos([FromQuery] ContratoAdopcionParametros parametros)
        {
            var resul = await _contratoAdopcionService.GetAllContratos(parametros);
            var lista = _mapper.Map<IEnumerable<ContratoAdopcionForList>>(resul);
            Response.AddPagination(resul.CurrentPage, resul.PageSize,
                 resul.TotalCount, resul.TotalPages);
            return Ok(lista);
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
                return NotFound("Id de contrato no existe.");
            var modelo = _mapper.Map<ContratoAdopcionForDetailDto>(resul);
            return Ok(modelo);
        }
        [HttpGet("GetAll")]
        public ActionResult GetAll()
        {
            var lista = _contratoAdopcionService.GetAll();
            var mapped = _mapper.Map<IEnumerable<ContratoAdopcionForDetailDto>>(lista);
            return Ok(mapped);
        }
        [HttpGet("GetAllRechazoCancelado")]
        public ActionResult GetAllRechazoCancelado()
        {
            var lista = _contratoAdopcionService.GetAllRechazoCancelado();
            var mapped = _mapper.Map<IEnumerable<ContratoRechazoForReturnDto>>(lista);
            return Ok(mapped);
        }
        [HttpGet("GetContratoByIdMascota/{id}")]
        public async Task<ActionResult<ContratoAdopcionReturnDto>> GetContratoByIdMascota(int id)
        {
            var mascota = await _mascotaService.GetMascotaById(id);
            if (mascota==null)
                return NotFound(null);
            if (!mascota.EstadoSituacion.Equals("Activo"))
                return BadRequest(null);
            var resul = await _contratoAdopcionService.FindByCondition(x => x.MascotaId == id).FirstOrDefaultAsync();
            var modelo = _mapper.Map<ContratoAdopcionReturnDto>(resul);
            return Ok(modelo);
        }
        [HttpPost("GenerarContrato")]
        public async Task<IActionResult> GenerarContrato([FromBody] ContratoAdopcionForCreate contrato) {
            var c = await _contratoAdopcionService.CreateContratoAdopcion(contrato);
            if (c != null) {
                var mapeado = _mapper.Map<ContratoAdopcionReturnDto>(c);
                return Ok(mapeado);
            }
            return BadRequest(new { mensaje = "Ha ocurrido un error guardando los datos." });
        }
        [HttpPut("UpdateFecha")]
        public async Task<IActionResult> UpdateFecha([FromForm] FechaContratoForUpdateDto dto) {
            var modelo = await _contratoAdopcionService.GetById(dto.Id);
            if (modelo != null) {
                if (await _contratoAdopcionService.UpdateContratoAdopcion(dto))
                {
                    var res = _mapper.Map<ContratoAdopcionReturnDto>(modelo);
                    return Ok(res);
                }
                return BadRequest(new { mensaje = "Ha ocurrido un error actualizando los datos." });
            }
            return BadRequest(new { mensaje = "No se pudo encontrar el Contrato." });
        }
        [Authorize(Roles ="Administrador, Voluntario")]
        [HttpGet("DetailAdopcion/{id}")]
        public async Task<IActionResult> DetailAdopcion(int id) {
            var resul = _mapper.Map<ContratoAdopcionReturnDto>(await _contratoAdopcionService.GetById(id));
            if (resul==null)
                return BadRequest(null);
            return Ok(resul);
        }
        [HttpPut("{id}/AprobarAdopcion")]
        public async Task<IActionResult> AprobarAdopcion(int id) {
            var modelo = await _contratoAdopcionService.GetById(id);
            if (modelo!=null)
            {
                if (await _contratoAdopcionService.AprobarAdopcion(id, modelo.Mascota.Id)) {
                    var contrato = _mapper.Map<ContratoAdopcionReturnDto>(modelo);
                        return Ok(contrato);
                }
                else
                    return BadRequest(new { mensaje = "Hubo problemas al guardar los datos." });
            }
            return BadRequest(new { mensaje = "No se ha encontrado el Contrato." });
        }
        [HttpPut("RechazarAdopcion")]
        public async Task<IActionResult> RechazarAdopcion(ContratoRechazoForCreateDto contratoRechazo) {
            var modelo = await _contratoAdopcionService.GetById(contratoRechazo.ContratoAdopcionId);
            if (modelo!=null)
            {
                if (await _contratoAdopcionService.RechazarAdopcion(contratoRechazo.ContratoAdopcionId, modelo.Mascota.Id)) {
                    if (await _contratoAdopcionService.CreateContratoRechazo(contratoRechazo))
                    {
                        var contrato = _mapper.Map<ContratoAdopcionReturnDto>(modelo);
                        return Ok(contrato);
                    }
                    return BadRequest(new { mensaje = "Ocurrio un problema al tratar de generar el informe." });
                }

                return BadRequest(new { mensaje = "Ha ocurrido un problema al tratar de rechazar el Contrato." });
            }
            return BadRequest(new { mensaje = "No se ha encontrado el Contrato." });
        }
        [HttpPut("CancelarAdopcion")]
        public async Task<IActionResult> CancelarAdopcion(ContratoRechazoForCreateDto contratoRechazo) {
            var modelo = await _contratoAdopcionService.GetById(contratoRechazo.ContratoAdopcionId);
            if (modelo!=null)
            {
                if (await _contratoAdopcionService.CancelarAdopcion(contratoRechazo.ContratoAdopcionId, modelo.Mascota.Id))
                {
                    if (await _contratoAdopcionService.CreateContratoRechazo(contratoRechazo)) {
                        var contrato = _mapper.Map<ContratoAdopcionReturnDto>(modelo);
                        return Ok(contrato);
                    }
                    return BadRequest(new { mensaje = "Ha ocurrido un error al cancelar el Contrato." });
                }
                return BadRequest(new { mensaje = "Ha ocurrido un error al cancelar el Contrato." });
            }
            return BadRequest(new { mensaje = "No se ha encontrado el Contrato." });
        }
    }
}