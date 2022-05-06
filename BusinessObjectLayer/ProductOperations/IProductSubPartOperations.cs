using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessObjectLayer.ProductOperations
{
    public interface IProductSubPartOperations
    {
        Task AddProduct(ProductSubPart entity);
        Task DeleteProduct(ProductSubPart entity);
        Task EditProduct(ProductSubPart entity);
        Task<ProductSubPart> GetById(int id);
        Task<IEnumerable<ProductSubPart>> GetProduct();
    }
}