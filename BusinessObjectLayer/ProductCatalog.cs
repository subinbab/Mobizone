using DomainLayer;
using Repository;
using System;
using System.Collections.Generic;

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
        public void AddProduct(Product entity)
        {
            _repo.Add(entity);
            _repo.Save();
        }

        public void DeleteProduct(Product entity)
        {
            _repo.Delete(entity);
            _repo.Save();
        }

        public void EditProduct(Product entity)
        {
            _repo.Update(entity);
            _repo.Save();
        }

        public Product GetById(int id)
        {
            return _repo.GetById(id);
        }

        public IEnumerable<Product> GetProduct()
        {
            return _repo.Get();
        }
    }
}
