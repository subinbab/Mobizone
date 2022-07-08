using DomainLayer;
using Repository;
using System.Collections.Generic;
using System.Linq;
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
            data.IsActive = 0;
            _repo.Add(data);
            _repo.Save();
        }

        public async Task Edit(PrivacyPolicy data)
        {
            _repo.Update(data);
            _repo.Save();
        }

        

        public async Task<IEnumerable<PrivacyPolicy>> Get()
        {
            var result = await _repo.Get();
            return result.Where(c => c.IsActive.Equals(0));
        }
    }
}
