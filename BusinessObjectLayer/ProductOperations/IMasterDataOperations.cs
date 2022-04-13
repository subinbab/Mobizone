using DomainLayer.ProductModel.Master;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessObjectLayer.ProductOperations
{
    public interface IMasterDataOperations
    {
        Task Add(MasterTable data);
        Task<IEnumerable<MasterTable>> GetAll();
    }
}