using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webapi.business.Dtos.Mascotas;
using webapi.business.Helpers;
using webapi.business.Services.Interf;
using webapi.core.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace spatwebapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MascotaController : ControllerBase
    {
        private IMascotaService _mascotaService;
        private IDenunciaService _denunciaService;
        private readonly IMapper _mapper;
        public MascotaController(IMascotaService mascotaService, IMapper mapper,
            IDenunciaService denunciaService)
        {
            _mascotaService = mascotaService;
            _denunciaService = denunciaService;
            _mapper = mapper;
        }
        [HttpGet("GetAllMascotas")]
        public IEnumerable<MascotaForDetailedDto> GetAllMascotas()
        {
            var lista = _mascotaService.FindByCondition(x => x.ContratoAdopcion == null).ToList();
            var resul = _mapper.Map<IEnumerable<MascotaForDetailedDto>>(lista);
            return resul;
        }

        //[HttpGet("GetMascota/{id}")]
        //public ActionResult<Mascota> GetMascota(int id)
        //{
        //    var obj = _mascotaService.FindByCondition(x=>x.Id==id);
        //    if (obj == null) return null;
        //    var resul = _mapper.Map<MascotaForDetailedDto>(obj);
        //    return Ok(resul);
        //}
        [HttpGet("GetMascota/{id}")]
        public MascotaForDetailedDto GetMascota(int id)
        {
            var obj = _mascotaService.FindByCondition(x => x.Id == id).FirstOrDefault();
            if (obj == null) return null;
            var resul = _mapper.Map<MascotaForDetailedDto>(obj);
            return resul;
        }
        [HttpGet("GetMascotaDenuncia/{id}")]
        public async Task<ActionResult<MascotaForDetailedDto>> GetMascotaDenuncia(int id)
        {
            var denuncia =await _denunciaService.GetDenunciaById(id);
            if (denuncia == null)
                return NotFound();
            var obj = _mascotaService.FindByCondition(x => x.DenunciaId == id).FirstOrDefault();
            if (obj == null) return Ok(null);
            var resul = _mapper.Map<MascotaForDetailedDto>(obj);
            return Ok(resul);
        }

        [HttpGet("GetLastRegister")]
        public MascotaForDetailedDto GetLastRegister()
        {
            var obj = _mascotaService.GetAllMascotas().Result.LastOrDefault();
            if (obj == null)
                return null;
            var resul = _mapper.Map<MascotaForDetailedDto>(obj);
            return resul;
        }

        //Retornar el nombre de la mascota con su foto principal la cual tiene q establecerse
        //desde la vista de agregar mascota la principal
        [HttpGet("GetAllMascotaAdopcion")]
        [AllowAnonymous]
        public async Task<ActionResult> GetAllMascotaAdopcion([FromQuery] MascotaParametros parametros)
        {
            //var lista = _mascotaService.FindByCondition(x=>x.EstadoSituacion.Equals("Activo") && x.ContratoAdopcion==null).ToList();
            //var resul = _mapper.Map<IEnumerable<MascotaForAdopcionDto>>(lista);
            var resul = await _mascotaService.GetAllMascotas(parametros);
            return Ok(resul);
        }

        // POST: api/CasoMascota
        [HttpPost("CreateMascota")]
        public async Task<IActionResult> CreateMascota([FromBody] Mascota mascotaDto)
        {
            var mascota = await _mascotaService.CreateMascota(mascotaDto);
            if (mascota.Equals(null))
                return BadRequest("Hubo problemas al crear la Mascota.");

            return Ok(mascota);
            //if (await _mascotaService.CreateMascota(mascota))
            //{
            //return Ok(new { mensaje = "El registro de la mascota fue realizado de manera exitosa.", id = _mascotaService.GetIdLastMascota() });
            //}
            //else
            //    return BadRequest("Hubo problemas al realizar el Registro de la Mascota.");
        }
        [HttpPut("UpdateMascota")]
        public async Task<ActionResult> UpdateMascota([FromBody] Mascota mascota)
        {
            //var m= await _mascotaService.GetMascotaById(mascota.Id);
            //if (m == null) return null;
            var update = await _mascotaService.UpdateMascota(mascota);
            if (update.Equals(null))
                return BadRequest("Hubo problemas al Modificar el Registro de la Mascota.");
            else
                return Ok(update);
        }
        [HttpPut("ChangeStateSituacion/{id}")]
        public async Task<IActionResult> ChangeStateSituacion([FromBody] string estado, int id)
        {
            var mascota = await _mascotaService.GetMascotaById(id);
            //mascota.EstadoSituacion = estado;
            if (mascota == null)
                return NotFound("No se encontro la Mascota.");
            if (await _mascotaService.ChangeEstado(estado, id))
                return Ok("El estado de la Mascota " + mascota.Nombre + " se modifico correctamente.");
            else
                return BadRequest("Hubo problemas al modificar el estado de la Mascota " + mascota.Nombre + ".");
        }
        [HttpDelete("DeleteMascota/{id}")]
        public async Task<IActionResult> DeleteMascota(int id)
        {
            var mascota = await _mascotaService.GetMascotaById(id);
            if (mascota == null)
                return NotFound("La Mascota no fue encontrada.");
            if (await _mascotaService.DeleteMascota(mascota))
                return Ok(/*new { mensaje = */"El Registro de la Mascota fue eliminado de manera exitosa."/*, idcaso = valor }*/);
            else
                return BadRequest("Hubo problemas al Eliminar el Registro de la Mascota.");
        }
    }
}
