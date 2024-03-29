﻿using AutoMapper;
using System;
using System.Linq;
using webapi.business.Dtos.Adopciones;
using webapi.business.Dtos.SolicitudAdopcionRechazada;
using webapi.business.Dtos.Denuncias;
using webapi.business.Dtos.Fotos;
using webapi.business.Dtos.Mascotas;
using webapi.business.Dtos.ReportesSeguimientos;
using webapi.business.Dtos.ReporteTratamientos;
using webapi.business.Dtos.Seguimientos;
using webapi.business.Dtos.Usuario;
using webapi.core.Models;
using webapi.business.Dtos.Persona;
using webapi.business.Dtos.SolicitudAdopcionCancelada;

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
            CreateMap<User, UserForDetailedDto>();
            CreateMap<Persona, PersonaForReturnDto>()
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
            CreateMap<SolicitudAdopcionReturnDto, SolicitudAdopcion>();
            CreateMap<SolicitudAdopcion, SolicitudAdopcionReturnDto>();
            CreateMap<SolicitudAdopcionForCreate, SolicitudAdopcion>();
            CreateMap<SolicitudAdopcion, SolicitudAdopcionForList>();
            CreateMap<ContratoAdopcionDto, ContratoAdopcion>();
            CreateMap<ContratoAdopcion, ContratoAdopcionForReturnDto>();
            CreateMap<FechaSolicitudAdopcionForUpdateDto, SolicitudAdopcion>();
            CreateMap<Seguimiento, SeguimientoForReturnDto>();
            CreateMap<ReporteSeguimiento, ReporteSeguimientoForList>();
            CreateMap<SolicitudAdopcion, SolicitudAdopcionForDetailDto>();
            CreateMap<ReporteSeguimiento, ReporteSeguimientoForReturn>();
            CreateMap<ReporteSeguimientoForUpdate, ReporteSeguimiento>();

            CreateMap<PersonaForCreateUpdateDto, Persona>();

            CreateMap<ReporteTratamiento, ReporteTratamientoForReturnDto>();
            CreateMap<ReporteTratamientoForCreateDto, ReporteTratamiento>();
            CreateMap<ReporteTratamientoForUpdateDto, ReporteTratamiento>();
            CreateMap<AdopcionRechazada, SolicitudAdopcionRechazadaForReturnDto>();
            CreateMap<AdopcionCancelada, SolicitudAdopcionCanceladaForReturnDto>();
            CreateMap<AdopcionPresencialForCreateDto, SolicitudAdopcion>();
            CreateMap<SolicitudAdopcionRechazadaForCreateDto, AdopcionRechazada>();
            CreateMap<SolicitudAdopcionCanceladaForCreateDto, AdopcionCancelada>();
            CreateMap<FechaReporteTratamientoForUpdateDto, ReporteTratamiento>();
            CreateMap<User, UserRolesForReturn>()
                .ForMember(dest => dest.Roles, opt =>
                {
                    opt.MapFrom(d => d.UserRoles.Select((role) => new
                    { name = role.Role.Name }).Select(x => x.name));
                });
            CreateMap<RangoFechaSeguimientoDto, Seguimiento>()
    .ForMember(dest => dest.FechaInicio, opt =>
    {
        opt.MapFrom(src => src.RangoFechas[0] == "null" ? (DateTime?)null : Convert.ToDateTime(src.RangoFechas[0]));
    })
    .ForMember(dest => dest.FechaFin, opt =>
    {
        opt.MapFrom(src => src.RangoFechas[1] == "null" ? (DateTime?)null : Convert.ToDateTime(src.RangoFechas[1]));
    });
        }
    }
}