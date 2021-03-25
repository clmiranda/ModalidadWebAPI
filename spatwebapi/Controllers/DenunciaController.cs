﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using webapi.business.Dtos;
using webapi.business.Dtos.Denuncias;
using webapi.business.Helpers;
using webapi.business.Services.Interf;
using webapi.core.Models;

namespace spatwebapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class DenunciaController : ControllerBase
    {
        private readonly IConfiguration _config;
        private IDenunciaService _denunciaService;
        private readonly IMapper _mapper;
        public DenunciaController(IConfiguration config, IDenunciaService denunciaService, IMapper mapper)
        {
            _config = config;
            _denunciaService = denunciaService;
            _mapper = mapper;
        }
        [HttpGet("GetAll")]
        public ActionResult GetAll()
        {
            var lista = _denunciaService.GetAll();
            return Ok(lista);
        }
        [HttpGet("GetAllDenuncias")]
        public async Task<ActionResult> GetAllDenuncias([FromQuery]DenunciaParametros parametros) {
            //var resul= await _denunciaService.GetAllDenuncias();
            //var lista = _mapper.Map<IEnumerable<DenunciaForListDto>>(resul);
            //return lista;
            var resul = await _denunciaService.GetAllDenuncias(parametros);
            var lista = _mapper.Map<IEnumerable<DenunciaForListDto>>(resul);
            Response.AddPagination(resul.CurrentPage, resul.PageSize,
                 resul.TotalCount, resul.TotalPages);
            //_mapper.Map<IEnumerable<DenunciaForListDto>>(resul.Items);
            return Ok(lista);

        }
        //[HttpGet("GetAllDenunciasFilter")]
        //public async Task<IEnumerable<DenunciaFilterDto>> GetAllDenunciasFilter()
        //{
        //    var lista= await _denunciaService.GetAllDenuncias();
        //    var resul = _mapper.Map<IEnumerable<DenunciaFilterDto>>(lista);
        //    return resul;
        //}
        [HttpGet("GetDenuncia/{id}")]
        public async Task<DenunciaForListDto> GetDenuncia(int id)
        {
            //var obj= await _denunciaService.GetDenunciaById(id);
            //var resul = _mapper.Map<DenunciaForDetailedDto>(obj);
            //return resul;
            var obj= await _denunciaService.GetDenunciaById(id);
            if (obj == null) return null;
            var resul = _mapper.Map<DenunciaForListDto>(obj);
            return resul;
        }
        [HttpPost("CreateDenuncia")]
        public async Task<IActionResult> CreateDenuncia(Denuncia denuncia)
        {
            var d = await _denunciaService.CreateDenuncia(denuncia);
            if (d.Equals(null))
                return BadRequest(new { mensaje = "Hubo problemas al crear la denuncia." });
            return Ok();
        }
        [HttpPut("UpdateDenuncia")]
        public async Task<IActionResult> UpdateDenuncia(Denuncia denuncia)
        {
            var d=await _denunciaService.UpdateDenuncia(denuncia);
            if (d.Equals(null))
                return BadRequest(new { mensaje = "Hubo problemas al modificar la denuncia." });

            return Ok();
        }
        [HttpDelete("DeleteDenuncia/{id}")]
        public async Task<IActionResult> DeleteDenuncia(int id)
        {
            var denuncia= await _denunciaService.GetDenunciaById(id);
            if (await _denunciaService.DeleteDenuncia(denuncia))
                return Ok();
            else
                return BadRequest(new { mensaje = "Hubo problemas al eliminar el registro." });
        }
    }
}
