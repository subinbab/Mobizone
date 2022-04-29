using AutoMapper;
using DomainLayer;
using DomainLayer.ProductModel;
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
        Login _login;
        private readonly IMapper _mapper;
        private IConfiguration _configuration { get; }
        public adminApi(IConfiguration configuration, IMapper mapper)
        {
            _mapper = mapper;
            _configuration = configuration;
        }
        //Authenticate
        public Login Authenticate(LoginViewModel user)
        {
            _login = (Login)_mapper.Map<Login>(user);
            RequestHandler<Login> requestHandler = new RequestHandler<Login>(_configuration);
            requestHandler.url = "api/auth/admin";
            return requestHandler.Post(_login);
        }
        public bool CreateAbout(About about)
        {
            RequestHandler<About> requestHandler = new RequestHandler<About>(_configuration);
            using (HttpClient httpclient = new HttpClient())
            {
                string data = Newtonsoft.Json.JsonConvert.SerializeObject(about);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                string url = _configuration.GetSection("Development")["BaseApi"].ToString() + "api/users/UserCreate";
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
            RequestHandler<IEnumerable<About>> _requestHandler = new RequestHandler<IEnumerable<About>>(_configuration);
            _requestHandler.url = "api/Settings/AboutGet";
            return _requestHandler.Get();

        }
        public bool EditAbout (About about)
        {
            RequestHandler<About> _requestHandler = new RequestHandler<About>(_configuration);
            _requestHandler.url = "api/Settings/AboutPut";
            return _requestHandler.Edit(about);
        }
        public bool CreatePrivacy(PrivacyPolicy privacy)
        {
            using (HttpClient httpclient = new HttpClient())
            {
                string data = Newtonsoft.Json.JsonConvert.SerializeObject(privacy);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                string url = _configuration.GetSection("Development")["BaseApi"].ToString() + "api/users/UserCreate";
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
            RequestHandler<IEnumerable<PrivacyPolicy>> _requestHandler = new RequestHandler<IEnumerable<PrivacyPolicy>>(_configuration);
            _requestHandler.url = "api/Settings/PrivacyGet";
            return _requestHandler.Get();

        }
        public bool EditPrivacy(PrivacyPolicy privacy)
        {
            RequestHandler<PrivacyPolicy> _requestHandler = new RequestHandler<PrivacyPolicy>(_configuration);
            _requestHandler.url = "api/Settings/PrivacyPut";
            return _requestHandler.Edit(privacy);
        }
        public bool CreateContact(Contact contact)
        {
            using (HttpClient httpclient = new HttpClient())
            {
                string data = Newtonsoft.Json.JsonConvert.SerializeObject(contact);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                string url = _configuration.GetSection("Development")["BaseApi"].ToString() + "api/users/UserCreate";
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
            RequestHandler<IEnumerable<Contact>> _requestHandler = new RequestHandler<IEnumerable<Contact>>(_configuration);
            _requestHandler.url = "api/Settings/ContactGet";
            return _requestHandler.Get();

        }
        public bool EditContact(Contact contact)
        {
            RequestHandler<Contact> _requestHandler = new RequestHandler<Contact>(_configuration);
            _requestHandler.url = "api/Settings/ContactPut";
            return _requestHandler.Edit(contact);
        }
       
    }
}
