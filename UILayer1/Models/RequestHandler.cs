using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace UILayer.Models
{
    public class RequestHandler<T>
    {
        public string _url;
        public string url
        {
            get { return _url ?? String.Empty; }
            set { _url = value; }
        }
        public T content { get; set; }
        private IConfiguration _configuration { get; }
        public RequestHandler(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public  T Get()
        {
            ResponseModel<T> _responseModel = new ResponseModel<T>();
            _responseModel = null;
            try
            {
                
                using (HttpClient httpclient = new HttpClient())
                {
                    string url = _configuration.GetSection("Development")["BaseApi"].ToString() + _url;
                    Uri uri = new Uri(url);
                    httpclient.DefaultRequestHeaders.Authorization =
        new AuthenticationHeaderValue("Basic", "hello");
                    System.Threading.Tasks.Task<HttpResponseMessage> result = httpclient.GetAsync(uri);
                    if (result.Result.IsSuccessStatusCode)
                    {
                        System.Threading.Tasks.Task<string> response = result.Result.Content.ReadAsStringAsync();
                        _responseModel = Newtonsoft.Json.JsonConvert.DeserializeObject<ResponseModel<T>>(response.Result);
                    }
                    return _responseModel.result;
                }
            }
            catch (Exception ex)
            {
                return default(T);

            }
            
        }

        public bool Edit(T entity)
        {

            using (HttpClient httpclient = new HttpClient())
            {
                string data = Newtonsoft.Json.JsonConvert.SerializeObject(entity);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                string url = _configuration.GetSection("Development")["BaseApi"].ToString() + _url;
                Uri uri = new Uri(url);
                System.Threading.Tasks.Task<HttpResponseMessage> response = httpclient.PutAsync(uri, content);

                if (response.Result.IsSuccessStatusCode)
                {
                    return true;
                }
                return false;
            }
        }
    }
}
