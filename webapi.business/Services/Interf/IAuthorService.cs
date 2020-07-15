using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using webapi.core.Models;

namespace webapi.business.Services.Interf
{
    public interface IAuthorService
    {
        //Task<IEnumerable<Author>> GetAllAuthors();
        Task<Author> GetAuthorByName(string firstName);
        void CreateAuthor(Author author);
    }
}
