using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webapi.business.Services.Interf;
using webapi.data.Repositories.Interf;

namespace webapi.business.Services.Imp
{
    public class RolUserService : IRolUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        public RolUserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<string>> AsignarRoles(int id, string[] rolesUser)
        {
            var user = await _unitOfWork.UserRepository.FindById(id.ToString());

            var userRoles = await _unitOfWork.RolUserRepository.GetRolesUser(user);

            var selectedRoles = rolesUser;

            selectedRoles = selectedRoles ?? new string[] { };

            var result = await _unitOfWork.RolUserRepository.AsignarRolesUser(user, selectedRoles.Except(userRoles));
            if (!result.Succeeded)
                return null;
            result = await _unitOfWork.RolUserRepository.QuitarRolesUser(user, userRoles.Except(selectedRoles));
            if (!result.Succeeded)
                return null;
            return await _unitOfWork.RolUserRepository.GetRolesUser(user);
        }
    }
}
