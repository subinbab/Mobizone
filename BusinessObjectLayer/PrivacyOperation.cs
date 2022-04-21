using Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjectLayer
{
    public class PrivacyPolicy : IPrivacyOperation
    {
        IRepositoryOperations<PrivacyPolicy> _repo;
        public PrivacyPolicy(IRepositoryOperations<PrivacyPolicy> repo)
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

        public async  Task Get(PrivacyPolicy data)
        {
            _repo.Get();
        }
    }
}
