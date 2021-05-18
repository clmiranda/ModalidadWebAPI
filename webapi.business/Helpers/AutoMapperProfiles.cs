using AutoMapper;
using System;
using System.Linq;
using webapi.business.Dtos.Adopciones;
using webapi.business.Dtos.ContratoRechazo;
using webapi.business.Dtos.Denuncias;
using webapi.business.Dtos.Fotos;
using webapi.business.Dtos.Mascotas;
using webapi.business.Dtos.ReportesSeguimientos;
using webapi.business.Dtos.ReporteTratamientos;
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
            CreateMap<User, UserForDetailedDto>()
                .ForMember(d => d.Edad, options =>
              {
                  options.MapFrom(s => s.FechaNacimiento.CalculoEdad());
              });
            CreateMap<UserUpdateDto, User>();
            CreateMap<Denuncia, DenunciaForListDto>();
            CreateMap<Denuncia, DenunciaForDetailedDto>();
            CreateMap<DenunciaForDetailedDto, Denuncia>();
            CreateMap<Mascota, MascotaForDetailedDto>();
            CreateMap<Mascota, MascotaForReturn>();
            CreateMap<MascotaForCreateDto, Mascota>();
            CreateMap<MascotaForUpdateDto, Mascota>();
            CreateMap<Mascota, MascotaForAdopcionDto>()
                .ForMember(d => d.Foto, options =>
                {
                    options.MapFrom(s => s.Fotos.FirstOrDefault(x => x.IsPrincipal == true));
                });
            CreateMap<ContratoAdopcionReturnDto, ContratoAdopcion>();
            CreateMap<ContratoAdopcion, ContratoAdopcionReturnDto>();
            CreateMap<ContratoAdopcionForCreate, ContratoAdopcion>();
            CreateMap<FechaContratoForUpdateDto, ContratoAdopcion>();
            CreateMap<Seguimiento, SeguimientoForReturnDto>();
            CreateMap<ReporteSeguimiento, ReporteSeguimientoForList>();
            CreateMap<ContratoAdopcion, ContratoAdopcionForList>()
                .ForMember(d => d.RazonAdopcion, options =>
                {
                    options.MapFrom(s => s.Respuesta1);
                });
            CreateMap<ContratoAdopcion, ContratoAdopcionForDetailDto>();
            CreateMap<ReporteSeguimiento, ReporteSeguimientoForReturn>();
            CreateMap<ReporteSeguimientoForCreate, ReporteSeguimiento>();
            CreateMap<ReporteSeguimientoForUpdate, ReporteSeguimiento>();

            CreateMap<ReporteTratamiento, ReporteTratamientoForReturnDto>();
            CreateMap<ReporteTratamientoForCreateDto, ReporteTratamiento>();
            CreateMap<ReporteTratamientoForUpdateDto, ReporteTratamiento>();
            CreateMap<ContratoRechazo, ContratoRechazoForReturnDto>();
            CreateMap<ContratoRechazoForCreateDto, ContratoRechazo>();
            CreateMap<User, UserRolesForReturn>()
                .ForMember(dest => dest.Edad, opt =>
                {
                    opt.MapFrom(d => d.FechaNacimiento.CalculoEdad());
                })
                .ForMember(dest => dest.Roles, opt =>
                {
                    opt.MapFrom(d => d.UserRoles.Select((role) => new
                    { name = role.Role.Name }).Select(x => x.name));
                });
            CreateMap<ReporteSeguimientoForCreateAdmin, ReporteSeguimiento>();
            CreateMap<FechaReporteDto, Seguimiento>()
    .ForMember(dest => dest.FechaInicio, opt =>
    {
        opt.MapFrom(src => src.RangoFechas[0] == "null" ? (DateTime?)null : Convert.ToDateTime(src.RangoFechas[0]));
    })
    .ForMember(dest => dest.FechaConclusion, opt =>
    {
        opt.MapFrom(src => src.RangoFechas[1] == "null" ? (DateTime?)null : Convert.ToDateTime(src.RangoFechas[1]));
    });
        }
    }
}