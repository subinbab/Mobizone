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
        IRepositoryOperations<MyCart> _repo;
        public CartOperations(IRepositoryOperations<MyCart> repo)
        {
            _repo = repo;
        }
        public async Task Add(MyCart entity)
        {
            try
            {
                entity.IsActive = 0;
                await _repo.Add(entity);
                await _repo.Save();
            }
            catch (Exception ex)
            {

            }
        }

        public async Task Delete(MyCart entity)
        {
            entity.IsActive = 1;
            await _repo.Update(entity);
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
                result =  _repo.Get(n1 => n1.cartDetails.Where(c=> c.IsActive.Equals(0)));
            }
            catch(Exception ex)
            {
                result = null;
            }
            return await result;
        }
    }
}
