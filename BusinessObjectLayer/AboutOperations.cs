using DomainLayer;
using Repository;
using System;
using System.Collections.Generic;
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
            try
            {
                await _repo.Add(data);
                await _repo.Save();
            }
            catch(Exception ex)
            {

            }
        }

        public  async Task Edit(About data)
        {
            _repo.Update(data);
            _repo.Save();
        }

       
        public Task<IEnumerable<About>> Get()
        {
            return _repo.Get();
        }
    }
}
