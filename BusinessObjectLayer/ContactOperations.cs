using DomainLayer;
using Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessObjectLayer
{
    public class ContactOperations : IContactOperations
    {
        IRepositoryOperations<AdminContact> _repo;
        public ContactOperations(IRepositoryOperations<AdminContact>  repo)
        {
            _repo = repo;
        }
        public async Task Add(AdminContact data)
        {
            data.IsActive = 0;
            _repo.Add(data);
            _repo.Save();

        }

        public async Task Edit(AdminContact data)
        {
            _repo.Update(data);
            _repo.Save();
        }

        public async Task Get(AdminContact data)
        {
            _repo.Get();
        }

        public async Task<IEnumerable<AdminContact>> Get()
        {
            var result = await _repo.Get();
            return result.Where(c => c.IsActive.Equals(0));
        }
    }
}
