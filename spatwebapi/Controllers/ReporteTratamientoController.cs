using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using webapi.business.Dtos.Mascotas;
using webapi.business.Dtos.ReporteTratamientos;
using webapi.business.Services.Interf;

namespace spatwebapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "SuperAdministrador, Administrador")]
    public class ReporteTratamientoController : ControllerBase
    {
        private readonly IReporteTratamientoService _reporteTratamientoService;
        private readonly IMapper _mapper;
        public ReporteTratamientoController(IReporteTratamientoService reporteTratamientoService,
            IMapper mapper)
        {
            _reporteTratamientoService = reporteTratamientoService;
            _mapper = mapper;
        }
        [HttpGet("GetAllReporteTratamiento/{id}")]
        public async Task<IActionResult> GetAllReporteTratamiento(int id)
        {
            var mascota = await _reporteTratamientoService.GetAllReporteTratamiento(id);
            if (mascota == null)
                return Ok(null);

            mascota.ReporteTratamientos = mascota.ReporteTratamientos.OrderBy(x => x.FechaCreacion).ToList();
            var mapped = _mapper.Map<MascotaForReturn>(mascota);
            return Ok(mapped);
        }
        [HttpGet("GetReporteTratamiento/{id}")]
        public async Task<IActionResult> GetReporteTratamiento(int id)
        {
            var reporteTratamiento = await _reporteTratamientoService.GetReporteTratamiento(id);
            if (reporteTratamiento == null) return NotFound(null);
            var mapped = _mapper.Map<ReporteTratamientoForReturnDto>(reporteTratamiento);
            return Ok(mapped);
        }
        [HttpPost("CreateReporteTratamiento")]
        public async Task<IActionResult> CreateReporteTratamiento([FromBody] ReporteTratamientoForCreateDto reporteTratamientoDto)
        {
            var resultado = await _reporteTratamientoService.CreateReporteTratamiento(reporteTratamientoDto);
            if (resultado)
                return Ok();
            return BadRequest(new { mensaje = "Problemas al crear el reporte." });
        }
        [HttpPut("UpdateReporteTratamiento")]
        public async Task<IActionResult> UpdateReporteTratamiento(ReporteTratamientoForUpdateDto reporteTratamientoDto)
        {
            var resultado = await _reporteTratamientoService.UpdateReporteTratamiento(reporteTratamientoDto);
            if (resultado)
                return Ok();
            return BadRequest(new { mensaje = "Problemas al modificar el reporte." });
        }
        [HttpDelete("DeleteReporteTratamiento/{id}")]
        public async Task<IActionResult> DeleteReporteTratamiento(int id)
        {
            var reporteTratamiento = await _reporteTratamientoService.GetReporteTratamiento(id);
            if (reporteTratamiento == null) return NotFound(null);
            if (await _reporteTratamientoService.DeleteReporteTratamiento(reporteTratamiento))
                return Ok();
            else
                return BadRequest(new { mensaje = "Problemas al eliminar el registro." });
        }
        [HttpPut("UpdateFecha")]
        public async Task<IActionResult> UpdateFecha([FromForm] FechaReporteTratamientoForUpdateDto fechaReporteTratamientoDto)
        {
            var reporteTratamiento = await _reporteTratamientoService.GetById(fechaReporteTratamientoDto.Id);
            if (reporteTratamiento != null)
            {
                if (_reporteTratamientoService.VerifyDateIsRepeated(reporteTratamiento.Mascota, fechaReporteTratamientoDto))
                {
                    if (await _reporteTratamientoService.UpdateFecha(fechaReporteTratamientoDto))
                    {
                        var mapped = _mapper.Map<MascotaForReturn>(reporteTratamiento.Mascota);
                        return Ok(mapped);
                    }
                    return BadRequest(new { mensaje = "Ha ocurrido un error actualizando los datos." });
                }
                return BadRequest(new { mensaje = "No se puede asignar una fecha que ya fue asignada a otro registro" });
            }
            return BadRequest(new { mensaje = "No se pudo encontrar el reporte de tratamiento." });
        }
    }
}
