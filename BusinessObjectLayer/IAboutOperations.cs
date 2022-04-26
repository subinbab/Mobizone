using DomainLayer;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessObjectLayer
{
    public  interface IAboutOperations
    {
        Task Add(About data);
        Task Edit(About data);
        Task<IEnumerable<About>> Get();
    }
}
