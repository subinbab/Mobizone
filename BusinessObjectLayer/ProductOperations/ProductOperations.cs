using DomainLayer;
using DomainLayer.ProductModel;
using Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessObjectLayer.ProductOperations
{
    public class ProductOperations : IProductOperations
    {
        IRepositoryOperations<ProductEntity> _repo;
        ProductEntity _product;
        IEnumerable<ProductEntity> _products;
        public ProductOperations(IRepositoryOperations<ProductEntity> repo)
        {
            _repo = repo;
        }
        public void Add(ProductEntity product)
        {
            _repo.Add(product);
            _repo.Save();
        }
        public IEnumerable<ProductEntity> GetAll()
        {
            return _repo.Get(n1=> n1.specs,n2=> n2.images);
        }
    }
}
