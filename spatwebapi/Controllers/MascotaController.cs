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
    [Authorize(Roles = "SuperAdministrador, Administrador")]
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
        [HttpGet("GetAllMascotasForReport")]
        public async Task<IActionResult> GetAllMascotasForReport()
        {
            var lista = await _mascotaService.GetAllMascotasForReport();
            var mapped = _mapper.Map<IEnumerable<MascotaForDetailedDto>>(lista);
            return Ok(mapped);
        }
        [HttpGet("GetMascota/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetMascota(int id)
        {
            var mascota = await _mascotaService.GetMascotaById(id);
            if (mascota == null) return Ok(null);
            var mapped = _mapper.Map<MascotaForDetailedDto>(mascota);
            mapped.ReporteTratamientos = mapped.ReporteTratamientos.OrderBy(x => x.FechaCreacion).ToList();
            return Ok(mapped);
        }
        [HttpGet("GetMascotaDenuncia/{id}")]
        public async Task<IActionResult> GetMascotaDenuncia(int id)
        {
            var denuncia = await _denunciaService.GetDenunciaById(id);
            if (denuncia == null)
                return Ok(null);
            var mascota = _mascotaService.FindByCondition(x => x.DenunciaId == id).FirstOrDefault();
            if (mascota == null) return Ok(new MascotaForDetailedDto { DenunciaId = id });
            var mapped = _mapper.Map<MascotaForDetailedDto>(mascota);
            mapped.Fotos = mapped.Fotos.OrderByDescending(x => x.IsPrincipal).ToList();
            return Ok(mapped);
        }
        [HttpGet("GetAllMascotasForAdopcion")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllMascotasForAdopcion([FromQuery] MascotaForAdopcionParametros parametros)
        {
            var lista = await _mascotaService.GetAllMascotasForAdopcion(parametros);
            var mapped = _mapper.Map<IEnumerable<MascotaForAdopcionDto>>(lista);
            Response.AddPagination(lista.CurrentPage, lista.PageSize,
                 lista.TotalCount, lista.TotalPages);
            return Ok(mapped);
        }
        [HttpGet("GetAllMascotasForAdopcionPresencial")]
        public async Task<IActionResult> GetAllMascotasForAdopcionPresencial([FromQuery] MascotaForAdopcionParametros parametros)
        {
            var lista = await _mascotaService.GetAllMascotasForAdopcion(parametros);
            var mapped = _mapper.Map<IEnumerable<MascotaForReturn>>(lista);
            Response.AddPagination(lista.CurrentPage, lista.PageSize,
                 lista.TotalCount, lista.TotalPages);
            return Ok(mapped);
        }
        [HttpGet("GetAllMascotasForAdmin")]
        public async Task<IActionResult> GetAllMascotasForAdmin([FromQuery] MascotaParametros parametros)
        {
            var lista = await _mascotaService.GetAllMascotas(parametros);
            var mapped = _mapper.Map<IEnumerable<MascotaForDetailedDto>>(lista);
            Response.AddPagination(lista.CurrentPage, lista.PageSize,
                 lista.TotalCount, lista.TotalPages);
            return Ok(mapped);
        }
        [HttpPost("CreateMascota")]
        public async Task<IActionResult> CreateMascota([FromBody] MascotaForCreateDto mascotaDto)
        {
            var resultado = await _mascotaService.CreateMascota(mascotaDto);
            if (resultado.Equals(null))
                return BadRequest(new { mensaje = "Hubo problemas al crear la Mascota." });
            var mapped = _mapper.Map<MascotaForDetailedDto>(resultado);
            return Ok(mapped);
        }
        [HttpPut("UpdateMascota")]
        public async Task<IActionResult> UpdateMascota([FromBody] MascotaForUpdateDto mascotaDto)
        {
            var resultado = await _mascotaService.UpdateMascota(mascotaDto);
            if (resultado.Equals(null))
                return BadRequest(new { mensaje = "Hubo problemas al actualizar los datos." });
            var mapped = _mapper.Map<MascotaForDetailedDto>(resultado);
            return Ok(mapped);
        }
        [HttpPut("ChangeEstado/{id}")]
        public async Task<IActionResult> ChangeEstado([FromBody] string estado, int id)
        {
            var mascota = await _mascotaService.GetMascotaById(id);
            if (mascota == null)
                return NotFound(new { mensaje = "No se encontro la Mascota." });
            if (await _mascotaService.ChangeEstado(estado, id))
                return Ok();
            else
                return BadRequest(new { mensaje = "Problemas al modificar el estado de la mascota." });
        }
    }
}