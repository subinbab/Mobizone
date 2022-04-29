using DomainLayer.ProductModel;
using DomainLayer.Users;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjectLayer.User
{
    public class UserCreate : IUserCreate
    {
        ProductDbContext _userContext;
        IRepositoryOperations<UserRegistration> _userRepo;
        public UserCreate(IRepositoryOperations<UserRegistration> repo)
        {
            _userRepo = repo;
        }
        public async Task AddUserRegistration(UserRegistration user)
        {
            _userRepo.Add(user);
            _userRepo.Save();
        }

        public async Task<UserRegistration> Authenticate(string username, string password)
        {
            UserRegistration registration = new UserRegistration();
            var list= await _userRepo.Get();
            var user = list.Where(c => c.Email.Equals(username) && c.Password.Equals(password)).FirstOrDefault();
            
            return user;
        }

        public async Task Edit(UserRegistration user)
        {
            await _userRepo.Update(user);
            await _userRepo.Save();
        }

        public async  Task<IEnumerable<UserRegistration>> Get()
        {
            return await _userRepo.Get(n1=> n1.address);
        }
    }
}
