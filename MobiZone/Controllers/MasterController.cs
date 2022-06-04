using ApiLayer.Messages;
using ApiLayer.Models;
using BusinessObjectLayer.ProductOperations;
using DomainLayer.ProductModel;
using DomainLayer.ProductModel.Master;
using log4net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ApiLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MasterController : ControllerBase
    {
        private readonly ILog _log;
        ProductDbContext _context;
        IMasterDataOperations _masterOperations;
        ResponseModel<MasterTable> _response;
        IEnumerable<MasterTable> _masterDataList;
        MasterTable _masterData;
        IMessages _masterMessages;
        IWebHostEnvironment _webHostEnvironment;
        IProductOperations _productOperations;
        public MasterController(ProductDbContext context, IMasterDataOperations masterOperations, IWebHostEnvironment web, IProductOperations productOperations)
        {
            _context = context;
            _webHostEnvironment = web;
            _masterOperations = masterOperations;
            _response = new ResponseModel<MasterTable>();
            _masterData = new MasterTable();
            _log = LogManager.GetLogger(typeof(ProductController));
            _masterMessages = new MasterMessages(_webHostEnvironment);
            _productOperations = productOperations;
        }
        #region Create method for products
        [HttpPost]
        public IActionResult Post([FromBody] MasterTable masterData)
        {
            ResponseModel<string> _response = new ResponseModel<string>();
            try
            {
                _masterOperations.Add(masterData);
                string message = _masterMessages.Added + ", Response Message : " + new HttpResponseMessage(System.Net.HttpStatusCode.OK);
                _response.AddResponse(true, 0, "", message);
                var data = Newtonsoft.Json.JsonConvert.SerializeObject(_response);
                return new JsonResult(_response);
            }
            catch (Exception ex)
            {
                string message = _masterMessages.ExceptionError + ", Response Message : " + new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError);
                _response.AddResponse(false, 0, _masterMessages.ExceptionError, message);
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
                _masterDataList = _masterOperations.GetAll().Result;
                if (_masterDataList == null)
                {
                    ResponseModel<string> _response = new ResponseModel<string>();
                    string message = _masterMessages.Null + new HttpResponseMessage(System.Net.HttpStatusCode.OK);
                    _response.AddResponse(true, 0, _masterMessages.Null, message);
                    return new JsonResult(_response);
                }
                else
                {
                    ResponseModel<IEnumerable<MasterTable>> _response = new ResponseModel<IEnumerable<MasterTable>>();
                    string message = "" + new HttpResponseMessage(System.Net.HttpStatusCode.OK);
                    _response.AddResponse(true, 0, _masterDataList, message);

                    return new JsonResult(_response);
                }

            }
            catch (Exception ex)
            {
                ResponseModel<string> _response = new ResponseModel<string>();
                string message = _masterMessages.ExceptionError + new HttpResponseMessage(System.Net.HttpStatusCode.OK);
                _response.AddResponse(false, 0, _masterMessages.ExceptionError, message);
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
                List<MasterTable> data = new List<MasterTable>();
                data = _masterOperations.GetAll().Result.ToList();
                _masterData = data.Where(c => c.id.Equals(id)).FirstOrDefault();
                _masterOperations.Delete(_masterData);
                string message = _masterMessages.Deleted + new HttpResponseMessage(System.Net.HttpStatusCode.OK);
                _response.AddResponse(true, 0, _masterMessages.Deleted, message);
                return new JsonResult(_response);
            }
            catch (Exception ex)
            {
                ResponseModel<string> _response = new ResponseModel<string>();
                string message = _masterMessages.ExceptionError + new HttpResponseMessage(System.Net.HttpStatusCode.OK);
                _response.AddResponse(false, 0, _masterMessages.ExceptionError, message);
                _log.Error("log4net : error in the post controller", ex);
                return new JsonResult(_response);
            }
        }
        #endregion
        #region Update Method for Product
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] MasterTable product)
        {
            ResponseModel<string> _response = new ResponseModel<string>();
            try
            {
                 _masterOperations.Edit(product);
                await Task.Delay(1000);
                var products = _productOperations.GetAll().Result;
                foreach(var data in products)
                {
                    var datas = _productOperations.GetAll().Result;
                    await Task.Delay(1000);
                    var getById = datas.Where(c=> c.id.Equals(data.id)).FirstOrDefault();
                    _productOperations.EditProduct(getById);
                }
                string message = _masterMessages.Updated + new HttpResponseMessage(System.Net.HttpStatusCode.OK);
                _response.AddResponse(true, 0, _masterMessages.Updated, message);
                return new JsonResult(_response);
            }
            catch (Exception ex)
            {
                string message = _masterMessages.ExceptionError + new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError);
                _response.AddResponse(false, 0, _masterMessages.ExceptionError, message);
                _log.Error("log4net : error in the post controller", ex);
                return new JsonResult(_response);
            }
        }
        #endregion
    }
}
