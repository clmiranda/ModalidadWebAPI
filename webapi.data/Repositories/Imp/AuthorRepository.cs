using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using webapi.core.Models;
using webapi.data.Repositories.Interf;

namespace webapi.data.Repositories.Imp
{
    public class AuthorRepository: Repository<Author>, IAuthorRepository
    {
        public AuthorRepository(BDSpatContext context) : base(context) { }

        public Task<Author> GetByName(string name)
        {
            return context.Set<Author>().FirstOrDefaultAsync(author => author.Name == name);
            // return FirstOrDefault(author => author.Name == name);
        }
    }
}
