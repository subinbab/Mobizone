using DomainLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Repository
{
    public interface IRepositoryOperations<T>
    {
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        IEnumerable<T> Get();
        IQueryable<T> Get(params Expression<Func<T, object>>[] includes);
        T GetById(int Id);


        void Save();
    }
}
