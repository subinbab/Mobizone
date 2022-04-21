using DomainLayer.ProductModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessObjectLayer.ProductOperations
{
    public interface IProductImageOperations
    {
        void delete(Images data);
        Task<IEnumerable<Images>> get();
    }
}