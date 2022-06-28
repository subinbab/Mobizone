using DomainLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
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
        IUserApi _userApi;
        IConfiguration _config;
        IProductOpApi _productOpApi;
        List<MyCart> _carts;
        private readonly IDistributedCache _distributedCache;
        public ConfigController(IConfiguration config, IProductOpApi productOpApi, IUserApi userApi, IDistributedCache distributedCache)
        {
            _config = config;
            _productOpApi = productOpApi;
            _userApi = userApi;
            _distributedCache = distributedCache;
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
        public JsonResult CartCount()
        {
            int count = 0;
            if (User.Identity.IsAuthenticated)
            {
                if(User.Claims?.FirstOrDefault(x => x.Type.Equals("email", StringComparison.OrdinalIgnoreCase))?.Value != "admin@gmail.com")
                {
                    var user = _userApi.GetUserData().Where(c => c.Email.Equals(User.Claims?.FirstOrDefault(x => x.Type.Equals("email", StringComparison.OrdinalIgnoreCase))?.Value)).FirstOrDefault();
                    if (_userApi.GetCart().Result.Where(c => c.usersId.Equals(user.UserId)) != null && _userApi.GetCart().Result.Where(c => c.usersId.Equals(user.UserId)).Count() > 0)
                    {
                        count = _userApi.GetCart().Result.Where(c => c.usersId.Equals(user.UserId)).FirstOrDefault().cartDetails.Count();
                    }
                }
                }
            
            else
            {

                string name = _distributedCache.GetStringAsync("cart").Result;
                if(name != null)
                {
                    if (JsonConvert.DeserializeObject<List<MyCart>>(name) != null)
                    {
                        _carts = JsonConvert.DeserializeObject<List<MyCart>>(name);
                    }
                    if(_carts != null)
                    {
                        count = _carts.Count();
                    }
                    
                }
            }
                
            return new JsonResult(count);
        }
        public PartialViewResult unauthorized()
        {
            return PartialView("Unauthorized");
;        }
        [HttpGet("/config/GetProduct/{id}")]
        public JsonResult GetProduct(int id)
        {
            var data = _productOpApi.GetProduct(id).Result;
            return new JsonResult(data);
        }
    }

}
