using DomainLayer;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessObjectLayer
{
    public interface ICartOperations
    {
        Task Add(ProductCart entity);
        Task Delete(ProductCart entity);
        Task Edit(ProductCart entity);
        Task<ProductCart> GetById(int id);
        Task<IEnumerable<ProductCart>> Get();
    }
}