using DomainLayer;
using DomainLayer.Users;
using Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjectLayer
{
    public class CheckOutOperation : ICheckOutOperation
    {
        IRepositoryOperations<Checkout> _repo;
        public CheckOutOperation(IRepositoryOperations<Checkout> repo)
        {
            _repo = repo; 
        }
        public void delete(Checkout data)
        {
            data.IsActive = 1;
            _repo.Update(data);
            _repo.Save();
        }
        public async Task<IEnumerable<Checkout>> get()
        {
            return await _repo.Get(c=> c.user,c1=> c1.product ,c2=> c2.address);
        }
        public async Task Add(Checkout data)
        {
            try
            {
                data.IsActive = 0;
                _repo.Add(data);
                _repo.Save();
            }
            catch (Exception ex)
            {

            }
        }
        public async Task Edit(Checkout entity)
        {
            _repo.Update(entity);
            _repo.Save();
        }
    }
}
