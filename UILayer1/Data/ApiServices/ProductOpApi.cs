using DomainLayer;
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
    public class ProductOpApi
    {
        private IConfiguration Configuration { get; }
        public ProductOpApi(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IEnumerable<ProductEntity> GetProduct()
        {
            ResponseModel<IEnumerable<ProductEntity>> _responseModel = null;
            using (HttpClient httpclient = new HttpClient())
            {
                _responseModel = null;
                string url = Configuration.GetSection("Development")["BaseApi"].ToString() + "api/productop";
                Uri uri = new Uri(url);
                httpclient.DefaultRequestHeaders.Authorization =
    new AuthenticationHeaderValue("Basic", "hello");
                System.Threading.Tasks.Task<HttpResponseMessage> result = httpclient.GetAsync(uri);
                if (result.Result.IsSuccessStatusCode)
                {
                    System.Threading.Tasks.Task<string> response = result.Result.Content.ReadAsStringAsync();
                    _responseModel = Newtonsoft.Json.JsonConvert.DeserializeObject<ResponseModel<IEnumerable<ProductEntity>>>(response.Result);
                }
                return _responseModel.result;
            }
        }
        public ProductEntity GetProduct(int id)
        {
            ResponseModel<ProductEntity> _responseModel = null;
            using (HttpClient httpclient = new HttpClient())
            {
                string url = Configuration.GetSection("Development")["BaseApi"].ToString() + "api/productop/" + id;
                Uri uri = new Uri(url);
                System.Threading.Tasks.Task<HttpResponseMessage> result = httpclient.GetAsync(uri);
                if (result.Result.IsSuccessStatusCode)
                {
                    System.Threading.Tasks.Task<string> response = result.Result.Content.ReadAsStringAsync();
                    _responseModel = Newtonsoft.Json.JsonConvert.DeserializeObject<ResponseModel<ProductEntity>>(response.Result);
                }
                return _responseModel.result;
            }
        }
        public bool EditProduct(ProductEntity product)
        {

            using (HttpClient httpclient = new HttpClient())
            {
                string data = Newtonsoft.Json.JsonConvert.SerializeObject(product);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                string url = Configuration.GetSection("Development")["BaseApi"].ToString() + "api/productop";
                Uri uri = new Uri(url);
                System.Threading.Tasks.Task<HttpResponseMessage> response = httpclient.PutAsync(uri, content);

                if (response.Result.IsSuccessStatusCode)
                {
                    return true;
                }
                return false;
            }
        }
        public bool CreateProduct(ProductEntity product)
        {

            using (HttpClient httpclient = new HttpClient())
            {
                string data = Newtonsoft.Json.JsonConvert.SerializeObject(product);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                string url = Configuration.GetSection("Development")["BaseApi"].ToString() + "api/productop";
                //string url = "http://subin9408-001-site1.ftempurl.com/api/product";
                Uri uri = new Uri(url);
                httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", "hello");
                System.Threading.Tasks.Task<HttpResponseMessage> result = httpclient.PostAsync(uri, content);
                if (result.Result.IsSuccessStatusCode)
                {
                    return true;
                }
                return false;
            }
        }
        public bool DeleteProduct(int id)
        {
            using (HttpClient httpclient = new HttpClient())
            {
                string data = Newtonsoft.Json.JsonConvert.SerializeObject(id);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                string url = Configuration.GetSection("Development")["BaseApi"].ToString() + "api/productop/"
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
