using DomainLayer;
using DomainLayer.ProductModel;
using DomainLayer.Users;
using DTOLayer.UserModel;
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
    public class UserApi
    {
        private IConfiguration _configuration { get; }
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
            catch(Exception ex)
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
            catch(Exception ex)
            {
                return false;
            }

        }
        #endregion


        #region Creare an order
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
            catch(Exception ex)
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
                var result =  _requestHandler.Edit(checkout);
                if(result != null)
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

        #region Create an order
        public bool Createcart(Cart cart)
        {
            RequestHandler<Cart> requestHandler = new RequestHandler<Cart>(_configuration);
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

        public bool EditCart(Cart cart)
        {
            RequestHandler<Cart> _requestHandler = new RequestHandler<Cart>(_configuration);
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
        public async Task<IEnumerable<Cart>> GetCart()
        {
            RequestHandler<IEnumerable<Cart>> _requestHandler = new RequestHandler<IEnumerable<Cart>>(_configuration);
            try
            {
                _requestHandler.url = "api/users/GetCart";
                return _requestHandler.Get().result;
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

    }
}
