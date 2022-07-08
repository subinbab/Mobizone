using DomainLayer;
using Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjectLayer.ProductOperations
{
    public class ProductSubPartOperations : IProductSubPartOperations
    {
        IRepositoryOperations<ProductSubPart> _repo;
        public ProductSubPartOperations(IRepositoryOperations<ProductSubPart> repo)
        {
            _repo = repo;
        }
        public async Task AddProduct(ProductSubPart entity)
        {
            try
{
                entity.IsActive = 0;
                _repo.Add(entity);
                _repo.Save();
            }
            catch (Exception ex)
            {

            }
            
        }

        public async Task DeleteProduct(ProductSubPart entity)
        {
            entity.IsActive = 1;
            _repo.Update(entity);
            _repo.Save();
        }

        public async Task EditProduct(ProductSubPart entity)
        {
            
            _repo.Update(entity);
            _repo.Save();
        }

        public Task<ProductSubPart> GetById(int id)
        {
            return _repo.GetById(id);
        }

        public  Task<IEnumerable<ProductSubPart>> GetProduct()
        {
            return _repo.Get();
        }
    }
}
