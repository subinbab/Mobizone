using DomainLayer;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
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
                entity.IsActive = 0;
                _repo.Add(entity);
                _repo.Save();
            }
            catch (Exception ex)
            {

            }
        }

        public async Task Delete(CartDetails entity)
        {
            entity.IsActive = 1;
            _repo.Update(entity);
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
            var result = await _repo.Get();
            return result.Where(c => c.IsActive.Equals(0));
        }
    }
}
