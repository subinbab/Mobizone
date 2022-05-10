using DomainLayer;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessObjectLayer
{
    public interface IContactOperations
    {
        Task Add(AdminContact data);
        Task Edit(AdminContact data);
        Task<IEnumerable<AdminContact>> Get();
    }
}
