using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    [Authorize(Roles = "SuperAdministrador, Administrador")]
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
        public async Task<ActionResult> GetAllAdopciones([FromQuery] AdopcionParametros parametros)
        {
            var resul = await _adopcionService.GetAllAdopciones(parametros);
            var lista = _mapper.Map<IEnumerable<SolicitudAdopcionForList>>(resul);
            Response.AddPagination(resul.CurrentPage, resul.PageSize,
                 resul.TotalCount, resul.TotalPages);
            return Ok(lista);
        }
        [HttpGet("GetById/{id}")]
        public async Task<ActionResult> GetById(int id) {
            var resul = await _adopcionService.GetById(id);
            if (resul == null)
                return NotFound("Id de solicitud de adopción no existe.");
            var modelo = _mapper.Map<SolicitudAdopcionForDetailDto>(resul);
            return Ok(modelo);
        }
        //[HttpGet("GetInformeContrato/{id}")]
        //public async Task<ActionResult> GetInformeContrato(int id)
        //{
        //    var resul = await _adopcionService.GetById(id);
        //    if (resul==null)
        //        return NotFound("Id de contrato no existe.");
        //    var modelo = _mapper.Map<SolicitudAdopcionForDetailDto>(resul);
        //    return Ok(modelo);
        //}
        [HttpGet("GetAll")]
        public async Task<ActionResult> GetAll()
        {
            var lista = await _adopcionService.GetAll();
            var mapped = _mapper.Map<IEnumerable<SolicitudAdopcionForDetailDto>>(lista);
            return Ok(mapped);
        }
        [HttpGet("GetAllSolicitudesAdopcionRechazadas")]
        public async Task<ActionResult> GetAllSolicitudesAdopcionRechazadas()
        {
            var lista = await _adopcionService.GetAllSolicitudesAdopcionRechazadas();
            var mapped = _mapper.Map<IEnumerable<SolicitudAdopcionRechazadaForReturnDto>>(lista);
            return Ok(mapped);
        }
        [HttpGet("GetAllSolicitudesAdopcionCanceladas")]
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
            if (mascota==null)
                return Ok(null);
            if (!mascota.Estado.Equals("Activo"))
                return Ok(null);
            //var resul = await _adopcionService.FindByCondition(x => x.Mascota.Id == id).FirstOrDefaultAsync();
            //var modelo = _mapper.Map<SolicitudAdopcionReturnDto>(resul);
            //if (resul != null) return Ok(new SolicitudAdopcionReturnDto());
            return Ok(new SolicitudAdopcionReturnDto());
        }
        [AllowAnonymous]
        [HttpPost("CreateSolicitudAdopcion")]
        public async Task<IActionResult> CreateSolicitudAdopcion([FromBody] SolicitudAdopcionForCreate solicitudAdopcionDto) {
            var resul = await _adopcionService.CreateSolicitudAdopcion(solicitudAdopcionDto);
            if (resul != null) {
                //var mapeado = _mapper.Map<SolicitudAdopcionReturnDto>(resul);
                return Ok(null);
            }
            return BadRequest(new { mensaje = "Ha ocurrido un error guardando los datos." });
        }
        [HttpPut("UpdateFecha")]
        public async Task<IActionResult> UpdateFecha([FromForm] FechaSolicitudAdopcionForUpdateDto fechaSolicitudDto) {
            var modelo = await _adopcionService.GetById(fechaSolicitudDto.Id);
            if (modelo != null) {
                if (await _adopcionService.UpdateFecha(fechaSolicitudDto))
                {
                    var res = _mapper.Map<SolicitudAdopcionReturnDto>(modelo);
                    return Ok(res);
                }
                return BadRequest(new { mensaje = "Ha ocurrido un error actualizando los datos." });
            }
            return BadRequest(new { mensaje = "No se pudo encontrar la solicitud de adopción." });
        }
        //[HttpGet("DetailAdopcion/{id}")]
        //[AllowAnonymous]
        //public async Task<IActionResult> DetailAdopcion(int id) {
        //    var resul = _mapper.Map<SolicitudAdopcionReturnDto>(await _adopcionService.GetById(id));
        //    if (resul==null)
        //        return BadRequest(null);
        //    return Ok(resul);
        //}
        [HttpPut("{id}/AprobarSolicitudAdopcion")]
        public async Task<IActionResult> AprobarSolicitudAdopcion(int id) {
            var modelo = await _adopcionService.GetById(id);
            if (modelo!=null)
            {
                if (await _adopcionService.AprobarSolicitudAdopcion(id)) {
                    var solicitudAdopcion = _mapper.Map<SolicitudAdopcionReturnDto>(modelo);
                        return Ok(solicitudAdopcion);
                }
                else
                    return BadRequest(new { mensaje = "Hubo problemas al guardar los datos." });
            }
            return BadRequest(new { mensaje = "No se pudo encontrar la solicitud de adopción." });
        }
        [HttpPut("RechazarSolicitudAdopcion")]
        public async Task<IActionResult> RechazarSolicitudAdopcion(SolicitudAdopcionRechazadaForCreateDto solicitudAdopcionRechazadaDto) {
            var modelo = await _adopcionService.GetById(solicitudAdopcionRechazadaDto.SolicitudAdopcionId);
            if (modelo!=null)
            {
                if (await _adopcionService.RechazarSolicitudAdopcion(solicitudAdopcionRechazadaDto.SolicitudAdopcionId, modelo.Mascota.Id)) {
                    if (await _adopcionService.CreateSolicitudAdopcionRechazada(solicitudAdopcionRechazadaDto))
                    {
                        var solicitudAdopcion = _mapper.Map<SolicitudAdopcionReturnDto>(modelo);
                        return Ok(solicitudAdopcion);
                    }
                    return BadRequest(new { mensaje = "Ocurrio un problema al tratar de generar el informe." });
                }

                return BadRequest(new { mensaje = "Ha ocurrido un problema al tratar de rechazar la solicitud de adopción." });
            }
            return BadRequest(new { mensaje = "No se pudo encontrar la solicitud de adopción." });
        }
        [HttpPut("CancelarAdopcion")]
        public async Task<IActionResult> CancelarAdopcion(SolicitudAdopcionCanceladaForCreateDto solicitudAdopcionCanceladaDto) {
            var solicitudAdopcion = await _adopcionService.GetById(solicitudAdopcionCanceladaDto.SolicitudAdopcionId);
            if (solicitudAdopcion!=null)
            {
                if (await _adopcionService.CancelarAdopcion(solicitudAdopcionCanceladaDto.SolicitudAdopcionId, solicitudAdopcion.Mascota.Id))
                {
                    if (await _adopcionService.CreateSolicitudAdopcionCancelada(solicitudAdopcionCanceladaDto)) {
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