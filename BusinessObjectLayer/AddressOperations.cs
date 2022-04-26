using DomainLayer.Users;
using Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessObjectLayer
{
    public class AddressOperations
    {
        IRepositoryOperations<Address> _repo;
        public AddressOperations(IRepositoryOperations<Address> repo)
        {
            _repo = repo;
        }
        public void delete(Address data)
        {
            _repo.Delete(data);
            _repo.Save();
        }
        public Task<IEnumerable<Address>> get()
        {
            return _repo.Get();
        }
    }
}
