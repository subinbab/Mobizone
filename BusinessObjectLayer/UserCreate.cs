using DomainLayer.Users;
using Repository;
using System;
using System.Collections.Generic;
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
    }
}
