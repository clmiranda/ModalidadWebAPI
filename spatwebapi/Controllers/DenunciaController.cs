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
    [AllowAnonymous]
    public class DenunciaController : ControllerBase
    {
        private readonly IDenunciaService _denunciaService;
        private readonly IMapper _mapper;
        public DenunciaController(IDenunciaService denunciaService, IMapper mapper)
        {
            _denunciaService = denunciaService;
            _mapper = mapper;
        }
        [HttpGet("GetAll")]
        public async Task<ActionResult> GetAll()
        {
            var lista = await _denunciaService.GetAll();
            return Ok(lista);
        }
        [HttpGet("GetAllDenuncias")]
        public async Task<ActionResult> GetAllDenuncias([FromQuery]DenunciaParametros parametros) {
            var resul = await _denunciaService.GetAllDenuncias(parametros);
            var lista = _mapper.Map<IEnumerable<DenunciaForListDto>>(resul);
            Response.AddPagination(resul.CurrentPage, resul.PageSize,
                 resul.TotalCount, resul.TotalPages);
            return Ok(lista);

        }
        [HttpGet("GetDenuncia/{id}")]
        public async Task<IActionResult> GetDenuncia(int id)
        {
            var denuncia= await _denunciaService.GetDenunciaById(id);
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
            var resultado=await _denunciaService.UpdateDenuncia(denuncia);
            if (resultado.Equals(null))
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
