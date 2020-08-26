using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using webapi.business.Dtos.CasosMascota;
using webapi.business.Services.Interf;
using webapi.core.Models;

namespace spatwebapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class CasoMascotaController : ControllerBase
    {
        private ICasoMascotaService _casoMascotaService;
        private readonly IMapper _mapper;
        public CasoMascotaController(ICasoMascotaService casoMascotaService, IMapper mapper)
        {
            _casoMascotaService = casoMascotaService;
            _mapper = mapper;
        }
        // GET: api/CasoMascota
        [HttpGet("GetAllCasosMascota")]
        public async Task<IActionResult> GetAllCasosMascota()
        {
            var lista= await _casoMascotaService.GetAllCasosMascota();
            var resul = _mapper.Map<IEnumerable<CasoMascotaForListDto>>(lista);
            return Ok(resul);
        }

        [HttpGet("GetCasoMascota/{id}")]
        public async Task<CasoMascotaForDetailedDto> GetCasoMascota(int id)
        {
            var obj = await _casoMascotaService.GetCasoMascotaById(id);
            if (obj == null) return null;
            return _mapper.Map<CasoMascotaForDetailedDto>(obj);
        }
        [HttpGet("GetCasoMascotaByIdDenuncia/{id}")]
        public async Task<CasoMascotaForDetailedDto> GetCasoMascotaByIdDenuncia(int id)
        {
            var obj = await _casoMascotaService.GetCasoMascotaByIdDenuncia(id);
            if (obj == null) return null;
            var resul = _mapper.Map<CasoMascotaForDetailedDto>(obj);
            return resul;
        }

        // POST: api/CasoMascota
        [HttpPost("CreateCasoMascota")]
        public async Task<IActionResult> CreateCasoMascota([FromBody] CasoMascota casoMascota)
        {
            if (await _casoMascotaService.CreateCasoMascota(casoMascota)) {
                return Ok(new { mensaje = "El Caso de la Mascota fue creado de manera exitosa.", id= _casoMascotaService.GetIdLastCasoMascota() });
            }
            else
                return BadRequest("Hubo problemas al Registrar.");
        }
        [HttpPut("UpdateCasoMascota/{id}")]
        public async Task<IActionResult> UpdateCasoMascota(CasoMascota casoMascota)
        {
            if (await _casoMascotaService.UpdateCasoMascota(casoMascota))
                return Ok("El Caso de la Mascota fue modificado de manera exitosa.");
            else
                return BadRequest("Hubo problemas al Modificar el Registro.");
        }
        [HttpDelete("DeleteCasoMascota/{id}")]
        public async Task<IActionResult> DeleteCasoMascota(int id)
        {
            var casoMascota = await _casoMascotaService.GetCasoMascotaById(id);
            int valor = casoMascota.DenunciaId;
            if (await _casoMascotaService.DeleteCasoMascota(casoMascota))
                return Ok(new {mensaje= "El Caso de la Mascota fue eliminado de manera exitosa.",iddenuncia= valor });
            else
                return BadRequest("Hubo problemas al Eliminar el Registro.");
        }
    }
}
