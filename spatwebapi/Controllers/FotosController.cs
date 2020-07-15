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
using webapi.business.Dtos.Fotos;
using webapi.business.Helpers;
using webapi.core.Models;
using webapi.data.Repositories.Interf;

namespace spatwebapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FotosController : Controller
    {
        private readonly IUserRepository _repoUser;
        private readonly IMascotaRepository _repoMascota;
        private readonly IRepository<ContratoAdopcion> _repoContrato;
        private readonly IMapper _mapper;
        private readonly IOptions<ConfigurationCloudinary> _cloudinaryConfig;
        private IUnitOfWork _unitOfWork;
        private Cloudinary _cloudinary;
        public FotosController(IUserRepository repoUser, IMascotaRepository repoMascota, IMapper mapper,
            IOptions<ConfigurationCloudinary> cloudinaryConfig, IUnitOfWork unitOfWork, IRepository<ContratoAdopcion> repoContrato)
        {
            _cloudinaryConfig = cloudinaryConfig;
            _mapper = mapper;
            _repoUser = repoUser;
            _repoMascota = repoMascota;
            _unitOfWork = unitOfWork;
            _repoContrato = repoContrato;

            Account acc = new Account(
                _cloudinaryConfig.Value.CloudName,
                _cloudinaryConfig.Value.ApiKey,
                _cloudinaryConfig.Value.ApiSecret
            );

            _cloudinary = new Cloudinary(acc);
        }
        [AllowAnonymous]
        [HttpGet("{mascotaId}/GetAllFotosMascota")]
        public IEnumerable<FotoForReturnDto> GetAllFotosMascota(int mascotaId)
        {
            var lista = _repoMascota.GetAllFotosMascota(mascotaId);
            if (lista.Count() > 0)
            {
                var listaFotos = _mapper.Map<IEnumerable<FotoForReturnDto>>(lista);
                return listaFotos;
            }
            return null;
        }
        [AllowAnonymous]
        [HttpGet("{id}", Name = "GetFoto")]
        public async Task<IActionResult> GetFoto(int id)
        {
            var photoFromRepo = await _repoUser.GetFoto(id);

            var photo = _mapper.Map<FotoForReturnDto>(photoFromRepo);

            return Ok(photo);
        }
        //[HttpPost("AgregarFotoUsuario")]
        //public async Task<IActionResult> AgregarFotoUsuario(int idUsuario, [FromForm] FotoForCreationDto fotoUser) {
        //    if (idUsuario != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
        //        return Unauthorized("No tiene acceso a este recurso.");
        //    var userRepo = await _repoUser.GetById(idUsuario);
        //    var imagen = fotoUser.File;
        //    var resultUpload = new ImageUploadResult();

        //    if (imagen.Length > 0) {
        //        using (var stream=imagen.OpenReadStream())
        //        {
        //            var parametros = new ImageUploadParams()
        //            {
        //                File=new FileDescription(imagen.Name, stream),
        //                Transformation=new Transformation().Width(500).Height(500).Crop("fill").Gravity("face")
        //            };
        //            resultUpload = _cloudinary.Upload(parametros);
        //        }
        //    }
        //    //Uri en desuso, reemplazado por Url
        //    fotoUser.Url = resultUpload.Url.ToString();
        //    fotoUser.IdPublico = resultUpload.PublicId;

        //    var fotoMap = _mapper.Map<Foto>(fotoUser);
        //    if (userRepo.Fotos.Any(x => x.IsPrincipal))
        //        fotoMap.IsPrincipal = true;

        //    userRepo.Fotos.Add(fotoMap);
        //    if (await _repoUser.SaveAll())
        //        return Ok("Guardado exitosamente.");

        //    return BadRequest("Hubo problemas al guardar la foto.");
        //}
        [Authorize(Roles = "Admin")]
        [HttpPost("Mascota/{mascotaId}/AgregarFotoMascota")]
        [Consumes("multipart/form-data")]
        //[EnableCors("AllowOrigin")]

        //manejar con FromBody o con FromForm
        public async Task<IActionResult> AgregarFotoMascota(int mascotaId, [FromForm] FotoForCreationDto fotoMascota)
        {
            //if (mascotaId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            //    return Unauthorized("No tiene acceso a este recurso.");
            var mascotaRepo = await _repoMascota.GetById(mascotaId);
            Foto fotoMap = new Foto();
            List<FotoForReturnDto> listaFotos = new List<FotoForReturnDto>();
            foreach (var item in fotoMascota.Archivo)
            {
                var imagen = item;
                var resultUpload = new ImageUploadResult();

                if (imagen.Length > 0)
                {
                    using (var stream = imagen.OpenReadStream())
                    {
                        var parametros = new ImageUploadParams()
                        {
                            File = new FileDescription(imagen.Name, stream),
                            Transformation = new Transformation().Width(500).Height(500).Crop("fill").Gravity("face")
                        };
                        resultUpload = _cloudinary.Upload(parametros);
                    }
                }
                //Uri en desuso, reemplazado por Url
                fotoMascota.Url = resultUpload.SecureUrl.ToString();
                fotoMascota.IdPublico = resultUpload.PublicId;
                fotoMascota.MascotaId = mascotaId;

                fotoMap = _mapper.Map<Foto>(fotoMascota);
                mascotaRepo.Fotos.Add(fotoMap);
                if (!mascotaRepo.Fotos.Any(x => x.IsPrincipal))
                    fotoMap.IsPrincipal = true;


                listaFotos.Add(_mapper.Map<FotoForReturnDto>(fotoMap));
            }
            if (await _repoMascota.SaveAll())
            {
                //var photoToReturn = _mapper.Map<FotoForReturnDto>(fotoMap);
                //return CreatedAtRoute("GetFoto", new { mascotaId = mascotaId, id = fotoMap.Id }, photoToReturn);
                //return CreatedAtRoute("{mascotaId}/GetAllFotosMascota", new { mascotaId = mascotaId}, listaFotos);
                return Ok();
            }

            return BadRequest("Hubo problemas al guardar la foto.");
        }
        [AllowAnonymous]
        [HttpDelete("Mascota/{mascotaId}/EliminarFotoMascota/{idfoto}")]
        public async Task<IActionResult> EliminarFotoMascota(int mascotaId, int idfoto)
        {
            var mascota = await _repoMascota.GetById(mascotaId);

            //if (!mascota.Fotos.Any(p => p.MascotaId == mascotaId))
            //    return Unauthorized();

            var objetoFoto = await _repoMascota.GetFoto(idfoto);

            if (objetoFoto.IsPrincipal)
                return BadRequest("La foto establecida como Principal no puede eliminarse.");
            if (objetoFoto.IdPublico != null)
            {
                var deleteParams = new DeletionParams(objetoFoto.IdPublico);
                var result = _cloudinary.Destroy(deleteParams);

                if (result.Result == "ok")
                {
                    mascota.Fotos.Remove(objetoFoto);
                    _repoUser.DeleteFoto(idfoto);
                }
            }

            //if (objetoFoto.IdPublico == null)
            //    mascota.Fotos.Remove(objetoFoto);

            if (await _repoMascota.SaveAll())
                return Ok();
            //return CreatedAtRoute("Mascota/"+ mascotaId + "/GetAllFotosMascota", new { mascotaId = mascotaId });

            return BadRequest("La Foto no pudo ser eliminada.");
        }
        //[Authorize(Roles = "Admin, Voluntario, Adoptante")]
        //[HttpPost("Contrato/{contratoId}/AgregarFotoContratoAdopcion")]
        //[Consumes("multipart/form-data")]
        //public async Task<IActionResult> AgregarFotoContratoAdopcion(int contratoId, [FromForm] FotoForCreationDto fotoContrato)
        //{
        //    var contradoRepo = await _repoContrato.GetById(contratoId);
        //    contradoRepo.EstadoAdopcionId = 1;
        //    foreach (var item in fotoContrato.Archivo)
        //    {
        //        var imagen = item;
        //        var resultUpload = new ImageUploadResult();

        //        if (imagen.Length > 0)
        //        {
        //            using (var stream = imagen.OpenReadStream())
        //            {
        //                var parametros = new ImageUploadParams()
        //                {
        //                    File = new FileDescription(imagen.Name, stream),
        //                    Transformation = new Transformation().Width(500).Height(500).Crop("fill").Gravity("face")
        //                };
        //                resultUpload = _cloudinary.Upload(parametros);
        //            }
        //        }
        //        fotoContrato.Url = resultUpload.SecureUrl.ToString();
        //        fotoContrato.IdPublico = resultUpload.PublicId;
        //        fotoContrato.ContratoAdopcionId = contratoId;

        //        Foto fotoMap = _mapper.Map<Foto>(fotoContrato);
        //        contradoRepo.Fotos.Add(fotoMap);
        //    }
        //    if (await _unitOfWork.SaveAll())
        //    {
        //        return Json("La(s) imagen(es) se enviaron correctamente.");
        //    }
        //    return BadRequest("Ha ocurrido un error al subir la(s) imagen(es).");
        //}
    }
}