using DomainLayer;
using Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessObjectLayer
{
    public class ContactOperations : IContactOperations
    {
        IRepositoryOperations<Contact> _repo;
        public ContactOperations(IRepositoryOperations<Contact>  repo)
        {
            _repo = repo;
        }
        public async Task Add(Contact data)
        {
            _repo.Add(data);
            _repo.Save();

        }

        public async Task Edit(Contact data)
        {
            _repo.Update(data);
            _repo.Save();
        }

        public async Task Get(Contact data)
        {
            _repo.Get();
        }

        public Task<IEnumerable<Contact>> Get()
        {
          return  _repo.Get();
        }
    }
}
