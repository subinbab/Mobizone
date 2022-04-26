using DomainLayer;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessObjectLayer
{
    public interface IProductCatalog
    {
        Task AddProduct(Product entity);
        Task EditProduct(Product entity);
        Task<IEnumerable<Product>> GetProduct();
        Task<Product> GetById(int id);
        Task DeleteProduct(Product entity);
    }
}
