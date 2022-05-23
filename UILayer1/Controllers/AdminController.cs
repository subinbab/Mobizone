using AspNetCoreHero.ToastNotification.Abstractions;
using DomainLayer;
using DomainLayer.ProductModel.Master;
using DomainLayer.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using UIlayer.Data.ApiServices;
using UILayer.Data.ApiServices;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using DomainLayer.ProductModel;
using Microsoft.AspNetCore.Http;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using DTOLayer.Product;
using DocumentFormat.OpenXml.Presentation;
using DTOLayer.UserModel;
using DomainLayer.Users;
using System.Threading.Tasks;
using UILayer.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

using UILayer.Controllers;
using BusinessObjectLayer.ProductOperations;
using Repository;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace UIlayer.Controllers
{
    
    [Authorize(Roles="Admin")]
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
        List<Cart> _carts;
        IEnumerable<UserRegistration> _userDataList;
        // for testing ////////////////////////////firebase//////////////////////////////////
        private static string apiKey = "AIzaSyBvGbaacBA91vzQfmvUsF77eAJSYn6b4VE";
        private static string Bucket = "mobizone-55ea5.appspot.com";
        private static string AuthEmail = "subinbabusd720@gmail.com";
        private static string AuthPassword = "Subin@1999";
        //////////////////////////////////////////////////////
        public AdminController(IConfiguration configuration, INotyfService notyf, IMapper mapper, IWebHostEnvironment webHostEnvironment, IDistributedCache distributedCache)
        {
            _notyf = notyf;
            Configuration = configuration;
            pr = new ProductApi(Configuration);
            data = new Product();
            _masterApi = new MasterApi(Configuration);
            _opApi = new ProductOpApi(Configuration,mapper,webHostEnvironment);
            _userApi = new UserApi(Configuration);
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
            _distributedCache = distributedCache;
            _carts = new List<Cart>();
        }

        public IActionResult Dashboard()
        {
            try
            {

            }
            catch (Exception ex)
            {

            }
            return View("Home");
        }
        #region Index page
        [Authorize(Roles="Admin")]
        public async Task<ActionResult> Index(int? i)
        {
            ViewBag.Title = "Admin - Product List";
            try
            {

            }
           catch(Exception ex)
            {

            }
            return View();
        }
        #endregion

        #region Product Details page
        [Authorize]
        [HttpGet("ProductDetails/{id}")]
        public ActionResult ProductDetails(int id)
        {
            ProductEntity details = null;
            try
            {
                details =  _opApi.GetProduct(id).Result;
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
            catch(Exception ex)
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
            try
            {
                if (product.id == 0)
                {
                    if (_opApi.GetAll().Result.Any(c => c.model.Equals(product.model)))
                    {
                        _notyf.Error("Product Already Exist");
                        return RedirectToAction("");
                    }
                    else
                    {
                        
                        bool result = _opApi.CreateProduct(product);
                        if (result)
                        {
                            _notyf.Success("Prduct added");
                            var data = _opApi.GetAll().Result.Where(c => c.model.Equals(product.model)).FirstOrDefault();
                            return RedirectToAction("ProductsSubPart", new { id = data.specsId, productId = data.id });
                        }
                        else
                        {
                            _notyf.Error("Not Added");
                            return RedirectToAction("");
                        }
                        
                    }
                }
                else
                {
                    
                    bool result = _opApi.EditProduct(product);
                    if (result)
                    {
                        _notyf.Success("Product Updated");
                        var data = _opApi.GetAll().Result.Where(c => c.model.Equals(product.model)).FirstOrDefault();
                        return RedirectToAction("ProductsSubPart", new { id = data.specsId, productId = data.id });
                    }
                    else
                    {
                        _notyf.Error("Not Updated");
                        return RedirectToAction("");
                    }
                    
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("");
            }
            
        }
        [HttpGet("/admin/edit/{id}")]
        [Authorize]
        public async Task<ActionResult> Edit(int id)
        {
            var product = await _opApi.GetProduct(id);
            var data = (ProductViewModel)_mapper.Map<ProductViewModel>(product);
            ViewBag.BrandList = _masterApi.GetList((int)Master.Brand); ;
            ViewBag.SimType = _masterApi.GetList((int)Master.SimType);
            ViewBag.ProductType = _masterApi.GetList((int)Master.ProductType);
            ViewBag.Processor = _masterApi.GetList((int)Master.OsProcessor);
            ViewBag.Core = _masterApi.GetList((int)Master.OsCore);
            ViewBag.Ram = _masterApi.GetList((int)Master.Ram);
            ViewBag.Storage = _masterApi.GetList((int)Master.Storage);
            ViewBag.camFeatures = _masterApi.GetList((int)Master.CamFeature);

            return View("Create",data);
        }
        [HttpPost]
        public ActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                bool result = pr.EditProduct(product);
            }
            return RedirectToAction("");
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
        [HttpPost("Delete")]
        public IActionResult DeleteProduct(int id)
        {
            bool result = _opApi.DeleteProduct(id);
            if (result)
            {
                _notyf.Success("deleted");
            }
            else
            {
                _notyf.Error("Not deleted");

            }
            return RedirectToAction("");
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
            if (masterData.Any(c=> c.masterData.Equals(data.masterData)))
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
            IEnumerable < MasterTable > data  = _masterApi.GetAll();
            var masterdata = data.Where(c=> id.Equals(c.parantId));
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
            return new JsonResult(data.OrderByDescending(c=>c.id));
        }
        [HttpGet("admin/ProductDetails/AddImage/{id}")]
        public async Task<IActionResult> AddImage(int  id)
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
                bool result = _opApi.EditProduct(mappedData);
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
            catch(Exception ex)
            {

            }
            
            return RedirectToAction("ProductDetails", new { id = product.id });
        }
        [Authorize]
        public IActionResult Userdata()
        {
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
                adminApi adminApi = new adminApi(Configuration,_mapper);
                LoginViewModel user = new LoginViewModel();
                user.username = userName;
                user.password = password;
                HttpContext.Session.SetString("testKey", "testValue");
                user.sessionId = HttpContext.Session.Id;
                Login check = adminApi.Authenticate(user);
                if (check != null)
                {
                    if (check.rolesId == (int)RoleTypes.Admin)
                    {
                        var claims = new List<Claim>();
                        claims.Add(new Claim(ClaimTypes.Name, " Admin"));
                        claims.Add(new Claim("email", user.username));
                        claims.Add(new Claim(ClaimTypes.NameIdentifier, " Admin"));
                        claims.Add(new Claim("password", user.password));
                        claims.Add(new Claim(ClaimTypes.Role, "Admin"));
                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                        await HttpContext.SignInAsync(claimsPrincipal);
                        return Redirect("/admin");
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
                        var count = 0; 
                        foreach(var cart in _carts)
                        {
                            if (cart.sessionId.Equals(check.sessionId))
                            {
                                var insertData = cart;
                                _carts.Insert(count, insertData);
                            }
                            count++;
                        }
                        UserApi userApi = new UserApi(Configuration);
                        _userDataList = userApi.GetUserData();
                        var claims = new List<Claim>();
                        claims.Add(new Claim(ClaimTypes.Name, _userDataList.Where(c => c.Email.Equals(user.username)).FirstOrDefault().FirstName + " " + _userDataList.Where(c => c.Email.Equals(user.username)).FirstOrDefault().LastName));
                        claims.Add(new Claim("email", user.username));
                        claims.Add(new Claim(ClaimTypes.NameIdentifier, _userDataList.Where(c=>c.Email.Equals(user.username)).FirstOrDefault().FirstName + " " + _userDataList.Where(c => c.Email.Equals(user.username)).FirstOrDefault().LastName));
                        claims.Add(new Claim("password", user.password));
                        claims.Add(new Claim(ClaimTypes.Role, "User"));
                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                        await HttpContext.SignInAsync(claimsPrincipal);
                        return Redirect("/user");


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
                TempData["Error"] = "Load error please try again";
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
        public async Task<IActionResult> OrderUpdate(string status ,int orderId)
        {
            Checkout checkout = new Checkout();
            UserApi userApi = new UserApi(Configuration);   
            OrderStatus statuses = new OrderStatus();
            
                var checkoutData = userApi.GetCheckOut().Result.Where(c => c.orderId.Equals(orderId)).FirstOrDefault();
            if((OrderStatus)Enum.Parse(typeof(OrderStatus), status) == DomainLayer.OrderStatus.cancelled)
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
            if(id == 0)
            {
                return View("Index");
            }
            var checkoutList = _userApi.GetCheckOut().Result;
            var checkout = checkoutList.Where(c => c.orderId.Equals(id)).FirstOrDefault();
            var ProductDetails = _opApi.GetAll().Result.Where(c=> c.id.Equals(checkout.productId)).FirstOrDefault();
            ViewData["ProuductDetails"] = ProductDetails;
            ViewData["Address"] = _userApi.GetAddress().Result.Where(c => c.id.Equals(checkout.addressId)).FirstOrDefault();
            return View(checkout);
        }

        public IActionResult Contact()
        {

            adminApi _adminApi = new adminApi(Configuration,_mapper);
            var contactData = _adminApi.ContactGet().Result.FirstOrDefault();
            return View(contactData);
           
        }
        [HttpGet]
        public IActionResult ContactEdit()
        {
            adminApi _adminApi = new adminApi(Configuration,_mapper);
          var contactData = _adminApi.ContactGet().Result.FirstOrDefault();
            return View(contactData);
         
        }
        [HttpPost]
        public IActionResult ContactEdit(AdminContact contact)
        {
            adminApi _adminApi = new adminApi(Configuration,_mapper);
            _adminApi.EditContact(contact);
            return RedirectToAction("Contact");
        }


        [AllowAnonymous]
        public IActionResult Privacy()
        {
            adminApi _adminApi = new adminApi(Configuration,_mapper);
            var privacyData = _adminApi.PrivacyGet().Result.FirstOrDefault();
            return View(privacyData);
           
        }
        [HttpGet]
        public IActionResult PrivacyEdit()
        {
            adminApi _adminApi = new adminApi(Configuration,_mapper);
            var privacyData = _adminApi.PrivacyGet().Result.FirstOrDefault();
            return View(privacyData);
        }
        [HttpPost]
        public IActionResult PrivacyEdit(PrivacyPolicy privacy)
        {
            adminApi _adminApi = new adminApi(Configuration,_mapper);
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
        [HttpGet("admin/ProductDetails/DeleteImage/{id}")]
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
            bool result = imageApi.Delete(id);
            if (result)
            {
                _notyf.Success("Image deleted");
            }
            else
            {
                _notyf.Error("Not deleted");
            }
            return RedirectToAction("ProductDetails", new {id=sample.ProductEntityId});
        }
        [HttpPost]
        public async Task<ActionResult> quatity(ProductEntity product, string newQuantity)
        {
            var datalist = await _opApi.GetAll();
            ProductEntity products = new ProductEntity();
            products = datalist.ToList().Where(c => c.model.Equals(product.model)).FirstOrDefault();
            products.quantity = product.quantity + Convert.ToInt32(newQuantity);
            var productEntity = (ProductViewModel)_mapper.Map<ProductViewModel>(products);
            bool result = _opApi.EditProduct(productEntity);
            return RedirectToAction("Index");
        }
        [RequestFormLimits(MultipartBodyLengthLimit = 104857600)]
        [HttpGet("denied")]
        [AllowAnonymous]
        public IActionResult denied(string returnUrl)
        {
            return View();
        }
        [HttpGet("admin/disable/{id}")]
        public async  Task<IActionResult> Disable(int id)
        {
            var datas = await _opApi.GetProduct();
            var data = datas.Where(c => c.id == id).FirstOrDefault();
            data.status = 1;
            var mappedData = (ProductViewModel)_mapper.Map<ProductViewModel>(data);
            _opApi.EditProduct(mappedData);
            return RedirectToAction("Index");
        }
        [HttpGet("/admin/Enable/{id}")]
        public async Task<IActionResult> Enable(int id)
        {
            var datas = await _opApi.GetProduct();
            var data = datas.Where(c => c.id == id).FirstOrDefault();
            data.status = 0;
            var mappedData = (ProductViewModel)_mapper.Map<ProductViewModel>(data);
            _opApi.EditProduct(mappedData);
            return RedirectToAction("Index");
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
            foreach(Ram ram in data)
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
        public IActionResult ProductsSubPart(ProductSubPart productSubPart,string storage , string ram)
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
            return new JsonResult(EnumConvertion.EnumToString<OrderStatus>());
        }
        [HttpGet]
        public IActionResult RoleType()
        {
            return new JsonResult(EnumConvertion.EnumToString<RoleTypes>());
        }
    }
}
