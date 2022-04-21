using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DomainLayer;
using DomainLayer.Users;

namespace BusinessObjectLayer.User
{
    public interface IUserCreate
    {

        Task AddUserRegistration (UserRegistration user);
        Task<IEnumerable<UserRegistration>> Get();
        Task<UserRegistration> Authenticate(string username , string password);
    }
}
