using DomainLayer.Users;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessObjectLayer
{
    public interface IAddressOperations
    {
        void delete(Address data);
        Task<IEnumerable<Address>> get();
        Task Edit(Address address);
    }
}
