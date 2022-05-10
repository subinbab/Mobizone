using DomainLayer;
using Repository;
using System.Collections.Generic;
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

        public Task<IEnumerable<AdminContact>> Get()
        {
          return  _repo.Get();
        }
    }
}
