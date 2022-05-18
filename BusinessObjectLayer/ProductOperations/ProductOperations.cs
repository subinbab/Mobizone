using DomainLayer;
using DomainLayer.ProductModel;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<IEnumerable<ProductEntity>> Search(string name)
        {
            var data = _repo.Get().Result.Where(c => c.name.ToLower().StartsWith(name.ToLower()));
            var result = data.ToList();
            return data;
        }

        public async Task Add(ProductEntity product)
        {
            _repo.Add(product);
            _repo.Save();
        }

        public async Task DeleteProduct(ProductEntity entity)
        {
            try
            {
                await _repo.Delete(entity);
                await _repo.Save();
            }
            catch (Exception ex)
            {

            }
            
        }

        public async Task EditProduct(ProductEntity entity)
        {
            if(entity.quantity == 0)
            {
                entity.status = ProductStatus.disable;
                await _repo.Update(entity);
                await _repo.Save();
            }
            else
            {
                await _repo.Update(entity);
                await _repo.Save();
            }
            
        }

        public async Task<IEnumerable<ProductEntity>> GetAll()
        {
            return await _repo.Get(n1=> n1.specs,n2=> n2.images , n3=> n3.specs.rams,n4=> n4.specs.storages);
        }

        public async Task<ProductEntity> GetById(int id)
        {
            var datalist = _repo.Get(n1 => n1.specs, n2 => n2.images, n3 => n3.specs.rams,n4=> n4.specs.storages).Result;
            return  datalist.Where(c => c.id.Equals(id)).FirstOrDefault();
        }

        public async Task<IEnumerable<ProductEntity>> SortByPrice()
        {
            return _repo.Get(n1 => n1.specs, n2 => n2.images).Result.OrderBy(c => c.price);
        }

        public async Task<IEnumerable<ProductEntity>> SortByBrand(string name)
        {
            var data = _repo.Get(n1 => n1.specs, n2 => n2.images).Result.Where(c => c.productBrand.Equals(name));
            return data.OrderBy(c => c.productBrand);
        }
    }
}
