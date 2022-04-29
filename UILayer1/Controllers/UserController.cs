using AspNetCoreHero.ToastNotification.Abstractions;


using DomainLayer;
using DomainLayer.ProductModel.Master;
using DomainLayer.Users;
using DTOLayer.UserModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
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
        private readonly INotyfService _notyf;
        private readonly IWebHostEnvironment _webHostEnvironment;
        MasterApi _masterApi;
        UserRegistration _user { get; set; }



        INotyfService _notyfService;

        public UserController(IConfiguration configuration, INotyfService notyf)

        {
            _configuration = configuration;
            userApi  = new UserApi(_configuration);
            _opApi = new ProductOpApi(_configuration);

            _masterApi = new MasterApi(_configuration);
            _notyf = notyf;




        }
        public IActionResult Index()
        {
            try
            {
                ViewBag.BrandList = _masterApi.GetList((int)Master.Brand);
                var data = _opApi.GetAll().Result;
                if(data != null)
                {
                    return View(data);
                }
                else
                {
                    return View(null);
                }
                
            }
            catch(Exception ex)
            {
                return View(null);
            }
            
           
        }
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login(string loginUrl)
        {
            ViewBag.BrandList = _masterApi.GetList((int)Master.Brand);
            ViewData["LoginUrl"] = loginUrl;
            ViewBag.BrandList = _masterApi.GetList((int)Master.Brand);
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
            ViewBag.BrandList = _masterApi.GetList((int)Master.Brand);
            TempData["Error"] = "Invalid Email or Password";
            return View("Login");
        }
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            ViewBag.BrandList = _masterApi.GetList((int)Master.Brand);
            return Redirect("/");
        }
        
        [HttpGet("registration")]
        public IActionResult Registration()
        
        {
            ViewBag.BrandList = _masterApi.GetList((int)Master.Brand);
            return View();
        }
        [HttpPost("registration")]
        public IActionResult Registration(UserViewModel user)
        {
            UserApi userApi = new UserApi(_configuration);
            var userList = userApi.GetUserData();
            if(userList.Any(c=> c.Email.Equals(user.Email)))
            {
                _notyf.Error("User Already Registered");
            }
            else
            {
                bool result = userApi.CreateUser(user);
                if (result)
                {
                    _notyf.Success("Successfully Registered new user");
                }
                else
                {
                    _notyf.Error("UserAddedError");

                }
            }
            
            ViewBag.BrandList = _masterApi.GetList((int)Master.Brand);
            userApi.CreateUser(user);
            return Redirect("/");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            ViewBag.BrandList = _masterApi.GetList((int)Master.Brand);
            return View();
        }
        public IActionResult Contact()
        {

            adminApi _adminApi = new adminApi(_configuration);
            var contactData = _adminApi.ContactGet().Result.FirstOrDefault();
            ViewBag.BrandList = _masterApi.GetList((int)Master.Brand);
            return View(contactData);
            
        }
        public IActionResult Privacy()
        {
            adminApi _adminApi = new adminApi(_configuration);
            var privacyData = _adminApi.PrivacyGet().Result.FirstOrDefault();
            ViewBag.BrandList = _masterApi.GetList((int)Master.Brand);
            return View(privacyData);

            

        }

        public IActionResult About()
        {

            adminApi _adminApi = new adminApi(_configuration);
            var aboutData = _adminApi.AboutGet().Result.FirstOrDefault();
            ViewBag.BrandList = _masterApi.GetList((int)Master.Brand);
            return View(aboutData);
           

        }
        public IActionResult Company()
        {
            ViewBag.BrandList = _masterApi.GetList((int)Master.Brand);
            ViewBag.BrandList = _masterApi.GetList((int)Master.Brand);
            return View();
        }
        [Authorize(Roles ="User")]
        [HttpGet]
        public IActionResult checkout(int id)
        {
            var data = _opApi.GetProduct(id).Result;
            ViewData["ProductDetails"] = data;
            _user = userApi.GetUserData().Where(c=> c.Email.Equals(User.Identity.Name.ToString())).FirstOrDefault();
            ViewData["userData"] = _user;
            ViewBag.BrandList = _masterApi.GetList((int)Master.Brand);
            return View();
        }
        [HttpPost]
        public IActionResult checkout(Checkout checkout)
        {
            if(checkout == null)
            {
                _notyf.Error("Not Added");
                ViewBag.BrandList = _masterApi.GetList((int)Master.Brand);
                return RedirectToAction("Index");
            }
            else
            {
                var data = _opApi.GetProduct(checkout.productId).Result;
                Random rnd = new Random();
                checkout.orderId = rnd.Next();
                checkout.status = OrderStatus.orderplaced;
                checkout.price = checkout.quatity * data.price;
                bool result = userApi.CreateCheckOut(checkout);
                ViewBag.orderId = checkout.orderId;
                _notyf.Success("succesfully orderd");
                ViewBag.BrandList = _masterApi.GetList((int)Master.Brand);
                return View("Orderplaced");
            }
            
        }
        [Authorize(Roles = "User")]
        public IActionResult Orderplaced()
        {
            ViewBag.BrandList = _masterApi.GetList((int)Master.Brand);
            return View();
        }
        public IActionResult OrderList()
        {
            ViewBag.BrandList = _masterApi.GetList((int)Master.Brand);
            return View();
        }
        public IActionResult DetailedOrderPage()
        {
            ViewBag.BrandList = _masterApi.GetList((int)Master.Brand);
            return View();
        }
        public IActionResult order()
        {
            ViewBag.BrandList = _masterApi.GetList((int)Master.Brand);
            return View();
        }
        public IActionResult AddtoCart()
        {
            ViewBag.BrandList = _masterApi.GetList((int)Master.Brand);
            return View();
        }
        public IActionResult CartPage()
        {
            ViewBag.BrandList = _masterApi.GetList((int)Master.Brand);
            return View();
        }
        public IActionResult Account()
        {
            ViewBag.BrandList = _masterApi.GetList((int)Master.Brand);
            _user = userApi.GetUserData().Where(c => c.Email.Equals(User.Identity.Name.ToString())).FirstOrDefault();
            ViewData["userData"] = _user;
            return View();
        }
        [HttpGet]
        public IActionResult Address()
        {
            ViewBag.BrandList = _masterApi.GetList((int)Master.Brand);
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
            ViewBag.BrandList = _masterApi.GetList((int)Master.Brand);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult filter(string brandName)
        {
            ViewBag.BrandList = _masterApi.GetList((int)Master.Brand);
            var filteredData = _opApi.Filter(brandName).Result;
            return View("Index", filteredData);
        }
        [HttpGet]
        public async Task<IActionResult> ProductDetails(int id)
        {
           var details = await _opApi.GetProduct(id); details = await _opApi.GetProduct(id);
            ViewBag.BrandList = _masterApi.GetList((int)Master.Brand);
            return View(details);
        }
    }
}
