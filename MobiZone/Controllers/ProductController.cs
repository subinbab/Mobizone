using ApiLayer.Messages;
using ApiLayer.Models;
using BusinessObjectLayer;
using DomainLayer;
using log4net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web.Helpers;

namespace ApiLayer.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        #region Private Variables
        private readonly ILog _log;
        ProductDbContext _context;
        IProductCatalog _catalog;
        ResponseModel<Product> _response;
        IEnumerable<Product> _productList;
        Product _product;
        IMessages _productMessages;
        #endregion
        /*IRepositoryOperations<Product> _repo;*/
        public ProductController(ProductDbContext context, IProductCatalog catalog)
        {
            #region Object Assigning
            _context = context;
            _catalog = catalog;
            _response = new ResponseModel<Product>();
            _product = new Product();
            _log = LogManager.GetLogger(typeof(ProductController));
            _productMessages = new ProductMessages();
            #endregion

        }
        #region Post Method for Product
        [HttpPost]
        public IActionResult Post([FromBody] Product product)
        {
            ResponseModel<string> _response = new ResponseModel<string>();
            try
            {
                _catalog.AddProduct(product);
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

        #region Update Method for Product
        [HttpPut]
        public IActionResult Put([FromBody] Product product)
        {
            ResponseModel<string> _response = new ResponseModel<string>();
            try
            {
                _catalog.EditProduct(product);
                string message = _productMessages.Updated + new HttpResponseMessage(System.Net.HttpStatusCode.OK);
                _response.AddResponse(true, 0, _productMessages.Updated, message);
                return new JsonResult(_response);
            }
            catch (Exception ex)
            {
                string message = _productMessages.ExceptionError + new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError);
                _response.AddResponse(false, 0, _productMessages.ExceptionError, message);
                _log.Error("log4net : error in the post controller", ex);
                return new JsonResult(_response);
            }
        }
        #endregion

        #region Get Method for Products
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                _productList = _catalog.GetProduct();
                if (_productList == null)
                {
                    ResponseModel<string> _response = new ResponseModel<string>();
                    string message = _productMessages.Null + new HttpResponseMessage(System.Net.HttpStatusCode.OK);
                    _response.AddResponse(true, 0, _productMessages.Null, message);
                    return new JsonResult(_response);
                }
                else
                {
                    ResponseModel<IEnumerable<Product>> _response = new ResponseModel<IEnumerable<Product>>();
                    string message = "" + new HttpResponseMessage(System.Net.HttpStatusCode.OK);
                    _response.AddResponse(true, 0, _productList, message);

                    return new JsonResult(_response);
                }

            }
            catch (Exception ex)
            {
                ResponseModel<string> _response = new ResponseModel<string>();
                string message = _productMessages.ExceptionError + new HttpResponseMessage(System.Net.HttpStatusCode.OK);
                _response.AddResponse(false, 0, _productMessages.ExceptionError, message);
                _log.Error("log4net : error in the post controller", ex);
                return new JsonResult(_response);
            }

        }
        #endregion

        #region Get Method for product
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                _product = _catalog.GetById(id);
                if (_product == null)
                {
                    ResponseModel<string> _response = new ResponseModel<string>();
                    string message = _productMessages.Null + new HttpResponseMessage(System.Net.HttpStatusCode.OK);
                    _response.AddResponse(true, 0, null, message);
                    return new JsonResult(_response);
                }
                else
                {
                    ResponseModel<Product> _response = new ResponseModel<Product>();
                    string message = "" + new HttpResponseMessage(System.Net.HttpStatusCode.OK);
                    _response.AddResponse(true, 0, _product, message);
                    return new JsonResult(_response);
                }

            }
            catch (Exception ex)
            {
                ResponseModel<string> _response = new ResponseModel<string>();
                string message = _productMessages.ExceptionError + new HttpResponseMessage(System.Net.HttpStatusCode.OK);
                _response.AddResponse(false, 0, _productMessages.ExceptionError, message);
                _log.Error("log4net : error in the post controller", ex);
                return new JsonResult(_response);
            }

        }
        #endregion

        #region Delete Method for Product
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                ResponseModel<string> _response = new ResponseModel<string>();
                _product = _catalog.GetById(id);
                _catalog.DeleteProduct(_product);
                string message = _productMessages.Deleted + new HttpResponseMessage(System.Net.HttpStatusCode.OK);
                _response.AddResponse(true, 0, _productMessages.Deleted, message);
                return new JsonResult(_response);
            }
            catch (Exception ex)
            {
                ResponseModel<string> _response = new ResponseModel<string>();
                string message = _productMessages.ExceptionError + new HttpResponseMessage(System.Net.HttpStatusCode.OK);
                _response.AddResponse(false, 0, _productMessages.ExceptionError, message);
                _log.Error("log4net : error in the post controller", ex);
                return new JsonResult(_response);
            }
        }
        #endregion
    }
}
