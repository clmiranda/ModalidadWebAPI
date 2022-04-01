using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using webapi.business.Dtos.Adopciones;
using webapi.business.Dtos.SolicitudAdopcionCancelada;
using webapi.business.Dtos.SolicitudAdopcionRechazada;
using webapi.business.Helpers;
using webapi.business.Pagination;
using webapi.business.Services.Interf;

namespace spatwebapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdopcionController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IAdopcionService _adopcionService;
        private readonly IMascotaService _mascotaService;
        public AdopcionController(IAdopcionService adopcionService,
            IMapper mapper, IMascotaService mascotaService)
        {
            _adopcionService = adopcionService;
            _mapper = mapper;
            _mascotaService = mascotaService;
        }
        [HttpGet("GetAllAdopciones")]
        [Authorize(Roles = "SuperAdministrador, Administrador")]
        public async Task<ActionResult> GetAllAdopciones([FromQuery] AdopcionParametros parametros)
        {
            var lista = await _adopcionService.GetAllAdopciones(parametros);
            var mapped = _mapper.Map<IEnumerable<SolicitudAdopcionForList>>(lista);
            mapped = mapped.OrderByDescending(x => x.FechaAdopcion).ToList().ToList();
            Response.AddPagination(lista.CurrentPage, lista.PageSize,
                 lista.TotalCount, lista.TotalPages);
            return Ok(mapped);
        }
        [HttpGet("GetById/{id}")]
        [Authorize(Roles = "SuperAdministrador, Administrador, Voluntario")]
        public async Task<ActionResult> GetById(int id)
        {
            var solicitudAdopcion = await _adopcionService.GetById(id);
            if (solicitudAdopcion == null)
                return Ok(null);
            var mapped = _mapper.Map<SolicitudAdopcionForDetailDto>(solicitudAdopcion);
            return Ok(mapped);
        }
        [HttpGet("GetAll")]
        [Authorize(Roles = "SuperAdministrador, Administrador")]
        public async Task<ActionResult> GetAll()
        {
            var lista = await _adopcionService.GetAll();
            var mapped = _mapper.Map<IEnumerable<SolicitudAdopcionForDetailDto>>(lista);
            return Ok(mapped);
        }
        [HttpGet("GetAllSolicitudesAdopcionRechazadas")]
        [Authorize(Roles = "SuperAdministrador, Administrador")]
        public async Task<ActionResult> GetAllSolicitudesAdopcionRechazadas()
        {
            var lista = await _adopcionService.GetAllSolicitudesAdopcionRechazadas();
            var mapped = _mapper.Map<IEnumerable<SolicitudAdopcionRechazadaForReturnDto>>(lista);
            return Ok(mapped);
        }
        [HttpGet("GetAllSolicitudesAdopcionCanceladas")]
        [Authorize(Roles = "SuperAdministrador, Administrador")]
        public async Task<ActionResult> GetAllSolicitudesAdopcionCanceladas()
        {
            var lista = await _adopcionService.GetAllSolicitudesAdopcionCanceladas();
            var mapped = _mapper.Map<IEnumerable<SolicitudAdopcionCanceladaForReturnDto>>(lista);
            return Ok(mapped);
        }
        [AllowAnonymous]
        [HttpGet("GetSolicitudAdopcionByIdMascota/{id}")]
        public async Task<ActionResult<SolicitudAdopcionReturnDto>> GetSolicitudAdopcionByIdMascota(int id)
        {
            var mascota = await _mascotaService.GetMascotaById(id);
            if (mascota == null)
                return Ok(null);
            if (!mascota.Estado.Equals("Activo"))
                return Ok(null);
            return Ok(new SolicitudAdopcionReturnDto());
        }
        [AllowAnonymous]
        [HttpPost("CreateSolicitudAdopcion")]
        public async Task<IActionResult> CreateSolicitudAdopcion([FromBody] SolicitudAdopcionForCreate solicitudAdopcionDto)
        {
            var resultado = await _adopcionService.CreateSolicitudAdopcion(solicitudAdopcionDto);
            if (resultado != null)
                return Ok(null);
            return BadRequest(new { mensaje = "Ha ocurrido un error guardando los datos." });
        }
        [HttpPut("UpdateFecha")]
        [Authorize(Roles = "SuperAdministrador, Administrador")]
        public async Task<IActionResult> UpdateFecha([FromForm] FechaSolicitudAdopcionForUpdateDto fechaSolicitudDto)
        {
            var solicitudAdopcion = await _adopcionService.GetById(fechaSolicitudDto.Id);
            if (solicitudAdopcion != null)
            {
                if (await _adopcionService.UpdateFecha(fechaSolicitudDto))
                    return Ok();
                return BadRequest(new { mensaje = "Ha ocurrido un error actualizando los datos." });
            }
            return BadRequest(new { mensaje = "No se pudo encontrar la solicitud de adopción." });
        }
        [HttpPut("{id}/AprobarSolicitudAdopcion")]
        [Authorize(Roles = "SuperAdministrador, Administrador")]
        public async Task<IActionResult> AprobarSolicitudAdopcion(int id)
        {
            var solicitudAdopcion = await _adopcionService.GetById(id);
            if (solicitudAdopcion != null)
            {
                if (await _adopcionService.AprobarSolicitudAdopcion(id))
                {
                    var mapped = _mapper.Map<SolicitudAdopcionReturnDto>(solicitudAdopcion);
                    return Ok(mapped);
                }
                else
                    return BadRequest(new { mensaje = "Hubo problemas al guardar los datos." });
            }
            return BadRequest(new { mensaje = "No se pudo encontrar la solicitud de adopción." });
        }
        [HttpPut("RechazarSolicitudAdopcion")]
        [Authorize(Roles = "SuperAdministrador, Administrador")]
        public async Task<IActionResult> RechazarSolicitudAdopcion(SolicitudAdopcionRechazadaForCreateDto solicitudAdopcionRechazadaDto)
        {
            var solicitudAdopcion = await _adopcionService.GetById(solicitudAdopcionRechazadaDto.SolicitudAdopcionId);
            if (solicitudAdopcion != null)
            {
                if (await _adopcionService.RechazarSolicitudAdopcion(solicitudAdopcionRechazadaDto.SolicitudAdopcionId, solicitudAdopcion.Mascota.Id))
                {
                    if (await _adopcionService.CreateSolicitudAdopcionRechazada(solicitudAdopcionRechazadaDto))
                    {
                        var mapped = _mapper.Map<SolicitudAdopcionReturnDto>(solicitudAdopcion);
                        return Ok(mapped);
                    }
                    return BadRequest(new { mensaje = "Ocurrio un problema al tratar de generar el informe." });
                }

                return BadRequest(new { mensaje = "Ha ocurrido un problema al tratar de rechazar la solicitud de adopción." });
            }
            return BadRequest(new { mensaje = "No se pudo encontrar la solicitud de adopción." });
        }
        [HttpPut("CancelarAdopcion")]
        [Authorize(Roles = "SuperAdministrador, Administrador")]
        public async Task<IActionResult> CancelarAdopcion(SolicitudAdopcionCanceladaForCreateDto solicitudAdopcionCanceladaDto)
        {
            var solicitudAdopcion = await _adopcionService.GetById(solicitudAdopcionCanceladaDto.SolicitudAdopcionId);
            if (solicitudAdopcion != null)
            {
                if (await _adopcionService.CancelarAdopcion(solicitudAdopcionCanceladaDto.SolicitudAdopcionId, solicitudAdopcion.Mascota.Id))
                {
                    if (await _adopcionService.CreateSolicitudAdopcionCancelada(solicitudAdopcionCanceladaDto))
                    {
                        var mapped = _mapper.Map<SolicitudAdopcionReturnDto>(solicitudAdopcion);
                        return Ok(mapped);
                    }
                    return BadRequest(new { mensaje = "Ha ocurrido un error al cancelar la solicitud de adopción." });
                }
                return BadRequest(new { mensaje = "Ha ocurrido un error al cancelar la solicitud de adopción." });
            }
            return BadRequest(new { mensaje = "No se pudo encontrar la solicitud de adopción." });
        }
    }
}