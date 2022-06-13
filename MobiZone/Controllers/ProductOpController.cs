using ApiLayer.Messages;
using ApiLayer.Models;
using AutoMapper;
using BusinessObjectLayer.ProductOperations;
using DomainLayer.ProductModel;
using DomainLayer.ProductModel.Master;
using DTOLayer.Product;
using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ApiLayer.Controllers
{
    [Authorize(AuthenticationSchemes = "BasicAuthentication")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductOpController : ControllerBase
    {
        private readonly ILog _log;
        ProductDbContext _context;
        IProductOperations _productOperations;
        ResponseModel<ProductEntity> _response;
        IEnumerable<ProductEntity> _productDataList;
        ProductEntity _productData;
        IMessages _productMessages;
        IMessages _masterMessages;
        IWebHostEnvironment _webHostEnvironment;
        IMapper _mapper;
        IEnumerable<MasterTable> _masterDataList;
        IMasterDataOperations _masterOperations;
        IStorageOperations _storageOperations;
        IRamOperations _ramOperations;
        IProductSubPartOperations _subPartOperations;
        IEnumerable<ProductSubPart> _productSubParts;
        ProductSubPart _productSubPart;
        IEnumerable<Ram> _rams;
        IEnumerable<Storage> _storages;
        public ProductOpController(ProductDbContext context, IProductOperations productOperations, IWebHostEnvironment web, IMapper mapper, IMasterDataOperations masterDataOperations , IStorageOperations storageOperations , IRamOperations ramOperations, IProductSubPartOperations subPartOperations)
        {
            #region Object Assigning
            _context = context;
            _webHostEnvironment = web;
            _productOperations = productOperations;
            _response = new ResponseModel<ProductEntity>();
            _productData = new ProductEntity();
            _log = LogManager.GetLogger(typeof(ProductController));
            _productMessages = new ProductMessages(_webHostEnvironment);
            _masterMessages = new MasterMessages(_webHostEnvironment);
            _mapper = mapper;
            _masterOperations = masterDataOperations;
            _storageOperations = storageOperations;
            _ramOperations = ramOperations;
            _subPartOperations = subPartOperations;
            #endregion
        }

        #region Post Method for Product
        
        [HttpPost("CreateProduct")]
        public IActionResult CreateProduct([FromBody] ProductEntity product)
        {
            ResponseModel<string> _response = new ResponseModel<string>();
            try
            {
                _productOperations.Add(product);
                string message = _productMessages.Added + ", Response Message : " + new HttpResponseMessage(System.Net.HttpStatusCode.OK);
                _response.AddResponse(true, 0, "", message);
                var data = Newtonsoft.Json.JsonConvert.SerializeObject(_response);
                return new JsonResult(_response);
            }
            catch (Exception ex)
            {
                string message = _productMessages.ExceptionError + ", Response Message : " + new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError);
                _response.AddResponse(false, 0, _productMessages.ExceptionError, message);
                _log.Error("log4net : error in the post controller", ex);
                return new JsonResult(_response);
            }

        }
        #endregion

        #region GetList Method for Products
        /*[Authorize(Roles ="Admin")]*/
        [HttpGet("GetList")]
        public ResponseModel<IEnumerable<ProductListViewModel>> GetList()
        {
            ResponseModel<IEnumerable<ProductListViewModel>> _response = new ResponseModel<IEnumerable<ProductListViewModel>>();
            try
            {
                _productDataList = _productOperations.GetAll().Result;
                if (_productDataList == null)
                {
                    string message = _productMessages.Null +" "+ new HttpResponseMessage(System.Net.HttpStatusCode.OK);
                    _response.AddResponse(true, 0, null, message);
                    /*                    var json = JsonConvert.SerializeObject(_response, Formatting.Indented,
                              new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.None });*/
                    /*                    return Content(json, "application/json");*/
                    return _response;
                }
                else
                {
                    IEnumerable<ProductListViewModel> productList = (IEnumerable<ProductListViewModel>)_mapper.Map<IEnumerable<ProductListViewModel>>(_productDataList);
                    string message = "" + new HttpResponseMessage(System.Net.HttpStatusCode.OK);
                    _response.AddResponse(true, 0, productList, message);

                    return _response;
                }

            }
            catch (Exception ex)
            {
                string message = _productMessages.ExceptionError +" : "+ex.Message+" : "+ new HttpResponseMessage(System.Net.HttpStatusCode.OK);
                _response.AddResponse(false, 0, null, message);
                _log.Error("log4net : error in the post controller", ex);
                return _response;
            }

        }
        #endregion
        #region GetAll Method for Products
        [HttpGet("GetAll")]
        public async  Task<ResponseModel<IEnumerable<ProductEntity>>> GetAll()
        {
            ResponseModel<IEnumerable<ProductEntity>> _response = new ResponseModel<IEnumerable<ProductEntity>>();
            try
            {
                _productDataList = await _productOperations.GetAll();
                if (_productDataList == null)
                {
                    string message = _productMessages.Null + " " + new HttpResponseMessage(System.Net.HttpStatusCode.OK);
                    _response.AddResponse(true, 0, null, message);
                    /*                    var json = JsonConvert.SerializeObject(_response, Formatting.Indented,
                              new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.None });*/
                    /*                    return Content(json, "application/json");*/
                    return _response;
                }
                else
                {
                    string message = "" + new HttpResponseMessage(System.Net.HttpStatusCode.OK);
                    _response.AddResponse(true, 0, _productDataList, message);

                    return _response;
                }

            }
            catch (Exception ex)
            {
                string message = _productMessages.ExceptionError + " : " + ex.Message + " : " + new HttpResponseMessage(System.Net.HttpStatusCode.OK);
                _response.AddResponse(false, 0, null, message);
                _log.Error("log4net : error in the post controller", ex);
                return _response;
            }

        }
        #endregion

        #region GetDetails Method for product
        [HttpGet("GetDetails/{id}")]
        public ResponseModel<ProductEntity> GetDetails(int id)
        {
            ResponseModel<ProductEntity> _response = new ResponseModel<ProductEntity>();
            try
            {
                _productData = _productOperations.GetById(id).Result;
                if (_productData == null)
                {
                    string message = _productMessages.Null +" , "+ new HttpResponseMessage(System.Net.HttpStatusCode.OK);
                    _response.AddResponse(true, 0, null, message);
                    return _response;
                }
                else
                {
                    string message = "" + new HttpResponseMessage(System.Net.HttpStatusCode.OK);
                    _response.AddResponse(true, 0, _productData, message);
                    return _response;
                }

            }
            catch (Exception ex)
            {
                string message = _productMessages.ExceptionError + new HttpResponseMessage(System.Net.HttpStatusCode.OK);
                _response.AddResponse(false, 0,null, message);
                _log.Error("log4net : error in the post controller", ex);
                return _response;
            }

        }
        #endregion


        #region Search Product
        [HttpGet("Search/{name}")]
        public ResponseModel<IEnumerable<ProductEntity>> Search(string name)
        {
            ResponseModel<IEnumerable<ProductEntity>> _response = new ResponseModel<IEnumerable<ProductEntity>>();
            try
            {
                _productDataList = _productOperations.Search(name).Result;
                if (_productData == null)
                {
                    string message = _productMessages.Null + " , " + new HttpResponseMessage(System.Net.HttpStatusCode.NoContent);
                    _response.AddResponse(true, 0, null, message);
                    return _response;
                }
                else
                {
                    string message = "" + new HttpResponseMessage(System.Net.HttpStatusCode.OK);
                    _response.AddResponse(true, 0, _productDataList, message);
                    return _response;
                }

            }
            catch (Exception ex)
            {
                string message = _productMessages.ExceptionError + new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError);
                _response.AddResponse(false, 0, null, message);
                _log.Error("log4net : error in the post controller", ex);
                return _response;
            }

        }
        #endregion
        #region Sort Product By price 
        [HttpGet("SortByPriceAscending")]
        public ResponseModel<IEnumerable<ProductEntity>> SortByPrice()
        {
            ResponseModel<IEnumerable<ProductEntity>> _response = new ResponseModel<IEnumerable<ProductEntity>>();
            try
            {
                _productDataList = _productOperations.SortByPriceAscending().Result;
                if (_productData == null)
                {
                    string message = _productMessages.Null + " , " + new HttpResponseMessage(System.Net.HttpStatusCode.NoContent);
                    _response.AddResponse(true, 0, null, message);
                    return _response;
                }
                else
                {
                    string message = "" + new HttpResponseMessage(System.Net.HttpStatusCode.OK);
                    _response.AddResponse(true, 0, _productDataList, message);
                    return _response;
                }

            }
            catch (Exception ex)
            {
                string message = _productMessages.ExceptionError + new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError);
                _response.AddResponse(false, 0, null, message);
                _log.Error("log4net : error in the post controller", ex);
                return _response;
            }

        }
        [HttpGet("SortByPriceDescending")]
        public ResponseModel<IEnumerable<ProductEntity>> SortByPriceDescending()
        {
            ResponseModel<IEnumerable<ProductEntity>> _response = new ResponseModel<IEnumerable<ProductEntity>>();
            try
            {
                _productDataList = _productOperations.SortByPriceDescending().Result;
                if (_productData == null)
                {
                    string message = _productMessages.Null + " , " + new HttpResponseMessage(System.Net.HttpStatusCode.NoContent);
                    _response.AddResponse(true, 0, null, message);
                    return _response;
                }
                else
                {
                    string message = "" + new HttpResponseMessage(System.Net.HttpStatusCode.OK);
                    _response.AddResponse(true, 0, _productDataList, message);
                    return _response;
                }

            }
            catch (Exception ex)
            {
                string message = _productMessages.ExceptionError + new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError);
                _response.AddResponse(false, 0, null, message);
                _log.Error("log4net : error in the post controller", ex);
                return _response;
            }

        }
        #endregion
        #region Filter Product By brand 
        [HttpGet("FilterByBrand/{name}")]
        public ResponseModel<IEnumerable<ProductEntity>> FilterByBrand(string name)
        {
            ResponseModel<IEnumerable<ProductEntity>> _response = new ResponseModel<IEnumerable<ProductEntity>>();
            try
            {
                _productDataList = _productOperations.FilterByBrand(name).Result;
                if (_productData == null)
                {
                    string message = _productMessages.Null + " , " + new HttpResponseMessage(System.Net.HttpStatusCode.NoContent);
                    _response.AddResponse(true, 0, null, message);
                    return _response;
                }
                else
                {
                    string message = "" + new HttpResponseMessage(System.Net.HttpStatusCode.OK);
                    _response.AddResponse(true, 0, _productDataList, message);
                    return _response;
                }

            }
            catch (Exception ex)
            {
                string message = _productMessages.ExceptionError + new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError);
                _response.AddResponse(false, 0, null, message);
                _log.Error("log4net : error in the post controller", ex);
                return _response;
            }

        }
        #endregion
        #region Delete Method for Product
        [HttpDelete("{id}")]
        public ResponseModel<ProductEntity> Delete(int id)
        {
            ResponseModel<ProductEntity> _response = new ResponseModel<ProductEntity>();
            try
            { 
                List<ProductEntity> data = new List<ProductEntity>(); 
                data = _productOperations.GetAll().Result.ToList();
                _productData = data.Where(c => c.id.Equals(id)).FirstOrDefault();
                _productOperations.DeleteProduct(_productData);
                string message = _productMessages.Deleted + new HttpResponseMessage(System.Net.HttpStatusCode.OK);
                _response.AddResponse(true, 0, null, message);
                return _response;
            }
            catch (Exception ex)
            {
                string message = _productMessages.ExceptionError + new HttpResponseMessage(System.Net.HttpStatusCode.OK);
                _response.AddResponse(false, 0, null, message);
                _log.Error("log4net : error in the post controller", ex);
                return _response;
            }
        }
        #endregion
        #region Update Method for Product
        [HttpPut]
        public ResponseModel<ProductEntity> Put([FromBody] ProductEntity product)
        {
            ResponseModel<ProductEntity> _response = new ResponseModel<ProductEntity>();
            ProductEntity entity = new ProductEntity();
            entity = product;
            List<Ram> rams = new List<Ram>();
            List<Storage> storages = new List<Storage>();
            try
            {
               
                foreach(Ram data in product.specs.rams)
                {
                    if (data.id != 0)
                    {
                        _ramOperations.Edit(data);
                        /*entity.specs.rams.Remove(data);*/
                    }
                    else {
                        Ram ramAdd = new Ram();
                        ramAdd.id = 0;
                        ramAdd.ram = data.ram;
                        ramAdd.specificatiionid = data.specificatiionid;
                        rams.Add(ramAdd);
                    }
                    
                }
                foreach(Storage data in product.specs.storages)
                {
                    if(data.id != 0)
                    {
                        _storageOperations.Edit(data);
                        /*entity.specs.storages.Remove(data);*/
                    }
                    else
                    {
                        Storage ramAdd = new Storage();
                        ramAdd.id = 0;
                        ramAdd.storage = data.storage;
                        ramAdd.specificationid = data.specificationid;
                        storages.Add(ramAdd);
                    }
                    
                }
                product.specs.rams = null;
                product.specs.storages = null;
                product.specs.rams = rams;
                product.specs.storages = storages;
                _productOperations.EditProduct(product);
                string message = _productMessages.Updated + new HttpResponseMessage(System.Net.HttpStatusCode.OK);
                _response.AddResponse(true, 0,null, message);
                return _response;
            }
            catch (Exception ex)
            {
                string message = _productMessages.ExceptionError + new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError);
                _response.AddResponse(false, 0,null, message);
                _log.Error("log4net : error in the post controller", ex);
                return _response;
            }
        }
        #endregion

        #region Get Method for Master data
        [HttpGet("MasterData")]
        public ResponseModel<IEnumerable<MasterTable>> MasterData()
        {
            ResponseModel<IEnumerable<MasterTable>> _response = new ResponseModel<IEnumerable<MasterTable>>();
            try
            {
                _masterDataList = _masterOperations.GetAll().Result;
                if (_masterDataList == null)
                {
                    string message = _masterMessages.Null +" : " +  new HttpResponseMessage(System.Net.HttpStatusCode.OK);
                    _response.AddResponse(true, 0, null, message);
                    return _response;
                }
                else
                {
                    
                    string message = "" + new HttpResponseMessage(System.Net.HttpStatusCode.OK);
                    _response.AddResponse(true, 0, _masterDataList, message);

                    return _response;
                }

            }
            catch (Exception ex)
            {
                string message = _masterMessages.ExceptionError + " : "+ new HttpResponseMessage(System.Net.HttpStatusCode.OK);
                _response.AddResponse(false, 0,null, message);
                _log.Error("log4net : error in the post controller", ex);
                return _response;
            }

        }
        #endregion

        public ResponseModel<IEnumerable<ProductSubPart>> GetProductSubPart()
        {
            ResponseModel<IEnumerable<ProductSubPart>> _response = new ResponseModel<IEnumerable<ProductSubPart>>();
            try
            {
                _productSubParts = _subPartOperations.GetProduct().Result;
                if (_masterDataList == null)
                {
                    string message = _masterMessages.Null + " : " + new HttpResponseMessage(System.Net.HttpStatusCode.OK);
                    _response.AddResponse(true, 0, null, message);
                    return _response;
                }
                else
                {

                    string message = "" + new HttpResponseMessage(System.Net.HttpStatusCode.OK);
                    _response.AddResponse(true, 0, _productSubParts, message);

                    return _response;
                }

            }
            catch (Exception ex)
            {
                string message = _masterMessages.ExceptionError + " : " + new HttpResponseMessage(System.Net.HttpStatusCode.OK);
                _response.AddResponse(false, 0, null, message);
                _log.Error("log4net : error in the post controller", ex);
                return _response;
            }
        }
        #region Post Method for Product
        [HttpPost("PostProductSubPart")]
        public IActionResult PostProductSubPart([FromBody] ProductSubPart product)
        {
            ResponseModel<string> _response = new ResponseModel<string>();
            try
            {
                _subPartOperations.AddProduct(product);
                string message = _productMessages.Added + ", Response Message : " + new HttpResponseMessage(System.Net.HttpStatusCode.OK);
                _response.AddResponse(true, 0, "", message);
                var data = Newtonsoft.Json.JsonConvert.SerializeObject(_response);
                return new JsonResult(_response);
            }
            catch (Exception ex)
            {
                string message = _productMessages.ExceptionError + ", Response Message : " + new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError);
                _response.AddResponse(false, 0, _productMessages.ExceptionError, message);
                _log.Error("log4net : error in the post controller", ex);
                return new JsonResult(_response);
            }

        }
        #endregion

        #region Get Method for Rams
        [HttpGet("GetRams")]
        public ResponseModel<IEnumerable<Ram>> GetRams()
        {
            ResponseModel<IEnumerable<Ram>> _response = new ResponseModel<IEnumerable<Ram>>();
            try
            {
               _rams = _ramOperations.Get().Result;
                if (_rams == null)
                {
                    string message = _masterMessages.Null + " : " + new HttpResponseMessage(System.Net.HttpStatusCode.OK);
                    _response.AddResponse(true, 0, null, message);
                    return _response;
                }
                else
                {

                    string message = "" + new HttpResponseMessage(System.Net.HttpStatusCode.OK);
                    _response.AddResponse(true, 0, _rams, message);

                    return _response;
                }

            }
            catch (Exception ex)
            {
                string message = _masterMessages.ExceptionError + " : " + new HttpResponseMessage(System.Net.HttpStatusCode.OK);
                _response.AddResponse(false, 0, null, message);
                _log.Error("log4net : error in the post controller", ex);
                return _response;
            }

        }
        #endregion

        #region Get Method for Storages
        [HttpGet("GetStorages")]
        public ResponseModel<IEnumerable<Storage>> GetStorages()
        {
            ResponseModel<IEnumerable<Storage>> _response = new ResponseModel<IEnumerable<Storage>>();
            try
            {
                _storages = _storageOperations.Get().Result;
                if (_storages == null)
                {
                    string message = _masterMessages.Null + " : " + new HttpResponseMessage(System.Net.HttpStatusCode.OK);
                    _response.AddResponse(true, 0, null, message);
                    return _response;
                }
                else
                {

                    string message = "" + new HttpResponseMessage(System.Net.HttpStatusCode.OK);
                    _response.AddResponse(true, 0, _storages, message);

                    return _response;
                }

            }
            catch (Exception ex)
            {
                string message = _masterMessages.ExceptionError + " : " + new HttpResponseMessage(System.Net.HttpStatusCode.OK);
                _response.AddResponse(false, 0, null, message);
                _log.Error("log4net : error in the post controller", ex);
                return _response;
            }

        }
        #endregion
        #region Delete Method for Product
        [HttpDelete("DeleteRam/{id}")]
        public ResponseModel<ProductEntity> DeleteRam(int id)
        {
            ResponseModel<ProductEntity> _response = new ResponseModel<ProductEntity>();
            try
            {
                var RamData = _ramOperations.Get().Result.Where(c => c.id.Equals(id)).FirstOrDefault();
                _ramOperations.DeleteProduct(RamData);
                string message = _productMessages.Deleted + new HttpResponseMessage(System.Net.HttpStatusCode.OK);
                _response.AddResponse(true, 0, null, message);
                return _response;
            }
            catch (Exception ex)
            {
                string message = _productMessages.ExceptionError + new HttpResponseMessage(System.Net.HttpStatusCode.OK);
                _response.AddResponse(false, 0, null, message);
                _log.Error("log4net : error in the post controller", ex);
                return _response;
            }
        }
        #endregion
        #region Delete Method for Product
        [HttpDelete("DeleteStorage/{id}")]
        public ResponseModel<ProductEntity> DeleteStorage(int id)
        {
            ResponseModel<ProductEntity> _response = new ResponseModel<ProductEntity>();
            try
            {
                var StorageData = _storageOperations.Get().Result.Where(c => c.id.Equals(id)).FirstOrDefault();
                _storageOperations.DeleteProduct(StorageData);
                string message = _productMessages.Deleted + new HttpResponseMessage(System.Net.HttpStatusCode.OK);
                _response.AddResponse(true, 0, null, message);
                return _response;
            }
            catch (Exception ex)
            {
                string message = _productMessages.ExceptionError + new HttpResponseMessage(System.Net.HttpStatusCode.OK);
                _response.AddResponse(false, 0, null, message);
                _log.Error("log4net : error in the post controller", ex);
                return _response;
            }
        }
        #endregion

    }
}
