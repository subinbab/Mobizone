using DomainLayer;
using Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjectLayer
{
    public class CartDetailsOperation : ICartDetailsOperation
    {
        ProductDbContext _context;
        IRepositoryOperations<CartDetails> _repo;
        public CartDetailsOperation(IRepositoryOperations<CartDetails> repo)
        {
            _repo = repo;
        }
        public async Task Add(CartDetails entity)
        {
            try
            {
                _repo.Add(entity);
                _repo.Save();
            }
            catch (Exception ex)
            {

            }
        }

        public async Task Delete(CartDetails entity)
        {
            _repo.Delete(entity);
            _repo.Save();
        }

        public async Task Edit(CartDetails entity)
        {
            _repo.Update(entity);
            _repo.Save();
        }

        public Task<CartDetails> GetById(int id)
        {
            return _repo.GetById(id);
        }

        public async Task<IEnumerable<CartDetails>> Get()
        {
            return await _repo.Get();
        }
    }
}
