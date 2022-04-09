using System;
using System.Collections.Generic;
using System.Text;
using DomainLayer;
using DomainLayer.Users;

namespace BusinessObjectLayer.User
{
    public interface IUserCreate
    {
        
        void AddUserRegistration (UserRegistration user);
    }
}
