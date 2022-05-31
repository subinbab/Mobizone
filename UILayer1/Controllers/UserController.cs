using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using DocumentFormat.OpenXml.Spreadsheet;
using DomainLayer;
using DomainLayer.ProductModel;
using DomainLayer.ProductModel.Master;
using DomainLayer.Users;
using DTOLayer.Product;
using DTOLayer.UserModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
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
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        MasterApi _masterApi;
        List<Cart> _carts;
        UserRegistration _user { get; set; }
        private readonly IDistributedCache _distributedCache;


        INotyfService _notyfService;

        public UserController(IConfiguration configuration, INotyfService notyf, IMapper mapper, IWebHostEnvironment webHostEnvironment, IDistributedCache distributedCache)

        {
            _configuration = configuration;
            userApi = new UserApi(_configuration);
            _opApi = new ProductOpApi(_configuration, mapper, webHostEnvironment);

            _masterApi = new MasterApi(_configuration);
            _notyf = notyf;
            _mapper = mapper;
            _distributedCache = distributedCache;
            _carts = new List<Cart>();
            // HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(_carts));



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
                    if (productCount > 10)
                    {
                        cout += 1;
                    }
                    productCount = productCount - 10;
                }
                var result = data.Skip((int)count * 10).Take(10);
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
            if (userList.Any(c => c.Email.Equals(user.Email)))
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
            ForgetPasswordViewModel email = new ForgetPasswordViewModel();
            email.emailSent = false;
            ViewBag.BrandList = _masterApi.GetList((int)Master.Brand);
            return View(email);
        }
        [HttpPost]
        [AllowAnonymous]
        public IActionResult ForgotPassword(ForgetPasswordViewModel data)
        {
            if (ModelState.IsValid)
            {
                HttpContext.Session.SetString("key", "value");


                ModelState.Clear();
                var userDetails = userApi.GetUserData().Where(check => check.Email.Equals(data.email)).FirstOrDefault();
                if (userDetails != null)
                {

                    var session = HttpContext.Session.Id;
                    data.emailSent = true;
                    MailRequest mailRequest = new MailRequest();
                    mailRequest.Body = "<a href='http://localhost:58738/user/ResetPassword/" + data.email + "/" + session + "'>Click Here</a>";
                    mailRequest.Subject = "ResetPassword";
                    mailRequest.ToEmail = userDetails.Email;
                    var checkEmail = userApi.PostMail(mailRequest);
                    return View(data);
                }


            }
            ViewBag.BrandList = _masterApi.GetList((int)Master.Brand);
            return View(data);
        }

        [HttpGet("/user/ResetPassword/{email}/{sessionId}")]
        public ActionResult ResetPassword(string email, string sessionId)
        {
            if (sessionId == HttpContext.Session.Id)
            {
                var userDetails = userApi.GetUserData().Where(check => check.Email.Equals(email)).FirstOrDefault();
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
            try
            {
                UserRegistration register = new UserRegistration();
                register = userApi.GetUserData().Where(c => c.Email.Equals(resetPassword.User.Email)).FirstOrDefault();
                register.Password = resetPassword.newPassword;
                var result = userApi.EditUser(register);
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
        [HttpGet]
        public IActionResult order(int id)
        {
            var data = _opApi.GetProduct(id).Result;
            ViewData["ProductDetails"] = data;
            _user = userApi.GetUserData().Where(c => c.Email.Equals(User.Claims?.FirstOrDefault(x => x.Type.Equals("email", StringComparison.OrdinalIgnoreCase))?.Value)).FirstOrDefault();
            ViewData["userData"] = _user;
            ViewBag.BrandList = _masterApi.GetList((int)Master.Brand);
            return View();
        }
        [Authorize(Roles = "User")]
        [HttpGet]
        public IActionResult CartOrder(List<ProductEntity> productList)
        {
            List<ProductEntity> productListForOrder = new List<ProductEntity>();
            foreach (ProductEntity product in productList)
            {
                productListForOrder.Add(_opApi.GetProduct(product.id).Result);
            }
            var data = productListForOrder;
            ViewData["ProductDetails"] = data;
            _user = userApi.GetUserData().Where(c => c.Email.Equals(User.Claims?.FirstOrDefault(x => x.Type.Equals("email", StringComparison.OrdinalIgnoreCase))?.Value)).FirstOrDefault();
            ViewData["userData"] = _user;
            ViewBag.BrandList = _masterApi.GetList((int)Master.Brand);
            return View();
        }
        [HttpPost]
        public IActionResult order(Checkout checkout)
        {
            if (checkout == null)
            {
                _notyf.Error("Not Added");
                ViewBag.BrandList = _masterApi.GetList((int)Master.Brand);
                return RedirectToAction("Index");
            }
            else
            {
                var data = _opApi.GetProduct(checkout.productId).Result;
                data.quantity = data.quantity - checkout.quantity;
                if (data.quantity == 0)
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
                _notyf.Success("succesfully ordered");
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
        [Authorize(Roles = "User")]
        [HttpGet]
        public IActionResult AddtoCart()
        {
            List<CartDetails> cartList = new List<CartDetails>();
            try
            {
                
                if (User.Identity.IsAuthenticated)
                {
                    var user = userApi.GetUserData().Where(c => c.Email.Equals(User.Claims?.FirstOrDefault(x => x.Type.Equals("email", StringComparison.OrdinalIgnoreCase))?.Value)).FirstOrDefault();
                    try
                    {
                        string name = JsonConvert.SerializeObject(userApi.GetCart().Result);
                        if (JsonConvert.DeserializeObject<List<Cart>>(name) != null)
                        {
                            _carts = JsonConvert.DeserializeObject<List<Cart>>(name);
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
                        if (JsonConvert.DeserializeObject<List<Cart>>(name) != null)
                        {
                            _carts = JsonConvert.DeserializeObject<List<Cart>>(name);
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
            bool check = false;
            List<Cart> cartListSession = new List<Cart>();
            List<CartDetails> cartList = new List<CartDetails>();
            CartDetails cartDetails = new CartDetails();
            cartDetails.productId = id;
            cartDetails.quantity = 1;
            var productData = _opApi.GetAll().Result.Where(c => c.id.Equals(id)).FirstOrDefault();
            cartDetails.price = 1 * productData.price;


            cartList.Add(cartDetails);
            Cart cart = new Cart();
            MyCart productCart = new MyCart();
            HttpContext.Session.SetString("testKey", "testValue");
            cart.sessionId = HttpContext.Session.Id;
            productCart.sessionId = HttpContext.Session.Id;
            cart.cartDetails = cartList;
            productCart.cartDetails = cartList;


            if (User.Identity.IsAuthenticated)
            {
                int cartDetailsCheck = 0;
                try
                {
                    UserRegistration user = userApi.GetUserData().Where(c => c.Email.Equals(User.Claims?.FirstOrDefault(x => x.Type.Equals("email", StringComparison.OrdinalIgnoreCase))?.Value)).FirstOrDefault();
                    IEnumerable<MyCart> productCartListFromDb = userApi.GetCart().Result;
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
                        userApi.EditCart(productCartBySessioId);
                    }
                    else
                    {
                        productCart.usersId = user.UserId;
                        userApi.Createcart(productCart);
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
                    _carts = JsonConvert.DeserializeObject<List<Cart>>(name);

                    if (_carts != null || _carts.Count > 0)
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
            try
            {
                string name = _distributedCache.GetStringAsync("cart").Result;
                if (JsonConvert.DeserializeObject<List<Cart>>(name) != null)
                {
                    _carts = JsonConvert.DeserializeObject<List<Cart>>(name);
                }

            }
            catch (Exception ex)
            {
            }
            List<CartDetails> cartDetailsList = new List<CartDetails>();
            if (_carts.ToList().Where(c => c.sessionId.Equals(HttpContext.Session.Id)) != null)
            {
                var data = _carts.ToList().Where(c => c.sessionId.Equals(HttpContext.Session.Id));

                var count = 0;

                foreach (var item in data)
                {
                    if (item.sessionId.Equals(HttpContext.Session.Id))
                    {
                        cartDetailsList.Add(item.cartDetails.FirstOrDefault());
                    }
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
            _user = userApi.GetUserData().Where(c => c.Email.Equals(User.Claims?.FirstOrDefault(x => x.Type.Equals("Email", StringComparison.OrdinalIgnoreCase))?.Value)).FirstOrDefault();
            ViewData["userData"] = _user;
            return View();
        }
        public IActionResult Account()
        {
            ViewBag.BrandList = _masterApi.GetList((int)Master.Brand);
            _user = userApi.GetUserData().Where(c => c.Email.Equals(User.Claims?.FirstOrDefault(x => x.Type.Equals("Email", StringComparison.OrdinalIgnoreCase))?.Value)).FirstOrDefault();
            ViewData["userData"] = _user;
            return View();
        }
        [HttpGet("/user/DeleteAddress/{id}")]
        public IActionResult DeleteAddress(int id)
        {
            var result = userApi.DeleteAddress(id);
            ViewBag.BrandList = _masterApi.GetList((int)Master.Brand);
            _user = userApi.GetUserData().Where(c => c.Email.Equals(User.Claims?.FirstOrDefault(x => x.Type.Equals("Email", StringComparison.OrdinalIgnoreCase))?.Value)).FirstOrDefault();
            ViewData["userData"] = _user;
            return RedirectToAction("Account");
        }

        [HttpGet]
        public IActionResult Address(int id)
        {

            _user = userApi.GetUserData().Where(c => c.Email.Equals(User.Claims?.FirstOrDefault(x => x.Type.Equals("Email", StringComparison.OrdinalIgnoreCase))?.Value)).FirstOrDefault();
            var address = _user.address.Where(c => c.id.Equals(id)).FirstOrDefault();
            ViewBag.BrandList = _masterApi.GetList((int)Master.Brand);
            return View(address);
        }
        [HttpPost("/user/address")]
        public IActionResult Address(Address addreses)
        {
            List<Address> addresses = new List<Address>();
            addresses.Add(addreses);
            _user = userApi.GetUserData().Where(c => c.Email.Equals(User.Claims?.FirstOrDefault(x => x.Type.Equals("Email", StringComparison.OrdinalIgnoreCase))?.Value)).FirstOrDefault();
            _user.address = addresses;
            bool result = userApi.EditUser(_user);
            ViewBag.BrandList = _masterApi.GetList((int)Master.Brand);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult filter(string brandName)
        {

            ViewBag.Title = " Mobizone - Filter ";

            ViewBag.BrandList = _masterApi.GetList((int)Master.Brand);
            IEnumerable<ProductEntity> filteredData = _opApi.GetAll().Result.Where(c => c.status.Equals(ProductStatus.enable)); 
            if(filteredData != null)
            {
                if (brandName != null)
                {
                    filteredData = _opApi.Filter(brandName).Result;
                }
                int count = 0;
                var productCount = filteredData.Count();
                int cout = 0;
                for (int i = 0; i <= 0; i++)
                {
                    if (productCount > 10)
                    {
                        cout += 1;
                    }
                    productCount = productCount - 10;
                }
                 result = filteredData.Skip((int)count * 10).Take(10);
                ViewBag.count = cout;
            }
            

            return View("Index", result);
        }


        public IActionResult Sort(string price)
        {
            ViewBag.Title = " Mobizone - Price(Low to High)";
            ViewBag.count = 0;
            ViewBag.BrandList = _masterApi.GetList((int)Master.Brand);
            var SortedData = _opApi.Sort(price).Result;
            return View("Index", SortedData);
        }

        public IActionResult Sortby(string price)
        {
            ViewBag.Title = "Mobizone - Price(High to Low )";
            ViewBag.count = 0;
            ViewBag.BrandList = _masterApi.GetList((int)Master.Brand);
            var SortedData = _opApi.Sortby(price).Result;
            return View("Index", SortedData);
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
            var user = userApi.GetUserData().Where(c => c.Email.Equals(User.Claims?.FirstOrDefault(x => x.Type.Equals("email", StringComparison.OrdinalIgnoreCase))?.Value)).FirstOrDefault();
            var userOrders = userApi.GetCheckOut().Result.Where(c => c.userId.Equals(user.UserId));
            foreach (var checkOutData in userOrders)
            {
                var product = _opApi.GetAll().Result.Where(c => c.id.Equals(checkOutData.productId)).FirstOrDefault();
                checkOutData.product = product;

            }
            return View(userOrders);
        }
        [HttpGet]
        public IActionResult OrderDetails(int id)
        {
            if (id == 0)
            {
                return View("Index");
            }
            var checkoutList = userApi.GetCheckOut().Result;
            var checkout = checkoutList.Where(c => c.id.Equals(id)).FirstOrDefault();
            var ProductDetails = _opApi.GetAll().Result.Where(c => c.id.Equals(checkout.productId)).FirstOrDefault();
            ViewBag.Product = ProductDetails;
            ViewData["Address"] = userApi.GetAddress().Result.Where(c => c.id.Equals(checkout.addressId)).FirstOrDefault();

            return View(checkout);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgetPassword()
        {

            return View();
        }


        /*    return Json();
*/

        /*     [HttpPost]
             public IActionResult sort(string price)
               {
                   ViewBag.count = 0;
                   ViewBag.PriceList = _

               }*/
        [HttpPost]
        public IActionResult Search(string name)

        {
            ViewBag.count = 0;
            var data = _opApi.Search(name).Result;
            return View("Index", data);
        }
        [HttpPost]
        public IActionResult quantity(int quantity, int id)
        {
            bool check = false;
            if (User.Identity.IsAuthenticated)
            {
                var user = userApi.GetUserData().Where(c => c.Email.Equals(User.Claims?.FirstOrDefault(x => x.Type.Equals("email", StringComparison.OrdinalIgnoreCase))?.Value)).FirstOrDefault();

                MyCart myCart = new MyCart();
                myCart = userApi.GetCart().Result.Where(c => c.usersId.Equals(user.UserId)).FirstOrDefault();

                foreach (var mycartData in myCart.cartDetails)
                {
                    if (mycartData.productId.Equals(id))
                    {
                        mycartData.quantity = quantity;
                        mycartData.price = quantity * mycartData.product.price;
                    }
                }
                userApi.EditCart(myCart);
            }
            else
            {
                string name = _distributedCache.GetStringAsync("cart").Result;
                _carts = JsonConvert.DeserializeObject<List<Cart>>(name);
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
            bool check = false;
            if (User.Identity.IsAuthenticated)
            {
                var user = userApi.GetUserData().Where(c => c.Email.Equals(User.Claims?.FirstOrDefault(x => x.Type.Equals("email", StringComparison.OrdinalIgnoreCase))?.Value)).FirstOrDefault();
                try
                {
                    string name = JsonConvert.SerializeObject(userApi.GetCart().Result);
                    if (JsonConvert.DeserializeObject<List<Cart>>(name) != null)
                    {
                        _carts = JsonConvert.DeserializeObject<List<Cart>>(name);
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
                    myCart = userApi.GetCart().Result.Where(c => c.usersId.Equals(user.UserId)).FirstOrDefault();
                    foreach (var mycartData in myCart.cartDetails)
                    {
                        if (mycartData.productId.Equals(id))
                        {
                            userApi.DeleteCartDetails(mycartData.id);
                        }
                    }
                    userApi.EditCart(myCart);
                }
            }
            else
            {


                string name = _distributedCache.GetStringAsync("cart").Result;
                _carts = JsonConvert.DeserializeObject<List<Cart>>(name);
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
                                        List<Cart> carts = _carts;
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
        public IActionResult CartOrder()
        {
            UserRegistration user;
            user = userApi.GetUserData().Where(c => c.Email.Equals(User.Claims?.FirstOrDefault(x => x.Type.Equals("email", StringComparison.OrdinalIgnoreCase))?.Value)).FirstOrDefault();
            var cartDataList = userApi.GetCart().Result;
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
            
            ViewData["userData"] = user;
            ViewBag.BrandList = _masterApi.GetList((int)Master.Brand);
            return View();
        }
        public IActionResult BuyCart(Checkout checkout)
        {
            UserRegistration user;
            user = userApi.GetUserData().Where(c => c.Email.Equals(User.Claims?.FirstOrDefault(x => x.Type.Equals("email", StringComparison.OrdinalIgnoreCase))?.Value)).FirstOrDefault();
            MyCart myCart = new MyCart();
            myCart = userApi.GetCart().Result.Where(c => c.usersId.Equals(user.UserId)).FirstOrDefault();
            foreach (var caratDetailsData in myCart.cartDetails)
            {
                var product = _opApi.GetAll().Result.Where(c => c.id.Equals(caratDetailsData.productId)).FirstOrDefault();
                caratDetailsData.product = product;
            }
            foreach(var data in myCart.cartDetails)
            {
                Checkout checkout1 = new Checkout();
                checkout1.addressId = checkout.addressId;
                Random rnd = new Random();
                checkout1.orderId = rnd.Next();
                checkout1.paymentModeId = checkout.paymentModeId;
                checkout1.userId = checkout.userId;
                checkout.status = OrderStatus.orderplaced;
                checkout.price = (int)data.price;
                bool result = userApi.CreateCheckOut(checkout);
            }
            userApi.DeleteCart(myCart.id);
            return View("orderplaced");
        }
        [HttpGet("/user/CancelCheckout/{orderId}")]
        public async Task<IActionResult> CancelCheckout(int orderId)
        {
            Checkout checkout = new Checkout();
            OrderStatus statuses = new OrderStatus();

            var checkoutData = userApi.GetCheckOut().Result.Where(c => c.orderId.Equals(orderId)).FirstOrDefault();
            checkoutData.status = OrderStatus.cancelled;
            checkoutData.cancelRequested = RoleTypes.User;
            userApi.EditCheckout(checkoutData);
            return Redirect("/");
        }
        [HttpGet]
        public IActionResult minus(int id)
        {
            bool check = false;
            if (User.Identity.IsAuthenticated)
            {
                var user = userApi.GetUserData().Where(c => c.Email.Equals(User.Claims?.FirstOrDefault(x => x.Type.Equals("email", StringComparison.OrdinalIgnoreCase))?.Value)).FirstOrDefault();

                MyCart myCart = new MyCart();
                myCart = userApi.GetCart().Result.Where(c => c.usersId.Equals(user.UserId)).FirstOrDefault();
                foreach (var caratDetailsData in myCart.cartDetails)
                {
                    var product = _opApi.GetAll().Result.Where(c => c.id.Equals(caratDetailsData.productId)).FirstOrDefault();
                    caratDetailsData.product = product;
                }
                foreach (var mycartData in myCart.cartDetails)
                {
                    if (mycartData.productId.Equals(id))
                    {
                        mycartData.quantity = mycartData.quantity -1;
                        mycartData.price = mycartData.quantity * mycartData.product.price;
                    }
                }
                userApi.EditCart(myCart);
            }
            else
            {
                string name = _distributedCache.GetStringAsync("cart").Result;
                _carts = JsonConvert.DeserializeObject<List<Cart>>(name);
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
                                        data1.quantity = data1.quantity -1;
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
        public IActionResult plus(int id)
        {
            bool check = false;
            if (User.Identity.IsAuthenticated)
            {
                var user = userApi.GetUserData().Where(c => c.Email.Equals(User.Claims?.FirstOrDefault(x => x.Type.Equals("email", StringComparison.OrdinalIgnoreCase))?.Value)).FirstOrDefault();

                MyCart myCart = new MyCart();
                myCart = userApi.GetCart().Result.Where(c => c.usersId.Equals(user.UserId)).FirstOrDefault();
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
                userApi.EditCart(myCart);
            }
            else
            {
                string name = _distributedCache.GetStringAsync("cart").Result;
                _carts = JsonConvert.DeserializeObject<List<Cart>>(name);
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
}




