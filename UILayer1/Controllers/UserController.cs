using AspNetCoreHero.ToastNotification.Abstractions;
using DomainLayer;
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
        INotyfService _notyfService;
        public UserController(IConfiguration configuration, INotyfService notyf)
        {
            _configuration = configuration;
            userApi  = new UserApi(_configuration);
            _opApi = new ProductOpApi(_configuration);
            _notyfService = notyf;
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
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }
        public IActionResult Contact()
        {
            adminApi _adminApi = new adminApi(_configuration);
            var contactData = _adminApi.ContactGet().Result.FirstOrDefault();
            return View(contactData);
            
        }
        public IActionResult Privacy()
        {
            adminApi _adminApi = new adminApi(_configuration);
            var privacyData = _adminApi.PrivacyGet().Result.FirstOrDefault();
            return View(privacyData);

            
        }

        public IActionResult About()
        {
            adminApi _adminApi = new adminApi(_configuration);
            var aboutData = _adminApi.AboutGet().Result.FirstOrDefault();
            return View(aboutData);
           
        }
        public IActionResult Company()
        {
            return View();
        }
        [Authorize]
        [HttpGet]
        public IActionResult checkout(int id)
        {
            var data = _opApi.GetProduct(id).Result;
            ViewData["ProductDetails"] = data;
            _user = userApi.GetUserData().Where(c=> c.Email.Equals(User.Identity.Name.ToString())).FirstOrDefault();
            ViewData["userData"] = _user;
            return View();
        }
        [HttpPost]
        public IActionResult checkout(Checkout checkout)
        {
            if(checkout == null)
            {
                _notyfService.Error("Not Added");
                return RedirectToAction("Index");
            }
            else
            {
                Random rnd = new Random();
                checkout.orderId = rnd.Next();
                checkout.status = OrderStatus.orderplaced;
                bool result = userApi.CreateCheckOut(checkout);
                ViewBag.orderId = checkout.orderId;
                _notyfService.Success("succesfully orderd");
                return View("Orderplaced");
            }
            
        }
        public IActionResult Orderplaced()
        {
            return View();
        }
        public IActionResult OrderList()
        {
            return View();
        }
        public IActionResult DetailedOrderPage()
        {
            return View();
        }
        public IActionResult order()
        {
            return View();
        }
        public IActionResult AddtoCart()
        {
            return View();
        }
        public IActionResult CartPage()
        {
            return View();
        }
        public IActionResult Account()
        {
            _user = userApi.GetUserData().Where(c => c.Email.Equals(User.Identity.Name.ToString())).FirstOrDefault();
            ViewData["userData"] = _user;
            return View();
        }
        [HttpGet]
        public IActionResult Address()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Address(Address addreses)
        {
            List<Address> addresses = new List<Address>();
            addresses.Add(addreses);
            _user = userApi.GetUserData().Where(c => c.Email.Equals(User.Identity.Name.ToString())).FirstOrDefault();
            _user.address = addresses; 
            bool result = userApi.EditUser(_user);
            return RedirectToAction("Index");
        }
    }
}
