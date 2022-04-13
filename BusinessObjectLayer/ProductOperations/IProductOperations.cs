using DomainLayer.ProductModel;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessObjectLayer.ProductOperations
{
    public interface IProductOperations
    {
        Task Add(ProductEntity product);
        Task<IEnumerable<ProductEntity>> GetAll();
        Task<ProductEntity> GetById(int id);
        Task DeleteProduct(ProductEntity entity);
        Task EditProduct(ProductEntity entity);
    }
}