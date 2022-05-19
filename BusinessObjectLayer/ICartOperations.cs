using DomainLayer;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessObjectLayer
{
    public interface ICartOperations
    {
        Task Add(Cart entity);
        Task Delete(Cart entity);
        Task Edit(Cart entity);
        Task<Cart> GetById(int id);
        Task<IEnumerable<Cart>> Get();
    }
}