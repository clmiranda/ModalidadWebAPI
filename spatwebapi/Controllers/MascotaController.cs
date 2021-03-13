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
        public async Task<ActionResult> GetAllMascotas([FromQuery] MascotaParametros parametros)
        {
            var resul = await _mascotaService.GetAllMascotas(parametros);
            //var lista = _mascotaService.FindByCondition(x => x.ContratoAdopcion == null).ToList();
            var lista = _mapper.Map<IEnumerable<MascotaForDetailedDto>>(resul);
            Response.AddPagination(resul.CurrentPage, resul.PageSize,
                 resul.TotalCount, resul.TotalPages);
            return Ok(lista);
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

        //[HttpGet("GetLastRegister")]
        //public MascotaForDetailedDto GetLastRegister()
        //{
        //    var obj = _mascotaService.GetAllMascotas().Result.LastOrDefault();
        //    if (obj == null)
        //        return null;
        //    var resul = _mapper.Map<MascotaForDetailedDto>(obj);
        //    return resul;
        //}

        //Retornar el nombre de la mascota con su foto principal la cual tiene q establecerse
        //desde la vista de agregar mascota la principal
        [HttpGet("GetAllMascotaAdopcion")]
        [AllowAnonymous]
        public async Task<ActionResult> GetAllMascotaAdopcion([FromQuery] MascotaParametros parametros)
        {
            var resul = await _mascotaService.GetAllMascotas(parametros);
            //var lista = _mascotaService.FindByCondition(x => x.ContratoAdopcion == null).ToList();
            var lista = _mapper.Map<IEnumerable<MascotaForAdopcionDto>>(resul);
            Response.AddPagination(resul.CurrentPage, resul.PageSize,
                 resul.TotalCount, resul.TotalPages);
            return Ok(lista);
        }

        // POST: api/CasoMascota
        [HttpPost("CreateMascota")]
        public async Task<IActionResult> CreateMascota([FromBody] Mascota mascota)
        {
            var m = await _mascotaService.CreateMascota(mascota);
            if (mascota.Equals(null))
                return BadRequest(new { mensaje = "Hubo problemas al crear la Mascota." });
            var mapped = _mapper.Map<MascotaForDetailedDto>(m);
            return Ok(mapped);
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
                return BadRequest(new { mensaje = "Hubo problemas al actualizar los datos." });
            var mapped = _mapper.Map<MascotaForDetailedDto>(update);
            return Ok(mapped);
        }
        [HttpPut("ChangeStateSituacion/{id}")]
        public async Task<IActionResult> ChangeStateSituacion([FromBody] string estado, int id)
        {
            var mascota = await _mascotaService.GetMascotaById(id);
            //mascota.EstadoSituacion = estado;
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
