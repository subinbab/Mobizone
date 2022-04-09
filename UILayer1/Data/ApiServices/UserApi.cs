using DTOLayer.UserModel;
using Microsoft.Extensions.Configuration;
using System;
using System.Configuration;
using System.Net.Http;
using System.Text;


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
                string url = Configuration.GetSection("Development")["BaseApi"].ToString() + "api/user";
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
