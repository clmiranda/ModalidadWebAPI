using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using webapi.business.Dtos.Fotos;
using webapi.business.Dtos.Mascotas;
using webapi.business.Services.Interf;
using webapi.data.Repositories.Interf;

namespace spatwebapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FotosController : Controller
    {
        private readonly IFotoService _fotoService;
        private readonly IMascotaService _mascotaService;
        private readonly IMapper _mapper;
        private readonly IUserRepository _repoUser;
        public FotosController(IUserRepository repoUser, IMapper mapper, IFotoService fotoService, IMascotaService mascotaService)
        {
            _repoUser = repoUser;
            _mapper = mapper;
            _fotoService = fotoService;
            _mascotaService = mascotaService;
        }
        [AllowAnonymous]
        [HttpGet("{id}", Name = "GetFoto")]
        public async Task<IActionResult> GetFoto(int id)
        {
            var fotoRepo = await _repoUser.GetFoto(id);
            var foto = _mapper.Map<FotoForReturnDto>(fotoRepo);
            return Ok(foto);
        }
        [Authorize(Roles = "Administrador")]
        [HttpPost("Mascota/{mascotaId}/AgregarFotoMascota")]
        public async Task<IActionResult> AgregarFotoMascota(int mascotaId, [FromForm] FotoForCreationDto dtoFoto)
        {
            var resultado = await _fotoService.AgregarFotoMascota(mascotaId, dtoFoto);
            if (!resultado)
                return BadRequest("No se pudo agregar la foto.");

            return Json("");
        }
        [HttpPost("Mascota/{id}/SetFotoPrincipalMascota/{idfoto}")]
        public async Task<IActionResult> SetFotoPrincipalMascota(int id, int idfoto)
        {
            if (await _fotoService.SetFotoPrincipalMascota(id, idfoto))
            {
                var model = await _fotoService.GetMascota(id);
                var mascota = _mapper.Map<MascotaForReturn>(model);
                return Ok(mascota);
            }
            return BadRequest(new { mensaje = "Hubo un conflicto al asignar la Foto Principal." });
        }
        [HttpDelete("Mascota/{mascotaId}/EliminarFotoMascota/{idfoto}")]
        public async Task<IActionResult> EliminarFotoMascota(int mascotaId, int idfoto)
        {
            if (await _fotoService.EliminarFoto(mascotaId, idfoto/*, "mascota"*/)) {
                var mascota = await _mascotaService.GetMascotaById(mascotaId);
                var mapped = _mapper.Map<MascotaForReturn>(mascota);
                return Ok(mapped);
            }
            return BadRequest(new { mensaje = "La foto no se pudo eliminar." });
        }
        //[HttpPost("Reporte/{reporteId}/AgregarFotoReporte")]
        //public async Task<IActionResult> AgregarFotoReporte(int id, IFormFile foto)
        //{
        //    var fotomascota = await _fotoService.AgregarFotoReporte(id, foto);
        //    if (fotomascota)
        //        return Ok(fotomascota);
        //    return BadRequest("No se pudo agregar la foto.");
        //    //var photoToReturn = _mapper.Map<PhotoForReturnDto>(fotomascota);
        //    //return CreatedAtRoute("GetPhoto", new { userId = noticiaId, id = photo.Id }, photoToReturn);
        //}
    }
}