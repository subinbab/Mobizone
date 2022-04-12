using DTOLayer.UserModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UILayer.Data.ApiServices;

namespace UILayer.Controllers
{
    public class UserController : Controller
    {
        IConfiguration _configuration;
        UserApi userApi;
        public UserController(IConfiguration configuration)
        {
            _configuration = configuration;
            userApi  = new UserApi(_configuration);
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Registration()
        
        {
            return View();
        }
        [HttpPost]
        public IActionResult Registration(UserViewModel user)
        {
             
            userApi.CreateUser(user);
            return View("Index");
        }
    }
}
