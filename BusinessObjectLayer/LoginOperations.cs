using DomainLayer;
using Repository;
using System;
using System.Collections.Generic;
using System.Text;
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
            _repo.Add(entity);
            _repo.Save();
        }

        public Task Delete(Login entity)
        {
            throw new NotImplementedException();
        }

        public Task Edit(Login entity)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Login>> Get()
        {
            return  _repo.Get();
        }

        public Login Get(int id)
        {
            throw new NotImplementedException();
        }
    }
}
