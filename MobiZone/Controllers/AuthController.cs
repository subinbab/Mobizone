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
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        public AuthController(ProductDbContext userContext, IUserCreate userCreate, IMapper mapper, IWebHostEnvironment web, ILoginOperations loginOperations, UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration)
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
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
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
                Login check = list.Where(c => c.username.Equals(data.username) && c.password.Equals(data.password)).FirstOrDefault();
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
        public async Task<ResponseModel<Login>> admin(LoginViewModel data)
        {
            try
            {
                ResponseModel<Login> _response = new ResponseModel<Login>();
                string message;
                string password = _sec.Encrypt("subin", data.password);
                var list = await _loginOperations.Get();
                Login check = list.Where(c => c.username.Equals(data.username) && c.password.Equals(data.password)).FirstOrDefault();
                /*UserRegistration check = _userCreate.Authenticate(data.userName, password);*/
                if (check != null)
                {
                    message = _userMessages.Added + new HttpResponseMessage(System.Net.HttpStatusCode.OK);
                    _response.AddResponse(true, 0, check, message);
                    return _response;
                }
                message = _userMessages.Null;
                _response.AddResponse(false, 0, null, message);
                return _response;
            }
            catch (Exception ex)
            {
                ResponseModel<Login> _response = new ResponseModel<Login>();
                _response.AddResponse(false, 0, null, ex.Message);
                return _response;
            }

        }
        
    }
}
