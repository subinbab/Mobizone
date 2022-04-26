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
            _repo.Delete(data);
            _repo.Save();
        }
        public Task<IEnumerable<Checkout>> get()
        {
            return _repo.Get();
        }
        public async Task Add(Checkout data)
        {
            _repo.Add(data);
            _repo.Save();
        }
        public async Task Edit(Checkout entity)
        {
            _repo.Update(entity);
            _repo.Save();
        }
    }
}
