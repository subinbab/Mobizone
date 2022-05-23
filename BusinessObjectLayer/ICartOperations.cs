using DomainLayer;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessObjectLayer
{
    public interface ICartOperations
    {
        Task Add(DbCart entity);
        Task Delete(DbCart entity);
        Task Edit(DbCart entity);
        Task<DbCart> GetById(int id);
        Task<IEnumerable<DbCart>> Get();
    }
}