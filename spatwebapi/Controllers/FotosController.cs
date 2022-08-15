using System.Linq;
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
        [HttpPost("Mascota/{idMascota}/AddFotoMascota")]
        public async Task<IActionResult> AddFotoMascota(int idMascota, [FromForm] FotoForCreationDto fotoDto)
        {
            var resultado = await _fotoService.AddFotoMascota(idMascota, fotoDto);
            if (resultado.Equals("ErrorCount"))
                return BadRequest(new { mensaje = "No se pueden agregar más de 4 fotos." });
            else if (resultado.Equals("ErrorSave"))
                return BadRequest(new { mensaje = "No se pudo agregar la(s) foto(s)." });
            var mascota = await _mascotaService.GetMascotaById(idMascota);
            var mapped = _mapper.Map<MascotaForDetailedDto>(mascota);
            return Ok(mapped);
        }
        [HttpPost("Mascota/{idMascota}/SetFotoPrincipalMascota/{idFoto}")]
        public async Task<IActionResult> SetFotoPrincipalMascota(int idMascota, int idFoto)
        {
            if (await _fotoService.SetFotoPrincipalMascota(idMascota, idFoto))
            {
                var mascota = await _fotoService.GetMascota(idMascota);
                var mapped = _mapper.Map<MascotaForDetailedDto>(mascota);
                mapped.Fotos = mapped.Fotos.OrderByDescending(x => x.IsPrincipal).ToList();
                return Ok(mapped);
            }
            return BadRequest(new { mensaje = "Conflicto al asignar la foto principal." });
        }
        [HttpDelete("Mascota/{idMascota}/DeleteFotoMascota/{idFoto}")]
        public async Task<IActionResult> DeleteFotoMascota(int idMascota, int idFoto)
        {
            if (await _fotoService.DeleteFotoMascota(idMascota, idFoto))
            {
                var mascota = await _mascotaService.GetMascotaById(idMascota);
                var mapped = _mapper.Map<MascotaForReturn>(mascota);
                return Ok(mapped);
            }
            return BadRequest(new { mensaje = "La foto no se pudo eliminar." });
        }
    }
}