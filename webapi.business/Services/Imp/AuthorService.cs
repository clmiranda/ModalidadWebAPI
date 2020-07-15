using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using webapi.business.Services.Interf;
using webapi.core.Models;
using webapi.data.Repositories.Interf;

namespace webapi.business.Services.Imp
{
    public class AuthorService : IAuthorService
    {
        private IUnitOfWork _unitOfWork;

        public AuthorService(IUnitOfWork unitOfWork)
        { _unitOfWork = unitOfWork; }

        //public Task<IEnumerable<Author>> GetAllAuthors() => _unitOfWork.AuthorRepository.GetAll();
        public Task<Author> GetAuthorByName(string firstName) => _unitOfWork.AuthorRepository.GetByName(firstName);
        public void CreateAuthor(Author author) => _unitOfWork.AuthorRepository.Insert(author);
    }
}
