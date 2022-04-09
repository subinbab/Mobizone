using DomainLayer.ProductModel;
using System.Collections;
using System.Collections.Generic;

namespace BusinessObjectLayer.ProductOperations
{
    public interface IProductOperations
    {
        void Add(ProductEntity product);
        IEnumerable<ProductEntity> GetAll();
    }
}