using DomainLayer;
using Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessObjectLayer
{
    public class ProductCatalog : IProductCatalog
    {
        ProductDbContext _context;
        IRepositoryOperations<Product> _repo;
        public ProductCatalog(ProductDbContext context )
        {
            _context = context;
            _repo = new RepositoryOperations<Product>(_context); 
        }
        public async Task AddProduct(Product entity)
        {
            _repo.Add(entity);
            _repo.Save();
        }

        public async Task DeleteProduct(Product entity)
        {
            _repo.Delete(entity);
            _repo.Save();
        }

        public async Task EditProduct(Product entity)
        {
            _repo.Update(entity);
            _repo.Save();
        }

        public Task<Product> GetById(int id)
        {
            return _repo.GetById(id);
        }

        public Task<IEnumerable<Product>> GetProduct()
        {
            return _repo.Get();
        }
    }
}
