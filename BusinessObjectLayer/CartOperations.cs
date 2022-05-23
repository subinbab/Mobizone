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
        IRepositoryOperations<DbCart> _repo;
        public CartOperations(IRepositoryOperations<DbCart> repo)
        {
            _repo = repo;
        }
        public async Task Add(DbCart entity)
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

        public async Task Delete(DbCart entity)
        {
            _repo.Delete(entity);
            _repo.Save();
        }

        public async Task Edit(DbCart entity)
        {
            _repo.Update(entity);
            _repo.Save();
        }

        public Task<DbCart> GetById(int id)
        {
            return _repo.GetById(id);
        }

        public async Task<IEnumerable<DbCart>> Get()
        {
            return await  _repo.Get(n1=>n1.cartDetails);
        }
    }
}
