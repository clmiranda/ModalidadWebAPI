using System.Threading.Tasks;
using webapi.business.Dtos.Persona;
using webapi.core.Models;

namespace webapi.business.Services.Interf
{
    public interface IPersonaService
    {
        Task<Persona> CreatePersona(PersonaForCreateUpdateDto personaDto);
        Task<Persona> UpdatePersona(PersonaForCreateUpdateDto personaDto);
    }
}
