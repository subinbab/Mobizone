using DomainLayer.Users;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Text;

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
        public bool Authenticate(LoginViewModel user)
        {
            using (HttpClient httpclient = new HttpClient())
            {
                string data = Newtonsoft.Json.JsonConvert.SerializeObject(user);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                string url = Configuration.GetSection("Development")["BaseApi"].ToString() + "api/auth/admin";
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
    }
}
