using DomainLayer;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessObjectLayer
{
    public interface ICartOperations
    {
        Task AddProduct(Cart entity);
        Task DeleteProduct(Cart entity);
        Task EditProduct(Cart entity);
        Task<Cart> GetById(int id);
        Task<IEnumerable<Cart>> GetProduct();
    }
}