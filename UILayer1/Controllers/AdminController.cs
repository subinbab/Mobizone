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

namespace UIlayer.Controllers
{
    
    [Authorize]
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
        private readonly IWebHostEnvironment _webHostEnvironment;
        IEnumerable<UserRegistration> _userDataList;
        public AdminController(IConfiguration configuration, INotyfService notyf, IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            _notyf = notyf;
            Configuration = configuration;
            pr = new ProductApi(Configuration);
            data = new Product();
            _masterApi = new MasterApi(Configuration);
            _opApi = new ProductOpApi(Configuration);
            _userApi = new UserApi(Configuration);
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
        }
        #region Index page
        [Authorize]
        public async Task<ActionResult> Index(int? i)
        {
            IEnumerable<ProductListViewModel> productList = null;
            try
            {
                productList = await _opApi.GetProduct();
            }
            catch(Exception ex)
            {

            }
           
            return View(productList);
        }
        #endregion

        #region Product Details page
        [Authorize]
        [HttpGet]
        public async Task<ActionResult> ProductDetails(int id)
        {
            ProductEntity details = null;
            try
            {
                details = await _opApi.GetProduct(id);
            }
            catch (Exception ex)
            {

            }

          /*  ProductEntity productEntity = new ProductEntity();
            productEntity =  lisdatas.Where(c => c.id.Equals(id)).FirstOrDefault();
            ViewData["images"] = productEntity.images;
            IEnumerable<ProductEntity> products = await _opApi.GetProduct();*/
/*            ProductEntity data =  products.Where(c => c.id.Equals(id)).FirstOrDefault();*/
            
            return View(details);
        }
        #endregion

        #region ProductCreate page
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.BrandList = _masterApi.GetList((int)Master.Brand); ;
            ViewBag.SimType = _masterApi.GetList((int)Master.SimType);
            ViewBag.ProductType = _masterApi.GetList((int)Master.ProductType);
            ViewBag.Processor = _masterApi.GetList((int)Master.OsProcessor);
            ViewBag.Core = _masterApi.GetList((int)Master.OsCore);
            ViewBag.Ram = _masterApi.GetList((int)Master.Ram);
            ViewBag.Storage = _masterApi.GetList((int)Master.Storage);
            ViewBag.camFeatures = _masterApi.GetList((int)Master.CamFeature);
            return View();
        }
        #endregion

        [HttpPost]
        [Authorize]
        public ActionResult Create(ProductViewModel product)
        {
            if (product.id == 0)
            {
                ProductViewModel data = new ProductViewModel();
                data = product;
                ProductEntity products = new ProductEntity();
                products = (ProductEntity)_mapper.Map<ProductEntity>(data);
                Images image;
                List<Images> images = new List<Images>();
                foreach (IFormFile files in data.imageFile)
                {

                    image = new Images();
                    image.imagePath = files.FileName;
                    images.Add(image);
                }
                products.images = images;




                string uniqueFileName = null;
                if (data.imageFile != null && data.imageFile.Count > 0)
                {
                    foreach (IFormFile files in data.imageFile)
                    {
                        string folder = "Product/Images";
                        string serverFolder = Path.Combine(_webHostEnvironment.WebRootPath, folder);
                        uniqueFileName = Guid.NewGuid().ToString() + "_" + files.FileName;
                        string folderPath = Path.Combine(uniqueFileName, serverFolder);
                        files.CopyTo(new FileStream(folderPath, FileMode.Create));
                    }
                }
                bool result = _opApi.CreateProduct(products);
                if (result)
                {
                    _notyf.Success("Prduct added");
                }
                else
                {
                    _notyf.Error("Not Added");
                }
            }
            else
            {
                var data = _opApi.GetProduct(product.id).Result;
                List<Images> images = new List<Images>();
                images = data.images.ToList();
                var mapperData = (ProductEntity)_mapper.Map<ProductEntity>(product);
                mapperData.images = images;
                bool result = _opApi.EditProduct(mapperData);
                if (result)
                {
                    _notyf.Success("Product Updated");
                }
                else
                {
                    _notyf.Error("Not Updated");
                }
            }
            

            return RedirectToAction("");
        }
        [HttpGet]
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
        
        [HttpDelete]
        public ActionResult Delete(int id)
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

            return RedirectToAction("Index");
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
            return View(masterdata);
        }
        [HttpGet("MasterDelete")]
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
        }
        [HttpGet("ProductList")]
        [Authorize]
        public async Task<ActionResult> ProductList()
        {
            
            var data = await _opApi.GetProduct();
            List<ProductListViewModel> productList = (List<ProductListViewModel>)_mapper.Map<List<ProductListViewModel>>(data);
            return new JsonResult(productList);
        }
        [HttpGet("ProductCreate")]
        public ActionResult ProductCreate()
        {
            ViewBag.BrandList = _masterApi.GetList((int)Master.Brand); ;
            ViewBag.SimType = _masterApi.GetList((int)Master.SimType);
            ViewBag.ProductType = _masterApi.GetList((int)Master.ProductType);
            ViewBag.Processor = _masterApi.GetList((int)Master.OsProcessor);
            ViewBag.Core = _masterApi.GetList((int)Master.OsCore);
            ViewBag.Ram = _masterApi.GetList((int)Master.Ram);
            ViewBag.Storage = _masterApi.GetList((int)Master.Storage);
            ViewBag.camFeatures = _masterApi.GetList((int)Master.CamFeature);
            return View();
        }
        [HttpPost("ProductCreate")]
        [Authorize]
        public ActionResult ProductCreate(ProductViewModel product )
        {
            if(!_opApi.GetProduct().Result.Any(c=> c.model.Equals(product.model)))
            {
                ProductViewModel data = new ProductViewModel();
                data = product;
                ProductEntity products = new ProductEntity();
                products = (ProductEntity)_mapper.Map<ProductEntity>(data);
                Images image;
                List<Images> images = new List<Images>();





                string uniqueFileName = null;
                if (data.imageFile != null && data.imageFile.Count > 0)
                {
                    foreach (IFormFile files in data.imageFile)
                    {
                        string folder = "Product/Images";
                        string serverFolder = Path.Combine(_webHostEnvironment.WebRootPath, folder);
                        uniqueFileName = Guid.NewGuid().ToString() + "_" + files.FileName;
                        string folderPath = Path.Combine(serverFolder, uniqueFileName);
                        files.CopyTo(new FileStream(folderPath, FileMode.Create));
                        image = new Images();
                        image.imagePath = uniqueFileName;
                        images.Add(image);
                    }
                    
                }

                bool result = _opApi.CreateProduct(products);
                if (result)
                {
                    _notyf.Success("Prduct added");
                }
                else
                {
                    _notyf.Error("Not Added");
                }
               
                return RedirectToAction("index");
            }
            else
            {
                _notyf.Error("Product already existed");
                return RedirectToAction("Index");
            }
        
        }
        [HttpGet]
        public async Task<IActionResult> AddImage(int  id)
        {
            var productEntity = await _opApi.GetProduct(id);


            ViewData["images"] = productEntity.images;
            var data = (ProductViewModel)_mapper.Map<ProductViewModel>(productEntity);
            return View(data);
        }
        [HttpPost("AddImages")]
        public async Task<IActionResult> AddImages(ProductViewModel product)
        {
            ProductViewModel data = new ProductViewModel();
            data = product;
            var datalist = await _opApi.GetAll();
            ProductEntity products = new ProductEntity();
            products = datalist.Where(c => c.model.Equals(product.model)).FirstOrDefault();
            Images image;
            List<Images> images = new List<Images>();
            images = products.images.ToList();
            foreach (IFormFile files in data.imageFile)
            {
                string folder = "Product/Images";
                string serverFolder = Path.Combine(_webHostEnvironment.WebRootPath, folder);
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + files.FileName;
                string folderPath = Path.Combine(serverFolder, uniqueFileName);
                files.CopyTo(new FileStream(folderPath, FileMode.Create));
                image = new Images();
                image.imagePath = uniqueFileName;
                images.Add(image);
            }
            products.images = images;
            bool result = _opApi.EditProduct(products);
            if (result)
            {
                _notyf.Success("Prduct added");
            }
            else
            {
                _notyf.Error("Not Added");
            }
            return View("index");
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
            adminApi userApi = new adminApi(Configuration);
            LoginViewModel user = new LoginViewModel();
            user.userName = userName;
            user.password = password;
            bool check = userApi.Authenticate(user);
            if (check)
            {
                var claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.Name, user.userName));
                claims.Add(new Claim("email", user.userName));
                claims.Add(new Claim(ClaimTypes.NameIdentifier, user.userName));
                claims.Add(new Claim("password", user.password));
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                await HttpContext.SignInAsync(claimsPrincipal);
                return Redirect("/admin");
            }
            TempData["Error"] = "Invalid Email or Password";
            return View("login");
        }
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();

            return View("login");
        }
        public IActionResult OrderList()
        {
            UserApi userApi = new UserApi(Configuration);
            var data = userApi.GetCheckOut().Result;
            return View(data);
        }
        [HttpPost]
        public async Task<IActionResult> OrderUpdate(List<string> status , List<int> orderId)
        {
            Checkout checkout = new Checkout();
            UserApi userApi = new UserApi(Configuration);   
            OrderStatus statuses = new OrderStatus();
            for (int i = 0;i<status.Count();i++)
            {
                OrderStatus MyStatus = (OrderStatus)Enum.Parse(typeof(OrderStatus), status[i], true);
                var checkoutData = userApi.GetCheckOut().Result.Where(c => c.orderId.Equals(orderId[i])).FirstOrDefault();
                checkoutData.status = MyStatus;
                userApi.EditCheckout(checkoutData);

            }
            var data = await userApi.GetCheckOut();
            return View("OrderList",data);
        }
        public IActionResult orderDetails(int id)
        {
            var checkoutList = _userApi.GetCheckOut().Result;
            var checkout = checkoutList.Where(c => c.orderId.Equals(id)).FirstOrDefault();
            ProductOpApi product = new ProductOpApi(Configuration);
            var ProductDetails = product.GetProduct(checkout.productId).Result;
            ViewData["ProuductDetails"] = ProductDetails;
            ViewData["Address"] = _userApi.GetAddress().Result.Where(c => c.id.Equals(checkout.addressId)).FirstOrDefault();
            return View(checkout);
        }

        public IActionResult Contact()
        {

            adminApi _adminApi = new adminApi(Configuration);
            var contactData = _adminApi.ContactGet().Result.FirstOrDefault();
            return View(contactData);
           
        }
        [HttpGet]
        public IActionResult ContactEdit()
        {
            adminApi _adminApi = new adminApi(Configuration);
          var contactData = _adminApi.ContactGet().Result.FirstOrDefault();
            return View(contactData);
         
        }
        [HttpPost]
        public IActionResult ContactEdit(Contact contact)
        {
            adminApi _adminApi = new adminApi(Configuration);
            _adminApi.EditContact(contact);
            return RedirectToAction("Contact");
        }



        public IActionResult Privacy()
        {
            adminApi _adminApi = new adminApi(Configuration);
            var privacyData = _adminApi.PrivacyGet().Result.FirstOrDefault();
            return View(privacyData);
           
        }
        [HttpGet]
        public IActionResult PrivacyEdit()
        {
            adminApi _adminApi = new adminApi(Configuration);
            var privacyData = _adminApi.PrivacyGet().Result.FirstOrDefault();
            return View(privacyData);
        }
        [HttpPost]
        public IActionResult PrivacyEdit(PrivacyPolicy privacy)
        {
            adminApi _adminApi = new adminApi(Configuration);
            _adminApi.EditPrivacy(privacy);
            return RedirectToAction("Privacy");
           
        }

        public IActionResult About()
        {
            adminApi _adminApi = new adminApi(Configuration);
            var aboutData = _adminApi.AboutGet().Result.FirstOrDefault();
            return View(aboutData);
        }
        [HttpGet]
        public IActionResult AboutEdit()
        {
            adminApi _adminApi = new adminApi(Configuration);
            var aboutData = _adminApi.AboutGet().Result.FirstOrDefault();
            return View(aboutData);
        }
        [HttpPost]
        public IActionResult AboutEdit(About about)
        {
            adminApi _adminApi = new adminApi(Configuration);
            _adminApi.EditAbout(about);
            return RedirectToAction("About");
        }
        public IActionResult Company()
        {
            return View();
        }
        [HttpGet("Admin/AddImage/ImageDelete/{id}")]
        public async Task<IActionResult> ImageDelete(int id)
        {
            ImageApi imageApi = new ImageApi(Configuration);
            bool result = imageApi.DeleteProduct(id);
            var data = imageApi.GetProduct();
            var sample = data.Where(c => c.id.Equals(id)).FirstOrDefault();
            var products = await _opApi.GetProduct();
            var product = products.Where(c=> c.id.Equals(sample.ProductEntityId)).FirstOrDefault();
            return RedirectToAction("AddImages",product);
        }
        [HttpPost]
        public async Task<ActionResult> quatity(ProductEntity product, string newQuantity)
        {
            var datalist = await _opApi.GetProduct();
            ProductEntity products = new ProductEntity();
            /* products = datalist.Where(c => c.model.Equals(product.model)).FirstOrDefault();*/
            products.quantity = product.quantity + Convert.ToInt32(newQuantity);
            bool result = _opApi.EditProduct(products);
            return RedirectToAction("Index");
        }

       
    }
}
