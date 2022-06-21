using BusinessObjectLayer.ProductOperations;
using DomainLayer.ProductModel;
using DTOLayer.Product;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UILayer.Data.ApiServices
{
    public interface IProductOpApi
    {
        bool AddProductSubPart(ProductSubPart productSubPart);
        Task<bool> CreateProduct(ProductViewModel product);
        bool DeleteProduct(int id);
        bool DeleteRam(int id);
        bool DeleteStorage(int id);
        Task<bool> EditProduct(ProductViewModel product);
        Task<IEnumerable<ProductEntity>> Filter(string name);
        Task<IEnumerable<ProductEntity>> GetAll();
        Task<IEnumerable<ProductListViewModel>> GetProduct();
        Task<ProductEntity> GetProduct(int id);
        IEnumerable<Ram> GetRams();
        IEnumerable<Storage> GetStorages();
        Task<IEnumerable<ProductEntity>> Search(string name);
        Task<IEnumerable<ProductEntity>> Sort();
        Task<IEnumerable<ProductEntity>> Sortby();
    }
}