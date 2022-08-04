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
            var data = _repo.Get(n1 => n1.specs, n2 => n2.images, n3 => n3.specs.rams, n4 => n4.specs.storages).Result.Where(c => c.name.ToLower().StartsWith(name.ToLower()));
            var result = data.ToList().Where(c=> c.IsActive.Equals(0));
            return data;
        }

        public async Task Add(ProductEntity product)
        {
            product.IsActive = 0;
            _repo.Add(product);
            _repo.Save();
        }

        public async Task DeleteProduct(ProductEntity entity)
        {
            try
            {
                entity.IsActive = 1;
                await _repo.Update(entity);
                await _repo.Save();
            }
            catch (Exception ex)
            {

            }
            
        }

        public async Task EditProduct(ProductEntity entity)
        {
            if (entity.purchasedNumber == null)
                entity.purchasedNumber = 0;
            if (entity.quantity == 0)
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
            var result = await _repo.Get(n1 => n1.specs, n2 => n2.images.Where(c=>c.IsActive.Equals(0)), n3 => n3.specs.rams.Where(c => c.IsActive.Equals(0)), n4 => n4.specs.storages.Where(c => c.IsActive.Equals(0)));
            return result.Where(c => c.IsActive.Equals(0));
        }

        public async Task<ProductEntity> GetById(int id)
        {
            var datalist = _repo.Get(n1 => n1.specs, n2 => n2.images.Where(c => c.IsActive.Equals(0)), n3 => n3.specs.rams.Where(c => c.IsActive.Equals(0)), n4=> n4.specs.storages.Where(c => c.IsActive.Equals(0))).Result;
            return  datalist.Where(c => c.id.Equals(id)&& c.IsActive.Equals(0)).FirstOrDefault();
        }

        public async Task<IEnumerable<ProductEntity>> SortByPriceAscending()
        {
            return _repo.Get(n1 => n1.specs, n2 => n2.images.Where(c => c.IsActive.Equals(0))).Result.OrderBy(c => c.price).Where(c=> c.IsActive.Equals(0));
        }
        public async Task<IEnumerable<ProductEntity>> SortByPriceDescending()
        {
            return _repo.Get(n1 => n1.specs, n2 => n2.images.Where(c => c.IsActive.Equals(0))).Result.OrderByDescending(c => c.price).Where(c => c.IsActive.Equals(0));
        }

        public async Task<IEnumerable<ProductEntity>> FilterByBrand(string name)
        {
           var data = _repo.Get(n1 => n1.specs, n2 => n2.images).Result.Where(c => c.productBrand.Equals(name));
            return data.OrderBy(c => c.productBrand).Where(c => c.IsActive.Equals(0));
        }

        
    }
}
