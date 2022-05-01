// this class is not using
using DomainLayer;
using DomainLayer.Users;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UILayer.Models;

namespace UIlayer.Data.ApiServices
{
    public class ProductApi
    {
        private  IConfiguration Configuration { get; }
        public ProductApi(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IEnumerable<Product> GetProduct()
        {
            RequestHandler<IEnumerable<Product>> _requestHandler = new RequestHandler<IEnumerable<Product>>(Configuration);
            _requestHandler.url = "api/product";
            return _requestHandler.Get().result;

        }
        public  Product GetProduct(int id)
        {
            RequestHandler<Product> _requestHandler = new RequestHandler<Product>(Configuration);
            _requestHandler.url = "api/product";
            return _requestHandler.Get().result;
        }
        public  bool EditProduct(Product product)
        {

            using (HttpClient httpclient = new HttpClient())
            {
                string data = Newtonsoft.Json.JsonConvert.SerializeObject(product);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                string url = Configuration.GetSection("Development")["BaseApi"].ToString() + "api/product";
                Uri uri = new Uri(url);
                System.Threading.Tasks.Task<HttpResponseMessage> response = httpclient.PutAsync(uri, content);
                
                if (response.Result.IsSuccessStatusCode)
                {
                    return true;
                }
                return false;
            }
        }
        public bool CreateProduct(Product product)
        {
            using (HttpClient httpclient = new HttpClient())
            {
                string data = Newtonsoft.Json.JsonConvert.SerializeObject(product);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                string url = Configuration.GetSection("Development")["BaseApi"].ToString() + "api/product";
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
        public  bool DeleteProduct(int id)
        {
            using (HttpClient httpclient = new HttpClient())
            {
                string data = Newtonsoft.Json.JsonConvert.SerializeObject(id);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                string url = Configuration.GetSection("Development")["BaseApi"].ToString() + "api/product/"
                +id;
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
