using DomainLayer;
using Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessObjectLayer
{
    public class PrivacyOperations : IPrivacyOperation
    {
        IRepositoryOperations<PrivacyPolicy> _repo;
        public PrivacyOperations(IRepositoryOperations<PrivacyPolicy> repo)
        {

            _repo = repo;
        }
        public async Task Add(PrivacyPolicy data)
        {
            _repo.Add(data);
            _repo.Save();
        }

        public async Task Edit(PrivacyPolicy data)
        {
            _repo.Update(data);
            _repo.Save();
        }

        

        public Task<IEnumerable<PrivacyPolicy>> Get()
        {
             return _repo.Get();
        }
    }
}
