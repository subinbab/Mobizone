using DomainLayer;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessObjectLayer
{
    public interface ICartDetailsOperation
    {
        Task Add(CartDetails entity);
        Task Delete(CartDetails entity);
        Task Edit(CartDetails entity);
        Task<IEnumerable<CartDetails>> Get();
        Task<CartDetails> GetById(int id);
    }
}