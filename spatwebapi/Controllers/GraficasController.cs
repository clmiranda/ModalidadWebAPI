using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using webapi.business.Services.Interf;

namespace spatwebapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "SuperAdministrador, Administrador")]
    public class GraficasController : ControllerBase
    {
        private readonly IGraficaService _graficaService;
        public GraficasController(IGraficaService graficaService)
        {
            _graficaService = graficaService;
        }
        [HttpPost("GetGraficasAdopciones")]
        public async Task<IActionResult> GetGraficasAdopciones(string[] fechas)
        {
            var resultado = await _graficaService.DatosAdopciones(fechas);
            return Ok(resultado);
        }
        [HttpPost("GetGraficasMascotas")]
        public async Task<IActionResult> GetGraficasMascotas(string[] fechas)
        {
            var resultado = await _graficaService.DatosMascotas(fechas);
            return Ok(resultado);
        }
        [HttpPost("GetGraficasReporteSeguimientos")]
        public async Task<IActionResult> GetGraficasReporteSeguimientos(string[] fechas)
        {
            var resultado = await _graficaService.DatosReporteSeguimientos(fechas);
            return Ok(resultado);
        }
        [HttpGet("GetDataForDashboard")]
        public async Task<IActionResult> GetDataForDashboard()
        {
            var resultado = await _graficaService.GetDataForDashboard();
            return Ok(resultado);
        }
    }
}