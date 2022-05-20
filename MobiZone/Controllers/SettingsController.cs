using ApiLayer.Messages;
using ApiLayer.Models;
using AutoMapper;
using BusinessObjectLayer;
using DomainLayer;
using DomainLayer.ProductModel;
using log4net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Repository;
using System;
using System.Collections.Generic;
using System.Net.Http;
 

namespace ApiLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SettingsController : ControllerBase
    {
        private readonly ILog _log;
        ProductDbContext _context;
        IPrivacyOperation _privacyOperation;
        IAboutOperations _aboutOperations;
        IContactOperations _contactOperations;
        IEnumerable<About> _about;
        IEnumerable<PrivacyPolicy> _privacyPolicy;
        IEnumerable<AdminContact> _contact;

        IMessages _productMessages;
        IWebHostEnvironment _webHostEnvironment;
        IMapper _mapper;


        public SettingsController(ProductDbContext context, IPrivacyOperation privacyOperation, IAboutOperations aboutOperations, IContactOperations contactOperations, IWebHostEnvironment web, IMapper mapper)

        {
            #region Object Assigning
            _context = context;
            _webHostEnvironment = web;
            _privacyOperation = privacyOperation;
            _aboutOperations = aboutOperations;
               _contactOperations = contactOperations;
            _log = LogManager.GetLogger(typeof(ProductController));
            _productMessages = new ProductMessages(_webHostEnvironment);
            _mapper = mapper;
            #endregion
        }

        [HttpPost("PrivacyPost")]
        public IActionResult PrivacyPost([FromBody] PrivacyPolicy privacy)
        {
            ResponseModel<string> _response = new ResponseModel<string>();
            try
            {
                _privacyOperation.Add(privacy);
                string message = "Content Added" + ", Response Message : " + new HttpResponseMessage(System.Net.HttpStatusCode.OK);
                _response.AddResponse(true, 0, "", message);
                var data = Newtonsoft.Json.JsonConvert.SerializeObject(_response);
                return new JsonResult(_response);
            }
            catch (Exception ex)
            {
                string message = "Exception occured" + ", Response Message : " + new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError);
                _response.AddResponse(false, 0, "", message);
                _log.Error("log4net : error in the post controller", ex);
                return new JsonResult(_response);
            }

        }
        [HttpGet("PrivacyGet")]
        public ResponseModel< IEnumerable<PrivacyPolicy>> PrivacyGet()
        {
            ResponseModel <IEnumerable<PrivacyPolicy>> _response = new ResponseModel<IEnumerable<PrivacyPolicy>>();
            try
            {
                _privacyPolicy = _privacyOperation.Get().Result;
                if (_privacyPolicy == null)
                {
                    string message = "" + " , " + new HttpResponseMessage(System.Net.HttpStatusCode.OK);
                    _response.AddResponse(true, 0,null, message);
                    return _response;
                }
                else
                {
                    string message = "" + new HttpResponseMessage(System.Net.HttpStatusCode.OK);
                    _response.AddResponse(true, 0, _privacyPolicy, message);
                    return _response;
                }

            }
            catch (Exception ex)
            {
                string message = "Exception occured" + new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError);
                _response.AddResponse(false, 0, null, message);
                _log.Error("log4net : error in the post controller", ex);
                return _response;
            }

        }

        #region Update Method for Privacy
        [HttpPut("PrivacyPut")]
        public IActionResult PrivacyPut([FromBody] PrivacyPolicy privacyPolicy)
        {
            ResponseModel<string> _response = new ResponseModel<string>();
            try
            {
                _privacyOperation.Edit(privacyPolicy);
                string message = _productMessages.Updated + new HttpResponseMessage(System.Net.HttpStatusCode.OK);
                _response.AddResponse(true, 0, _productMessages.Updated, message);
                return new JsonResult(_response);
            }
            catch (Exception ex)
            {
                string message = "error occured" + new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError);
                _response.AddResponse(false, 0, _productMessages.ExceptionError, message);
                _log.Error("log4net : error in the post controller", ex);
                return new JsonResult(_response);
            }
        }
        #endregion


        [HttpPost("AboutPost")]
        public IActionResult AboutPost([FromBody] About about)
        {
            ResponseModel<string> _response = new ResponseModel<string>();
            try
            {
                _aboutOperations.Add(about);
                string message = "Content Added" + ", Response Message : " + new HttpResponseMessage(System.Net.HttpStatusCode.OK);
                _response.AddResponse(true, 0, "", message);
                var data = Newtonsoft.Json.JsonConvert.SerializeObject(_response);
                return new JsonResult(_response);
            }
            catch (Exception ex)
            {
                string message = "Exception occured" + ", Response Message : " + new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError);
                _response.AddResponse(false, 0, "", message);
                _log.Error("log4net : error in the post controller", ex);
                return new JsonResult(_response);
            }

        }
        [HttpGet("AboutGet")]
        public ResponseModel<IEnumerable<About>> AboutGet()
        {
            ResponseModel<IEnumerable<About>> _response = new ResponseModel<IEnumerable<About>>();
            try
            {
                _about = _aboutOperations.Get().Result;
                if (_aboutOperations == null)
                {
                    string message = "Content added" + " , " + new HttpResponseMessage(System.Net.HttpStatusCode.OK);
                    _response.AddResponse(true, 0, null, message);
                    return _response;
                }
                else
                {
                    string message = "" + new HttpResponseMessage(System.Net.HttpStatusCode.OK);
                    _response.AddResponse(true, 0, _about, message);
                    return _response;
                }

            }
            catch (Exception ex)
            {
                string message = "Exception occured" + new HttpResponseMessage(System.Net.HttpStatusCode.OK);
                _response.AddResponse(false, 0, null, message);
                _log.Error("log4net : error in the post controller", ex);
                return _response;
            }

        }

        #region Update Method for About
        [HttpPut("AboutPut")]
        public IActionResult AboutPut([FromBody] About about)
        {
            ResponseModel<string> _response = new ResponseModel<string>();
            try
            {
                _aboutOperations.Edit(about);
                string message = _productMessages.Updated + new HttpResponseMessage(System.Net.HttpStatusCode.OK);
                _response.AddResponse(true, 0, _productMessages.Updated, message);
                return new JsonResult(_response);
            }
            catch (Exception ex)
            {
                string message = "error occured" + new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError);
                _response.AddResponse(false, 0, null, message);
                _log.Error("log4net : error in the post controller", ex);
                return new JsonResult(_response);
            }
        }
        #endregion


        [HttpPost("ContactPost")]
        public IActionResult ContactPost([FromBody] AdminContact contact)
        {
            ResponseModel<string> _response = new ResponseModel<string>();
            try
            {
                _contactOperations.Add(contact);
                string message = "Content Added" + ", Response Message : " + new HttpResponseMessage(System.Net.HttpStatusCode.OK);
                _response.AddResponse(true, 0, "", message);
                var data = Newtonsoft.Json.JsonConvert.SerializeObject(_response);
                return new JsonResult(_response);
            }
            catch (Exception ex)
            {
                string message = "Exception occured" + ", Response Message : " + new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError);
                _response.AddResponse(false, 0, "", message);
                _log.Error("log4net : error in the post controller", ex);
                return new JsonResult(_response);
            }

        }
        [HttpGet("ContactGet")]
        public ResponseModel<IEnumerable<AdminContact>> ContactGet()
        {
            ResponseModel<IEnumerable<AdminContact>> _response = new ResponseModel<IEnumerable<AdminContact>>();
            try
            { 
                _contact = _contactOperations.Get().Result;
                if (_contactOperations == null)
                {
                    string message = "Content added" + " , " + new HttpResponseMessage(System.Net.HttpStatusCode.OK);
                    _response.AddResponse(true, 0, null, message);
                    return _response;
                }
                else
                {
                    string message = "" + new HttpResponseMessage(System.Net.HttpStatusCode.OK);
                    _response.AddResponse(true, 0, _contact, message);
                    return _response;
                }

            }
            catch (Exception ex)
            {
                string message = "Exception occured" + new HttpResponseMessage(System.Net.HttpStatusCode.OK);
                _response.AddResponse(false, 0, null, message);
                _log.Error("log4net : error in the post controller", ex);
                return _response;
            }
        }
        #region Update Method for Contact
        [HttpPut("ContactPut")]
            public IActionResult ContactPut([FromBody] AdminContact contact)
        {
            ResponseModel<string> _response = new ResponseModel<string>();
            try
            {
                _contactOperations.Edit(contact);
                string message = "contact edited" + new HttpResponseMessage(System.Net.HttpStatusCode.OK);
                _response.AddResponse(true, 0, _productMessages.Updated, message);
                return new JsonResult(_response);
            }
            catch (Exception ex)
            {
                string message = "error occured" + new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError);
                _response.AddResponse(false, 0, null, message);
                _log.Error("log4net : error in the post controller", ex);
                return new JsonResult(_response);
            }
        }
        #endregion
    }
}
