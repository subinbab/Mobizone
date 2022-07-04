﻿using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using DocumentFormat.OpenXml.Wordprocessing;
using DomainLayer;
using DomainLayer.ProductModel;
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
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UILayer.Data.ApiServices;

namespace UILayer.Controllers
{

    public class UserController : Controller
    {
        IConfiguration _configuration;
        ProductOpApi _opApi;
        private readonly INotyfService _notyf;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        MasterApi _masterApi;
        List<MyCart> _carts = null;
        IUserApi _userApi; 
        UserRegistration _user { get; set; }
        private readonly IDistributedCache _distributedCache;
        public UserController(IConfiguration configuration, INotyfService notyf, IMapper mapper, IWebHostEnvironment webHostEnvironment, IDistributedCache distributedCache, IUserApi userApi)

        {
            _configuration = configuration;
            _userApi = userApi;
            _opApi = new ProductOpApi(_configuration, mapper, webHostEnvironment);

            _masterApi = new MasterApi(_configuration);
            _notyf = notyf;
            _mapper = mapper;
            _distributedCache = distributedCache;
            _carts = new List<MyCart>();
            // HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(_carts));
            ViewBag.WebLink = _configuration.GetSection("Development:WebLink").Value;
            


        }
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Index(int? count)
        {

            ViewBag.Title = "Mobizone-Home";
            try
            {
                if (count == null)
                {
                    count = 0;
                }
                ViewBag.BrandList = _masterApi.GetList((int)Master.Brand);
                var data = _opApi.GetAll().Result.Where(c => c.status.Equals(ProductStatus.enable));
                var productCount = data.Count();
                int cout = 0;
                for (int i = 0; i <= 0; i++)
                {
                    if (productCount > 12)
                    {
                        cout += 1;
                    }
                    productCount = productCount - 12;
                }
                var result = data.Skip((int)count * 12).Take(12);
                ViewBag.count = cout;
                if (data != null)
                {
                    return View(result);
                }
                else
                {
                    return View(null);
                }

            }
            catch (Exception ex)
            {
                return View(null);
            }


        }
        public PartialViewResult IndexPartial(int? count)
        {
            if (count == null)
            {
                count = 0;
            }
            ViewBag.BrandList = _masterApi.GetList((int)Master.Brand);
            var data = _opApi.GetAll().Result.Where(c => c.status.Equals(ProductStatus.enable));
            var productCount = data.Count();
            int cout = 0;
            for (int i = 0; i <= 0; i++)
            {
                if (productCount > 12)
                {
                    cout += 1;
                }
                productCount = productCount - 12;
            }
            var result = data.Skip((int)count * 12).Take(12);
            ViewBag.count = cout;
            return PartialView("PartialViews/_IndexPartialView", result);
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
        public async Task<IActionResult> Registration(UserViewModel user)
        {
            UserApi userApi = new UserApi(_configuration);
            var username = User.Claims?.FirstOrDefault(x => x.Type.Equals("email", StringComparison.OrdinalIgnoreCase))?.Value;
            var password = User.Claims?.FirstOrDefault(x => x.Type.Equals("password", StringComparison.OrdinalIgnoreCase))?.Value;
            var userList = userApi.GetUserData();
            if (userList.Any(c => c.Email.Equals(user.Email)))
            {
                _notyf.Error("User Already Registered");
                ViewBag.BrandList = _masterApi.GetList((int)Master.Brand);
                return RedirectToAction("Registration");
            }

            else
            {
                bool result = userApi.CreateUser(user);
                if (result)
                {
                    _notyf.Success("Successfully Registered new user");
                    var userDataList = userApi.GetUserData();
                    var claims = new List<Claim>();
                    claims.Add(new Claim(ClaimTypes.Name, userDataList.Where(c => c.Email.Equals(user.Email)).FirstOrDefault().FirstName + " " + userDataList.Where(c => c.Email.Equals(user.Email)).FirstOrDefault().LastName));
                    claims.Add(new Claim("email", user.Email));
                    claims.Add(new Claim(ClaimTypes.NameIdentifier, userDataList.Where(c => c.Email.Equals(user.Email)).FirstOrDefault().FirstName + " " + userDataList.Where(c => c.Email.Equals(user.Email)).FirstOrDefault().LastName));
                    claims.Add(new Claim("password", user.Password));
                    claims.Add(new Claim(ClaimTypes.Role, "User"));
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                    await HttpContext.SignInAsync(claimsPrincipal);
                    return Redirect("/");
                }
                else
                {
                    _notyf.Error("UserAddedError");
                    return RedirectToAction("Registration");
                }
            }

        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            ForgetPasswordViewModel email = new ForgetPasswordViewModel();
            email.emailSent = false;
            ViewBag.BrandList = _masterApi.GetList((int)Master.Brand);
            ViewBag.check = false;
            return View(email);
        }
        [HttpPost]
        [AllowAnonymous]
        public IActionResult ForgotPassword(ForgetPasswordViewModel data)
        {
            var username = User.Claims?.FirstOrDefault(x => x.Type.Equals("email", StringComparison.OrdinalIgnoreCase))?.Value;
            var password = User.Claims?.FirstOrDefault(x => x.Type.Equals("password", StringComparison.OrdinalIgnoreCase))?.Value;
            if (ModelState.IsValid)
            {
                HttpContext.Session.SetString("Email", "hello");
                var session = HttpContext.Session.Id;

                ModelState.Clear();
                var userDetails = _userApi.GetUserData().Where(check => check.Email.Equals(data.email)).FirstOrDefault();
                if (userDetails != null)
                {
                    var webLink = _configuration.GetSection("Development:WebLink").Value;
                    data.emailSent = true;
                    MailRequest mailRequest = new MailRequest();
            mailRequest.Body = "<h2 style='text-align:center'>MOBIZONE</h2><div><p>Hi, </p><p>Please click here to Reset Your Password</p><a style='padding: 2px; background - color:#c81913;text - decoration: none;border - radius: 2px;color: white;' href='"+ webLink + "/user/ResetPassword/" + data.email + "/" + session + "'>Click Here</a></div><style></style>";
                    mailRequest.Subject = "ResetPassword";
                    mailRequest.ToEmail = userDetails.Email;
                    var checkEmail = _userApi.PostMail(mailRequest);
                    ViewBag.check = false;
                    return View(data);
                }
                else
                {
                    ViewBag.check = true;
                }
               

            }
            ViewBag.BrandList = _masterApi.GetList((int)Master.Brand);
            return View(data);
        }

        [HttpGet("/user/ResetPassword/{email}/{sessionId}")]
        public ActionResult ResetPassword(string email, string sessionId)
        {
            var username = User.Claims?.FirstOrDefault(x => x.Type.Equals("email", StringComparison.OrdinalIgnoreCase))?.Value;
            var password = User.Claims?.FirstOrDefault(x => x.Type.Equals("password", StringComparison.OrdinalIgnoreCase))?.Value;
            if (sessionId == HttpContext.Session.Id)
            {
                var userDetails = _userApi.GetUserData().Where(check => check.Email.Equals(email)).FirstOrDefault();
                ResetPassword reset = new ResetPassword();
                reset.User = userDetails;
                return View(reset);
            }
            else
            {
                return RedirectToAction("ForgotPassword");
            }
        }

        [HttpPost]
        public ActionResult ResetPassword(ResetPassword resetPassword)
        {
            var username = User.Claims?.FirstOrDefault(x => x.Type.Equals("email", StringComparison.OrdinalIgnoreCase))?.Value;
            var password = User.Claims?.FirstOrDefault(x => x.Type.Equals("password", StringComparison.OrdinalIgnoreCase))?.Value;
            try
            {
                UserRegistration register = new UserRegistration();
                register = _userApi.GetUserData().Where(c => c.Email.Equals(resetPassword.User.Email)).FirstOrDefault();
                Security _sec = new Security();
                register.Password = _sec.Encrypt("admin",resetPassword.newPassword);
                var result = _userApi.EditUser(register);
                return Redirect("/Login");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        public IActionResult Contact()
        {

            adminApi _adminApi = new adminApi(_configuration, _mapper);
            var contactData = _adminApi.ContactGet().Result.FirstOrDefault();
            ViewBag.BrandList = _masterApi.GetList((int)Master.Brand);
            return View(contactData);

        }
        public IActionResult Privacy()
        {
            adminApi _adminApi = new adminApi(_configuration, _mapper);
            var privacyData = _adminApi.PrivacyGet().Result.FirstOrDefault();
            ViewBag.BrandList = _masterApi.GetList((int)Master.Brand);
            return View(privacyData);



        }

        public IActionResult About()
        {

            adminApi _adminApi = new adminApi(_configuration, _mapper);
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
        public IActionResult checkout(int orderId, string status)
        {
            return View("Orderplaced");
        }
        [Authorize(Roles = "User")]
        [HttpGet("/user/order/{id}")]
        public ActionResult order(int id)
        {
            ViewBag.ReturnUrl = "/user/order/" + id;
            var data = _opApi.GetProduct(id).Result;
            ViewData["ProductDetails"] = data;
            var username = User.Claims?.FirstOrDefault(x => x.Type.Equals("email", StringComparison.OrdinalIgnoreCase))?.Value;
            var password = User.Claims?.FirstOrDefault(x => x.Type.Equals("password", StringComparison.OrdinalIgnoreCase))?.Value;
            _user = _userApi.GetUserData().Where(c => c.Email.Equals(User.Claims?.FirstOrDefault(x => x.Type.Equals("email", StringComparison.OrdinalIgnoreCase))?.Value)).FirstOrDefault();
            ViewData["userData"] = _user;
            ViewBag.BrandList = _masterApi.GetList((int)Master.Brand);
            return View("order");
        }
        [Authorize(Roles = "User")]
        [HttpGet]
        public IActionResult CartOrder(List<ProductEntity> productList)
        {
            ViewBag.ReturnUrl = "/user/CartOrder/";
            var username = User.Claims?.FirstOrDefault(x => x.Type.Equals("email", StringComparison.OrdinalIgnoreCase))?.Value;
            var password = User.Claims?.FirstOrDefault(x => x.Type.Equals("password", StringComparison.OrdinalIgnoreCase))?.Value;
            List<ProductEntity> productListForOrder = new List<ProductEntity>();
            foreach (ProductEntity product in productList)
            {
                productListForOrder.Add(_opApi.GetProduct(product.id).Result);
            }
            var data = productListForOrder;
            ViewData["ProductDetails"] = data;
            _user = _userApi.GetUserData().Where(c => c.Email.Equals(User.Claims?.FirstOrDefault(x => x.Type.Equals("email", StringComparison.OrdinalIgnoreCase))?.Value)).FirstOrDefault();
            ViewData["userData"] = _user;
            ViewBag.BrandList = _masterApi.GetList((int)Master.Brand);
            return View();
        }
        [HttpPost("/user/order")]
        public PartialViewResult order(Checkout checkout,int addressId)
        {
            if (checkout == null)
            {
                _notyf.Error("Not Added");
                ViewBag.BrandList = _masterApi.GetList((int)Master.Brand);
                return PartialView("Orderplaced");
            }
            else
            {
                var data = _opApi.GetProduct(checkout.productId).Result;
                data.quantity = data.quantity - checkout.quantity;
                if (data.quantity == 0)
                {
                    data.status = ProductStatus.disable;
                }
                foreach(var address in checkout.addressList)
                {
                    if (address.IsChecked)
                    {
                        checkout.address = address;
                        checkout.addressId = address.id;
                    }
                }
                var mappedData = (ProductViewModel)_mapper.Map<ProductViewModel>(data);
                _opApi.EditProduct(mappedData);
                Random rnd = new Random();
                checkout.orderId = rnd.Next();
                checkout.status = OrderStatus.orderplaced;
                checkout.price = checkout.quantity * data.price;
                bool result = _userApi.CreateCheckOut(checkout);
                ViewBag.orderId = checkout.orderId;
                _notyf.Success("successfully ordered");
                ViewBag.BrandList = _masterApi.GetList((int)Master.Brand);
                return PartialView("Orderplaced");
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
        
        [HttpGet]
        public IActionResult AddtoCart()
        {
            var username = User.Claims?.FirstOrDefault(x => x.Type.Equals("email", StringComparison.OrdinalIgnoreCase))?.Value;
            var password = User.Claims?.FirstOrDefault(x => x.Type.Equals("password", StringComparison.OrdinalIgnoreCase))?.Value;
            List<CartDetails> cartList = new List<CartDetails>();
            try
            {
                
                if (User.Identity.IsAuthenticated && User.Claims?.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.OrdinalIgnoreCase))?.Value != "Admin") 
                {
                    var user = _userApi.GetUserData().Where(c => c.Email.Equals(User.Claims?.FirstOrDefault(x => x.Type.Equals("email", StringComparison.OrdinalIgnoreCase))?.Value)).FirstOrDefault();
                    try
                    {
                        string name = JsonConvert.SerializeObject(_userApi.GetCart().Result);
                        if (JsonConvert.DeserializeObject<List<MyCart>>(name) != null)
                        {
                            _carts = JsonConvert.DeserializeObject<List<MyCart>>(name);
                        }

                    }
                    catch (Exception ex)
                    {
                    }
                    if (_carts.ToList().Where(c => c.usersId.Equals(user.UserId)).FirstOrDefault() != null)
                    {
                        var data = _carts.ToList().Where(c => c.usersId.Equals(user.UserId)).FirstOrDefault();

                        var count = 0;
                        foreach (var item in data.cartDetails)
                        {
                            cartList.Add(item);

                        }
                    }
                    foreach (var data in cartList)
                    {
                        var product = _opApi.GetAll().Result.Where(c => c.id.Equals(data.productId)).FirstOrDefault();
                        data.product = product;
                    }
                }
                else
                {

                    try
                    {
                        string name = _distributedCache.GetStringAsync("cart").Result;
                        if (JsonConvert.DeserializeObject<List<MyCart>>(name) != null)
                        {
                            _carts = JsonConvert.DeserializeObject<List<MyCart>>(name);
                        }

                    }
                    catch (Exception ex)
                    {
                    }
                    if (_carts.ToList().Where(c => c.sessionId.Equals(HttpContext.Session.Id)) != null)
                    {
                        var data = _carts.ToList().Where(c => c.sessionId.Equals(HttpContext.Session.Id));

                        var count = 0;
                        foreach (var item in data)
                        {
                            if (item.sessionId.Equals(HttpContext.Session.Id))
                            {
                                cartList.Add(item.cartDetails.FirstOrDefault());
                            }
                        }
                    }
                    foreach (var data in cartList)
                    {
                        var product = _opApi.GetAll().Result.Where(c => c.id.Equals(data.productId)).FirstOrDefault();
                        data.product = product;
                    }
                }
            }
            catch(Exception ex)
            {
                cartList = null;
            }

            return View(cartList);
        }
        [HttpGet("/user/addtocart/{id}")]
        public IActionResult AddtoCart(int id)
        {
            var username = User.Claims?.FirstOrDefault(x => x.Type.Equals("email", StringComparison.OrdinalIgnoreCase))?.Value;
            var password = User.Claims?.FirstOrDefault(x => x.Type.Equals("password", StringComparison.OrdinalIgnoreCase))?.Value;
            bool check = false;
            List<MyCart> cartListSession = new List<MyCart>();
            List<CartDetails> cartList = new List<CartDetails>();
            CartDetails cartDetails = new CartDetails();
            cartDetails.productId = id;
            cartDetails.quantity = 1;
            var productData = _opApi.GetAll().Result.Where(c => c.id.Equals(id)).FirstOrDefault();
            cartDetails.price = 1 * productData.price;


            cartList.Add(cartDetails);
            MyCart cart = new MyCart();
            MyCart productCart = new MyCart();
            HttpContext.Session.SetString("testKey", "testValue");
            cart.sessionId = HttpContext.Session.Id;
            productCart.sessionId = HttpContext.Session.Id;
            cart.cartDetails = cartList;
            productCart.cartDetails = cartList;


            if (User.Identity.IsAuthenticated && User.Claims?.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.OrdinalIgnoreCase))?.Value != "Admin")
            {
                int cartDetailsCheck = 0;
                try
                {
                    UserRegistration user = _userApi.GetUserData().Where(c => c.Email.Equals(User.Claims?.FirstOrDefault(x => x.Type.Equals("email", StringComparison.OrdinalIgnoreCase))?.Value)).FirstOrDefault();
                    IEnumerable<MyCart> productCartListFromDb = _userApi.GetCart().Result;
                    if (productCartListFromDb.Any(c => c.usersId.Equals(user.UserId)))
                    {
                        var productCartBySessioId = productCartListFromDb.Where(c => c.usersId.Equals(user.UserId)).FirstOrDefault();
                        var cartDetailslList = productCartBySessioId.cartDetails;
                          if(cartDetailslList.Count == 0)
                        {
                            cartDetailslList.Add(cartDetails);
                        }
                        else
                        {
                            if(cartDetailslList.ToList().Any(c=> c.productId.Equals(id)))
                            {
                                foreach (var cartDetailsData in cartDetailslList.ToList())
                                {
                                    if (cartDetailsData.productId.Equals(id))
                                    {
                                        cartDetailsData.quantity = cartDetailsData.quantity + 1;
                                    }
                                }
                            }
                            else
                            {
                                cartDetailslList.Add(cartDetails);
                            }
                            
                        }

                        productCartBySessioId.cartDetails = cartDetailslList;
                        _userApi.EditCart(productCartBySessioId);
                    }
                    else
                    {
                        productCart.usersId = user.UserId;
                        _userApi.Createcart(productCart);
                    }
                }
                catch (Exception ex)
                {

                }

            }
            else
            {
                try
                {
                    string name = _distributedCache.GetStringAsync("cart").Result;
                    _carts = JsonConvert.DeserializeObject<List<MyCart>>(name);
                    if (_carts != null)
                    {
                        if (_carts.ToList().Any(c => c.sessionId.Equals(HttpContext.Session.Id)))
                        {
                            foreach (var data in _carts)
                            {
                                if (data.sessionId.Equals(HttpContext.Session.Id))
                                {
                                    foreach (var data1 in data.cartDetails)
                                    {
                                        if (data1.productId.Equals(id))
                                        {
                                            var product = _opApi.GetAll().Result.Where(c => c.id.Equals(data1.productId)).FirstOrDefault();
                                            data1.product = product;
                                            var quantity = data1.quantity;
                                            data1.quantity = quantity + 1;
                                            data1.price = data1.quantity * data1.product.price;
                                            check = true;
                                            /*cartList.Add(data1);*/
                                        }
                                        else
                                        {

                                        }

                                    }
                                }

                                /*  cartListSession.Add(data);*/
                            }
                            if (check == false)
                            {
                                _carts.Add(cart);
                            }
                            _distributedCache.SetStringAsync("cart", JsonConvert.SerializeObject(_carts));
                        }
                        else
                        {
                            _carts.Add(cart);
                            _distributedCache.SetStringAsync("cart", JsonConvert.SerializeObject(_carts));
                        }

                    }
                    else
                    {
                        _carts = new List<MyCart>();
                        _carts.Add(cart);
                        _distributedCache.SetStringAsync("cart", JsonConvert.SerializeObject(_carts));
                    }

                }
                catch (Exception ex)
                {
                    _carts.Add(cart);
                    _distributedCache.SetStringAsync("cart", JsonConvert.SerializeObject(_carts));

                }


            }


            return Redirect("/user/Addtocart");
        }


        public IActionResult AddtoCartPage()
        {
            return View();
        }

        public IActionResult CartPage()
        {
            ViewBag.BrandList = _masterApi.GetList((int)Master.Brand);
            return View();
        }
        public IActionResult ManageAddress()
        {
            var username = User.Claims?.FirstOrDefault(x => x.Type.Equals("email", StringComparison.OrdinalIgnoreCase))?.Value;
            var password = User.Claims?.FirstOrDefault(x => x.Type.Equals("password", StringComparison.OrdinalIgnoreCase))?.Value;
            _user = _userApi.GetUserData().Where(c => c.Email.Equals(User.Claims?.FirstOrDefault(x => x.Type.Equals("Email", StringComparison.OrdinalIgnoreCase))?.Value)).FirstOrDefault();
            ViewData["userData"] = _user;
            return View();
        }
        public IActionResult Account()
        {
            var username = User.Claims?.FirstOrDefault(x => x.Type.Equals("email", StringComparison.OrdinalIgnoreCase))?.Value;
            var password = User.Claims?.FirstOrDefault(x => x.Type.Equals("password", StringComparison.OrdinalIgnoreCase))?.Value;
            ViewBag.BrandList = _masterApi.GetList((int)Master.Brand);
            _user = _userApi.GetUserData().Where(c => c.Email.Equals(User.Claims?.FirstOrDefault(x => x.Type.Equals("Email", StringComparison.OrdinalIgnoreCase))?.Value)).FirstOrDefault();
            ViewData["userData"] = _user;
            return View();
        }
        [HttpGet("/user/DeleteAddress/{id}")]
        public IActionResult DeleteAddress(int id)
        {
            var username = User.Claims?.FirstOrDefault(x => x.Type.Equals("email", StringComparison.OrdinalIgnoreCase))?.Value;
            var password = User.Claims?.FirstOrDefault(x => x.Type.Equals("password", StringComparison.OrdinalIgnoreCase))?.Value;
            var result = _userApi.DeleteAddress(id);
            ViewBag.BrandList = _masterApi.GetList((int)Master.Brand);
            _user = _userApi.GetUserData().Where(c => c.Email.Equals(User.Claims?.FirstOrDefault(x => x.Type.Equals("Email", StringComparison.OrdinalIgnoreCase))?.Value)).FirstOrDefault();
            ViewData["userData"] = _user;
            return RedirectToAction("Account");
        }

        [HttpGet]
        public IActionResult Address(int id , string ReturnUrl)
        {
            var username = User.Claims?.FirstOrDefault(x => x.Type.Equals("email", StringComparison.OrdinalIgnoreCase))?.Value;
            var password = User.Claims?.FirstOrDefault(x => x.Type.Equals("password", StringComparison.OrdinalIgnoreCase))?.Value;
            ViewBag.ReturnUrl = ReturnUrl;
            _user = _userApi.GetUserData().Where(c => c.Email.Equals(User.Claims?.FirstOrDefault(x => x.Type.Equals("Email", StringComparison.OrdinalIgnoreCase))?.Value)).FirstOrDefault();
            var address = _user.address.Where(c => c.id.Equals(id)).FirstOrDefault();
            ViewBag.BrandList = _masterApi.GetList((int)Master.Brand);
            return View(address);
        }
        [HttpPost("/user/address")]
        public IActionResult Address(Address addreses,string ReturnUrl)
        {
            var username = User.Claims?.FirstOrDefault(x => x.Type.Equals("email", StringComparison.OrdinalIgnoreCase))?.Value;
            var password = User.Claims?.FirstOrDefault(x => x.Type.Equals("password", StringComparison.OrdinalIgnoreCase))?.Value;
            List<Address> addresses = new List<Address>();
            addresses.Add(addreses);
            _user = _userApi.GetUserData().Where(c => c.Email.Equals(User.Claims?.FirstOrDefault(x => x.Type.Equals("Email", StringComparison.OrdinalIgnoreCase))?.Value)).FirstOrDefault();
            _user.address = addresses;
            bool result = _userApi.EditUser(_user);
            ViewBag.BrandList = _masterApi.GetList((int)Master.Brand);
            if (ReturnUrl == null)
            {
                return Redirect("/user/ManageAddress");
            }
            return Redirect(ReturnUrl);
        }
        


        public IActionResult Sort()
        {
            int count = 0;
            ViewBag.Title = " Mobizone - Price(Low to High)";
            
            ViewBag.BrandList = _masterApi.GetList((int)Master.Brand);
            var SortedData = _opApi.Sort().Result.Where(c => c.status.Equals(ProductStatus.enable));
            var productCount = SortedData.Count();
            int cout = 0;
            for (int i = 0; i <= 0; i++)
            {
                if (productCount > 12)
                {
                    cout += 1;
                }
                productCount = productCount - 12;
            }
            ViewBag.count = cout;
            var result = SortedData.Skip((int)count * 12).Take(12);
return View("Index", result);
}
        public async Task<PartialViewResult> sortLowToHighPartial(int? count, string brandName ,string name)
        {
            if (brandName == "null")
                brandName = null;
            if (name == "null")
                name = null;
            IEnumerable<ProductEntity> SortedData = null;
            if (brandName != null && name != "null")
            {
                var data = await _opApi.Filter(brandName);
                SortedData = data.OrderBy(c => c.price);
            }
            else if (name != null && name != "null")
            {
                var data = await _opApi.Search(name);
                SortedData = data.OrderBy(c => c.price);
            }
            else
            {
                var data = await _opApi.Sort();
                SortedData = data.Where(c => c.status.Equals(ProductStatus.enable));
            }
            if (count == null)
            {
                count = 0;
            }
            ViewBag.Title = " Mobizone - Price(Low to High)";
            
            ViewBag.BrandList = _masterApi.GetList((int)Master.Brand);
            var productCount = SortedData.Count();
            int cout = 0;
            for (int i = 0; i <= 0; i++)
            {
                if (productCount > 12)
                {
                    cout += 1;
                }
                productCount = productCount - 12;
            }
            ViewBag.count = cout;
            var result = SortedData.Skip((int)count * 12).Take(12);
            return PartialView("PartialViews/_IndexPartialView", result);
        }

        public IActionResult Sortby()
        {
            ViewBag.Title = "Mobizone - Price(High to Low )";
            ViewBag.count = 0;
            ViewBag.BrandList = _masterApi.GetList((int)Master.Brand);
            var SortedData = _opApi.Sortby().Result.Where(c => c.status.Equals(ProductStatus.enable));
            return View("Index", SortedData);
        }
        public PartialViewResult SortHighToLowPartial(int? count , string brandName, string name)
        {
            if (brandName == "null")
                brandName = null;
            if (name == "null")
                name = null;
            IEnumerable<ProductEntity> SortedData = null;
            if (brandName != null && name != "null")
            {
                SortedData = _opApi.Filter(brandName).Result.OrderByDescending(c=>c.price);
            }
            else if (name != null && name != "null")
            {
                SortedData = _opApi.Search(name).Result.OrderByDescending(c => c.price);
            }
            else
            {
                SortedData = _opApi.Sortby().Result.Where(c => c.status.Equals(ProductStatus.enable));
            }
            if (count == null)
            {
                count = 0;
            }
            ViewBag.Title = " Mobizone - Price(High To Low)";

            ViewBag.BrandList = _masterApi.GetList((int)Master.Brand);
            
            var productCount = SortedData.Count();
            int cout = 0;
            for (int i = 0; i <= 0; i++)
            {
                if (productCount > 12)
                {
                    cout += 1;
                }
                productCount = productCount - 12;
            }
            ViewBag.count = cout;
            var result = SortedData.Skip((int)count * 12).Take(12);
            return PartialView("PartialViews/_IndexPartialView", result);
        }
        [HttpPost]
        public PartialViewResult filterByBrandName(string brandName)
        {
            if (brandName == "null")
                brandName = null;
            int cout = 0;
            int count = 0;
            ViewBag.Title = " Mobizone - Filter ";
            ViewBag.BrandList = _masterApi.GetList((int)Master.Brand);
            IEnumerable<ProductEntity> filteredData = _opApi.GetAll().Result;
            if (brandName != null)
            {
                filteredData = _opApi.Filter(brandName).Result;
                if (filteredData != null)
                {

                    var productCount = filteredData.Count();

                    for (int i = 0; i <= 0; i++)
                    {
                        if (productCount > 10)
                        {
                            cout += 1;
                        }
                        productCount = productCount - 12;
                    }
                    filteredData = filteredData.Skip((int)count * 10).Take(10);

                }
                ViewBag.count = cout;
                return PartialView("PartialViews/_IndexPartialView", filteredData);
            }
            ViewBag.count = cout;
            return PartialView("PartialViews/_IndexPartialView", filteredData);
        }
        [HttpGet]
        public async Task<IActionResult> ProductDetails(int id)
        {
            var details = await _opApi.GetProduct(id); details = await _opApi.GetProduct(id);
            ViewBag.BrandList = _masterApi.GetList((int)Master.Brand);
            return View(details);
        }
        public IActionResult MyOrders()
        {
            ViewBag.Status = Enum.GetNames(typeof(OrderStatus)).ToList();
            return View();
        }
        public PartialViewResult MyOrdersPartialView()
        {
            var username = User.Claims?.FirstOrDefault(x => x.Type.Equals("email", StringComparison.OrdinalIgnoreCase))?.Value;
            var password = User.Claims?.FirstOrDefault(x => x.Type.Equals("password", StringComparison.OrdinalIgnoreCase))?.Value;
            var user = _userApi.GetUserData().Where(c => c.Email.Equals(User.Claims?.FirstOrDefault(x => x.Type.Equals("email", StringComparison.OrdinalIgnoreCase))?.Value)).FirstOrDefault();
            var userOrders = _userApi.GetCheckOut().Result.Where(c => c.userId.Equals(user.UserId));
            foreach (var checkOutData in userOrders)
            {
                var product = _opApi.GetAll().Result.Where(c => c.id.Equals(checkOutData.productId)).FirstOrDefault();
                checkOutData.product = product;

            }
            return PartialView("PartialViews/_MyOrdersPartialView", userOrders);
        }
        public PartialViewResult FilterOrderByStatusName(string statusName)
        {
            IEnumerable<Checkout> userOrders = null;
            Enum.TryParse(statusName, out OrderStatus myStatus);
            var username = User.Claims?.FirstOrDefault(x => x.Type.Equals("email", StringComparison.OrdinalIgnoreCase))?.Value;
            var password = User.Claims?.FirstOrDefault(x => x.Type.Equals("password", StringComparison.OrdinalIgnoreCase))?.Value;
            var user = _userApi.GetUserData().Where(c => c.Email.Equals(User.Claims?.FirstOrDefault(x => x.Type.Equals("email", StringComparison.OrdinalIgnoreCase))?.Value)).FirstOrDefault();
            if(statusName != null)
            {
                userOrders = _userApi.GetCheckOut().Result.Where(c => c.userId.Equals(user.UserId) && c.status.Equals(myStatus));
            }
            else
            {
                userOrders = _userApi.GetCheckOut().Result.Where(c => c.userId.Equals(user.UserId));
            }
            foreach (var checkOutData in userOrders)
            {
                var product = _opApi.GetAll().Result.Where(c => c.id.Equals(checkOutData.productId)).FirstOrDefault();
                checkOutData.product = product;

            }
            return PartialView("PartialViews/_MyOrdersPartialView", userOrders);
        }
        [HttpGet]
        public IActionResult OrderDetails(int id)
        {
            if (id == 0)
            {
                return View("Index");
            }
            var checkoutList = _userApi.GetCheckOut().Result;
            var checkout = checkoutList.Where(c => c.id.Equals(id)).FirstOrDefault();
            var ProductDetails = _opApi.GetAll().Result.Where(c => c.id.Equals(checkout.productId)).FirstOrDefault();
            ViewData["ProductDetails"] = ProductDetails;
            ViewData["Address"] = _userApi.GetAddress().Result.Where(c => c.id.Equals(checkout.addressId)).FirstOrDefault();

            return View(checkout);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgetPassword()
        {

            return View();
        }

        [HttpPost]
        public IActionResult SearchNotPartial(string name)
        {
            ViewBag.count = 0;
            ViewBag.Search = name;
            ViewBag.BrandList = _masterApi.GetList((int)Master.Brand);
            var data = _opApi.Search(name).Result;
            return View("PartialViews/_IndexPartialView", data);
        }
        [HttpPost]
        public PartialViewResult Search(string name, int? count)
        {
            ViewBag.count = 0;

            ViewBag.BrandList = _masterApi.GetList((int)Master.Brand);
            var data = _opApi.Search(name).Result;
            return PartialView("PartialViews/_IndexPartialView", data);
        }
        [HttpPost]
        public IActionResult quantity(int quantity, int id)
        {
            var username = User.Claims?.FirstOrDefault(x => x.Type.Equals("email", StringComparison.OrdinalIgnoreCase))?.Value;
            var password = User.Claims?.FirstOrDefault(x => x.Type.Equals("password", StringComparison.OrdinalIgnoreCase))?.Value;
            bool check = false;
            if (User.Identity.IsAuthenticated && User.Claims?.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.OrdinalIgnoreCase))?.Value != "Admin")
            {
                var user = _userApi.GetUserData().Where(c => c.Email.Equals(User.Claims?.FirstOrDefault(x => x.Type.Equals("email", StringComparison.OrdinalIgnoreCase))?.Value)).FirstOrDefault();

                MyCart myCart = new MyCart();
                myCart = _userApi.GetCart().Result.Where(c => c.usersId.Equals(user.UserId)).FirstOrDefault();

                foreach (var mycartData in myCart.cartDetails)
                {
                    if (mycartData.productId.Equals(id))
                    {
                        mycartData.quantity = quantity;
                        mycartData.price = quantity * mycartData.product.price;
                    }
                }
                _userApi.EditCart(myCart);
            }
            else
            {
                string name = _distributedCache.GetStringAsync("cart").Result;
                _carts = JsonConvert.DeserializeObject<List<MyCart>>(name);
                if (_carts != null || _carts.Count > 0)
                {
                    if (_carts.ToList().Any(c => c.sessionId.Equals(HttpContext.Session.Id)))
                    {
                        foreach (var data in _carts)
                        {
                            foreach (var caratDetailsData in data.cartDetails)
                            {
                                var product = _opApi.GetAll().Result.Where(c => c.id.Equals(caratDetailsData.productId)).FirstOrDefault();
                                caratDetailsData.product = product;
                            }
                            if (data.sessionId.Equals(HttpContext.Session.Id))
                            {
                                foreach (var data1 in data.cartDetails)
                                {
                                    if (data1.productId.Equals(id))
                                    {
                                        data1.quantity = quantity;
                                        data1.price = data1.quantity * data1.product.price;
                                        /*cartList.Add(data1);*/
                                    }
                                    else
                                    {

                                    }

                                }
                            }

                            /*  cartListSession.Add(data);*/
                        }

                        _distributedCache.SetStringAsync("cart", JsonConvert.SerializeObject(_carts));
                    }
                }
            }
            return Redirect("/user/Addtocart");
        }

        [HttpGet]
        public IActionResult RemoveCart(int id)
        {
            var username = User.Claims?.FirstOrDefault(x => x.Type.Equals("email", StringComparison.OrdinalIgnoreCase))?.Value;
            var password = User.Claims?.FirstOrDefault(x => x.Type.Equals("password", StringComparison.OrdinalIgnoreCase))?.Value;
            bool check = false;
            if (User.Identity.IsAuthenticated && User.Claims?.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.OrdinalIgnoreCase))?.Value != "Admin")
            {
                var user = _userApi.GetUserData().Where(c => c.Email.Equals(User.Claims?.FirstOrDefault(x => x.Type.Equals("email", StringComparison.OrdinalIgnoreCase))?.Value)).FirstOrDefault();
                try
                {
                    string name = JsonConvert.SerializeObject(_userApi.GetCart().Result);
                    if (JsonConvert.DeserializeObject<List<MyCart>>(name) != null)
                    {
                        _carts = JsonConvert.DeserializeObject<List<MyCart>>(name);
                    }

                }
                catch (Exception ex)
                {
                }
                if (_carts.ToList().Where(c => c.usersId.Equals(user.UserId)) != null)
                {
                    var data = _carts.ToList().Where(c => c.usersId.Equals(user.UserId)).FirstOrDefault();

                    var count = 0;
                    foreach (var item in data.cartDetails.ToList())
                    {
                        if (item.productId.Equals(id))
                        {
                            data.cartDetails.Remove(item);
                        }

                    }
                    MyCart myCart = new MyCart();
                    myCart = _userApi.GetCart().Result.Where(c => c.usersId.Equals(user.UserId)).FirstOrDefault();
                    foreach (var mycartData in myCart.cartDetails)
                    {
                        if (mycartData.productId.Equals(id))
                        {
                            _userApi.DeleteCartDetails(mycartData.id);
                        }
                    }
                    _userApi.EditCart(myCart);
                }
            }
            else
            {


                string name = _distributedCache.GetStringAsync("cart").Result;
                _carts = JsonConvert.DeserializeObject<List<MyCart>>(name);
                if (_carts != null || _carts.Count > 0)
                {
                    if (_carts.ToList().Any(c => c.sessionId.Equals(HttpContext.Session.Id)))
                    {
                        foreach (var data in _carts.ToList())
                        {
                            foreach (var caratDetailsData in data.cartDetails)
                            {
                                var product = _opApi.GetAll().Result.Where(c => c.id.Equals(caratDetailsData.productId)).FirstOrDefault();
                                caratDetailsData.product = product;
                            }
                            if (data.sessionId.Equals(HttpContext.Session.Id))
                            {
                                foreach (var data1 in data.cartDetails)
                                {
                                    if (data1.productId.Equals(id))
                                    {
                                        // data.cartDetails.Remove(data1);
                                        /*cartList.Add(data1);*/
                                        _carts.Remove(data);
                                        List<MyCart> carts = _carts;
                                        _carts = carts;
                                    }
                                    else
                                    {

                                    }

                                }
                            }

                            /*  cartListSession.Add(data);*/
                        }

                        _distributedCache.SetStringAsync("cart", JsonConvert.SerializeObject(_carts));
                    }
                }
            }
            return Redirect("/user/Addtocart");
        }
        [HttpPost]
        [Authorize(Roles ="User")]
        public IActionResult CartOrder()
        {
            var username = User.Claims?.FirstOrDefault(x => x.Type.Equals("email", StringComparison.OrdinalIgnoreCase))?.Value;
            var password = User.Claims?.FirstOrDefault(x => x.Type.Equals("password", StringComparison.OrdinalIgnoreCase))?.Value;
            UserRegistration user;
            user = _userApi.GetUserData().Where(c => c.Email.Equals(User.Claims?.FirstOrDefault(x => x.Type.Equals("email", StringComparison.OrdinalIgnoreCase))?.Value)).FirstOrDefault();
            var cartDataList = _userApi.GetCart().Result;
            var vartData = cartDataList.Where(c => c.usersId.Equals(user.UserId)).FirstOrDefault();
            ProductEntity product;
            List<CartDetails> carts = new List<CartDetails>();
            foreach (var cartDetails in vartData.cartDetails)
            {
                product = _opApi.GetAll().Result.Where(c => c.id.Equals(cartDetails.productId)).FirstOrDefault();
                cartDetails.product = product;
                carts.Add(cartDetails);
            }
            ViewData["cartDetails"] = carts;
            ViewData["cart"] = vartData;
            ViewData["userData"] = user;
            ViewBag.BrandList = _masterApi.GetList((int)Master.Brand);
            return View();
        }
        public IActionResult BuyCart(Checkout checkout)
        {
            var username = User.Claims?.FirstOrDefault(x => x.Type.Equals("email", StringComparison.OrdinalIgnoreCase))?.Value;
            var password = User.Claims?.FirstOrDefault(x => x.Type.Equals("password", StringComparison.OrdinalIgnoreCase))?.Value;
            UserRegistration user;
            user = _userApi.GetUserData().Where(c => c.Email.Equals(User.Claims?.FirstOrDefault(x => x.Type.Equals("email", StringComparison.OrdinalIgnoreCase))?.Value)).FirstOrDefault();
            MyCart myCart = new MyCart();
            myCart = _userApi.GetCart().Result.Where(c => c.usersId.Equals(user.UserId)).FirstOrDefault();
            foreach (var caratDetailsData in myCart.cartDetails)
            {
                var product = _opApi.GetAll().Result.Where(c => c.id.Equals(caratDetailsData.productId)).FirstOrDefault();
                caratDetailsData.product = product;
            }
            foreach(var data in myCart.cartDetails)
            {
                Checkout checkout1 = new Checkout();
                checkout1.productId = data.productId;
                foreach (var address in checkout.addressList)
                {
                    if (address.IsChecked)
                    {
                        checkout1.address = address;
                        checkout1.addressId = address.id;
                    }
                }
                Random rnd = new Random();
                checkout1.quantity = (int)data.quantity;
                checkout1.orderId = rnd.Next();
                checkout1.paymentModeId = checkout.paymentModeId;
                checkout1.userId = checkout.userId;
                checkout1.status = OrderStatus.orderplaced;
                checkout1.price = (int)data.price;
                bool result = _userApi.CreateCheckOut(checkout1);
            }
            _userApi.DeleteCart(myCart.id);
            return View("orderplaced");
        }
        [HttpGet("/user/CancelCheckout/{orderId}")]
        public async Task<IActionResult> CancelCheckout(int orderId)
        {
            Checkout checkout = new Checkout();
            OrderStatus statuses = new OrderStatus();

            var checkoutData = _userApi.GetCheckOut().Result.Where(c => c.orderId.Equals(orderId)).FirstOrDefault();
            checkoutData.status = OrderStatus.cancelled;
            checkoutData.cancelRequested = RoleTypes.User;
            _userApi.EditCheckout(checkoutData);
            return Redirect("/");
        }
        [HttpGet]
        public IActionResult minus(int id)
        {
            var username = User.Claims?.FirstOrDefault(x => x.Type.Equals("email", StringComparison.OrdinalIgnoreCase))?.Value;
            var password = User.Claims?.FirstOrDefault(x => x.Type.Equals("password", StringComparison.OrdinalIgnoreCase))?.Value;
            bool check = false;
            if (User.Identity.IsAuthenticated && User.Claims?.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.OrdinalIgnoreCase))?.Value != "Admin")
            {
                var user = _userApi.GetUserData().Where(c => c.Email.Equals(User.Claims?.FirstOrDefault(x => x.Type.Equals("email", StringComparison.OrdinalIgnoreCase))?.Value)).FirstOrDefault();

                MyCart myCart = new MyCart();
                myCart = _userApi.GetCart().Result.Where(c => c.usersId.Equals(user.UserId)).FirstOrDefault();
                foreach (var caratDetailsData in myCart.cartDetails)
                {
                    var product = _opApi.GetAll().Result.Where(c => c.id.Equals(caratDetailsData.productId)).FirstOrDefault();
                    caratDetailsData.product = product;
                }
                foreach (var mycartData in myCart.cartDetails)
                {
                    if (mycartData.productId.Equals(id))
                    {
                        if ((mycartData.quantity - 1) * mycartData.product.price > 0)
                        {
                            mycartData.quantity = mycartData.quantity - 1;
                            mycartData.price = mycartData.quantity * mycartData.product.price;
                        }
    }
}
_userApi.EditCart(myCart);
}
else
{
string name = _distributedCache.GetStringAsync("cart").Result;
_carts = JsonConvert.DeserializeObject<List<MyCart>>(name);
if (_carts != null || _carts.Count > 0)
{
    if (_carts.ToList().Any(c => c.sessionId.Equals(HttpContext.Session.Id)))
    {
        foreach (var data in _carts)
        {
            foreach (var caratDetailsData in data.cartDetails)
            {
                var product = _opApi.GetAll().Result.Where(c => c.id.Equals(caratDetailsData.productId)).FirstOrDefault();
                caratDetailsData.product = product;
            }
            if (data.sessionId.Equals(HttpContext.Session.Id))
            {

                foreach (var data1 in data.cartDetails)
                {
                    if (data1.productId.Equals(id))
                    {
                        if ((data1.quantity-1) * data1.product.price > 0)
                        {
                            data1.quantity = data1.quantity - 1;
                            data1.price = data1.quantity * data1.product.price;
                        }
                        /*cartList.Add(data1);*/
                    }
                    else
                                    {

                                    }

                                }
                            }

                            /*  cartListSession.Add(data);*/
                        }

                        _distributedCache.SetStringAsync("cart", JsonConvert.SerializeObject(_carts));
                    }
                }
            }
            return Redirect("/user/Addtocart");
        }
        [HttpGet]
        public IActionResult plus(int id)
        {
            var username = User.Claims?.FirstOrDefault(x => x.Type.Equals("email", StringComparison.OrdinalIgnoreCase))?.Value;
            var password = User.Claims?.FirstOrDefault(x => x.Type.Equals("password", StringComparison.OrdinalIgnoreCase))?.Value;
            bool check = false;
            if (User.Identity.IsAuthenticated && User.Claims?.FirstOrDefault(x => x.Type.Equals("Role", StringComparison.OrdinalIgnoreCase))?.Value != "Admin")
            {
                var user = _userApi.GetUserData().Where(c => c.Email.Equals(User.Claims?.FirstOrDefault(x => x.Type.Equals("email", StringComparison.OrdinalIgnoreCase))?.Value)).FirstOrDefault();

                MyCart myCart = new MyCart();
                myCart = _userApi.GetCart().Result.Where(c => c.usersId.Equals(user.UserId)).FirstOrDefault();
                foreach (var caratDetailsData in myCart.cartDetails)
                {
                    var product = _opApi.GetAll().Result.Where(c => c.id.Equals(caratDetailsData.productId)).FirstOrDefault();
                    caratDetailsData.product = product;
                }
                foreach (var mycartData in myCart.cartDetails)
                {
                    if (mycartData.productId.Equals(id))
                    {
                        mycartData.quantity = mycartData.quantity + 1;
                        mycartData.price = mycartData.quantity * mycartData.product.price;
                    }
                }
                var checkNegative = myCart.cartDetails.Where(c => c.price > 0);
                if (checkNegative != null)
                {
                    _userApi.EditCart(myCart);
                }
                
            }
            else
            {
                string name = _distributedCache.GetStringAsync("cart").Result;
                _carts = JsonConvert.DeserializeObject<List<MyCart>>(name);
                if (_carts != null || _carts.Count > 0)
                {
                    if (_carts.ToList().Any(c => c.sessionId.Equals(HttpContext.Session.Id)))
                    {
                        foreach (var data in _carts)
                        {
                            foreach (var caratDetailsData in data.cartDetails)
                            {
                                var product = _opApi.GetAll().Result.Where(c => c.id.Equals(caratDetailsData.productId)).FirstOrDefault();
                                caratDetailsData.product = product;
                            }
                            if (data.sessionId.Equals(HttpContext.Session.Id))
                            {

                                foreach (var data1 in data.cartDetails)
                                {
                                    if (data1.productId.Equals(id))
                                    {
                                      
                                            data1.quantity = data1.quantity + 1;
                                            data1.price = data1.quantity * data1.product.price;
                                        
                                        
                                        /*cartList.Add(data1);*/
                                    }
                                    else
                                    {

                                    }

                                }
                            }

                            /*  cartListSession.Add(data);*/
                        }

                        _distributedCache.SetStringAsync("cart", JsonConvert.SerializeObject(_carts));
                    }
                }
            }
            return Redirect("/user/Addtocart");
             int pagination(int count)
            {
                int cout = 0;
                for (int i = 0; i <= 0; i++)
                {
                    if (count > 10)
                    {
                        cout += 1;
                    }
                    count = count - 10;
                }
                return cout;
            }
        }
        public class quantityObj
        {
            public int quantity { get; set; }
        }

    }
    public class Security
    {
        public string Encrypt(string key, string data)
        {
            string encData = null;
            byte[][] keys = GetHashKeys(key);

            try
            {
                encData = EncryptStringToBytes_Aes(data, keys[0], keys[1]);
            }
            catch (CryptographicException) { }
            catch (ArgumentNullException) { }

            return encData;
        }

        public string Decrypt(string key, string data)
        {
            string decData = null;
            byte[][] keys = GetHashKeys(key);

            try
            {
                decData = DecryptStringFromBytes_Aes(data, keys[0], keys[1]);
            }
            catch (CryptographicException) { }
            catch (ArgumentNullException) { }

            return decData;
        }

        private byte[][] GetHashKeys(string key)
        {
            byte[][] result = new byte[2][];
            Encoding enc = Encoding.UTF8;

            SHA256 sha2 = new SHA256CryptoServiceProvider();

            byte[] rawKey = enc.GetBytes(key);
            byte[] rawIV = enc.GetBytes(key);

            byte[] hashKey = sha2.ComputeHash(rawKey);
            byte[] hashIV = sha2.ComputeHash(rawIV);

            Array.Resize(ref hashIV, 16);

            result[0] = hashKey;
            result[1] = hashIV;

            return result;
        }

        //source: https://msdn.microsoft.com/de-de/library/system.security.cryptography.aes(v=vs.110).aspx
        private static string EncryptStringToBytes_Aes(string plainText, byte[] Key, byte[] IV)
        {
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            byte[] encrypted;

            using (AesManaged aesAlg = new AesManaged())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt =
                            new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }
            return Convert.ToBase64String(encrypted);
        }

        //source: https://msdn.microsoft.com/de-de/library/system.security.cryptography.aes(v=vs.110).aspx
        private static string DecryptStringFromBytes_Aes(string cipherTextString, byte[] Key, byte[] IV)
        {
            byte[] cipherText = Convert.FromBase64String(cipherTextString);

            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            string plaintext = null;

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt =
                            new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
            return plaintext;
        }
    }
}




