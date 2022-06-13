using log4net;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace UILayer.Models
{
    public class RequestHandler<T> : IRequestHandler<T>
    {
        string enocodedData;
        private readonly ILog _log;
        private string _url;
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
            enocodedData = Base64Encode(_configuration.GetSection("AuthenticationCredentials:username").Value + ":" + _configuration.GetSection("AuthenticationCredentials:password").Value);
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
                    httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Authorization",enocodedData);
                    System.Threading.Tasks.Task<HttpResponseMessage> result = httpclient.GetAsync(uri);
                    if (result.Result.IsSuccessStatusCode)
                    {
                        System.Threading.Tasks.Task<string> response = result.Result.Content.ReadAsStringAsync();
                        _responseModel = Newtonsoft.Json.JsonConvert.DeserializeObject<ResponseModel<T>>(response.Result);
                        if (_responseModel == null)
                        {
                            return null;
                        }
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
                    httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Authorization", enocodedData);
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
                        httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Authorization", enocodedData);
                        System.Threading.Tasks.Task<HttpResponseMessage> result = httpclient.PostAsync(uri, content);
                        if (result.Result.IsSuccessStatusCode)
                        {

                            System.Threading.Tasks.Task<string> response = result.Result.Content.ReadAsStringAsync();
                            _responseModel = Newtonsoft.Json.JsonConvert.DeserializeObject<ResponseModel<T>>(response.Result);
                            return _responseModel;
                        }
                        return default(ResponseModel<T>);
                    }
                    catch (Exception ex)
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
        public ResponseModel<T> Delete(int id)
        {
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
                    httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Authorization", enocodedData);
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

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
}
