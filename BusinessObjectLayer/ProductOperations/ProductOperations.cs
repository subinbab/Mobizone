using DomainLayer;
using DomainLayer.ProductModel;
using Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

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
        public async Task Add(ProductEntity product)
        {
            _repo.Add(product);
            _repo.Save();
        }

        public async Task DeleteProduct(ProductEntity entity)
        {
            _repo.Delete(entity);
            _repo.Save();
        }

        public async Task EditProduct(ProductEntity entity)
        {
            _repo.Update(entity);
            _repo.Save();
        }

        public async Task<IEnumerable<ProductEntity>> GetAll()
        {
            return await _repo.Get(n1=> n1.specs,n2=> n2.images);
        }

        public Task<ProductEntity> GetById(int id)
        {
            return _repo.GetById(id, n1 => n1.specs, n2 => n2.images);
        }
    }
}
