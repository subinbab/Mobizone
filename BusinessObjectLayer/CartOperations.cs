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
        IRepositoryOperations<ProductCart> _repo;
        public CartOperations(IRepositoryOperations<ProductCart> repo)
        {
            _repo = repo;
        }
        public async Task Add(ProductCart entity)
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

        public async Task Delete(ProductCart entity)
        {
            _repo.Delete(entity);
            _repo.Save();
        }

        public async Task Edit(ProductCart entity)
        {
            _repo.Update(entity);
            _repo.Save();
        }

        public Task<ProductCart> GetById(int id)
        {
            return _repo.GetById(id);
        }

        public async Task<IEnumerable<ProductCart>> Get()
        {
            return await  _repo.Get(n1=>n1.cartDetails);
        }
    }
}
