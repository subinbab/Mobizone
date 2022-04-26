using DomainLayer;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessObjectLayer
{
    public interface IContactOperations
    {
        Task Add(Contact data);
        Task Edit(Contact data);
        Task<IEnumerable<Contact>> Get();
    }
}
