using DomainLayer;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjectLayer
{
    public class CartOperations : ICartOperations
    {
        ProductDbContext _context;
        IRepositoryOperations<MyCart> _repo;
        public CartOperations(IRepositoryOperations<MyCart> repo)
        {
            _repo = repo;
        }
        public async Task Add(MyCart entity)
        {
            try
            {
                await _repo.Add(entity);
                await _repo.Save();
            }
            catch (Exception ex)
            {

            }
        }

        public async Task Delete(MyCart entity)
        {
            await _repo.Delete(entity);
            await _repo.Save();
        }

        public async Task Edit(MyCart entity)
        {
            await _repo.Update(entity);
            await _repo.Save();
        }

        public Task<MyCart> GetById(int id)
        {
            return _repo.GetById(id);
        }

        public async Task<IEnumerable<MyCart>> Get()
        {
            Task<IQueryable<MyCart>> result = null;
            try
            {
                result =  _repo.Get(n1 => n1.cartDetails, n2 => n2.cartDetails);
            }
            catch(Exception ex)
            {
                result = null;
            }
            return await result;
        }
    }
}
