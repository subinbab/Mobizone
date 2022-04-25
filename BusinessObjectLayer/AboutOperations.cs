using DomainLayer;
using Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjectLayer
{
    public class AboutOperations : IAboutOperations
    {
        IRepositoryOperations<About> _repo;
        public AboutOperations(IRepositoryOperations<About> repo)
        {
            _repo = repo;
        }

        public async Task Add(About data)
        {
            _repo.Add(data);
            _repo.Save();
        }

        public  async Task Edit(About data)
        {
            _repo.Update(data);
            _repo.Save();
        }

        public async Task Get(About data)
        {
            _repo.Get();
        }
    }
}
