using DomainLayer.ProductModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessObjectLayer.ProductOperations
{
    public interface IRamOperations
    {
        Task Add(Ram entity);
        Task DeleteProduct(Ram entity);
        Task Edit(Ram entity);
        Task<IEnumerable<Ram>> Get();
        Task<Ram> Get(int id);
    }
}