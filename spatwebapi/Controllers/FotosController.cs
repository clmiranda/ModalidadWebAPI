using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using webapi.business.Dtos.Fotos;
using webapi.business.Dtos.Mascotas;
using webapi.business.Helpers;
using webapi.business.Services.Interf;
using webapi.core.Models;
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
        private readonly IOptions<ConfigurationCloudinary> _cloudinaryConfig;
        private IUnitOfWork _unitOfWork;
        private Cloudinary _cloudinary;
        public FotosController(IUserRepository repoUser, IOptions<ConfigurationCloudinary> cloudinaryConfig, IUnitOfWork unitOfWork, IRepository<ContratoAdopcion> repoContrato, IMapper mapper, IFotoService fotoService, IMascotaService mascotaService)
        {
            _cloudinaryConfig = cloudinaryConfig;
            _repoUser = repoUser;
            _unitOfWork = unitOfWork;

            Account acc = new Account(
                _cloudinaryConfig.Value.CloudName,
                _cloudinaryConfig.Value.ApiKey,
                _cloudinaryConfig.Value.ApiSecret
            );

            _cloudinary = new Cloudinary(acc);
            _mapper = mapper;
            _fotoService = fotoService;
            _mascotaService = mascotaService;
        }
        //[AllowAnonymous]
        //[HttpGet("Mascota/{mascotaId}/GetAllFotosMascota")]
        //public IEnumerable<FotoForReturnDto> GetAllFotosMascota(int mascotaId)
        //{
        //    var lista = _repoMascota.GetAllFotosMascota(mascotaId);
        //    if (lista.Count() > 0)
        //    {
        //        var listaFotos = _mapper.Map<IEnumerable<FotoForReturnDto>>(lista);
        //        return listaFotos;
        //    }
        //    return null;
        //}
        [AllowAnonymous]
        [HttpGet("{id}", Name = "GetFoto")]
        public async Task<IActionResult> GetFoto(int id)
        {
            var photoFromRepo = await _repoUser.GetFoto(id);
            var photo = _mapper.Map<FotoForReturnDto>(photoFromRepo);
            return Ok(photo);
        }
        [Authorize(Roles = "Administrador")]
        [HttpPost("Mascota/{mascotaId}/AgregarFotoMascota")]
        public async Task<IActionResult> AgregarFotoMascota(int mascotaId, [FromForm] FotoForCreationDto fotoMascota)
        {
            var fotomascota = await _fotoService.AgregarFotoMascota(mascotaId, fotoMascota);
            if (fotomascota == null)
                return BadRequest("No se pudo agregar la foto.");

            return Json(fotomascota);
            //var photoToReturn = _mapper.Map<PhotoForReturnDto>(fotomascota);
            //return CreatedAtRoute("GetPhoto", new { userId = noticiaId, id = photo.Id }, photoToReturn);
        }
        [HttpPost("Mascota/{id}/SetFotoPrincipalMascota/{idfoto}")]
        public async Task<IActionResult> SetFotoPrincipalMascota(int id, int idfoto)
        {
            if (await _fotoService.SetFotoPrincipalMascota(id, idfoto)) {
                var model = await _fotoService.GetMascota(id);
                var mascota = _mapper.Map<MascotaForReturn>(model);
                return Ok(mascota);
            }
            return BadRequest("Hubo un conflicto al asignar la Foto Principal.");
        }
        [HttpDelete("Mascota/{mascotaId}/EliminarFotoMascota/{idfoto}")]
        public async Task<IActionResult> EliminarFotoMascota(int mascotaId, int idfoto)
        {
            if (await _fotoService.EliminarFoto(mascotaId, idfoto, "mascota")) {
                var mascota = await _mascotaService.GetMascotaById(mascotaId);
                var mapped = _mapper.Map<MascotaForReturn>(mascota);
                return Ok(mapped);
            }
            return BadRequest("La foto no se pudo eliminar.");
        }
    }
}