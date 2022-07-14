using DomainLayer.Users;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessObjectLayer
{
    public class AddressOperations : IAddressOperations
    {
        IRepositoryOperations<Address> _repo;
        public AddressOperations(IRepositoryOperations<Address> repo)
        {
            _repo = repo;
        }

        public async Task Add(Address address)
        {
            try
            {
                address.IsActive = 0;
                _repo.Add(address);
                _repo.Save();
            }
           catch(Exception ex)
            {

            }

        }

        public void delete(Address data)
        {
            try
            {
                data.IsActive = 1;
                _repo.Update(data);
                _repo.Save();
            }
            catch (Exception ex)
            {

            }
        }

        public async Task Edit(Address address)
        {
            _repo.Update(address);
            _repo.Save();
        }

        public async  Task<IEnumerable<Address>> get()
        {
            var result = await _repo.Get();
            return result.Where(c => c.IsActive.Equals(0));
        }
    }
}
