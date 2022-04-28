using DomainLayer;
using DomainLayer.Users;
using DTOLayer.Product;
using DTOLayer.UserModel;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UILayer.Models;

namespace UILayer.Data.ApiServices
{
    public class adminApi
    {
        private IConfiguration Configuration { get; }
        public adminApi(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        //Authenticate
        public Login Authenticate(LoginViewModel user)
        {
            using (HttpClient httpclient = new HttpClient())
            {
                ResponseModel<Login> _responseModel = new ResponseModel<Login>();
                string data = Newtonsoft.Json.JsonConvert.SerializeObject(user);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                string url = Configuration.GetSection("Development")["BaseApi"].ToString() + "api/auth/admin";
                //string url = "http://subin9408-001-site1.ftempurl.com/api/product";
                Uri uri = new Uri(url);
                System.Threading.Tasks.Task<HttpResponseMessage> result = httpclient.PostAsync(uri, content);
                if (result.Result.IsSuccessStatusCode)
                {

                    System.Threading.Tasks.Task<string> response = result.Result.Content.ReadAsStringAsync();
                    _responseModel = Newtonsoft.Json.JsonConvert.DeserializeObject<ResponseModel<Login>>(response.Result);
                    return _responseModel.result;
                }
                return null;

            }
        }
        public bool CreateAbout(About about)
        {
            using (HttpClient httpclient = new HttpClient())
            {
                string data = Newtonsoft.Json.JsonConvert.SerializeObject(about);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                string url = Configuration.GetSection("Development")["BaseApi"].ToString() + "api/users/UserCreate";
                //string url = "http://subin9408-001-site1.ftempurl.com/api/Settings/AboutPost";
                Uri uri = new Uri(url);
                System.Threading.Tasks.Task<HttpResponseMessage> result = httpclient.PostAsync(uri, content);
                if (result.Result.IsSuccessStatusCode)
                {
                    return true;
                }
                return false;
            }
        }

        public async Task<IEnumerable<About>> AboutGet()

        {
            RequestHandler<IEnumerable<About>> _requestHandler = new RequestHandler<IEnumerable<About>>(Configuration);
            _requestHandler.url = "api/Settings/AboutGet";
            return _requestHandler.Get();

        }
        public bool EditAbout (About about)
        {
            RequestHandler<About> _requestHandler = new RequestHandler<About>(Configuration);
            _requestHandler.url = "api/Settings/AboutPut";
            return _requestHandler.Edit(about);
        }
        public bool CreatePrivacy(PrivacyPolicy privacy)
        {
            using (HttpClient httpclient = new HttpClient())
            {
                string data = Newtonsoft.Json.JsonConvert.SerializeObject(privacy);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                string url = Configuration.GetSection("Development")["BaseApi"].ToString() + "api/users/UserCreate";
                //string url = "http://subin9408-001-site1.ftempurl.com/api/Settings/PrivacyPost";
                Uri uri = new Uri(url);
                System.Threading.Tasks.Task<HttpResponseMessage> result = httpclient.PostAsync(uri, content);
                if (result.Result.IsSuccessStatusCode)
                {
                    return true;
                }
                return false;
            }
        }

        public async Task<IEnumerable<PrivacyPolicy>> PrivacyGet()

        {
            RequestHandler<IEnumerable<PrivacyPolicy>> _requestHandler = new RequestHandler<IEnumerable<PrivacyPolicy>>(Configuration);
            _requestHandler.url = "api/Settings/PrivacyGet";
            return _requestHandler.Get();

        }
        public bool EditPrivacy(PrivacyPolicy privacy)
        {
            RequestHandler<PrivacyPolicy> _requestHandler = new RequestHandler<PrivacyPolicy>(Configuration);
            _requestHandler.url = "api/Settings/PrivacyPut";
            return _requestHandler.Edit(privacy);
        }
        public bool CreateContact(Contact contact)
        {
            using (HttpClient httpclient = new HttpClient())
            {
                string data = Newtonsoft.Json.JsonConvert.SerializeObject(contact);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                string url = Configuration.GetSection("Development")["BaseApi"].ToString() + "api/users/UserCreate";
                //string url = "http://subin9408-001-site1.ftempurl.com/api/Settings/ContactPost";
                Uri uri = new Uri(url);
                System.Threading.Tasks.Task<HttpResponseMessage> result = httpclient.PostAsync(uri, content);
                if (result.Result.IsSuccessStatusCode)
                {
                    return true;
                }
                return false;
            }
        }

        public async Task<IEnumerable<Contact>> ContactGet()

        {
            RequestHandler<IEnumerable<Contact>> _requestHandler = new RequestHandler<IEnumerable<Contact>>(Configuration);
            _requestHandler.url = "api/Settings/ContactGet";
            return _requestHandler.Get();

        }
        public bool EditContact(Contact contact)
        {
            RequestHandler<Contact> _requestHandler = new RequestHandler<Contact>(Configuration);
            _requestHandler.url = "api/Settings/ContactPut";
            return _requestHandler.Edit(contact);
        }
       
    }
}
