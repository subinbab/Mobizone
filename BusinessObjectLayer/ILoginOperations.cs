using DomainLayer;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjectLayer
{
    public interface ILoginOperations
    {
        Task Add(Login entity);
        Task Edit(Login entity);
        Task<IEnumerable<Login>> Get();
        Login Get(int id);
        Task Delete(Login entity);
    }
}
