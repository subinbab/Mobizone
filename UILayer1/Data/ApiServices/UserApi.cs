using DomainLayer;
using DomainLayer.ProductModel;
using DomainLayer.Users;
using DTOLayer.UserModel;
using log4net;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using UILayer.Models;

namespace UILayer.Data.ApiServices
{
    public class UserApi : IUserApi
    {
        private IConfiguration _configuration { get; }
        private readonly ILog _log;
        public UserApi(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public bool CreateUser(UserViewModel user)
        {
            try
            {
                RequestHandler<UserViewModel> requestHandler = new RequestHandler<UserViewModel>(_configuration);
                requestHandler.url = "api/users/UserCreate";
                if (requestHandler.Post(user).IsSuccess)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                return false;
            }
        }
        //getuser data
        public IEnumerable<UserRegistration> GetUserData()
        {
            RequestHandler<IEnumerable<UserRegistration>> _requestHandler = new RequestHandler<IEnumerable<UserRegistration>>(_configuration);
            try
            {
                _requestHandler.url = "api/users/userdata";
                return _requestHandler.Get().result;
            }
            catch (Exception ex)
            {
                return null;
            }

        }



        //Authenticate
        /*public bool Authenticate(LoginViewModel user)
        {
            using (HttpClient httpclient = new HttpClient())
            {
                string data = Newtonsoft.Json.JsonConvert.SerializeObject(user);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                string url = _configuration.GetSection("Development")["BaseApi"].ToString() + "api/auth";
                //string url = "http://subin9408-001-site1.ftempurl.com/api/product";
                Uri uri = new Uri(url);
                System.Threading.Tasks.Task<HttpResponseMessage> result = httpclient.PostAsync(uri, content);
                if (result.Result.StatusCode==System.Net.HttpStatusCode.OK)
                {
                    return true;
                }
                return false; 

            }
        }*/
        public bool ForgotPassword(UserRegistration User)
        {
            try
            {
                RequestHandler<UserRegistration> requestHandler = new RequestHandler<UserRegistration>(_configuration);
                requestHandler.url = "api/users/userdata";
                if (requestHandler.Edit(User).IsSuccess)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                return false;
            }

        }

        #region Edit user method
        public bool EditUser(UserRegistration product)
        {
            RequestHandler<UserRegistration> _requestHandler = new RequestHandler<UserRegistration>(_configuration);
            try
            {
                _requestHandler.url = "api/users";
                var result = _requestHandler.Edit(product);
                if (result != null)
                {
                    if (result.IsSuccess)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }

        }
        #endregion


        #region Create an order
        public bool CreateCheckOut(Checkout checkout)
        {
            RequestHandler<Checkout> requestHandler = new RequestHandler<Checkout>(_configuration);
            try
            {
                requestHandler.url = "api/Users/CheckOutData";
                var result = requestHandler.Post(checkout);
                if (result != null)
                {
                    if (result.IsSuccess)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion

        public async Task<IEnumerable<Checkout>> GetCheckOut()
        {
            RequestHandler<IEnumerable<Checkout>> _requestHandler = new RequestHandler<IEnumerable<Checkout>>(_configuration);
            try
            {
                _requestHandler.url = "api/users/CheckOutData";
                return _requestHandler.Get().result;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public bool EditCheckout(Checkout checkout)
        {
            RequestHandler<Checkout> _requestHandler = new RequestHandler<Checkout>(_configuration);
            try
            {
                _requestHandler.url = "api/users/CheckoutPut";
                var result = _requestHandler.Edit(checkout);
                if (result != null)
                {
                    if (result.IsSuccess)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public async Task<IEnumerable<Address>> GetAddress()
        {
            RequestHandler<IEnumerable<Address>> _requestHandler = new RequestHandler<IEnumerable<Address>>(_configuration);
            try
            {
                _requestHandler.url = "api/users/address";
                return _requestHandler.Get().result;
            }
            catch
            {
                return null;
            }
        }

        #region Create an cart
        public bool Createcart(MyCart cart)
        {
            RequestHandler<MyCart> requestHandler = new RequestHandler<MyCart>(_configuration);
            try
            {
                requestHandler.url = "api/Users/CreateCart";
                var result = requestHandler.Post(cart);
                if (result != null)
                {
                    if (result.IsSuccess)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion
        #region delete method for address
        public bool DeleteAddress(int id)
        {
            RequestHandler<Address> requestHandler = new RequestHandler<Address>(_configuration);
            try
            {
                requestHandler.url = "api/Users/AddressDelete/";
                if (requestHandler.Delete(id).IsSuccess)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
                return false;
            }
        }
        #endregion

        public bool EditCart(MyCart cart)
        {
            RequestHandler<MyCart> _requestHandler = new RequestHandler<MyCart>(_configuration);
            try
            {
                _requestHandler.url = "api/users/UpdateCart";
                var result = _requestHandler.Edit(cart);
                if (result != null)
                {
                    if (result.IsSuccess)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }

        }
        public async Task<IEnumerable<MyCart>> GetCart()
        {
            IEnumerable<MyCart> result = null;
            RequestHandler<IEnumerable<MyCart>> _requestHandler = new RequestHandler<IEnumerable<MyCart>>(_configuration);
            try
            {
                _requestHandler.url = "api/users/GetCart";
                try
                {
                    if (_requestHandler.Get().result == null || _requestHandler.Get() == null)
                    {

                    }
                    else
                    {
                        result = _requestHandler.Get().result;
                    }
                }
                catch (Exception ex)
                {

                }
                if (result == null)
                {
                    result = null;
                }
                return result;
            }
            catch
            {
                return null;
            }
        }

        public async Task<IEnumerable<Checkout>> GetUserOrders()
        {
            RequestHandler<IEnumerable<Checkout>> _requestHandler = new RequestHandler<IEnumerable<Checkout>>(_configuration);
            try
            {
                _requestHandler.url = "api/users/GetCheckout";
                return _requestHandler.Get().result;
            }
            catch
            {
                return null;
            }
        }
        public bool PostMail(MailRequest mailRequest)
        {
            try
            {
                RequestHandler<MailRequest> requestHandler = new RequestHandler<MailRequest>(_configuration);
                requestHandler.url = "api/mail/send";
                if (requestHandler.Post(mailRequest).IsSuccess)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                return false;
            }
        }

        #region delete method for cartDetails
        public bool DeleteCartDetails(int id)
        {
            RequestHandler<CartDetails> requestHandler = new RequestHandler<CartDetails>(_configuration);
            try
            {
                requestHandler.url = "api/Users/DeleteCartDetails/";
                if (requestHandler.Delete(id).IsSuccess)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
                return false;
            }
        }
        #endregion
        #region delete method for cartDetails
        public bool DeleteCart(int id)
        {
            RequestHandler<CartDetails> requestHandler = new RequestHandler<CartDetails>(_configuration);
            try
            {
                requestHandler.url = "api/Users/DeleteCart/";
                if (requestHandler.Delete(id).IsSuccess)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
                return false;
            }
        }
        #endregion

    }
}
