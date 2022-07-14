using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using BusinessObjectLayer.ProductOperations;
using DomainLayer;
using DomainLayer.ProductModel;
using DomainLayer.ProductModel.Master;
using DomainLayer.Users;
using DTOLayer.Product;
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
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using UIlayer.Data.ApiServices;
using UILayer.Data.ApiServices;
using UILayer.Models;

namespace UIlayer.Controllers
{

    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        Product data = null;
        IConfiguration Configuration { get; }
        ProductApi pr;
        MasterApi _masterApi;
        ProductOpApi _opApi;
        UserApi _userApi;
        private readonly INotyfService _notyf;
        private readonly IMapper _mapper;
        private readonly IDistributedCache _distributedCache;
        private readonly IWebHostEnvironment _webHostEnvironment;
        List<MyCart> _carts;
        IEnumerable<UserRegistration> _userDataList;

        public AdminController(IConfiguration configuration, INotyfService notyf, IMapper mapper, IWebHostEnvironment webHostEnvironment, IDistributedCache distributedCache)
        {
            _notyf = notyf;
            Configuration = configuration;
            pr = new ProductApi(Configuration);
            data = new Product();
            _masterApi = new MasterApi(Configuration);
            _opApi = new ProductOpApi(Configuration, mapper, webHostEnvironment);
            _userApi = new UserApi(Configuration);
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
            _distributedCache = distributedCache;
            _carts = new List<MyCart>();
            if (ViewBag.CartCount == null)
            {
                ViewBag.CartCount = 0;
            }
        }

        public IActionResult List()
        {
            try
            {

            }
            catch (Exception ex)
            {

            }
            return View("Index");
        }
        #region Index page
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Index(int? i)
        {
            var username = User.Claims?.FirstOrDefault(x => x.Type.Equals("email", StringComparison.OrdinalIgnoreCase))?.Value;
            var password = User.Claims?.FirstOrDefault(x => x.Type.Equals("password", StringComparison.OrdinalIgnoreCase))?.Value;
            ViewBag.Title = "Admin - Product List";
            try
            {
                ViewBag.UsersCount = _userApi.GetUserData().Count();
                ViewBag.ProductCount = _opApi.GetAll().Result.Count();
            }
            catch (Exception ex)
            {

            }
            return View("Home");
        }
        #endregion

        #region Product Details page
        [Authorize]
        public ActionResult ProductDetails(int id)
        {
            ViewBag.ReturnUrl = "/admin/ProductDetails?id=" + id;
            ProductEntity details = null;
            try
            {
                details = _opApi.GetProduct(id).Result;
            }
            catch (Exception ex)
            {
                details = null;
            }
            return View(details);
        }
        #endregion

        #region ProductCreate page
        [Authorize]
        public ActionResult Create()
        {
            try
            {
                ViewBag.BrandList = _masterApi.GetList((int)Master.Brand);
                ViewBag.SimType = _masterApi.GetList((int)Master.SimType);
                ViewBag.ProductType = _masterApi.GetList((int)Master.ProductType);
                ViewBag.Processor = _masterApi.GetList((int)Master.OsProcessor);
                ViewBag.Core = _masterApi.GetList((int)Master.OsCore);
                ViewBag.Ram = _masterApi.GetList((int)Master.Ram);
                ViewBag.Storage = _masterApi.GetList((int)Master.Storage);
                ViewBag.camFeatures = _masterApi.GetList((int)Master.CamFeature);
            }
            catch (Exception ex)
            {
                ViewBag.BrandList = null;
                ViewBag.SimType = null;
                ViewBag.ProductType = null;
                ViewBag.Processor = null;
                ViewBag.Core = null;
                ViewBag.Ram = null;
                ViewBag.Storage = null;
                ViewBag.camFeatures = null;
            }
            return View();
        }
        #endregion`
        [HttpPost]
        [Authorize]
        public ActionResult Create(ProductViewModel product)
        {
            ViewBag.BrandList = _masterApi.GetList((int)Master.Brand);
            ViewBag.SimType = _masterApi.GetList((int)Master.SimType);
            ViewBag.ProductType = _masterApi.GetList((int)Master.ProductType);
            ViewBag.Processor = _masterApi.GetList((int)Master.OsProcessor);
            ViewBag.Core = _masterApi.GetList((int)Master.OsCore);
            ViewBag.Ram = _masterApi.GetList((int)Master.Ram);
            ViewBag.Storage = _masterApi.GetList((int)Master.Storage);
            ViewBag.camFeatures = _masterApi.GetList((int)Master.CamFeature);
            try
            {
                if (product.id == 0)
                {
                    if (_opApi.GetAll().Result.Any(c => c.model.Equals(product.model)))
                    {
                        _notyf.Error("Product Already Exist");
                        return View("Create",product);
                    }
                    else
                    {

                        bool result = _opApi.CreateProduct(product).Result;
                        if (result)
                        {
                            _notyf.Success("Product added");
                            var data = _opApi.GetAll().Result.Where(c => c.model.Equals(product.model)).FirstOrDefault();
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            _notyf.Error("Not Added");
                            return RedirectToAction("Create");
                        }

                    }
                }
                else
                {
                    bool result = _opApi.EditProduct(product).Result;
                    if (result)
                    {
                        _notyf.Success("Product Updated");
                        var data = _opApi.GetAll().Result.Where(c => c.model.Equals(product.model)).FirstOrDefault();
                        return RedirectToAction("ProductDetails", new { id = product.id });
                    }
                    else
                    {
                        _notyf.Error("Not Updated");
                        return RedirectToAction("Create");
                    }

                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Create");
            }

        }
        [HttpGet("/admin/edit/{id}")]
        [Authorize]
        public async Task<ActionResult> Edit(int id)
        {
            List<string> ramList = _masterApi.GetList((int)Master.Ram);
            List<string> storageList = _masterApi.GetList((int)Master.Storage);
            var product = await _opApi.GetProduct(id);
            var data = (ProductViewModel)_mapper.Map<ProductViewModel>(product);
            data.specs.ram = new List<string>();
            foreach (var rams in product.specs.rams)
            {
                data.specs.ram.Add(rams.ram);
                ramList.Remove(rams.ram);
            }
            data.specs.storage = new List<string>();
            foreach (var storages in product.specs.storages)
            {
                data.specs.storage.Add(storages.storage);
                storageList.Remove(storages.storage);
            }
            ViewBag.BrandList = _masterApi.GetList((int)Master.Brand); ;
            ViewBag.SimType = _masterApi.GetList((int)Master.SimType);
            ViewBag.ProductType = _masterApi.GetList((int)Master.ProductType);
            ViewBag.Processor = _masterApi.GetList((int)Master.OsProcessor);
            ViewBag.Core = _masterApi.GetList((int)Master.OsCore);
            ViewBag.Ram = _masterApi.GetList((int)Master.Ram);
            ViewBag.Storage = _masterApi.GetList((int)Master.Storage);
            ViewBag.camFeatures = _masterApi.GetList((int)Master.CamFeature);

            return View("Create", data);
        }
        [HttpPost]
        public ActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                bool result = pr.EditProduct(product);
            }
            return RedirectToAction("ProductDetails");
        }

        [HttpGet("Delete/{id}")]
        public IActionResult Delete(int id)
        {

            /*bool result = _opApi.DeleteProduct(id);*/
            /*if (result)
            {
                _notyf.Success("deleted");
            }
            else
            {
                _notyf.Error("Not deleted");

            }*/
            var data = _opApi.GetProduct(id).Result;

            return PartialView("Delete", data);
        }
        [HttpPost("admin/Delete")]
        public IActionResult DeleteProduct(ProductEntity data)
        {
            bool result = _opApi.DeleteProduct(data.id);
            if (result)
            {
                _notyf.Success("deleted");
            }
            else
            {
                _notyf.Error("Not deleted");

            }
            return RedirectToAction("List");
        }
        public ActionResult prductmodel()
        {
            return View();
        }
        [HttpGet]
        [Authorize]
        public ActionResult MasterData()
        {
            /*var enumData = from Master e in Enum.GetValues(typeof(Master))
                           select new
                           {
                               ID = (int)e,
                               Name = e.ToString()
                           };
            ViewBag.EnumList = new SelectList(enumData, "ID", "Name");*/
            return View();

        }
        [HttpPost]
        public ActionResult MasterData(MasterTable data)
        {
            IEnumerable<MasterTable> masterData = _masterApi.GetAll();
            if (masterData.Any(c => c.masterData.Equals(data.masterData)))
            {
                _notyf.Error("data already exist");
            }
            else
            {
                bool result = _masterApi.Add(data);
                if (result)
                {
                    _notyf.Success(Configuration.GetSection("Master")["MasterAdded"].ToString());
                }
                else
                {
                    _notyf.Error(Configuration.GetSection("Master")["MasterAddedError"].ToString());
                }
            }

            ModelState.Clear();
            return View();
        }
        [HttpGet("MasterList")]
        [Authorize]
        public ActionResult MasterList(int id)
        {
            IEnumerable<MasterTable> data = _masterApi.GetAll();
            var masterdata = data.Where(c => id.Equals(c.parantId));
            ViewBag.MasterTitle = (Master)id;
            return View(masterdata);
        }
        /* [HttpGet]
         [Authorize]*/
        /* public async Task<ActionResult> MasterEdit(int id)
         {
            *//* var product = await _opApi.GetProduct(id);
             var data = (ProductViewModel)_mapper.Map<ProductViewModel>(product);
             ViewBag.BrandList = _masterApi.GetList((int)Master.Brand); ;
             ViewBag.SimType = _masterApi.GetList((int)Master.SimType);
             ViewBag.ProductType = _masterApi.GetList((int)Master.ProductType);
             ViewBag.Processor = _masterApi.GetList((int)Master.OsProcessor);
             ViewBag.Core = _masterApi.GetList((int)Master.OsCore);
             ViewBag.Ram = _masterApi.GetList((int)Master.Ram);
             ViewBag.Storage = _masterApi.GetList((int)Master.Storage);
             ViewBag.camFeatures = _masterApi.GetList((int)Master.CamFeature);

             return *//*Redirect("Index");*/
        /* }
         [HttpPost]
         public ActionResult MasterEdit(Product product)
         {
             if (ModelState.IsValid)
             {
                 bool result = pr.EditProduct(product);
             }
             return RedirectToAction("");
         }*/

        /* [HttpGet("MasterDelete")]
         public ActionResult MasterDelete(int id)
         {

             bool result = _masterApi.Delete(id);
             if (result)
             {
                 _notyf.Success(Configuration.GetSection("Master")["MasterDeleted"].ToString());
             }
             else
             {
                 _notyf.Error(Configuration.GetSection("Master")["MasterDeletedError"].ToString());
             }
             return RedirectToAction("MasterList");
         }*/
        [HttpGet("ProductList")]
        [Authorize]
        public async Task<ActionResult> ProductList()
        {

            var data = await _opApi.GetProduct();
            return new JsonResult(data.OrderByDescending(c => c.id));
        }
        [HttpGet("admin/ProductDetails/AddImage/{id}")]
        public async Task<IActionResult> AddImage(int id)
        {
            var productEntity = await _opApi.GetProduct(id);


            ViewData["images"] = productEntity.images;
            var data = (ProductViewModel)_mapper.Map<ProductViewModel>(productEntity);
            return PartialView(data);
        }
        [HttpPost("AddImages")]
        public async Task<IActionResult> AddImages(ProductViewModel product)
        {
            ProductEntity details = null;
            try
            {

                var data = _opApi.GetAll().Result.Where(c => c.id.Equals(product.id)).FirstOrDefault();
                var mappedData = (ProductViewModel)_mapper.Map<ProductViewModel>(data);
                mappedData.imageFile = product.imageFile;
                bool result = _opApi.EditProduct(mappedData).Result;
                details = _opApi.GetProduct(product.id).Result;
                if (result)
                {
                    _notyf.Success("Image added");
                }
                else
                {
                    _notyf.Error("Not Added");
                }


            }
            catch (Exception ex)
            {

            }

            return RedirectToAction("ProductDetails", new { id = product.id });
        }
        [Authorize]
        public IActionResult Userdata()
        {
            var username = User.Claims?.FirstOrDefault(x => x.Type.Equals("email", StringComparison.OrdinalIgnoreCase))?.Value;
            var password = User.Claims?.FirstOrDefault(x => x.Type.Equals("password", StringComparison.OrdinalIgnoreCase))?.Value;
            UserApi userApi = new UserApi(Configuration);
            _userDataList = userApi.GetUserData();
            return View(_userDataList);
        }

        public IActionResult _ImagePreview()
        {
            return PartialView();
        }
        [AllowAnonymous]
        [HttpGet("login")]
        public IActionResult Login(string returnUrl)

        {
            /*var myString = returnUrl;
            myString = myString.Substring(1);*/
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }
        [AllowAnonymous]
        [HttpPost("/Login")]
        public async Task<IActionResult> Validate(string userName, string password, string ReturnUrl)
        {
            try
            {
                adminApi adminApi = new adminApi(Configuration, _mapper);
                LoginViewModel user = new LoginViewModel();
                user.username = userName;
                user.password = password;
                HttpContext.Session.SetString("testKey", "testValue");
                user.sessionId = HttpContext.Session.Id;
                Login check = adminApi.Authenticate(user);
                UserRegistration userData = _userApi.GetUserData().Where(c => c.Email.Equals(check.username)).FirstOrDefault();
                if (check != null)
                {
                    if (check.rolesId == (int)RoleTypes.Admin)
                    {
                        var claims = new List<Claim>();
                        claims.Add(new Claim(ClaimTypes.Name, " Admin"));
                        claims.Add(new Claim("email", user.username));
                        claims.Add(new Claim(ClaimTypes.NameIdentifier, " Admin"));
                        claims.Add(new Claim("password", user.password));
                        claims.Add(new Claim("Role", "Admin"));
                        claims.Add(new Claim(ClaimTypes.Role, "Admin"));
                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                        await HttpContext.SignInAsync(claimsPrincipal);
                        return Redirect("/admin");
                    }
                    else
                    {

                        UserApi userApi = new UserApi(Configuration);
                        _userDataList = userApi.GetUserData();
                        var claims = new List<Claim>();
                        claims.Add(new Claim(ClaimTypes.Name, _userDataList.Where(c => c.Email.Equals(user.username)).FirstOrDefault().FirstName + " " + _userDataList.Where(c => c.Email.Equals(user.username)).FirstOrDefault().LastName));
                        claims.Add(new Claim("email", user.username));
                        claims.Add(new Claim(ClaimTypes.NameIdentifier, _userDataList.Where(c => c.Email.Equals(user.username)).FirstOrDefault().FirstName + " " + _userDataList.Where(c => c.Email.Equals(user.username)).FirstOrDefault().LastName));
                        claims.Add(new Claim("password", user.password));
                        claims.Add(new Claim(ClaimTypes.Role, "User"));
                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                        await HttpContext.SignInAsync(claimsPrincipal);
                        var username = User.Claims?.FirstOrDefault(x => x.Type.Equals("email", StringComparison.OrdinalIgnoreCase))?.Value;
                        string name = _distributedCache.GetStringAsync("cart").Result;
                        if (name != null)
                        {
                            _carts = JsonConvert.DeserializeObject<List<MyCart>>(name);
                        }

                        var cartsfromDb = _userApi.GetCart().Result.Where(c => c.usersId.Equals(userData.UserId)).FirstOrDefault();
                        if(_carts != null)
                        {
                            if (_carts.Any(c => c.sessionId.Equals(check.sessionId)))
                            {
                                var cartWithSameSessionId = _carts.Where(c => c.sessionId.Equals(check.sessionId));
                                foreach (var cart in cartWithSameSessionId)
                                {
                                    if (cart.sessionId.Equals(check.sessionId))
                                    {
                                        foreach (var cartDetailsData in cart.cartDetails.ToList())
                                        {
                                            if (cartsfromDb != null)
                                            {
                                                if (cartsfromDb.cartDetails.Any(c => c.productId.Equals(cartDetailsData.productId)))
                                                {
                                                    foreach (var cartDetailsFromDb in cartsfromDb.cartDetails.ToList())
                                                    {
                                                        if (cartDetailsData.productId.Equals(cartDetailsFromDb.productId))
                                                        {
                                                            cartDetailsFromDb.quantity = cartDetailsFromDb.quantity + 1;
                                                        }

                                                    }
                                                }
                                                else
                                                {
                                                    cartsfromDb.cartDetails.Add(cartDetailsData);
                                                }

                                            }

                                        }
                                        _userApi.EditCart(cartsfromDb);
                                    }
                                    else
                                    {
                                        _userApi.Createcart(cart);
                                    }

                                }

                            }
                        }
                        


                    }
                    await _distributedCache.SetStringAsync("cart", JsonConvert.SerializeObject(""));
                    if (ReturnUrl == null)
                    {
                        return Redirect("/");
                    }
                    else
                    {
                        return Redirect(ReturnUrl);
                    }
                }
                else
                {
                    TempData["Error"] = "Invalid Email or Password !";
                    return Redirect("login");
                }

            }
            catch (Exception ex)
            {
                TempData["Error"] = "Invalid Email or Password !";
                return Redirect("login");
            }

        }
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();

            return View("login");
        }
        [HttpGet]
        public IActionResult OrderList()
        {
            /* UserApi userApi = new UserApi(Configuration);
             var data = userApi.GetCheckOut().Result;
             if(data != null)
             {
                 List<string> productName = new List<string>();
                 foreach (var checkoutData in data)
                 {
                     productName.Add(_opApi.GetAll().Result.Where(c => c.id.Equals(checkoutData.productId)).FirstOrDefault().name);
                 }
                 ViewData["ProductList"] = productName;
             }*/
            return View("OrderList");

        }
        [HttpPost("Admin/user/checkout")]
        public async Task<IActionResult> OrderUpdate(string status, int orderId)
        {
            Checkout checkout = new Checkout();
            UserApi userApi = new UserApi(Configuration);
            OrderStatus statuses = new OrderStatus();

            var checkoutData = userApi.GetCheckOut().Result.Where(c => c.orderId.Equals(orderId)).FirstOrDefault();
            if ((OrderStatus)Enum.Parse(typeof(OrderStatus), status) == DomainLayer.OrderStatus.cancelled)
            {
                checkoutData.cancelRequested = RoleTypes.Admin;
            }
            checkoutData.status = (OrderStatus)Enum.Parse(typeof(OrderStatus), status);
            userApi.EditCheckout(checkoutData);


            var data = await userApi.GetCheckOut();
            return RedirectToAction("OrderList");
        }
        [HttpGet("/admin/orderDetails/{id}")]
        public IActionResult orderDetails(int id)
        {
            if (id == 0)
            {
                return View("Index");
            }
            var checkoutList = _userApi.GetCheckOut().Result;
            var checkout = checkoutList.Where(c => c.orderId.Equals(id)).FirstOrDefault();
            var ProductDetails = _opApi.GetAll().Result.Where(c => c.id.Equals(checkout.productId)).FirstOrDefault();
            ViewData["ProductDetails"] = ProductDetails;
            ViewData["Address"] = _userApi.GetAddress().Result.Where(c => c.id.Equals(checkout.addressId)).FirstOrDefault();
            return View(checkout);
        }

        public IActionResult Contact()
        {

            adminApi _adminApi = new adminApi(Configuration, _mapper);
            var contactData = _adminApi.ContactGet().Result.FirstOrDefault();
            return View(contactData);

        }
        [HttpGet]
        public IActionResult ContactEdit()
        {
            adminApi _adminApi = new adminApi(Configuration, _mapper);
            var contactData = _adminApi.ContactGet().Result.FirstOrDefault();
            return View(contactData);

        }
        [HttpPost]
        public IActionResult ContactEdit(AdminContact contact)
        {
            adminApi _adminApi = new adminApi(Configuration, _mapper);
            _adminApi.EditContact(contact);
            return RedirectToAction("Contact");
        }


        [AllowAnonymous]
        public IActionResult Privacy()
        {
            adminApi _adminApi = new adminApi(Configuration, _mapper);
            var privacyData = _adminApi.PrivacyGet().Result.FirstOrDefault();
            return View(privacyData);

        }
        [HttpGet]
        public IActionResult PrivacyEdit()
        {
            adminApi _adminApi = new adminApi(Configuration, _mapper);
            var privacyData = _adminApi.PrivacyGet().Result.FirstOrDefault();
            return View(privacyData);
        }
        [HttpPost]
        public IActionResult PrivacyEdit(PrivacyPolicy privacy)
        {
            adminApi _adminApi = new adminApi(Configuration, _mapper);
            _adminApi.EditPrivacy(privacy);
            return RedirectToAction("Privacy");

        }

        public IActionResult About()
        {
            adminApi _adminApi = new adminApi(Configuration, _mapper);
            var aboutData = _adminApi.AboutGet().Result.FirstOrDefault();
            return View(aboutData);
        }
        [HttpGet]
        public IActionResult AboutEdit()
        {
            adminApi _adminApi = new adminApi(Configuration, _mapper);
            var aboutData = _adminApi.AboutGet().Result.FirstOrDefault();
            return View(aboutData);
        }
        [HttpPost]
        public IActionResult AboutEdit(About about)
        {
            adminApi _adminApi = new adminApi(Configuration, _mapper);
            _adminApi.EditAbout(about);
            return RedirectToAction("About");
        }
        public IActionResult Company()
        {
            return View();
        }
        public async Task<IActionResult> DeleteImage(int id)
        {
            ImageApi imageApi = new ImageApi(Configuration);
            var data = imageApi.Get();
            var sample = data.Where(c => c.id.Equals(id)).FirstOrDefault();
            return PartialView("ImageDelete", sample);
        }
        [HttpPost]
        public async Task<IActionResult> ImageDelete(int id)
        {
            ImageApi imageApi = new ImageApi(Configuration);
            var data = imageApi.Get();
            var sample = data.Where(c => c.id.Equals(id)).FirstOrDefault();
            var products = await _opApi.GetAll();
            var product = products.Where(c => c.id.Equals(sample.ProductEntityId)).FirstOrDefault();
            bool result = await imageApi.DeleteAsync(id, sample.imagePath);
            if (result)
            {
                _notyf.Success("Image deleted");
            }
            else
            {
                _notyf.Error("Not deleted");
            }
            return RedirectToAction("ProductDetails", new { id = sample.ProductEntityId });
        }
        [HttpPost]
        public async Task<ActionResult> quatity(ProductEntity product, string newQuantity)
        {
            var datalist = await _opApi.GetAll();
            ProductEntity products = new ProductEntity();
            products = datalist.ToList().Where(c => c.model.Equals(product.model)).FirstOrDefault();
            products.quantity = products.quantity + Convert.ToInt32(newQuantity);
            var productEntity = (ProductViewModel)_mapper.Map<ProductViewModel>(products);
            productEntity.specs.ram = new List<string>();
            if (products.specs.rams != null || products.specs.rams.Count() > 0)
            {
                foreach (var data in products.specs.rams)
                {
                    productEntity.specs.ram.Add(data.ram);
                }
            }
            productEntity.specs.storage = new List<string>();
            if (products.specs.storages != null || products.specs.storages.Count() > 0)
            {
                foreach (var data in products.specs.storages)
                {
                    productEntity.specs.storage.Add(data.storage);
                }
            }

            bool result = _opApi.EditProduct(productEntity).Result;
            return RedirectToAction("ProductDetails", new { id = products.id });
        }
        [RequestFormLimits(MultipartBodyLengthLimit = 104857600)]
        [HttpGet("denied")]
        [AllowAnonymous]
        public IActionResult denied(string returnUrl)
        {
            return View();
        }
        [HttpGet("admin/disable/{id}")]
        public async Task<IActionResult> Disable(int id, string returnUrl)
        {
            var datas = await _opApi.GetAll();
            var data = datas.Where(c => c.id == id).FirstOrDefault();
            data.status = ProductStatus.disable;
            var mappedData = (ProductViewModel)_mapper.Map<ProductViewModel>(data);
            mappedData.specs.ram = new List<string>();
            if (data.specs.rams != null || data.specs.rams.Count() > 0)
            {
                foreach (var ramData in data.specs.rams)
                {
                    mappedData.specs.ram.Add(ramData.ram);
                }
            }
            mappedData.specs.storage = new List<string>();
            if (data.specs.storages != null || data.specs.storages.Count() > 0)
            {
                foreach (var storageData in data.specs.storages)
                {
                    mappedData.specs.storage.Add(storageData.storage);
                }
            }
            _opApi.EditProduct(mappedData);
            if (returnUrl != null)
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("List");
        }
        [HttpGet("/admin/Enable/{id}")]
        public async Task<IActionResult> Enable(int id, string returnUrl)
        {
            var datas = await _opApi.GetAll();
            var data = datas.Where(c => c.id == id).FirstOrDefault();
            data.status = 0;
            var mappedData = (ProductViewModel)_mapper.Map<ProductViewModel>(data);
            mappedData.specs.ram = new List<string>();
            if (data.specs.rams != null || data.specs.rams.Count() > 0)
            {
                foreach (var ramData in data.specs.rams)
                {
                    mappedData.specs.ram.Add(ramData.ram);
                }
            }
            mappedData.specs.storage = new List<string>();
            if (data.specs.storages != null || data.specs.storages.Count() > 0)
            {
                foreach (var storageData in data.specs.storages)
                {
                    mappedData.specs.storage.Add(storageData.storage);
                }
            }
            _opApi.EditProduct(mappedData);
            if (returnUrl != null)
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("List");
        }
        [HttpGet]
        public IActionResult EditMaster(int id)
        {
            var datas = _masterApi.GetAll();
            var data = datas.Where(c => c.id.Equals(id)).FirstOrDefault();
            return PartialView("EditMaster1", data);
        }
        [HttpPost]
        public IActionResult MasterEdit(MasterTable data)
        {
            bool result = _masterApi.Edit(data);
            return RedirectToAction("MasterList", new { id = data.parantId });
        }
        [HttpGet("admin/EditMaster/{id}")]
        public IActionResult DeleteMaster(int id)
        {
            var datas = _masterApi.GetAll();
            var data = datas.Where(c => c.id.Equals(id)).FirstOrDefault();
            return PartialView("EditMaster1", data);
        }
        [HttpPost]
        public IActionResult DeleteMaster(MasterTable data)
        {
            bool result = _masterApi.Edit(data);
            return RedirectToAction("MasterList", new { id = data.parantId });
        }
        [HttpGet]
        public IActionResult ProductsSubPart(int id, int productId)
        {
            var datas = _opApi.GetRams();
            var data = datas.Where(c => c.specificatiionid.Equals(id));
            List<string> rams = new List<string>();
            foreach (Ram ram in data)
            {
                rams.Add(ram.ram);
            }
            ViewBag.Rams = rams;
            ViewBag.ProductId = productId;
            var data2 = _opApi.GetStorages().Where(c => c.specificationid.Equals(id));
            List<string> storages = new List<string>();
            foreach (Storage s in data2)
            {
                storages.Add(s.storage);
            }
            ViewBag.Storages = storages;
            return View();
        }
        public IActionResult ProductsSubPart(ProductSubPart productSubPart, string storage, string ram)
        {
            var rams = _opApi.GetRams().Where(c => c.ram.Equals(ram)).FirstOrDefault();
            var storages = _opApi.GetStorages().Where(c => c.storage.Equals(storage)).FirstOrDefault();
            productSubPart.ramId = rams.id;
            productSubPart.storageId = storages.id;
            bool result = _opApi.AddProductSubPart(productSubPart);
            return RedirectToAction("");
        }
        [HttpGet]
        public IActionResult OrderStatus()
        {
            var data = EnumConvertion.EnumToString<OrderStatus>();
            return new JsonResult(data);
        }
        [HttpGet]
        public IActionResult RoleType()
        {
            return new JsonResult(EnumConvertion.EnumToString<RoleTypes>());
        }
        [HttpGet("RemoveRam/{id}")]
        public IActionResult RemoveRam(int id)
        {
            var data = _opApi.GetRams().Where(c => c.id.Equals(id)).FirstOrDefault();
            return View();
        }
        [HttpGet("Admin/MasterDelete/{id}")]
        public PartialViewResult MasterDelete(int id)
        {
            var result = _masterApi.GetAll().Where(c => c.id.Equals(id)).FirstOrDefault();
            return PartialView(result);
        }
        [HttpPost]
        public IActionResult MasterDelete(MasterTable masterTable)
        {
            var result = _masterApi.Delete(masterTable.id);
            if (result)
            {
                _notyf.Success("Master Data is deleted succesfully");
            }
            else
            {
                _notyf.Error("Master Data is not deleted ");
            }  
            return Redirect("/masterlist?id="+masterTable.parantId);
        }
    }
}
