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
        }
        public ActionResult Index(int? i)
        {
            IEnumerable<Product> products = pr.GetProduct();
            return View(products);
        }

        public ActionResult Details(int id)
        {
            Product product = pr.GetProduct(id);
            return View(product);
        }

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(ViewProductModel product)
        {
            if (ModelState.IsValid)
            {
                data.Id = 0;
                data.Name = product.Name;
                data.Model = product.Model;
                data.Price = product.Price;
                data.Description = product.Description;
                bool result = pr.CreateProduct(data);
                if (result)
                {
                    _notyf.Success(Configuration.GetSection("Products")["ProductAdded"].ToString());
                }
                else
                {
                    _notyf.Error(Configuration.GetSection("Products")["ProductAddedError"].ToString());
                }
            }
            
            return RedirectToAction("");
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            Product product = pr.GetProduct(id);
            return View(product);
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
        [HttpGet]
        public ActionResult Delete(int id)
        {
            data = pr.GetProduct(id);
            return View(data);
        }
        public ActionResult DeleteProduct(int id)
        {
            bool result = pr.DeleteProduct(id);
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
        public ActionResult MasterList(int id)
        {
            IEnumerable < MasterTable > data  = _masterApi.GetAll();
            return View(data);
        }
        [HttpGet("ProductList")]
        public ActionResult ProductList()
        {
            
            var data = _opApi.GetProduct();
            List<ProductListViewModel> productList = (List<ProductListViewModel>)_mapper.Map<List<ProductListViewModel>>(data);
            return View(productList);
        }
        [HttpGet("ProductCreate")]
        public ActionResult ProductCreate()
        {
            List<string> brandList = new List<string>();
            
            IEnumerable<MasterTable> data1 = _masterApi.GetAll().Where(s => s.parantId == (int)Master.Brand);
            foreach(var item in data1)
            {
                brandList.Add(item.masterData.ToString());
            }
            ViewBag.BrandList = brandList;
            List<string> simType = new List<string>();

            IEnumerable<MasterTable> data2 = _masterApi.GetAll().Where(s => s.parantId == (int)Master.SimType);
            foreach (var item in data2)
            {
                simType.Add(item.masterData.ToString());
            }
            ViewBag.SimType = simType;
            List<string> productType = new List<string>();

            IEnumerable<MasterTable> data3 = _masterApi.GetAll().Where(s => s.parantId == (int)Master.ProductType);
            foreach (var item in data3)
            {
                productType.Add(item.masterData.ToString());
            }
            ViewBag.ProductType = productType;
            List<string> processor = new List<string>();

            IEnumerable<MasterTable> data4 = _masterApi.GetAll().Where(s => s.parantId == (int)Master.OsProcessor);
            foreach (var item in data4)
            {
                processor.Add(item.masterData.ToString());
            }
            ViewBag.Processor = processor;
            List<string> core = new List<string>();

            IEnumerable<MasterTable> data5 = _masterApi.GetAll().Where(s => s.parantId == (int)Master.OsCore);
            foreach (var item in data5)
            {
                core.Add(item.masterData.ToString());
            }
            ViewBag.Core = core;
            List<string> ram = new List<string>();

            IEnumerable<MasterTable> data7 = _masterApi.GetAll().Where(s => s.parantId == (int)Master.Ram);
            foreach (var item in data7)
            {
                ram.Add(item.masterData.ToString());
            }
            ViewBag.Ram = ram;
            List<string> storage = new List<string>();

            IEnumerable<MasterTable> data = _masterApi.GetAll().Where(s => s.parantId == (int)Master.Storage);
            foreach (var item in data)
            {
                storage.Add(item.masterData.ToString());
            }
            ViewBag.Storage = storage;
            List<string> camFeature = new List<string>();

            IEnumerable<MasterTable> data8 = _masterApi.GetAll().Where(s => s.parantId == (int)Master.CamFeature);
            foreach (var item in data8)
            {
                camFeature.Add(item.masterData.ToString());
            }
            ViewBag.camFeatures = camFeature;
            return View();
        }
        [HttpPost("ProductCreate")]
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
    }
}
