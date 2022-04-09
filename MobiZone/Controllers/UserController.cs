using ApiLayer.Messages;
using ApiLayer.Models;
using BusinessObjectLayer.User;
using DomainLayer.Users;
using log4net;
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
    public class UserController : ControllerBase
    {
        private readonly ILog _log;
        ProductDbContext _userContext;
        IUserCreate _userCreate;
        UserRegistration _user;
        IMessages _userMessages;
        ResponseModel<UserRegistration> _userResponse;
        public UserController(ProductDbContext userContext,IUserCreate userCreate )
        {
            _userContext = userContext;
            _userCreate = userCreate;
            _user = new UserRegistration();
            _userMessages = new UserMessages();
            _userResponse = new ResponseModel<UserRegistration>();
            _log = LogManager.GetLogger(typeof(UserController));
        }
        [HttpPost]

        public IActionResult Post([FromBody]UserRegistration users)
        {
            ResponseModel<string> _userResponse = new ResponseModel<string>();
            try
            {
                _userCreate.AddUserRegistration(users);
                string message = _userMessages.Added + ",Response Message : " + new HttpResponseMessage(System.Net.HttpStatusCode.OK);
                _userResponse.AddResponse(true,0,null,message);
                return new JsonResult(_userResponse);
            }
            catch(Exception ex)
            {
                _userCreate.AddUserRegistration(users);
                string message = _userMessages.ExceptionError + ",Respons Message : " + new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError);
                _userResponse.AddResponse(false, 0,_userMessages.ExceptionError, message);
                _log.Error("log4net:Error in post controller", ex);
                return new JsonResult(_userResponse);
            }
            

        }
    }
}
