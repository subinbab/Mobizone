using DomainLayer;
using Repository;
using System;
using System.Collections.Generic;
using System.Text;

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
        public void Add(Login entity)
        {
            _repo.Add(entity);
            _repo.Save();
        }

        public void Delete(Login entity)
        {
            throw new NotImplementedException();
        }

        public void Edit(Login entity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Login> Get()
        {
            return _repo.Get(n1 => n1.roles);
        }

        public Login Get(int id)
        {
            throw new NotImplementedException();
        }
    }
}
