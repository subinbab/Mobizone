using DomainLayer.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessObjectLayer
{
    public interface IForgotPassword
    {
        UserRegistration forgotPassword(int userId);
    }
}
