using ApiLayer.Messages;
using ApiLayer.Models;
using BusinessObjectLayer.ProductOperations;
using DomainLayer.ProductModel;
using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace ApiLayer.Controllers
{
    [Authorize]
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

        public ProductOpController(ProductDbContext context, IProductOperations productOperations)
        {
            #region Object Assigning
            _context = context;
            _productOperations = productOperations;
            _response = new ResponseModel<ProductEntity>();
            _productData = new ProductEntity();
            _log = LogManager.GetLogger(typeof(ProductController));
            _productMessages = new ProductMessages();
            #endregion
        }

        #region Post Method for Product
        [HttpPost]
        public IActionResult Post([FromBody] ProductEntity product)
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

        #region Get Method for Products
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                _productDataList = _productOperations.GetAll();
                if (_productDataList == null)
                {
                    ResponseModel<string> _response = new ResponseModel<string>();
                    string message = _productMessages.Null + new HttpResponseMessage(System.Net.HttpStatusCode.OK);
                    _response.AddResponse(true, 0, _productMessages.Null, message);
                    return new JsonResult(_response);
                }
                else
                {
                    ResponseModel<IEnumerable<ProductEntity>> _response = new ResponseModel<IEnumerable<ProductEntity>>();
                    string message = "" + new HttpResponseMessage(System.Net.HttpStatusCode.OK);
                    _response.AddResponse(true, 0, _productDataList, message);

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
    }
}
