using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using webapi.business.Dtos.Persona;
using webapi.business.Services.Interf;
using webapi.core.Models;
using webapi.data.Repositories.Interf;

namespace webapi.business.Services.Imp
{
    public class PersonaService: IPersonaService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public PersonaService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<Persona> CreatePersona(PersonaForCreateUpdateDto personaDto)
        {
            var persona = _mapper.Map<Persona>(personaDto);
            _unitOfWork.PersonaRepository.Insert(persona);
            if (await _unitOfWork.SaveAll())
                return persona;
            return null;
        }
        public async Task<Persona> UpdatePersona(PersonaForCreateUpdateDto personaDto)
        {
            var persona = await _unitOfWork.PersonaRepository.GetById(personaDto.Id);
            var mapped = _mapper.Map(personaDto, persona);
            _unitOfWork.PersonaRepository.Update(mapped);
            if (await _unitOfWork.SaveAll())
                return mapped;
            return null;
        }
    }
}