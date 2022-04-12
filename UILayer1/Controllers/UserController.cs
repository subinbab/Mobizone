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
        public UserController(IConfiguration configuration)
        {
            _configuration = configuration;
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
            UserApi userApi = new UserApi(_configuration);
            userApi.CreateUser(user);
            return View("Index");
        }
        public IActionResult Contact()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }
        public IActionResult Company()
        {
            return View();
        }
    }
}
