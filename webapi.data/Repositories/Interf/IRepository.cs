using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using webapi.core.Models;

namespace webapi.data.Repositories.Interf
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> GetById(int id);
        void Insert(T entity);
        void Update(T entity);
        void Delete(T entity);
        T GetFirst();
        T GetLast();
        Task<IEnumerable<T>> FindByCondition(Expression<Func<T, bool>> expression);
    }
}
