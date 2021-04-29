using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webapi.business.Services.Interf;

namespace spatwebapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GraficasController : Controller
    {
        private readonly IGraficaService _graficaService;
        public GraficasController(IGraficaService graficaService)
        {
            _graficaService = graficaService;
        }
        [HttpGet("GetGraficasAdopciones")]
        public async Task<IActionResult> GetGraficasAdopciones(string filtro= "3 meses")
        {
            var resultado = await _graficaService.DatosAdopciones(filtro);
            return Ok(resultado);
        }
        [HttpGet("GetGraficasMascotas")]
        public async Task<IActionResult> GetGraficasMascotas(string filtro = "3 meses")
        {
            var resultado = await _graficaService.DatosMascotas(filtro);
            return Ok(resultado);
        }
        [HttpGet("GetGraficasReporteSeguimientos")]
        public async Task<IActionResult> GetGraficasReporteSeguimientos(string filtro = "3 meses")
        {
            var resultado = await _graficaService.DatosReporteSeguimientos(filtro);
            return Ok(resultado);
        }
    }
}