using DomainLayer.Users;
using DTOLayer.UserModel;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Text;
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
    }
}
