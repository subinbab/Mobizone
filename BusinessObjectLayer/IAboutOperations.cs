using DomainLayer;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjectLayer
{
   public  interface IAboutOperations
    {
        Task Add(About data);
        Task Edit(About data);
        Task Get(About data);
    }
}
