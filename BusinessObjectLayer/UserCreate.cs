using DomainLayer.Users;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessObjectLayer.User
{
    public class UserCreate : IUserCreate
    {
        ProductDbContext _userContext;
        IRepositoryOperations<UserRegistration> _userRepo;
        public UserCreate(ProductDbContext userContext)
        {
            _userContext = userContext;
            _userRepo = new RepositoryOperations<UserRegistration>(_userContext);
        }
        public void AddUserRegistration(UserRegistration user)
        {
            _userRepo.Add(user);
            _userRepo.Save();
        }

        public UserRegistration Authenticate(string username, string password)
        {
            UserRegistration registration = new UserRegistration();
            var list= _userRepo.Get();
            var user = list.Where(c => c.Email.Equals(username) && c.Password.Equals(password)).FirstOrDefault();
            
            return user;
        }

        public IEnumerable<UserRegistration> Get()
        {
            return _userRepo.Get();
        }
    }
}
