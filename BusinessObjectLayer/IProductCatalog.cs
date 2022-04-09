using DomainLayer;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessObjectLayer
{
    public interface IProductCatalog
    {
        void AddProduct(Product entity);
        void EditProduct(Product entity);
        IEnumerable<Product> GetProduct();
        Product GetById(int id);
        void DeleteProduct(Product entity);
    }
}
