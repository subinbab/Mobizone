using DomainLayer;
using Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjectLayer
{
    public class CartOperations : ICartOperations
    {
        ProductDbContext _context;
        IRepositoryOperations<Cart> _repo;
        public CartOperations(IRepositoryOperations<Cart> repo)
        {
            _repo = repo;
        }
        public async Task AddProduct(Cart entity)
        {
            _repo.Add(entity);
            _repo.Save();
        }

        public async Task DeleteProduct(Cart entity)
        {
            _repo.Delete(entity);
            _repo.Save();
        }

        public async Task EditProduct(Cart entity)
        {
            _repo.Update(entity);
            _repo.Save();
        }

        public Task<Cart> GetById(int id)
        {
            return _repo.GetById(id);
        }

        public Task<IEnumerable<Cart>> GetProduct()
        {
            return _repo.Get();
        }
    }
}
