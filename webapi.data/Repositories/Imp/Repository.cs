using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using webapi.core.Models;
using webapi.data.Repositories.Interf;

namespace webapi.data.Repositories.Imp
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        protected readonly BDSpatContext context;
        private DbSet<T> entities;
        string errorMessage = string.Empty;
        public Repository(BDSpatContext context)
        {
            this.context = context;
            entities = context.Set<T>();
        }
        public async Task<IEnumerable<T>> GetAll()
        {
            return await entities.ToListAsync();
        }
        public async Task<T> GetById(int id)
        {
            return await entities.SingleOrDefaultAsync(s => s.Id == id);
        }
        public void Insert(T entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");
            entities.Add(entity);
            //context.SaveChanges();
        }
        public void Update(T entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");
            entities.Update(entity);
            //context.SaveChanges();
        }
        public void Delete(T entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");
            entities.Remove(entity);
            //context.SaveChanges();
        }
        public T GetFirst()
        {
            var obj= entities.ToList().FirstOrDefault();
            return obj;
        }
        public T GetLast()
        {
            var obj =  entities.ToList().LastOrDefault();
            return obj;
        }
        //public async  Task<IEnumerable<T>> FindByCondition(Expression<Func<T, bool>> expression)
        //{
        //    return await entities.Where(expression).ToListAsync();
        //}
        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
            //return entities.Where(expression).AsNoTracking();
            return entities.Where(expression).AsQueryable();
        }
    }
}
