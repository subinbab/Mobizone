using DomainLayer.Users;
using DTOLayer.UserModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using UILayer.Data.ApiServices;

namespace UILayer.Controllers
{
    public class UserController : Controller
    {
        IConfiguration _configuration;
        UserApi userApi;
        ProductOpApi _opApi;
        UserRegistration _user { get; set; }
        public UserController(IConfiguration configuration)
        {
            _configuration = configuration;
            userApi  = new UserApi(_configuration);
            _opApi = new ProductOpApi(_configuration);
        }
        public IActionResult Index()
        {
            var data = _opApi.GetAll().Result;
            return View(data);
        }
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login(string loginUrl)
        {
            
            ViewData["LoginUrl"] = loginUrl;
            return View();
            
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task <IActionResult> Login(LoginViewModel data)
        {
            UserApi userApi = new UserApi(_configuration);
            LoginViewModel user = new LoginViewModel();
            /* user.userName = userName;
             user.password = password;*/
            _user = userApi.GetUserData().ToList().Where(c=> c.Email.Equals(c.Email)).FirstOrDefault();
            user = data;
            bool check = userApi.Authenticate(user);
            if (check)
            {
                var claims = new List<Claim>();
               
                claims.Add(new Claim("password", user.password));
                claims.Add(new Claim(ClaimTypes.NameIdentifier, user.userName));
                claims.Add(new Claim(ClaimTypes.Name, user.userName));
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                await HttpContext.SignInAsync(claimsPrincipal);
                return Redirect("Index");
            }
            TempData["Error"] = "Invalid Email or Password";
            return View("Login");
        }
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/");
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
        public IActionResult checkout(int id)
        {
            var data = _opApi.GetProduct(id).Result;
            ViewData["ProductDetails"] = data;
            ViewData["userData"] = _user;
            return View();
        }
        public IActionResult order()
        {
            return View();
        }
        public IActionResult Account()
        {
            return View();
        }
    }
}
