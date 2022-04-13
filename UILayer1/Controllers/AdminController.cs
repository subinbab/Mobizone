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

namespace UIlayer.Controllers
{
    

    public class AdminController : Controller
    {
        Product data = null;
        IConfiguration Configuration { get; }
        ProductApi pr;
        MasterApi _masterApi;
        ProductOpApi _opApi;
        private readonly INotyfService _notyf;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        IEnumerable<UserRegistration> _userDataList;
        DataCollection _collection;
        public AdminController(IConfiguration configuration, INotyfService notyf, IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            _notyf = notyf;
            Configuration = configuration;
            pr = new ProductApi(Configuration);
            data = new Product();
            _masterApi = new MasterApi(Configuration);
            _opApi = new ProductOpApi(Configuration);
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
            _collection = new DataCollection(Configuration);
        }
        [Authorize]
        public ActionResult Index(int? i)
        {
            var data = _opApi.GetProduct();
            List<ProductListViewModel> productList = (List<ProductListViewModel>)_mapper.Map<List<ProductListViewModel>>(data);
            return View(productList);
        }
        [Authorize]
        public ActionResult Details(int id)
        {
            Product product = pr.GetProduct(id);
            return View(product);
        }
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.BrandList = _collection.FetchData((int)Master.Brand);
            ViewBag.SimType = _collection.FetchData((int)Master.SimType);
            ViewBag.ProductType = _collection.FetchData((int)Master.ProductType);
            ViewBag.Processor = _collection.FetchData((int)Master.OsProcessor);
            ViewBag.Core = _collection.FetchData((int)Master.OsCore);
            ViewBag.Ram = _collection.FetchData((int)Master.Ram);
            ViewBag.Storage = _collection.FetchData((int)Master.Storage);
            ViewBag.camFeatures = _collection.FetchData((int)Master.CamFeature);
            return View();
        }
        [HttpPost]
        [Authorize]
        public ActionResult Create(ProductViewModel product)
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
            /*if (data.imageFile !=null && data.imageFile.Count > 0)
            {
                foreach (IFormFile files in data.imageFile)
                {
                    string folder = "Product/Images";
                    string serverFolder = Path.Combine(_webHostEnvironment.WebRootPath, folder);
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + files.FileName;
                    files.CopyTo(new FileStream(serverFolder, FileMode.Create));
                }
            }*/
            bool result = _opApi.CreateProduct(products);
            if (result)
            {
                _notyf.Success("Prduct added");
            }
            else
            {
                _notyf.Error("Not Added");
            }

            return RedirectToAction("");
        }
        [HttpGet]
        [Authorize]
        public async Task<ActionResult> Edit(int id)
        {
            var product = await _opApi.GetProduct(id);
            var data = (ProductViewModel)_mapper.Map<ProductViewModel>(product);
            ViewBag.BrandList = _collection.FetchData((int)Master.Brand);
            ViewBag.SimType = _collection.FetchData((int)Master.SimType);
            ViewBag.ProductType = _collection.FetchData((int)Master.ProductType);
            ViewBag.Processor = _collection.FetchData((int)Master.OsProcessor);
            ViewBag.Core = _collection.FetchData((int)Master.OsCore);
            ViewBag.Ram = _collection.FetchData((int)Master.Ram);
            ViewBag.Storage = _collection.FetchData((int)Master.Storage);
            ViewBag.camFeatures = _collection.FetchData((int)Master.CamFeature);

            return View(data);
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
                _notyf.Success(Configuration.GetSection("Products")["ProductDeleted"].ToString());
            }
            else
            {
                _notyf.Error(Configuration.GetSection("Products")["ProductDeltedError"].ToString());
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
            bool result = _masterApi.Add(data);
            if (result)
            {
                _notyf.Success(Configuration.GetSection("Master")["MasterAdded"].ToString());
            }
            else
            {
                _notyf.Error(Configuration.GetSection("Master")["MasterAddedError"].ToString());
            }
            ModelState.Clear();
            return View();
        }
        [HttpGet("MasterList")]
        [Authorize]
        public ActionResult MasterList(int id)
        {
            IEnumerable < MasterTable > data  = _masterApi.GetAll();
            return View(data);
        }
        [HttpGet("ProductList")]
        [Authorize]
        public ActionResult ProductList()
        {
            
            var data = _opApi.GetProduct();
            List<ProductListViewModel> productList = (List<ProductListViewModel>)_mapper.Map<List<ProductListViewModel>>(data);
            return new JsonResult(productList);
        }
        [HttpGet("ProductCreate")]
        public ActionResult ProductCreate()
        {
            ViewBag.BrandList = _collection.FetchData((int)Master.Brand);
            ViewBag.SimType = _collection.FetchData((int)Master.SimType);
            ViewBag.ProductType = _collection.FetchData((int)Master.ProductType);
            ViewBag.Processor = _collection.FetchData((int)Master.OsProcessor);
            ViewBag.Core = _collection.FetchData((int)Master.OsCore);
            ViewBag.Ram = _collection.FetchData((int)Master.Ram);
            ViewBag.Storage = _collection.FetchData((int)Master.Storage);
            ViewBag.camFeatures = _collection.FetchData((int)Master.CamFeature);
            return View();
        }
        [HttpPost("ProductCreate")]
        [Authorize]
        public ActionResult ProductCreate(ProductViewModel product )
        {
            ProductViewModel data = new ProductViewModel();
            data = product;
            ProductEntity products = new ProductEntity();
            products = (ProductEntity)_mapper.Map<ProductEntity>(data);
            Images image;
            List< Images > images = new List< Images >();
            foreach(IFormFile files in data.imageFile)
            {
                
                image = new Images();
                image.imagePath = files.FileName;
                images.Add( image );
            }
            products.images = images;




            string uniqueFileName = null;
            /*if (data.imageFile !=null && data.imageFile.Count > 0)
            {
                foreach (IFormFile files in data.imageFile)
                {
                    string folder = "Product/Images";
                    string serverFolder = Path.Combine(_webHostEnvironment.WebRootPath, folder);
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + files.FileName;
                    files.CopyTo(new FileStream(serverFolder, FileMode.Create));
                }
            }*/
            bool result = _opApi.CreateProduct(products);
            if (result)
            {
                _notyf.Success("Prduct added");
            }
            else
            {
                _notyf.Error("Not Added");
            }

            return RedirectToAction("Index");
        }
        [Authorize]
        public IActionResult Userdata()
        {
            UserApi userApi = new UserApi(Configuration);
            _userDataList = userApi.GetUserData();
            return View(_userDataList);
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
            UserApi userApi = new UserApi(Configuration);
            LoginViewModel user = new LoginViewModel();
            user.userName = userName;
            user.password = password;
            bool check = userApi.Authenticate(user);
            if (check)
            {
                var claims = new List<Claim>();
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
            return Redirect("/");
        }
        public IActionResult OrderList()
        {
            return View();
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
