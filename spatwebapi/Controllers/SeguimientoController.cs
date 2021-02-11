﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using webapi.business.Dtos.Seguimientos;
using webapi.business.Dtos.Usuario;
using webapi.business.Helpers;
using webapi.business.Pagination;
using webapi.business.Services.Interf;
using webapi.core.Models;
using webapi.data.Repositories.Interf;

namespace spatwebapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeguimientoController : ControllerBase
    {
        private readonly HttpContext _httpContext;
        private ISeguimientoService _seguimientoService;
        private IReporteSeguimientoService _reporteSeguimientoService;
        private IUserService _userService;
        private readonly IMapper _mapper;
        public SeguimientoController(ISeguimientoService seguimientoService, IMapper mapper,
            IUserService userService, IReporteSeguimientoService reporteSeguimientoService,
            IHttpContextAccessor httpContextAccessor)
        {
            _seguimientoService = seguimientoService;
            _userService = userService;
            _reporteSeguimientoService = reporteSeguimientoService;
            _mapper = mapper;
            _httpContext = httpContextAccessor.HttpContext;
        }
        [AllowAnonymous]
        [HttpGet("GetAllSeguimiento")]
        public async Task<ActionResult> GetAllSeguimiento([FromQuery] SeguimientoParametros parametros)
        {
            int idUser = int.Parse(_httpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var resul = await _seguimientoService.GetAllSeguimiento(parametros, idUser);
            var lista = _mapper.Map<List<SeguimientoForReturnDto>>(resul.ToList());
            Response.AddPagination(resul.CurrentPage, resul.PageSize,
                 resul.TotalCount, resul.TotalPages);
            return Ok(lista);
        }
        [AllowAnonymous]
        [HttpGet("ListSeguimiento")]
        public IEnumerable<SeguimientoForReturnDto> ListSeguimiento() {
            var lista = _seguimientoService.GetAll();
            var mapeado= _mapper.Map<IEnumerable<SeguimientoForReturnDto>>(lista);
            return mapeado;
        }
        [HttpGet("GetAllVoluntarios")]
        public IEnumerable<UserForDetailedDto> GetAllVoluntarios(/*[FromQuery] VoluntarioParameters voluntarioParameters*/)
        {
            var lista = _seguimientoService.GetAllVoluntarios();
            var mapped = _mapper.Map<IEnumerable<UserForDetailedDto>>(lista);
            return mapped;
            //var listaToReturn = _mapper.Map<IEnumerable<UserForListDto>>(lista);

            //Response.AddPagination(lista.CurrentPage, lista.PageSize, lista.TotalCount, lista.TotalPages);
            //return Ok(listaToReturn);
        }
        [AllowAnonymous]
        [HttpGet("GetSeguimiento/{id}")]
        public async Task<ActionResult> GetSeguimiento(int id)
        {
            var aux = await _seguimientoService.GetById(id);
            if (aux==null)
                return NotFound("Seguimiento no encontrado.");
            var seg = _mapper.Map<SeguimientoForReturnDto>(aux);
            return Ok(seg);
        }
        [AllowAnonymous]
        [HttpPut("SaveSeguimiento")]
        public async Task<IActionResult> SaveSeguimiento([FromBody] Seguimiento seguimiento)
        {
            //var objeto = await _seguimientoService.GetById(seguimiento.Id);
            //objeto.CantidadVisitas = seguimiento.CantidadVisitas;
            if (await _seguimientoService.UpdateSeguimiento(seguimiento)) {
                return Ok("Datos actualizados correctamente, se generaron los reportes respectivos.");
            }
            return BadRequest("Ocurrio un problema al actualizar los datos.");
        }
        [AllowAnonymous]
        [HttpPut("UpdateFecha")]
        public async Task<IActionResult> UpdateFecha([FromBody] FechaReporteDto dto)
        {
            //var objeto = await _seguimientoService.GetById(seguimiento.Id);
            //objeto.CantidadVisitas = seguimiento.CantidadVisitas;
            var o= await _seguimientoService.GetById(dto.Id);
            if (o != null) {
                if (await _seguimientoService.UpdateFecha(dto))
                {
                    var mapped = _mapper.Map<IEnumerable<SeguimientoForReturnDto>>(_seguimientoService.GetAll());
                    return Ok(mapped);
                }
                return BadRequest("Ocurrio un problema al actualizar los datos.");
            }
            return BadRequest("No existe el Seguimiento.");
        }
        [AllowAnonymous]
        [HttpPut("{id}/User/{idUser}")]
        public async Task<IActionResult> CheckedVoluntarioAsignado(int id, int idUser)
        {
            //var seguimiento = await _seguimientoService.GetById(id);
            //var user = await _userService.GetUsuario(idUser);
            if (await _seguimientoService.CheckedVoluntarioAsignado(id, idUser)) {
                var voluntarios = _seguimientoService.GetAllVoluntarios();
                var mapper = _mapper.Map<IEnumerable<UserForDetailedDto>>(voluntarios);
                return Ok(mapper);
            }
            return BadRequest("Ocurrio un problema.");
        }


        [Authorize(Roles ="Voluntario")]
        [HttpGet("ListVoluntarioSeguimientos")]
        public async Task<IActionResult> ListVoluntarioSeguimientos([FromQuery] SeguimientoParametros parametros)
        {
            //var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //var lista = _mapper.Map<IEnumerable<SeguimientoForReturnDto>>(_seguimientoService.GetSeguimientoForVoluntario(int.Parse(id)));
            //return lista;
            int idUser = int.Parse(_httpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var resul = await _seguimientoService.GetAllSeguimiento(parametros, idUser);
            var lista = _mapper.Map<IEnumerable<SeguimientoForReturnDto>>(resul);
            Response.AddPagination(resul.CurrentPage, resul.PageSize,
                 resul.TotalCount, resul.TotalPages);
            return Ok(lista);
        }

        //Rol Voluntario
        [Authorize(Roles = "Voluntario")]
        [HttpPost("{id}/AsignarSeguimiento")]
        public async Task<IActionResult> AsignarSeguimiento(int id)
        {
            var resul = await _seguimientoService.AsignarSeguimiento(id);
            if (resul)
                return Ok("Se ha asignado el seguimiento.");

            return BadRequest("Huno un problema al asignar el seguimiento.");
        }
        [Authorize(Roles = "Voluntario")]
        [HttpPost("{id}/RechazarSeguimiento")]
        public async Task<IActionResult> RechazarSeguimiento(int id)
        {
            var resul = await _seguimientoService.RechazarSeguimiento(id);
            if (resul)
                return Ok("Se ha rechazado el seguimiento.");

            return BadRequest("Huno un problema al rechazar el seguimiento.");
        }
    }
}