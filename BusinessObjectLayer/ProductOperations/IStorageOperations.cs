using DomainLayer.ProductModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessObjectLayer.ProductOperations
{
    public interface IStorageOperations
    {
        Task Add(Storage entity);
        Task DeleteProduct(Storage entity);
        Task Edit(Storage entity);
        Task<IEnumerable<Storage>> Get();
        Task<Storage> Get(int id);
    }
}