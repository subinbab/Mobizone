using DomainLayer;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
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
                data.IsActive = 0;
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

       
        public async Task<IEnumerable<About>> Get()
        {
            var result = await _repo.Get();
            return result.Where(c => c.IsActive.Equals(0));
        }
    }
}