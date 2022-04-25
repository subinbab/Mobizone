using DomainLayer;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessObjectLayer
{
    public interface ICheckOutOperation
    {
        void delete(Checkout data);
        Task<IEnumerable<Checkout>> get();
        Task Add(Checkout data);
        Task EditProduct(Checkout entity);
    }
}