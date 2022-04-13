using DomainLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface IRepositoryOperations<T>
    {
        Task Add(T entity);
        Task Update(T entity);
        Task Delete(T entity);
        Task<IEnumerable<T>> Get();
        Task<IQueryable<T>> Get(params Expression<Func<T, object>>[] includes);
        Task<T> GetById(int Id);
        Task<T> GetById(int id, params Expression<Func<T, object>>[] includeProperties);


        Task Save();
    }
}
