using ApiLayer.Messages;
using ApiLayer.Models;
using BusinessObjectLayer.ProductOperations;
using DomainLayer.ProductModel;
using log4net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace ApiLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesOperations : ControllerBase
    {
        private readonly ILog _log;
        ProductDbContext _context;
        IProductImageOperations _productOperations;
        ResponseModel<Images> _response;
        IEnumerable<Images> _productDataList;
        Images _productData;
        IMessages _productMessages;
        IWebHostEnvironment _webHostEnvironment;

        public ImagesOperations(ProductDbContext context, IProductImageOperations productOperations, IWebHostEnvironment web)
        {

            #region Object Assigning
            _context = context;
            _webHostEnvironment = web;
            _productOperations = productOperations;
            _response = new ResponseModel<Images>();
            _productData = new Images();
            _log = LogManager.GetLogger(typeof(ProductController));
            _productMessages = new ProductMessages(_webHostEnvironment);
            #endregion
        }

        #region Get Method for Images
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                ResponseModel<IEnumerable<Images>> _response = new ResponseModel<IEnumerable<Images>>();
                List<Images> data = new List<Images>();
                data = _productOperations.get().Result.ToList();
               /* _productData = data.Where(c => c.id.Equals()).FirstOrDefault();*/
               /* _productOperations.delete(_productData);*/
                string message = _productMessages.Deleted + new HttpResponseMessage(System.Net.HttpStatusCode.OK);
                _response.AddResponse(true, 0, data, message);
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

        #region Delete Method for Product
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                ResponseModel<string> _response = new ResponseModel<string>();
                List<Images> data = new List<Images>();
                data = _productOperations.get().Result.ToList();
                _productData = data.Where(c => c.id.Equals(id)).FirstOrDefault();
                _productOperations.delete(_productData);
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
