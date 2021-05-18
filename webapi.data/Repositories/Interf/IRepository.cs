using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using webapi.core.Models;

namespace webapi.data.Repositories.Interf
{
    public interface IRepository<T> where T : BaseEntity
    {
        IQueryable<T> GetAll();
        Task<T> GetById(int id);
        void Insert(T entity);
        void Update(T entity);
        void Delete(T entity);
        IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression);
        IQueryable<T> GetAllNoTraking();
        Task<T> GetByIdNoTraking(int id);
    }
}
