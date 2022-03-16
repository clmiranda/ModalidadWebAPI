using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;
using webapi.business.Dtos.Fotos;
using webapi.business.Helpers;
using webapi.business.Services.Interf;
using webapi.core.Models;
using webapi.data.Repositories.Interf;

namespace webapi.business.Services.Imp
{
    public class FotoService : IFotoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOptions<ConfigurationCloudinary> _cloudinaryConfig;
        private readonly IMapper _mapper;
        private readonly Cloudinary _cloudinary;

        public FotoService(IUnitOfWork unitOfWork,
             IOptions<ConfigurationCloudinary> cloudinaryConfig,
             IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _cloudinaryConfig = cloudinaryConfig;
            _mapper = mapper;
            Account acc = new Account(
                _cloudinaryConfig.Value.CloudName,
                _cloudinaryConfig.Value.ApiKey,
                _cloudinaryConfig.Value.ApiSecret
            );
            _cloudinary = new Cloudinary(acc);
        }
        public async Task<Mascota> GetMascota(int id)
        {
            return await _unitOfWork.MascotaRepository.GetById(id);
        }
        public bool VerifyFotoIsPrincipal(Foto foto)
        {
            if (foto.IsPrincipal)
                return false;
            return true;
        }
        public async Task<bool> SetFotoPrincipalMascota(int id, int idfoto)
        {
            var mascota = await _unitOfWork.MascotaRepository.GetById(id);
            if (!mascota.Fotos.Any(x => x.Id == idfoto))
                return false;

            var foto = await _unitOfWork.FotoRepository.GetById(idfoto);

            if (VerifyFotoIsPrincipal(foto))
            {
                var principal = await _unitOfWork.FotoRepository.FindByCondition(x => x.Mascota.Id == id && x.IsPrincipal).FirstOrDefaultAsync();
                principal.IsPrincipal = false;

                foto.IsPrincipal = true;
                return await _unitOfWork.SaveAll();
            }
            return false;
        }

        public async Task<string> AddFotoMascota(int idMascota, FotoForCreationDto fotoMascota)
        {
            var mascotaRepo = await _unitOfWork.MascotaRepository.GetById(idMascota);
            if (mascotaRepo.Fotos.Count() + fotoMascota.Archivo.Count() > 4)
                return "ErrorCount";
            else
            {
                //mascotaRepo.Fotos = new List<Foto>();
                Foto fotoMap = new Foto();
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
                                //Transformation = new Transformation().Width(1000).Height(1000).Crop("fill").Gravity("face")
                            };
                            resultUpload = _cloudinary.Upload(parametros);
                        }
                    }
                    fotoMascota.Url = resultUpload.SecureUrl.ToString();
                    fotoMascota.IdPublico = resultUpload.PublicId;
                    fotoMascota.MascotaId = idMascota;

                    fotoMap = _mapper.Map<Foto>(fotoMascota);
                    mascotaRepo.Fotos.Add(fotoMap);
                    if (!mascotaRepo.Fotos.Any(x => x.IsPrincipal))
                        fotoMap.IsPrincipal = true;
                }
                if (await _unitOfWork.SaveAll())
                    return "Ok";
                return "ErrorSave";
            }
        }
        public async Task<bool> DeleteFotoMascota(int id, int idfoto)
        {
            var foto = await _unitOfWork.FotoRepository.GetById(idfoto);

            if (foto == null) return false;

            if (VerifyFotoIsPrincipal(foto))
            {
                if (foto.IdPublico != null)
                {
                    var parametros = new DeletionParams(foto.IdPublico);
                    var resultado = _cloudinary.Destroy(parametros);

                    if (resultado.Result.Equals("ok"))
                        _unitOfWork.FotoRepository.Delete(foto);
                }
                else
                    return false;

                return await _unitOfWork.SaveAll();
            }
            return false;
        }
        public bool DeleteAllFotoMascota(Mascota mascota)
        {
            foreach (var foto in mascota.Fotos)
            {
                if (foto.IdPublico != null)
                {
                    var parametros = new DeletionParams(foto.IdPublico);
                    var resultado = _cloudinary.Destroy(parametros);

                    if (resultado.Result.Equals("ok"))
                        _unitOfWork.FotoRepository.Delete(foto);
                }
                else
                    return false;
            }
            return true;
        }
        public async Task<bool> AgregarFotoReporte(int id, IFormFile archivo)
        {
            var reporteRepo = await _unitOfWork.ReporteSeguimientoRepository.GetById(id);
            Foto foto = new Foto();
            var resultUpload = new ImageUploadResult();

            if (archivo.Length > 0)
            {
                using (var stream = archivo.OpenReadStream())
                {
                    var parametros = new ImageUploadParams()
                    {
                        File = new FileDescription(archivo.Name, stream),
                        //Transformation = new Transformation().Width(500).Height(500).Crop("fill").Gravity("face")
                    };
                    resultUpload = _cloudinary.Upload(parametros);
                }
            }
            foto.Url = resultUpload.SecureUrl.ToString();
            foto.IdPublico = resultUpload.PublicId;
            foto.ReporteSeguimientoId = id;
            foto.FechaCreacion = DateTime.Now;
            foto.IsPrincipal = true;

            reporteRepo.Foto = new Foto();
            reporteRepo.Foto = foto;
            return await _unitOfWork.SaveAll();
        }
        public async Task<bool> DeleteFotoReporteSeguimiento(int idFoto)
        {
            var foto = await _unitOfWork.FotoRepository.GetById(idFoto);
            if (foto.IdPublico != null)
            {
                var eliminar = new DeletionParams(foto.IdPublico);
                var x = _cloudinary.Destroy(eliminar);

                if (x.Result.Equals("ok"))
                    _unitOfWork.FotoRepository.Delete(foto);
            }
            else
                return false;
            return await _unitOfWork.SaveAll();
        }
    }
}
