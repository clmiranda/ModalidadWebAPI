﻿using System;
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
        IQueryable<T> GetAll();
        //Task<IEnumerable<T>> GetAll();
        Task<T> GetById(int id);
        void Insert(T entity);
        void Update(T entity);
        void Delete(T entity);
        T GetFirst();
        T GetLast();
        IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression);
        IQueryable<T> GetAllNoTraking();
        Task<T> GetByIdNoTraking(int id);
    }
}
