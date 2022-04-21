using DomainLayer.ProductModel;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using UILayer.Models;

namespace UILayer.Data.ApiServices
{
    public class ImageApi
    {
        private IConfiguration Configuration { get; }
        public ImageApi(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IEnumerable<Images> GetProduct()
        {
            ResponseModel<IEnumerable<Images>> _responseModel = new ResponseModel<IEnumerable<Images>>();
            using (HttpClient httpclient = new HttpClient())
            {
                string url = Configuration.GetSection("Development")["BaseApi"].ToString() + "api/productop";
                Uri uri = new Uri(url);
                httpclient.DefaultRequestHeaders.Authorization =
    new AuthenticationHeaderValue("Basic", "hello");
                System.Threading.Tasks.Task<HttpResponseMessage> result = httpclient.GetAsync(uri);
                if (result.Result.IsSuccessStatusCode)
                {
                    System.Threading.Tasks.Task<string> response = result.Result.Content.ReadAsStringAsync();
                    _responseModel = Newtonsoft.Json.JsonConvert.DeserializeObject<ResponseModel<IEnumerable<Images>>>(response.Result);
                }
                return _responseModel.result;
            }
        }

        public bool DeleteProduct(int id)
        {
            using (HttpClient httpclient = new HttpClient())
            {
                string data = Newtonsoft.Json.JsonConvert.SerializeObject(id);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                string url = Configuration.GetSection("Development")["BaseApi"].ToString() + "api/imagesoperations/"
                + id;
                Uri uri = new Uri(url);
                System.Threading.Tasks.Task<HttpResponseMessage> result = httpclient.DeleteAsync(uri);
                if (result.Result.IsSuccessStatusCode)
                {
                    return true;
                }
                return false;
            }
        }
    }
}
