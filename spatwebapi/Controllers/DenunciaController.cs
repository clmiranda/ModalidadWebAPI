using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using webapi.business.Dtos.Denuncias;
using webapi.business.Helpers;
using webapi.business.Pagination;
using webapi.business.Services.Interf;
using webapi.core.Models;

namespace spatwebapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "SuperAdministrador, Administrador")]
    public class DenunciaController : ControllerBase
    {
        private readonly IDenunciaService _denunciaService;
        private readonly IMapper _mapper;
        public DenunciaController(IDenunciaService denunciaService,
                                  IMapper mapper)
        {
            _denunciaService = denunciaService;
            _mapper = mapper;
        }
        [HttpGet("GetAllDenuncias")]
        public async Task<IActionResult> GetAllDenuncias([FromQuery] DenunciaParametros parametros)
        {
            var resultado = await _denunciaService.GetAllDenuncias(parametros);
            var mapped = _mapper.Map<IEnumerable<DenunciaForListDto>>(resultado);
            Response.AddPagination(resultado.CurrentPage, resultado.PageSize,
                 resultado.TotalCount, resultado.TotalPages);
            return Ok(mapped);

        }
        [HttpGet("GetAllDenunciasForReport")]
        public async Task<IActionResult> GetAllDenunciasForReport()
        {
            var lista = await _denunciaService.GetAllDenunciasForReport();
            var mapped = _mapper.Map<IEnumerable<DenunciaForListDto>>(lista);
            return Ok(mapped);

        }
        [HttpGet("GetDenuncia/{id}")]
        public async Task<IActionResult> GetDenuncia(int id)
        {
            var denuncia = await _denunciaService.GetDenunciaById(id);
            if (denuncia == null) return NotFound(null);
            var mapped = _mapper.Map<DenunciaForListDto>(denuncia);
            return Ok(mapped);
        }
        [HttpPost("CreateDenuncia")]
        public async Task<IActionResult> CreateDenuncia(Denuncia denuncia)
        {
            var resultado = await _denunciaService.CreateDenuncia(denuncia);
            if (resultado.Equals(null))
                return BadRequest(new { mensaje = "Hubo problemas al crear la denuncia." });
            return Ok();
        }
        [HttpPut("UpdateDenuncia")]
        public async Task<IActionResult> UpdateDenuncia(Denuncia denuncia)
        {
            var resultado = await _denunciaService.UpdateDenuncia(denuncia);
            if (resultado.Equals(null))
                return BadRequest(new { mensaje = "Hubo problemas al modificar la denuncia." });
            return Ok();
        }
        [HttpDelete("DeleteDenuncia/{id}")]
        public async Task<IActionResult> DeleteDenuncia(int id)
        {
            var denuncia = await _denunciaService.GetDenunciaById(id);
            if (await _denunciaService.DeleteDenuncia(denuncia))
                return Ok();
            return BadRequest(new { mensaje = "Hubo problemas al eliminar el registro." });
        }
    }
}
