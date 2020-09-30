using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using webapi.business.Dtos.Mascotas;
using webapi.business.Services.Interf;
using webapi.core.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace spatwebapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class MascotaController : ControllerBase
    {
        private IMascotaService _mascotaService;
        private readonly IMapper _mapper;
        public MascotaController(IMascotaService mascotaService, IMapper mapper)
        {
            _mascotaService = mascotaService;
            _mapper = mapper;
        }
        [HttpGet("GetAllMascotas")]
        public IEnumerable<MascotaForReturn> GetAllMascotas()
        {
            var lista = _mascotaService.GetAllMascotas();
            var resul = _mapper.Map<IEnumerable<MascotaForReturn>>(lista);
            return resul;
        }

        [HttpGet("GetMascota/{id}")]
        public async Task<MascotaForDetailedDto> GetMascota(int id)
        {
            var obj = await _mascotaService.GetMascotaById(id);
            if (obj == null) return null;
            var resul = _mapper.Map<MascotaForDetailedDto>(obj);
            return resul;
        }
        [HttpGet("GetMascotaAdopcion/{id}")]
        public async Task<MascotaForReturn> GetMascotaAdopcion(int id)
        {
            var obj = await _mascotaService.GetMascotaById(id);
            if (obj == null) return null;
            var resul = _mapper.Map<MascotaForReturn>(obj);
            return resul;
        }

        //Retornar el nombre de la mascota con su foto principal la cual tiene q establecerse
        //desde la vista de agregar mascota la principal
        [HttpGet("GetAllMascotaAdopcion")]
        public IEnumerable<MascotaForAdopcionDto> GetAllMascotaAdopcion()
        {
            var lista = _mascotaService.GetAllMascotaAdopcion();
            var resul = _mapper.Map<IEnumerable<MascotaForAdopcionDto>>(lista);
            return resul;
        }

        // POST: api/CasoMascota
        [HttpPost("CreateMascota")]
        public async Task<IActionResult> CreateMascota([FromBody] Mascota mascota)
        {
            if (await _mascotaService.CreateMascota(mascota))
            {
                return Ok(new { mensaje = "El registro de la mascota fue realizado de manera exitosa.", id = _mascotaService.GetIdLastMascota() });
            }
            else
                return BadRequest("Hubo problemas al realizar el Registro de la Mascota.");
        }
        [HttpPut("UpdateMascota/{id}")]
        public async Task<IActionResult> UpdateMascota([FromBody] Mascota mascota)
        {
            if (await _mascotaService.UpdateMascota(mascota))
                return Ok("El Registro de la Mascota fue modificado de manera exitosa.");
            else
                return BadRequest("Hubo problemas al Modificar el Registro de la Mascota.");
        }
        [HttpPut("ChangeStateSituacion/{id}")]
        public async Task<IActionResult> ChangeStateSituacion([FromBody] string estado, int id)
        {
            var mascota = await _mascotaService.GetMascotaById(id);
            mascota.EstadoSituacion = estado;
            if (await _mascotaService.UpdateMascota(mascota))
                return Ok("El Estado de la Mascota "+mascota.Nombre+ " fue modificado a "+estado+".");
            else
                return BadRequest("Hubo problemas al Modificar el Estado de la Mascota "+mascota.Nombre+".");
        }
        [HttpDelete("DeleteMascota/{id}")]
        public async Task<IActionResult> DeleteMascota(int id)
        {
            var mascota = await _mascotaService.GetMascotaById(id);
            //int valor = mascota.CasoMascotaId;
            if (await _mascotaService.DeleteMascota(mascota))
                return Ok(/*new { mensaje = */"El Registro de la Mascota fue eliminado de manera exitosa."/*, idcaso = valor }*/);
            else
                return BadRequest("Hubo problemas al Eliminar el Registro de la Mascota.");
        }
    }
}
