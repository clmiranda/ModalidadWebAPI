using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webapi.business.Dtos.Mascotas;
using webapi.business.Helpers;
using webapi.business.Pagination;
using webapi.business.Services.Interf;

namespace spatwebapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MascotaController : ControllerBase
    {
        private readonly IMascotaService _mascotaService;
        private readonly IDenunciaService _denunciaService;
        private readonly IMapper _mapper;
        public MascotaController(IMascotaService mascotaService, IMapper mapper,
            IDenunciaService denunciaService)
        {
            _mascotaService = mascotaService;
            _denunciaService = denunciaService;
            _mapper = mapper;
        }
        [HttpGet("GetAll")]
        public ActionResult GetAll()
        {
            var lista = _mascotaService.GetAll();
            return Ok(lista);
        }
        [HttpGet("GetMascota/{id}")]
        public async Task<IActionResult> GetMascota(int id)
        {
            var mascota = await _mascotaService.GetMascotaById(id);
            if (mascota == null) return NotFound(null);
            var resul = _mapper.Map<MascotaForDetailedDto>(mascota);
            return Ok(resul);
        }
        [HttpGet("GetMascotaDenuncia/{id}")]
        public async Task<IActionResult> GetMascotaDenuncia(int id)
        {
            var denuncia = await _denunciaService.GetDenunciaById(id);
            if (denuncia == null)
                return NotFound(null);
            var obj = _mascotaService.FindByCondition(x => x.DenunciaId == id).FirstOrDefault();
            if (obj == null) return Ok(new MascotaForDetailedDto{ DenunciaId=id});
            var resul = _mapper.Map<MascotaForDetailedDto>(obj);
            return Ok(resul);
        }

        [HttpGet("GetAllMascotaAdopcion")]
        [AllowAnonymous]
        public async Task<ActionResult> GetAllMascotaAdopcion([FromQuery] MascotaParametros parametros)
        {
            var resul = await _mascotaService.GetAllMascotas(parametros);
            var lista = _mapper.Map<IEnumerable<MascotaForAdopcionDto>>(resul);
            Response.AddPagination(resul.CurrentPage, resul.PageSize,
                 resul.TotalCount, resul.TotalPages);
            return Ok(lista);
        }

        [HttpGet("GetAllMascotaAdmin")]
        [AllowAnonymous]
        public async Task<ActionResult> GetAllMascotaAdmin([FromQuery] MascotaParametros parametros)
        {
            var resul = await _mascotaService.GetAllMascotas(parametros);
            //var lista = _mascotaService.FindByCondition(x => x.ContratoAdopcion == null).ToList();
            var lista = _mapper.Map<IEnumerable<MascotaForDetailedDto>>(resul);
            lista = lista.OrderByDescending(x=>x.Estado.Equals("Inactivo")).ToList();
            Response.AddPagination(resul.CurrentPage, resul.PageSize,
                 resul.TotalCount, resul.TotalPages);
            return Ok(lista);
        }

        [HttpPost("CreateMascota")]
        public async Task<IActionResult> CreateMascota([FromBody] MascotaForCreateDto dto)
        {
            var mascota = await _mascotaService.CreateMascota(dto);
            if (mascota.Equals(null))
                return BadRequest(new { mensaje = "Hubo problemas al crear la Mascota." });
            var mapped = _mapper.Map<MascotaForDetailedDto>(mascota);
            return Ok(mapped);
        }

        [HttpPut("UpdateMascota")]
        public async Task<ActionResult> UpdateMascota([FromBody] MascotaForUpdateDto dto)
        {
            var mascota = await _mascotaService.UpdateMascota(dto);
            if (mascota.Equals(null))
                return BadRequest(new { mensaje = "Hubo problemas al actualizar los datos." });
            var mapped = _mapper.Map<MascotaForDetailedDto>(mascota);
            return Ok(mapped);
        }

        [HttpPut("ChangeStateSituacion/{id}")]
        public async Task<IActionResult> ChangeStateSituacion([FromBody] string estado, int id)
        {
            var mascota = await _mascotaService.GetMascotaById(id);
            if (mascota == null)
                return NotFound(new { mensaje = "No se encontro la Mascota." });
            if (await _mascotaService.ChangeEstado(estado, id))
                return Ok();
            else
                return BadRequest(new { mensaje = "Hubo problemas al modificar el estado de la mascota." });
        }

        [HttpDelete("DeleteMascota/{id}")]
        public async Task<IActionResult> DeleteMascota(int id)
        {
            var mascota = await _mascotaService.GetMascotaById(id);
            if (mascota == null)
                return NotFound(new { mensaje = "La mascota no fue encontrada." });
            if (await _mascotaService.DeleteMascota(mascota))
                return Ok();
            else
                return BadRequest(new { mensaje = "Hubo problemas al eliminar el registro." });
        }
    }
}
