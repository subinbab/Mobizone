using DomainLayer;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessObjectLayer
{
    public interface ICartOperations
    {
        Task Add(MyCart entity);
        Task Delete(MyCart entity);
        Task Edit(MyCart entity);
        Task<MyCart> GetById(int id);
        Task<IEnumerable<MyCart>> Get();
    }
}