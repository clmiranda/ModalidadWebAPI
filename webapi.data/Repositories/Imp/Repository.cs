using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using webapi.core.Models;
using webapi.data.Repositories.Interf;

namespace webapi.data.Repositories.Imp
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        protected readonly BDSpatContext context;
        private readonly DbSet<T> entities;
        public Repository(BDSpatContext context)
        {
            this.context = context;
            entities = context.Set<T>();
        }
        public IQueryable<T> GetAll()
        {
            return entities.AsQueryable();
        }
        public async Task<T> GetById(int id)
        {
            return await entities.SingleOrDefaultAsync(s => s.Id == id);
        }
        public void Insert(T entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");
            entities.Add(entity);
        }
        public void Update(T entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");
            entities.Update(entity);
        }
        public void Delete(T entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");
            entities.Remove(entity);
        }
        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
            return entities.Where(expression).AsQueryable();
        }
        public IQueryable<T> GetAllNoTraking()
        {
            return entities.AsNoTracking();
        }
        public async Task<T> GetByIdNoTraking(int id)
        {
            return await entities.AsNoTracking().SingleOrDefaultAsync(s => s.Id == id);
        }
    }
}
