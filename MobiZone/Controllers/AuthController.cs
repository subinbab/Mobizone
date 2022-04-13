using ApiLayer.Messages;
using ApiLayer.Models;
using AutoMapper;
using BusinessObjectLayer;
using BusinessObjectLayer.User;
using DomainLayer;
using DomainLayer.Users;
using DTOLayer.UserModel;
using log4net;
using Microsoft.AspNetCore.Hosting;

using Microsoft.AspNetCore.Mvc;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;


namespace ApiLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILog _log;
        ProductDbContext _userContext;
        IUserCreate _userCreate;
        UserRegistration _user;
        IMessages _userMessages;
        ResponseModel<UserDataViewModel> _userResponse;
        IMapper _mapper;
        IEnumerable<UserRegistration> _userList;
        List<UserDataViewModel> _userDataList;
        IWebHostEnvironment _webHostEnvironment;
        Security _sec;
        ILoginOperations _loginOperations;
        public AuthController(ProductDbContext userContext, IUserCreate userCreate, IMapper mapper, IWebHostEnvironment web, ILoginOperations loginOperations)
        {
            _webHostEnvironment = web;
            _userContext = userContext;
            _userCreate = userCreate;
            _user = new UserRegistration();
            _userMessages = new UserMessages(_webHostEnvironment);
            _userResponse = new ResponseModel<UserDataViewModel>();
            _log = LogManager.GetLogger(typeof(UsersController));
            _mapper = mapper;
            _userDataList = new List<UserDataViewModel>();
            _sec = new Security();
            _loginOperations = loginOperations;
        }
        [HttpPost]
        public async Task<IActionResult> post(LoginViewModel data)
        {
            try
            {
                ResponseModel<Login> _response = new ResponseModel<Login>();
                string message;
                string password = _sec.Encrypt("subin", data.password);
                var list = await _loginOperations.Get();
                Login check = list.Where(c => c.username.Equals(data.userName) && c.password.Equals(data.password)).FirstOrDefault();
                /*UserRegistration check = _userCreate.Authenticate(data.userName, password);*/
                if (check != null)
                {
                    message = _userMessages.Added + new HttpResponseMessage(System.Net.HttpStatusCode.OK);
                    _response.AddResponse(true, 0, check, message);
                    return Ok();
                }
                message = _userMessages.Null;
                _response.AddResponse(false, 0, null, message);
                return NotFound();
            }
            catch (Exception ex)
            {
                ResponseModel<string> _response = new ResponseModel<string>();
                _response.AddResponse(false, 0, null, ex.Message);
                return BadRequest();
            }

        }
        [HttpPost("admin")]
        public async Task<HttpResponseMessage> admin(LoginViewModel data)
        {
            try
            {
                ResponseModel<Login> _response = new ResponseModel<Login>();
                string message;
                string password = _sec.Encrypt("subin", data.password);
                var list = await _loginOperations.Get();
                Login check = list.Where(c => c.username.Equals(data.userName) && c.password.Equals(data.password)&& c.roleId.Equals(2)).FirstOrDefault();
                /*UserRegistration check = _userCreate.Authenticate(data.userName, password);*/
                if (check != null)
                {
                    message = _userMessages.Added + new HttpResponseMessage(System.Net.HttpStatusCode.OK);
                    _response.AddResponse(true, 0, check, message);
                    return new HttpResponseMessage(System.Net.HttpStatusCode.OK);
                }
                message = _userMessages.Null;
                _response.AddResponse(false, 0, null, message);
                return new HttpResponseMessage(System.Net.HttpStatusCode.NotFound);
            }
            catch (Exception ex)
            {
                ResponseModel<string> _response = new ResponseModel<string>();
                _response.AddResponse(false, 0, null, ex.Message);
                return new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError);
            }

        }

    }
}
