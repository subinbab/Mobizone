using DomainLayer;
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
        private IConfiguration Configuration { get; }
        public UserApi(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public bool CreateUser(UserViewModel user)
        {
            using (HttpClient httpclient = new HttpClient())
            {
                string data = Newtonsoft.Json.JsonConvert.SerializeObject(user);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                string url = Configuration.GetSection("Development")["BaseApi"].ToString() + "api/users/UserCreate";
                //string url = "http://subin9408-001-site1.ftempurl.com/api/product";
                Uri uri = new Uri(url);
                System.Threading.Tasks.Task<HttpResponseMessage> result = httpclient.PostAsync(uri, content);
                if (result.Result.IsSuccessStatusCode)
                {
                    return true;
                }
                return false;
            }
        }
        //getuser data
        public IEnumerable<UserRegistration> GetUserData()
        {
            
            ResponseModel<IEnumerable<UserRegistration>> _responseModel = null;
            using (HttpClient httpclient = new HttpClient())
            {
                _responseModel = null;
                string url = Configuration.GetSection("Development")["BaseApi"].ToString() + "api/users/userdata";
                Uri uri = new Uri(url);
                System.Threading.Tasks.Task<HttpResponseMessage> result = httpclient.GetAsync(uri);
                if (result.Result.IsSuccessStatusCode)
                {
                    System.Threading.Tasks.Task<string> response = result.Result.Content.ReadAsStringAsync();
                    _responseModel = Newtonsoft.Json.JsonConvert.DeserializeObject<ResponseModel<IEnumerable<UserRegistration>>>(response.Result);
                }

                return _responseModel.result;
            }
        }
        //Authenticate
        public bool Authenticate(LoginViewModel user)
        {
            using (HttpClient httpclient = new HttpClient())
            {
                string data = Newtonsoft.Json.JsonConvert.SerializeObject(user);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                string url = Configuration.GetSection("Development")["BaseApi"].ToString() + "api/auth";
                //string url = "http://subin9408-001-site1.ftempurl.com/api/product";
                Uri uri = new Uri(url);
                System.Threading.Tasks.Task<HttpResponseMessage> result = httpclient.PostAsync(uri, content);
                if (result.Result.StatusCode==System.Net.HttpStatusCode.OK)
                {
                    return true;
                }
                return false; 

            }
        }
        public bool EditUser(UserRegistration product)
        {
            RequestHandler<UserRegistration> _requestHandler = new RequestHandler<UserRegistration>(Configuration);
            _requestHandler.url = "api/users";
            /*using (HttpClient httpclient = new HttpClient())
            {
                string data = Newtonsoft.Json.JsonConvert.SerializeObject(product);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                string url = Configuration.GetSection("Development")["BaseApi"].ToString() + "api/productop";
                Uri uri = new Uri(url);
                System.Threading.Tasks.Task<HttpResponseMessage> response = httpclient.PutAsync(uri, content);

                if (response.Result.IsSuccessStatusCode)
                {
                    return true;
                }*/
            return _requestHandler.Edit(product);

        }
        public bool CreateCheckOut(Checkout checkout)
        {

            using (HttpClient httpclient = new HttpClient())
            {
                string data = Newtonsoft.Json.JsonConvert.SerializeObject(checkout);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                string url = Configuration.GetSection("Development")["BaseApi"].ToString() + "api/Users/CheckOutData";
                //string url = "http://subin9408-001-site1.ftempurl.com/api/product";
                Uri uri = new Uri(url);
                httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", " c3ViaW5AZ21haWwuY29tOmN4djJzQ0tPaFA0YmFTblhzMlY2Ymc9PQ==");
                System.Threading.Tasks.Task<HttpResponseMessage> result = httpclient.PostAsync(uri, content);
                if (result.Result.IsSuccessStatusCode)
                {
                    return true;
                }
                return false;
            }
        }
        public async Task<IEnumerable<Checkout>> GetCheckOut()
        {
            RequestHandler<IEnumerable<Checkout>> _requestHandler = new RequestHandler<IEnumerable<Checkout>>(Configuration);
            _requestHandler.url = "api/users/CheckOutData";
            return _requestHandler.Get();

        }
    }
}
