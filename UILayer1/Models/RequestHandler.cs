using DocumentFormat.OpenXml.Spreadsheet;
using DomainLayer;
using log4net;
using Microsoft.Extensions.Configuration;
using System;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace UILayer.Models
{
    public class RequestHandler<T>
    {
        private readonly ILog _log;
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
            _log = LogManager.GetLogger(typeof(RequestHandler<T>));
        }

        #region get generic method
        public ResponseModel<T> Get()
        {
            ResponseModel<T> _responseModel = new ResponseModel<T>();
            _responseModel = null;
            try
            {
                using (HttpClient httpclient = new HttpClient())
                {
                    string url = _configuration.GetSection("Development")["BaseApi"].ToString() + _url;
                    Uri uri = new Uri(url);
                    httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", "hello");
                    System.Threading.Tasks.Task<HttpResponseMessage> result = httpclient.GetAsync(uri);
                    if (result.Result.IsSuccessStatusCode)
                    {
                        System.Threading.Tasks.Task<string> response = result.Result.Content.ReadAsStringAsync();
                        _responseModel = Newtonsoft.Json.JsonConvert.DeserializeObject<ResponseModel<T>>(response.Result);
                    }
                    return _responseModel;
                }
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
                return default(ResponseModel<T>);

            }

        }
        #endregion

        #region Edit Generic method
        public ResponseModel<T> Edit(T entity)
        {
            ResponseModel<T> _responseModel = new ResponseModel<T>();
            try
            {
                using (HttpClient httpclient = new HttpClient())
                {
                    string data = Newtonsoft.Json.JsonConvert.SerializeObject(entity);
                    StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                    string url = _configuration.GetSection("Development")["BaseApi"].ToString() + _url;
                    Uri uri = new Uri(url);
                    System.Threading.Tasks.Task<HttpResponseMessage> result = httpclient.PutAsync(uri, content);

                    if (result.Result.IsSuccessStatusCode)
                    {
                        System.Threading.Tasks.Task<string> response = result.Result.Content.ReadAsStringAsync();
                        _responseModel = Newtonsoft.Json.JsonConvert.DeserializeObject<ResponseModel<T>>(response.Result);
                        return _responseModel;
                    }
                    return default(ResponseModel<T>);
                }
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
                return default(ResponseModel<T>);
            }
            
        }
        #endregion

        #region Post Generic Method
        public ResponseModel<T> Post(T entity)
        {
            ResponseModel<T> _responseModel = new ResponseModel<T>();
            try
            {
                using (HttpClient httpclient = new HttpClient())
                {
                    try
                    {
                        string data = Newtonsoft.Json.JsonConvert.SerializeObject(entity);
                        StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                        string url = _configuration.GetSection("Development")["BaseApi"].ToString() + _url;
                        Uri uri = new Uri(url);
                        httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", " c3ViaW5AZ21haWwuY29tOmN4djJzQ0tPaFA0YmFTblhzMlY2Ymc9PQ==");
                        System.Threading.Tasks.Task<HttpResponseMessage> result = httpclient.PostAsync(uri, content);
                        if (result.Result.IsSuccessStatusCode)
                        {

                            System.Threading.Tasks.Task<string> response = result.Result.Content.ReadAsStringAsync();
                            _responseModel = Newtonsoft.Json.JsonConvert.DeserializeObject<ResponseModel<T>>(response.Result);
                            return _responseModel;
                        }
                        return default(ResponseModel<T>);
                    }
                    catch(Exception ex)
                    {
                        return default(ResponseModel<T>);
                    }

                }
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
                return default(ResponseModel<T>);
            }

        }
        #endregion

        #region delete generic method
        public ResponseModel<T> Delete(int id) {
            ResponseModel<T> _responseModel = new ResponseModel<T>();
            try
            {
                using (HttpClient httpclient = new HttpClient())
                {
                    string data = Newtonsoft.Json.JsonConvert.SerializeObject(id);
                    StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                    string url = _configuration.GetSection("Development")["BaseApi"].ToString() + _url +
                    +id;
                    Uri uri = new Uri(url);
                    System.Threading.Tasks.Task<HttpResponseMessage> result = httpclient.DeleteAsync(uri);
                    if (result.Result.IsSuccessStatusCode)
                    {
                        System.Threading.Tasks.Task<string> response = result.Result.Content.ReadAsStringAsync();
                        _responseModel = Newtonsoft.Json.JsonConvert.DeserializeObject<ResponseModel<T>>(response.Result);
                        return _responseModel;
                    }
                    return default(ResponseModel<T>);
                }
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
                return default(ResponseModel<T>);
            }

        }
        #endregion
    }
}
