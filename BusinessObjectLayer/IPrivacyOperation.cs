using DomainLayer;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessObjectLayer
{
    public interface IPrivacyOperation
    {
        Task Add(PrivacyPolicy data);
        Task Edit(PrivacyPolicy data);
        Task<IEnumerable<PrivacyPolicy>> Get();
    }
}
