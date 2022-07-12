using AutoMapper;
using BusinessObjectLayer.ProductOperations;
using DomainLayer;
using DomainLayer.ProductModel;
using DTOLayer.Product;
using Firebase.Auth;
using Firebase.Storage;
using log4net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UILayer.Models;

namespace UILayer.Data.ApiServices
{
    public class ProductOpApi : IProductOpApi
    {
        private readonly ILog _log;
        private IConfiguration _configuration { get; }
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        // for testing ////////////////////////////firebase//////////////////////////////////
        private static string apiKey = "AIzaSyBvGbaacBA91vzQfmvUsF77eAJSYn6b4VE";
        private static string Bucket = "mobizone-55ea5.appspot.com";
        private static string AuthEmail = "subinbabu4127@gmail.com";
        private static string AuthPassword = "Subin@1999";
        //////////////////////////////////////////////////////
        public ProductOpApi(IConfiguration configuration, IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            _configuration = configuration;
            _log = LogManager.GetLogger(typeof(ProductOpApi));
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
        }

        #region Get all method
        public async Task<IEnumerable<ProductEntity>> GetAll()
        {
            try
            {
                RequestHandler<IEnumerable<ProductEntity>> _requestHandler = new RequestHandler<IEnumerable<ProductEntity>>(_configuration);
                _requestHandler.url = "api/productop/GetAll";
                var result = _requestHandler.Get();
                if (result != null)
                {
                    return result.result;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
                return null;
            }

        }
        #endregion


        #region Get list method
        public async Task<IEnumerable<ProductListViewModel>> GetProduct()

        {
            try
            {
                RequestHandler<IEnumerable<ProductListViewModel>> _requestHandler = new RequestHandler<IEnumerable<ProductListViewModel>>(_configuration);
                _requestHandler.url = "api/productop/GetList";
                return _requestHandler.Get().result;
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
                return null;
            }

        }
        #endregion

        #region filter method
        public async Task<IEnumerable<ProductEntity>> Filter(string name)

        {
            try
            {
                RequestHandler<IEnumerable<ProductEntity>> _requestHandler = new RequestHandler<IEnumerable<ProductEntity>>(_configuration);
                _requestHandler.url = "api/productop/FilterByBrand/" + name;
                var result = _requestHandler.Get();
                if (result != null)
                {
                    return result.result;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
                return null;
            }

        }
        #endregion

        #region Get one method
        public async Task<ProductEntity> GetProduct(int id)
        {
            try
            {
                RequestHandler<ProductEntity> _requestHandler = new RequestHandler<ProductEntity>(_configuration);
                _requestHandler.url = "api/productop/GetDetails/" + id;
                return _requestHandler.Get().result;

            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
                return null;
            }

        }
        #endregion

        #region edit method
        public async Task<bool> EditProduct(ProductViewModel product)
        {
            try
            {
                var datas = GetAll().Result;
                var data = datas.Where(c => c.id.Equals(product.id)).FirstOrDefault();
                if (product.imageFile != null)
                {
                    Images image;
                    List<Images> images = new List<Images>();
                    images = data.images.ToList();
                    if (product.imageFile != null)
                    {
                        foreach (IFormFile files in product.imageFile)
                        {
                            FileStream fs;
                            string folder = "Product/Images";
                            string serverFolder = Path.Combine(_webHostEnvironment.WebRootPath, folder);
                            string uniqueFileName = Guid.NewGuid().ToString() + "_" + files.FileName;
                            string folderPath = Path.Combine(serverFolder, uniqueFileName);
                            using (fs = new FileStream(folderPath, FileMode.Create))
                            {
                                await files.CopyToAsync(fs);
                            }
                            //files.CopyTo(new FileStream(folderPath, FileMode.Create));
                            
                            try
                            {
                                fs = new FileStream(folderPath, FileMode.Open);
                                var auth = new FirebaseAuthProvider(new FirebaseConfig(apiKey));
                                var a = auth.SignInWithEmailAndPasswordAsync(AuthEmail, AuthPassword).Result;
                                var cancellationToken = new CancellationTokenSource();
                                var upload = new FirebaseStorage(Bucket,
                                    new FirebaseStorageOptions
                                    {
                                        AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                                        ThrowOnCancel = true
                                    })
                                    .Child("assets")
                                    .Child($"{uniqueFileName}.{Path.GetExtension(files.FileName).Substring(1)}")
                                    .PutAsync(fs, cancellationToken.Token);
                                image = new Images();
                                image.imagePath = await upload;
                                images.Add(image);
                                System.IO.DirectoryInfo di = new DirectoryInfo(serverFolder);

                                foreach (FileInfo file in di.GetFiles())
                                {
                                    file.Delete();
                                }
                                foreach (DirectoryInfo dir in di.GetDirectories())
                                {
                                    dir.Delete(true);
                                }
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                    }
                    else
                    {
                        images = null;
                    }
                    data.images = images;
                }
                data.name = product.name;
                data.productBrand = product.productBrand;
                data.quantity = product.quantity;
                data.description = product.description;
                data.status = product.status;
                data.price = product.price;
                data.model = product.model;
                data.purchasedNumber = product.purchasedNumber;
                List<Ram> rams = new List<Ram>();
                
                if (product.specs != null)
                {
                    if (product.specs.ram != null)
                    {
                        foreach (var ramData in data.specs.rams)
                        {
                            DeleteRam(ramData.id);
                        }
                        foreach (var ramData in product.specs.ram)
                        {
                            Ram ram = new Ram();
                            ram.ram = ramData;
                            rams.Add(ram);
                        }
                    }
                }

                List<Storage> storages = new List<Storage>();
                
                if (product.specs != null)
                {
                    if (product.specs.storage != null)
                    {
                        foreach (var storage1 in data.specs.storages)
                        {
                            DeleteStorage(storage1.id);
                        }
                        foreach (var storageData in product.specs.storage)
                        {
                            Storage storage = new Storage();
                            storage.storage = storageData;
                            storages.Add(storage);
                        }
                    }
                }

                data.specs.storages = storages;
                data.specs.rams = rams;


                /*var mapperData = (ProductEntity)_mapper.Map<ProductEntity>(data);
                mapperData.images = images;*/
                RequestHandler<ProductEntity> _requestHandler = new RequestHandler<ProductEntity>(_configuration);
                _requestHandler.url = "api/productop";
                var result = _requestHandler.Edit(data);
                if (result != null)
                {
                    if (result.IsSuccess)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
                return false;
            }

        }
        #endregion

        #region create method
        public async Task<bool> CreateProduct(ProductViewModel product)
        {
            try
            {
                List<Ram> rams = new List<Ram>();
                foreach (var ramData in product.specs.ram)
                {
                    Ram ram = new Ram();
                    ram.ram = ramData;
                    rams.Add(ram);
                }
                List<Storage> storages = new List<Storage>();
                foreach (var storageData in product.specs.storage)
                {
                    Storage storage = new Storage();
                    storage.storage = storageData;
                    storages.Add(storage);
                }
                product.specs.rams = rams;
                product.specs.storages = storages;
                ProductViewModel data = new ProductViewModel();
                data = product;
                ProductEntity products = new ProductEntity();
                products = (ProductEntity)_mapper.Map<ProductEntity>(data);
                products.status = ProductStatus.enable;
                Images image;
                List<Images> images = new List<Images>();
                string uniqueFileName = null;
                if (product.imageFile != null)
                {
                    //Images image;
                    //List<Images> images = new List<Images>();
                    //images = data.images.ToList();
                    if (product.imageFile != null)
                    {
                        foreach (IFormFile files in product.imageFile)
                        {
                            FileStream fs;
                            string folder = "Product/Images";
                            string serverFolder = Path.Combine(_webHostEnvironment.WebRootPath, folder);
                            uniqueFileName = Guid.NewGuid().ToString() + "_" + files.FileName;
                            string folderPath = Path.Combine(serverFolder, uniqueFileName);
                            using (fs = new FileStream(folderPath,FileMode.Create))
                            {
                                await files.CopyToAsync(fs);
                            }
                            //files.CopyTo(new FileStream(folderPath, FileMode.Create));
                            try
                            {
                                fs = new FileStream(folderPath, FileMode.Open);
                                var auth = new FirebaseAuthProvider(new FirebaseConfig(apiKey));
                                var a = auth.SignInWithEmailAndPasswordAsync(AuthEmail, AuthPassword).Result;
                                var cancellationToken = new CancellationTokenSource();
                                var upload = new FirebaseStorage(Bucket,
                                    new FirebaseStorageOptions
                                    {
                                        AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                                        ThrowOnCancel = true
                                    })
                                    .Child("assets")
                                    .Child($"{uniqueFileName}.{Path.GetExtension(files.FileName).Substring(1)}")
                                    .PutAsync(fs, cancellationToken.Token);
                                image = new Images();
                                image.imagePath = await upload;
                                images.Add(image);
                                System.IO.DirectoryInfo di = new DirectoryInfo(serverFolder);

                                foreach (FileInfo file in di.GetFiles())
                                {
                                    file.Delete();
                                }
                                foreach (DirectoryInfo dir in di.GetDirectories())
                                {
                                    dir.Delete(true);
                                }
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                    }
                    else
                    {
                        images = null;
                    }
                    products.images = images;
                }
                RequestHandler<ProductEntity> requestHandler = new RequestHandler<ProductEntity>(_configuration);
                requestHandler.url = "api/productop/CreateProduct";
                var result =  requestHandler.Post(products);
                if (result != null)
                {
                    if (result.IsSuccess)
                        return true;
                    return false;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
                return false;
            }
        }
        #endregion

        #region delete method
        public bool DeleteProduct(int id)
        {
            RequestHandler<ProductEntity> requestHandler = new RequestHandler<ProductEntity>(_configuration);
            try
            {
                requestHandler.url = "api/productop/";
                if (requestHandler.Delete(id).IsSuccess)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
                return false;
            }
        }
        #endregion

        public IEnumerable<Ram> GetRams()
        {
            RequestHandler<IEnumerable<Ram>> _requestHandler = new RequestHandler<IEnumerable<Ram>>(_configuration);
            _requestHandler.url = "api/productop/getrams";
            return _requestHandler.Get().result;
        }
        public IEnumerable<Storage> GetStorages()
        {
            RequestHandler<IEnumerable<Storage>> _requestHandler = new RequestHandler<IEnumerable<Storage>>(_configuration);
            _requestHandler.url = "api/productop/getstorages";
            return _requestHandler.Get().result;
        }
        #region Add method for master data
        public bool AddProductSubPart(ProductSubPart productSubPart)
        {
            try
            {
                RequestHandler<ProductSubPart> request = new RequestHandler<ProductSubPart>(_configuration);
                request.url = "api/productop/PostProductSubPart";
                var result = request.Post(productSubPart);
                if (result != null)
                {
                    if (result.IsSuccess)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion

        #region Get method for Search
        public async Task<IEnumerable<ProductEntity>> Search(string name)
        {
            try
            {
                RequestHandler<IEnumerable<ProductEntity>> _requestHandler = new RequestHandler<IEnumerable<ProductEntity>>(_configuration);
                _requestHandler.url = "api/productop/search/" + name;
                return _requestHandler.Get().result;

            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
                return null;
            }

        }
        #endregion
        #region method for sort Ascending
        public async Task<IEnumerable<ProductEntity>> Sort()
        {
            RequestHandler<IEnumerable<ProductEntity>> _requestHandler = new RequestHandler<IEnumerable<ProductEntity>>(_configuration);
            try
            {
                _requestHandler.url = "api/productop/SortByPriceAscending ";
                return _requestHandler.Get().result;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        #endregion
        #region method for descending Sort
        public async Task<IEnumerable<ProductEntity>> Sortby()
        {
            RequestHandler<IEnumerable<ProductEntity>> _requestHandler = new RequestHandler<IEnumerable<ProductEntity>>(_configuration);
            try
            {
                _requestHandler.url = "api/productop/SortByPriceDescending ";
                return _requestHandler.Get().result;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        #endregion

        #region delete method
        public bool DeleteRam(int id)
        {
            RequestHandler<ProductEntity> requestHandler = new RequestHandler<ProductEntity>(_configuration);
            try
            {
                requestHandler.url = "api/productop/DeleteRam/";
                if (requestHandler.Delete(id).IsSuccess)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
                return false;
            }
        }
        #endregion
        #region delete method
        public bool DeleteStorage(int id)
        {
            RequestHandler<ProductEntity> requestHandler = new RequestHandler<ProductEntity>(_configuration);
            try
            {
                requestHandler.url = "api/productop/DeleteStorage/";
                if (requestHandler.Delete(id).IsSuccess)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
                return false;
            }
        }
        #endregion
    }
}

