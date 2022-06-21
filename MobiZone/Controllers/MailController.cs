using ApiLayer.Messages;
using ApiLayer.Models;
using BusinessObjectLayer;
using DomainLayer;
using log4net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ApiLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MailController : ControllerBase
    {
        private readonly IMailService _mailService;
        private readonly ILog _log;
        public MailController(IMailService _mailService)
        {
           this._mailService = _mailService;
            /*_log = LogManager.GetLogger(typeof(ProductController));*/
        }
        [HttpPost("send")]
        public async Task<IActionResult> SendMail( MailRequest request)
        {
            ResponseModel<string> _response = new ResponseModel<string>();
            try
            {
                await _mailService.SendEmailAsync(request);
                string message = ", Response Message : " + new HttpResponseMessage(System.Net.HttpStatusCode.OK);
                _response.AddResponse(true, 0, "", message);
                var data = Newtonsoft.Json.JsonConvert.SerializeObject(_response);
                return new JsonResult(_response);
            }
            catch(Exception ex)
            {
                string message = ", Response Message : " + new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError);
                _response.AddResponse(false, 0,null, message);
                _log.Error("log4net : error in the post controller", ex);
                throw;
            }
        }
    }
}
