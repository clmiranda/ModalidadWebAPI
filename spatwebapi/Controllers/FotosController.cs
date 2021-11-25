using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using webapi.business.Dtos.Fotos;
using webapi.business.Dtos.Mascotas;
using webapi.business.Services.Interf;

namespace spatwebapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "SuperAdministrador, Administrador")]
    public class FotosController : Controller
    {
        private readonly IFotoService _fotoService;
        private readonly IMascotaService _mascotaService;
        private readonly IMapper _mapper;
        public FotosController(IMapper mapper, IFotoService fotoService, IMascotaService mascotaService)
        {
            _mapper = mapper;
            _fotoService = fotoService;
            _mascotaService = mascotaService;
        }
        //[AllowAnonymous]
        //[HttpGet("{id}", Name = "GetFoto")]
        //public async Task<IActionResult> GetFoto(int id)
        //{
        //    var fotoRepository = atsewait _fotoService.GetFoto(id);
        //    var foto = _mapper.Map<FotoForReturnDto>(fotoRepository);
        //    return Ok(foto);
        //}
        [HttpPost("Mascota/{mascotaId}/AddFotoMascota")]
        public async Task<IActionResult> AddFotoMascota(int mascotaId, [FromForm] FotoForCreationDto dtoFoto)
        {
            var resultado = await _fotoService.AddFotoMascota(mascotaId, dtoFoto);
            if (!resultado)
                return BadRequest("No se pudo agregar la foto.");
            return Json("");
        }
        [HttpPost("Mascota/{id}/SetFotoPrincipalMascota/{idfoto}")]
        public async Task<IActionResult> SetFotoPrincipalMascota(int id, int idfoto)
        {
            if (await _fotoService.SetFotoPrincipalMascota(id, idfoto))
            {
                var modelo = await _fotoService.GetMascota(id);
                var mascota = _mapper.Map<MascotaForReturn>(modelo);
                return Ok(mascota);
            }
            return BadRequest(new { mensaje = "Conflicto al asignar la foto principal." });
        }
        [HttpDelete("Mascota/{mascotaId}/DeleteFotoMascota/{idfoto}")]
        public async Task<IActionResult> DeleteFotoMascota(int mascotaId, int idfoto)
        {
            if (await _fotoService.DeleteFotoMascota(mascotaId, idfoto)) {
                var mascota = await _mascotaService.GetMascotaById(mascotaId);
                var mapped = _mapper.Map<MascotaForReturn>(mascota);
                return Ok(mapped);
            }
            return BadRequest(new { mensaje = "La foto no se pudo eliminar." });
        }
    }
}