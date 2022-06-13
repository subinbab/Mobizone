using DomainLayer;
using DomainLayer.Users;
using DTOLayer.UserModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UILayer.Data.ApiServices
{
    public interface IUserApi
    {
        bool Createcart(MyCart cart);
        bool CreateCheckOut(Checkout checkout);
        bool CreateUser(UserViewModel user);
        bool DeleteAddress(int id);
        bool DeleteCart(int id);
        bool DeleteCartDetails(int id);
        bool EditCart(MyCart cart);
        bool EditCheckout(Checkout checkout);
        bool EditUser(UserRegistration product);
        bool ForgotPassword(UserRegistration User);
        Task<IEnumerable<Address>> GetAddress();
        Task<IEnumerable<MyCart>> GetCart();
        Task<IEnumerable<Checkout>> GetCheckOut();
        IEnumerable<UserRegistration> GetUserData();
        Task<IEnumerable<Checkout>> GetUserOrders();
        bool PostMail(MailRequest mailRequest);
    }
}