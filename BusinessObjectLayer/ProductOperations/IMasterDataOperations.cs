using DomainLayer.ProductModel.Master;
using System.Collections.Generic;

namespace BusinessObjectLayer.ProductOperations
{
    public interface IMasterDataOperations
    {
        void Add(MasterTable data);
        IEnumerable<MasterTable> GetAll();
    }
}