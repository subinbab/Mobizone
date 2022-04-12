using DomainLayer;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessObjectLayer
{
    public interface ILoginOperations
    {
        void Add(Login entity);
        void Edit(Login entity);
        IEnumerable<Login> Get();
        Login Get(int id);
        void Delete(Login entity);
    }
}
