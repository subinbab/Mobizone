using DomainLayer.ProductModel.Master;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using UILayer.Models;

namespace UILayer.Data.ApiServices
{
    public class MasterApi
    {
        private IConfiguration Configuration { get; }
        public MasterApi(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        #region Add method for master data
        public bool Add(MasterTable masterData)
        {
            using (HttpClient httpclient = new HttpClient())
            {
                string data = Newtonsoft.Json.JsonConvert.SerializeObject(masterData);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                string url = Configuration.GetSection("Development")["BaseApi"].ToString() + "api/Master";
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
        #endregion
        #region Get Method for Master Data
        public IEnumerable<MasterTable> GetAll()
        {
            ResponseModel<IEnumerable<MasterTable>> _responseModel = null;
            using (HttpClient httpclient = new HttpClient())
            {
                _responseModel = null;
                string url = Configuration.GetSection("Development")["BaseApi"].ToString() + "api/master";
                Uri uri = new Uri(url);
                System.Threading.Tasks.Task<HttpResponseMessage> result = httpclient.GetAsync(uri);
                if (result.Result.IsSuccessStatusCode)
                {
                    System.Threading.Tasks.Task<string> response = result.Result.Content.ReadAsStringAsync();
                    _responseModel = Newtonsoft.Json.JsonConvert.DeserializeObject<ResponseModel<IEnumerable<MasterTable>>>(response.Result);
                }
                return _responseModel.result;
            }
        }
        #endregion

    }
}
