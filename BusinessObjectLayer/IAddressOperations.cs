using DomainLayer.Users;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjectLayer
{
    public interface IAddressOperations
    {
        void delete(Address data);
        Task<IEnumerable<Address>> get();
    }
}
