using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public async Task<Foto> GetFoto(int id)
        {
            return await _unitOfWork.FotoRepository.GetById(id);
        }
        public async Task<Mascota> GetMascota(int id)
        {
            return await _unitOfWork.MascotaRepository.GetById(id);
        }
        public async Task<bool> SetFotoPrincipalMascota(int id, int idfoto)
        {
            var mascota = await _unitOfWork.MascotaRepository.GetById(id);
            if (!mascota.Fotos.Any(x => x.Id == idfoto))
                return false;

            var foto = await _unitOfWork.FotoRepository.GetById(idfoto);
            if (foto.IsPrincipal)
                return false;

            var principal = await _unitOfWork.FotoRepository.FindByCondition(x => x.Mascota.Id == id && x.IsPrincipal).FirstOrDefaultAsync();
            principal.IsPrincipal = false;

            foto.IsPrincipal = true;
            return await _unitOfWork.SaveAll();
        }

        public async Task<bool> AgregarFotoMascota(int id, FotoForCreationDto fotoMascota)
        {
            var mascotaRepo = await _unitOfWork.MascotaRepository.GetById(id);
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
                            Transformation = new Transformation().Width(500).Height(500).Crop("fill").Gravity("face")
                        };
                        resultUpload = _cloudinary.Upload(parametros);
                    }
                }
                fotoMascota.Url = resultUpload.SecureUrl.ToString();
                fotoMascota.IdPublico = resultUpload.PublicId;
                fotoMascota.MascotaId = id;

                fotoMap = _mapper.Map<Foto>(fotoMascota);
                mascotaRepo.Fotos = new List<Foto>();
                mascotaRepo.Fotos.Add(fotoMap);
                if (!mascotaRepo.Fotos.Any(x => x.IsPrincipal))
                    fotoMap.IsPrincipal = true;
            }
            return await _unitOfWork.SaveAll();
        }

        public async Task<bool> EliminarFoto(int id, int idfoto)
        {
            var foto = await _unitOfWork.FotoRepository.GetById(idfoto);

            if (foto == null)
                return false;
            if (foto.IsPrincipal)
                return false;

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
                        Transformation = new Transformation().Width(500).Height(500).Crop("fill").Gravity("face")
                    };
                    resultUpload = _cloudinary.Upload(parametros);
                }
            }
            foto.Url = resultUpload.SecureUrl.ToString();
            foto.IdPublico = resultUpload.PublicId;
            foto.ReporteSeguimientoId = id;
            foto.FechaAgregado = DateTime.Now;
            foto.IsPrincipal = true;

            reporteRepo.Fotos = new List<Foto>();
            reporteRepo.Fotos.Add(foto);
            return await _unitOfWork.SaveAll();
        }
    }
}
