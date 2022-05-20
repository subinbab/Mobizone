using DomainLayer.Users;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessObjectLayer
{
    public class ForgotPassword : IForgotPassword
    {
        ProductDbContext _userContext;
        IRepositoryOperations<UserRegistration> _userRepo;
        public ForgotPassword(IRepositoryOperations<UserRegistration> repo)
        {
            _userRepo = repo;
        }
        public UserRegistration forgotPassword(int userId)
        {
            var data = _userRepo.Get().Result;
            var check = data.ToList().Where(x => x.UserId == userId).FirstOrDefault();
            return check;
        }
    }
}
