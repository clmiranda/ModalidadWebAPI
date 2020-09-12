using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using webapi.business.Dtos;
using webapi.business.Dtos.Adopciones;
using webapi.business.Dtos.CasosMascota;
using webapi.business.Dtos.Denuncias;
using webapi.business.Dtos.Fotos;
using webapi.business.Dtos.Mascotas;
using webapi.business.Dtos.ReportesSeguimientos;
using webapi.business.Dtos.Seguimientos;
using webapi.business.Dtos.Usuario;
using webapi.core.Models;

namespace webapi.business.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<FotoForCreationDto, Foto>();
            CreateMap<Foto, FotoForReturnDto>();
            CreateMap<UserForRegisterDto, User>();
            CreateMap<User, UserTokenToReturnDto>();
            CreateMap<User, UserForListDto>();
            CreateMap<User, UserForDetailedDto>();

            CreateMap<CasoMascota, CasoMascotaForListDto>()
                .ForMember(d => d.TituloDenuncia, options =>
                  {
                      options.MapFrom(s => s.Denuncia.Titulo);
                  });
            //CreateMap<Denuncia, DenunciaFilterDto>()
            //    .ForMember(d=>d.Id, options=>
            //    {
            //        options.MapFrom(s=>s.CasoMascotas.FirstOrDefault(x=>x.IdDenuncia==s.Id).IdDenuncia);
            //    })
            //    .ForMember(d => d.Titulo, options =>
            //    {
            //        options.MapFrom(s => s.Titulo);
            //    })
            //    .ForMember(d => d.Descripcion, options =>
            //    {
            //        options.MapFrom(s => s.Descripcion);
            //    });
            CreateMap<Denuncia, DenunciaForListDto>()
                .ForMember(d => d.CasoMascota, options =>
                  {
                      options.MapFrom(s => s.CasoMascotas.FirstOrDefault());
                  });
            CreateMap<Denuncia, DenunciaForDetailedDto>()
                .ForMember(d=>d.CasoMascotas, options=>
                {
                    options.MapFrom(s=>s.CasoMascotas.FirstOrDefault());
                });
            CreateMap<DenunciaForDetailedDto, Denuncia>();
            CreateMap<CasoMascota, CasoMascotaForDetailedDto>();
            CreateMap<Mascota, MascotaForDetailedDto>()
                .ForMember(d=>d.CasoMascotaId, options=>
                {
                    options.MapFrom(s=>s.CasoMascotaId);
                })
                .ForMember(d => d.TituloCaso, options =>
                {
                    options.MapFrom(s => s.CasoMascota.Titulo);
                })
                .ForMember(d => d.TituloDenuncia, options =>
                {
                    options.MapFrom(s => s.CasoMascota.Denuncia.Titulo);
                });
            CreateMap<Mascota, MascotaForReturn>();
            CreateMap<Mascota, MascotaForAdopcionDto>()
                .ForMember(d => d.Foto, options =>
                {
                    options.MapFrom(s => s.Fotos.FirstOrDefault(x=>x.IsPrincipal==true));
                });
            CreateMap<ContratoAdopcionReturnDto, ContratoAdopcion>();
            CreateMap<ContratoAdopcion, ContratoAdopcionReturnDto>();
            CreateMap<Seguimiento, SeguimientoForReturnDto>();
                //.ForMember(d=>d.CantidadReportes, options=>
                //{
                //    options.MapFrom(s=>s.ReporteSeguimientos.Count());
                //});
            CreateMap<ContratoAdopcion, ContratoAdopcionForList>();
            CreateMap<ReporteSeguimiento, ReporteSeguimientoForReturn>();
            CreateMap<ReporteSeguimientoForCreate, ReporteSeguimiento>();
            CreateMap<ReporteSeguimientoForUpdate, ReporteSeguimiento>();
        }
    }
}