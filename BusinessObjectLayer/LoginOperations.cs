using DomainLayer;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessObjectLayer
{
    public class LoginOperations : ILoginOperations
    {
        ProductDbContext _context;
        IRepositoryOperations<Login> _repo;

        public LoginOperations(IRepositoryOperations<Login> repo)
        {
            _repo = repo;
        }
        public async Task Add(Login entity)
        {
            entity.IsActive = 0;
            _repo.Add(entity);
            _repo.Save();
        }

        public Task Delete(Login entity)
        {
            throw new NotImplementedException();
        }

        public async Task Edit(Login entity)
        {
            _repo.Update(entity);
            _repo.Save();
        }

        public async Task<IEnumerable<Login>> Get()
        {
            var result = await _repo.Get();
            return result.Where(c => c.IsActive.Equals(0));
        }

        public Login Get(int id)
        {
            throw new NotImplementedException();
        }
    }
}
