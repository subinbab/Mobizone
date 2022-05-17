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
        public async Task Add(Cart entity)
        {
            _repo.Add(entity);
            _repo.Save();
        }

        public async Task Delete(Cart entity)
        {
            _repo.Delete(entity);
            _repo.Save();
        }

        public async Task Edit(Cart entity)
        {
            _repo.Update(entity);
            _repo.Save();
        }

        public Task<Cart> GetById(int id)
        {
            return _repo.GetById(id);
        }

        public Task<IEnumerable<Cart>> Get()
        {
            return _repo.Get();
        }
    }
}
