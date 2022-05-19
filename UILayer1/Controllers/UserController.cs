using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using DocumentFormat.OpenXml.Bibliography;
using DomainLayer;
using DomainLayer.ProductModel.Master;
using DomainLayer.Users;
using DTOLayer.Product;
using DTOLayer.UserModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using UILayer.Data.ApiServices;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

namespace UILayer.Controllers
{
    
    public class UserController : Controller
    {
        IConfiguration _configuration;
        UserApi userApi;
        ProductOpApi _opApi;
        private readonly INotyfService _notyf;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        MasterApi _masterApi;
        List<Cart> _carts;
        UserRegistration _user { get; set; }
       


        INotyfService _notyfService;

        public UserController(IConfiguration configuration, INotyfService notyf, IMapper mapper, IWebHostEnvironment webHostEnvironment)

        {
            _configuration = configuration;
            userApi  = new UserApi(_configuration);
            _opApi = new ProductOpApi(_configuration,mapper, webHostEnvironment);

            _masterApi = new MasterApi(_configuration);
            _notyf = notyf;
            _mapper = mapper;
            _carts = new List<Cart>();
           // HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(_carts));



        }
        public IActionResult Index(int? count )
        {
            try
            {
                if(count == null)
                {
                    count = 0;
                }
                ViewBag.BrandList = _masterApi.GetList((int)Master.Brand);
                var data = _opApi.GetAll().Result.Where(c=>c.status.Equals(ProductStatus.enable));
                var productCount = data.Count();
                int cout = 0;
                for(int i = 0; i <= 0; i++)
                {
                    if (productCount > 10)
                    {
                        cout += 1;
                    }
                    productCount = productCount - 10;
                }
                var result = data.Skip((int)count * 10).Take(10);
                ViewBag.count = cout;
                if(data != null)
                {
                    return View(result);
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
        /*[AllowAnonymous]
        [HttpPost]
        public async Task <IActionResult> Login(LoginViewModel data)
        {
            UserApi userApi = new UserApi(_configuration);
            LoginViewModel user = new LoginViewModel();
            *//* user.userName = userName;
             user.password = password;*//*
            _user = userApi.GetUserData().ToList().Where(c=> c.Email.Equals(c.Email)).FirstOrDefault();
            user = data;
            bool check = userApi.Authenticate(user);
            if (check)
            {
                var claims = new List<Claim>();
               
                claims.Add(new Claim("password", user.password));
                claims.Add(new Claim(ClaimTypes.NameIdentifier, user.username));
                claims.Add(new Claim(ClaimTypes.Name, user.username));
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                await HttpContext.SignInAsync(claimsPrincipal);
                return Redirect("Index");
            }
            ViewBag.BrandList = _masterApi.GetList((int)Master.Brand);
            TempData["Error"] = "Invalid Email or Password";
            return View("Login");
        }*/
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
        public async Task<IActionResult> Registration(UserViewModel user)
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

            adminApi _adminApi = new adminApi(_configuration,_mapper);
            var contactData = _adminApi.ContactGet().Result.FirstOrDefault();
            ViewBag.BrandList = _masterApi.GetList((int)Master.Brand);
            return View(contactData);
            
        }
        public IActionResult Privacy()
        {
            adminApi _adminApi = new adminApi(_configuration,_mapper);
            var privacyData = _adminApi.PrivacyGet().Result.FirstOrDefault();
            ViewBag.BrandList = _masterApi.GetList((int)Master.Brand);
            return View(privacyData);

            

        }

        public IActionResult About()
        {

            adminApi _adminApi = new adminApi(_configuration,_mapper);
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
        [HttpGet]
        public IActionResult checkout(int ordereId , string status)
        {
            return View("Orderplaced");
        }
        [Authorize(Roles ="User")]
        [HttpGet]
        public IActionResult order(int id)
        {
            var data = _opApi.GetProduct(id).Result;
            ViewData["ProductDetails"] = data;
            _user = userApi.GetUserData().Where(c=> c.Email.Equals(User.Claims?.FirstOrDefault(x => x.Type.Equals("email", StringComparison.OrdinalIgnoreCase))?.Value)).FirstOrDefault();
            ViewData["userData"] = _user;
            ViewBag.BrandList = _masterApi.GetList((int)Master.Brand);
            return View();
        }
        [HttpPost]
        public IActionResult order(Checkout checkout)
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
                data.quantity = data.quantity - checkout.quantity;
                if(data.quantity == 0)
                {
                    data.status = ProductStatus.disable;
                }
                var mappedData = (ProductViewModel)_mapper.Map<ProductViewModel>(data);
                _opApi.EditProduct(mappedData);
                Random rnd = new Random();
                checkout.orderId = rnd.Next();
                checkout.status = OrderStatus.orderplaced;
                checkout.price = checkout.quantity * data.price;
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
        public IActionResult AddtoCart(int id)
        {
            /*Cart cart = new Cart();*/
            //cart.productId = id;
            /* _carts = JsonConvert.DeserializeObject<List<Cart>>(HttpContext.Session.GetString("cart"));
             _carts.Add(cart);
             HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(_carts));
             ViewBag.BrandList = _masterApi.GetList((int)Master.Brand);*/
            var cartListFromDb = userApi.GetCart().Result;
            
            CartDetails cartDetails = new CartDetails();
            cartDetails.productId = id;
            List<CartDetails> cartList = new List<CartDetails>();

            cartList.Add(cartDetails);
            Cart cart = new Cart();

            if (User.Identity.IsAuthenticated)
            {
                var userData = userApi.GetUserData().Where(c => c.Email.Equals(User.Claims?.FirstOrDefault(x => x.Type.Equals("Email", StringComparison.OrdinalIgnoreCase))?.Value)).FirstOrDefault();
                cart.userId = userData;
            }


            cart.cartDetails = cartList;
            HttpContext.Session.SetString("testKey","testValue");
            cart.sessionId = HttpContext.Session.Id;
            if (cartListFromDb.Any(c => c.sessionId.Equals(HttpContext.Session.Id)))
            {
                var existedCart = cartListFromDb.Where(c => c.sessionId.Equals(cart.sessionId)).FirstOrDefault();
                //cartDetails.productId = id;
                existedCart.cartDetails.Add(cartDetails);
                userApi.EditCart(existedCart);
            }
            else
            {
                var result = userApi.Createcart(cart);
            }
            return Redirect("/user/index");

        }
        public IActionResult CartPage()
        {
            ViewBag.BrandList = _masterApi.GetList((int)Master.Brand);
            return View();
        }
        public IActionResult Account()
        {
            ViewBag.BrandList = _masterApi.GetList((int)Master.Brand);
            _user = userApi.GetUserData().Where(c => c.Email.Equals(User.Claims?.FirstOrDefault(x => x.Type.Equals("Email", StringComparison.OrdinalIgnoreCase))?.Value)).FirstOrDefault();
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
            ViewBag.count = 0;
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
       /* public IActionResult CartDetails()
        {

            return Json();
        }*/
       [HttpPost]
       public IActionResult Search(string name)
        {
            ViewBag.count = 0;
            var data = _opApi.Search(name).Result;
            return View("Index", data);
        }
    }
}
