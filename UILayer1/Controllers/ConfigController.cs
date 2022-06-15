using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using UIlayer.Data.ApiServices;
using UILayer.Data.ApiServices;

namespace UILayer.Controllers
{
    public class ConfigController : Controller
    {
        IConfiguration _config;
        IProductOpApi _productOpApi;
        public ConfigController(IConfiguration config, IProductOpApi productOpApi)
        {
            _config = config;
            _productOpApi = productOpApi;
        }
        public JsonResult GetApi()
        {
            var data = _config.GetSection("Development:BaseApi").Value;
            return new JsonResult(data);
        }
        public JsonResult GetProducts()
        {
            var data = _productOpApi.GetAll().Result;
            return new JsonResult(data);
        }
        public JsonResult GetHeader()
        {
            var header = Base64Encode(_config.GetSection("AuthenticationCredentials:username").Value + ":" + _config.GetSection("AuthenticationCredentials:password").Value);
            return new JsonResult(header);
        }
        public IActionResult Test()
{
            var data = _productOpApi.GetAll().Result.FirstOrDefault();
            return View(data);
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
    }

}
