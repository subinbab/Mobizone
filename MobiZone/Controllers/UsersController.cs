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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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
    public class UsersController : ControllerBase
    {
        private readonly ILog _log;
        ProductDbContext _userContext;
        IUserCreate _userCreate;
        UserRegistration _user;
        AddressOperations _address;
        IMessages _userMessages;
        ResponseModel<UserDataViewModel> _userResponse;
        IMapper _mapper;
        IEnumerable<UserRegistration> _userList; 
        IEnumerable<Address> _addressList;
        List<UserDataViewModel> _userDataList;
        IWebHostEnvironment _webHostEnvironment;
        Security _sec;
        ILoginOperations _loginOperations;
        IAddressOperations _addressOperations;
        Login _login;
        ICheckOutOperation _checkOutOperation;
        IEnumerable<Checkout> _checkout;
        Address _addresData;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        public UsersController(ProductDbContext userContext, IUserCreate userCreate, IMapper mapper, IWebHostEnvironment web, ILoginOperations loginOperations,IAddressOperations addressOperations,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration, ICheckOutOperation checkOutOperation)
        {
            _webHostEnvironment = web;
            _userContext = userContext;
            _userCreate = userCreate;
            _addressOperations = addressOperations;
            _user = new UserRegistration();
            _configuration = configuration;
            _login = new Login();
            _userMessages = new UserMessages(_webHostEnvironment);
            _userResponse = new ResponseModel<UserDataViewModel>();
            _log = LogManager.GetLogger(typeof(UsersController));
            _mapper = mapper;
            _userDataList = new List<UserDataViewModel>();
            _sec = new Security();
            _loginOperations = loginOperations;
            _userManager = userManager;
            _roleManager = roleManager;
            
            _checkOutOperation = checkOutOperation;
        }
        [HttpPost("UserCreate")]
        public IActionResult post([FromBody] UserViewModel users)
        {
            _user = (UserRegistration)_mapper.Map<UserRegistration>(users);
            string myDateTime = DateTime.Now.ToString();
            _user.createdOn = DateTime.Now;
            _user.modifiedOn = DateTime.Now;
            _user.createdBy = users.FirstName + " " + users.LastName;
            _user.modifiedBy = users.FirstName + " " + users.LastName;
            _user.Password = _sec.Encrypt("subin", users.Password);
            _login.username = users.Email;
            _login.password = users.Password;
            _login.createdOn = DateTime.Now;
            _login.createdBy = users.FirstName + " " + users.LastName;
            _login.modifiedOn = DateTime.Now;
            _login.modifiedBy = users.FirstName + " " + users.LastName;
            _login.roleId = (int)RoleTypes.User;
            ResponseModel<string> _userResponse = new ResponseModel<string>();
            try
            {
                _userCreate.AddUserRegistration(_user);
                _loginOperations.Add(_login);
                string message = _userMessages.Added + ",Response Message : " + new HttpResponseMessage(System.Net.HttpStatusCode.OK);
                _userResponse.AddResponse(true, 0, null, message);
                return new JsonResult(_userResponse);
            }
            catch (Exception ex)
            {
                _userCreate.AddUserRegistration(_user);
                string message = _userMessages.ExceptionError + ",Respons Message : " + new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError);
                _userResponse.AddResponse(false, 0, _userMessages.ExceptionError, message);
                _log.Error("log4net:Error in post controller", ex);
                return new JsonResult(_userResponse);
            }


        }


        #region Get Method for users
        [HttpGet("userdata")]
        public IActionResult userdata()
        {
            try
            {
                _userList = _userCreate.Get().Result;
                if (_userList == null)
                {
                    ResponseModel<string> _response = new ResponseModel<string>();
                    string message = _userMessages.Null + new HttpResponseMessage(System.Net.HttpStatusCode.OK);
                    _response.AddResponse(true, 0, _userMessages.Null, message);
                    return new JsonResult(_response);
                }
                else
                {
                    ResponseModel<IEnumerable<UserRegistration>> _response = new ResponseModel<IEnumerable<UserRegistration>>();
                    /*_userDataList = (List< UserDataViewModel>)_mapper.Map<List<UserDataViewModel>>(_userList);*/
                    string message = "" + new HttpResponseMessage(System.Net.HttpStatusCode.OK);
                    _response.AddResponse(true, 0, _userList, message);

                    return new JsonResult(_response);
                }

            }
            catch (Exception ex)
            {
                ResponseModel<string> _response = new ResponseModel<string>();
                string message = _userMessages.ExceptionError + new HttpResponseMessage(System.Net.HttpStatusCode.OK) + ex.Message;
                _response.AddResponse(false, 0, _userMessages.ExceptionError, message);
                _log.Error("log4net : error in the post controller", ex);
                return new JsonResult(_response);
            }

        }
        #endregion
        [HttpPost]
        public IActionResult Login(LoginViewModel data)
        {
            try
            {
                ResponseModel<Login> _response = new ResponseModel<Login>();
                string message;
                Login check = _loginOperations.Get().Result.Where(c=> c.username.Equals(data.userName)&&c.password.Equals(data.password)&&c.roleId.Equals((int)RoleTypes.User)).FirstOrDefault();
                if (check != null)
                {
                    message = _userMessages.Added + new HttpResponseMessage(System.Net.HttpStatusCode.OK) ;
                    _response.AddResponse(true, 0, check, message);
                        return new JsonResult(_response);
                }
                message = _userMessages.Null;
                _response.AddResponse(false,0, null, message);
                return new JsonResult(_response);
            }
            catch(Exception ex)
            {
                ResponseModel<string> _response = new ResponseModel<string>();
                _response.AddResponse(false,0, null, ex.Message);
                return new JsonResult(_response);
            }
            
        }
        [HttpGet("Address")]
        public IActionResult Address()
        {
            try
            {
                /*  _addressList = _addressOperations.Get().Result;*/
                _addressList = _addressOperations.get().Result;
                if (_addressList == null)
                {
                    ResponseModel<string> _response = new ResponseModel<string>();
                    string message = _userMessages.Null + new HttpResponseMessage(System.Net.HttpStatusCode.OK);
                    _response.AddResponse(true, 0, _userMessages.Null, message);
                    return new JsonResult(_response);
                }
                else
                {
                    ResponseModel<IEnumerable<Address>> _response = new ResponseModel<IEnumerable<Address>>();
                    /*_userDataList = (List< UserDataViewModel>)_mapper.Map<List<UserDataViewModel>>(_userList);*/
                    string message = "" + new HttpResponseMessage(System.Net.HttpStatusCode.OK);
                    _response.AddResponse(true, 0, _addressList, message);

                    return new JsonResult(_response);
                }

            }
            catch (Exception ex)
            {
                ResponseModel<string> _response = new ResponseModel<string>();
                string message = _userMessages.ExceptionError + new HttpResponseMessage(System.Net.HttpStatusCode.OK) + ex.Message;
                _response.AddResponse(false, 0, _userMessages.ExceptionError, message);
                _log.Error("log4net : error in the post controller", ex);
                return new JsonResult(_response);
            }
        }

        [HttpPut("Address")]
        public IActionResult Address(Address address)
        {
            try
            {
                /*  _addressList = _addressOperations.Get().Result;*/
                _addressOperations.Edit(address);

                    return Ok();

            }
            catch (Exception ex)
            {
                ResponseModel<string> _response = new ResponseModel<string>();
                string message = _userMessages.ExceptionError + new HttpResponseMessage(System.Net.HttpStatusCode.OK) + ex.Message;
                _response.AddResponse(false, 0, _userMessages.ExceptionError, message);
                _log.Error("log4net : error in the post controller", ex);
                return BadRequest();
            }
        }

        [HttpDelete("Address/{id}")]
        public IActionResult Address(int id)
        {
            try
            {
                /*  _addressList = _addressOperations.Get().Result;*/
                _addresData = _addressOperations.get().Result.Where(c => c.id.Equals(id)).FirstOrDefault();
                _addressOperations.delete(_addresData);

                return Ok();

            }
            catch (Exception ex)
            {
                ResponseModel<string> _response = new ResponseModel<string>();
                string message = _userMessages.ExceptionError + new HttpResponseMessage(System.Net.HttpStatusCode.OK) + ex.Message;
                _response.AddResponse(false, 0, _userMessages.ExceptionError, message);
                _log.Error("log4net : error in the post controller", ex);
                return BadRequest();
            }
        }
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] UserViewModel model)
        {
            ResponseModel<string> _response = new ResponseModel<string>();
            var userExists = await _userManager.FindByNameAsync(model.Email);
            if (userExists != null)
            {
                _response.AddResponse(true, 0, null, "not regsitered");
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }
                

            IdentityUser user = new()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.FirstName + "" + model.LastName
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                _response.AddResponse(true, 0, null, "cant register");
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }
                

            return Ok(_response);
        }

        /*[HttpPost]
        [Route("register-admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterModel model)
        {
            var userExists = await _userManager.FindByNameAsync(model.Username);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });

            IdentityUser user = new()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });

            if (!await _roleManager.RoleExistsAsync(UserRoles.Admin))
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
            if (!await _roleManager.RoleExistsAsync(UserRoles.User))
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.User));

            if (await _roleManager.RoleExistsAsync(UserRoles.Admin))
            {
                await _userManager.AddToRoleAsync(user, UserRoles.Admin);
            }
            if (await _roleManager.RoleExistsAsync(UserRoles.Admin))
            {
                await _userManager.AddToRoleAsync(user, UserRoles.User);
            }
            return Ok(new Response { Status = "Success", Message = "User created successfully!" });
        }*/

        #region Update Method for Users
        [HttpPut]
        public IActionResult Put([FromBody] UserRegistration product)
        {
            ResponseModel<string> _response = new ResponseModel<string>();
            try
            {
                _userCreate.Edit(product);
                string message = _userMessages.Updated + new HttpResponseMessage(System.Net.HttpStatusCode.OK);
                _response.AddResponse(true, 0, _userMessages.Updated, message);
                return new JsonResult(_response);
            }
            catch (Exception ex)
            {
                string message = _userMessages.ExceptionError + new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError);
                _response.AddResponse(false, 0, _userMessages.ExceptionError, message);
                _log.Error("log4net : error in the post controller", ex);
                return new JsonResult(_response);
            }
        }
        #endregion

        #region Post Method for Checkout
        [HttpPost("CheckOutData")]
        public IActionResult CheckOutData([FromBody] Checkout checkoutData)
        {
            ResponseModel<string> _response = new ResponseModel<string>();
            try
            {
                _checkOutOperation.Add(checkoutData);
                string message = "added" + ", Response Message : " + new HttpResponseMessage(System.Net.HttpStatusCode.OK);
                _response.AddResponse(true, 0, "", message);
                var data = Newtonsoft.Json.JsonConvert.SerializeObject(_response);
                return new JsonResult(_response);
            }
            catch (Exception ex)
            {
                string message = "error occured" + ", Response Message : " + new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError);
                _response.AddResponse(false, 0, null, message);
                _log.Error("log4net : error in the post controller", ex);
                return new JsonResult(_response);
            }

        }
        #endregion
        #region GetDetails Method for Checkout
        [HttpGet("CheckOutData")]
        public ResponseModel<IEnumerable<Checkout>> CheckOutData(int id)
        {
            ResponseModel<IEnumerable<Checkout>> _response = new ResponseModel<IEnumerable<Checkout>>();
            try
            {
                _checkout = _checkOutOperation.get().Result;
                if (_checkout == null)
                {
                    string message = " null data" + " , " + new HttpResponseMessage(System.Net.HttpStatusCode.OK);
                    _response.AddResponse(true, 0, null, message);
                    return _response;
                }
                else
                {
                    string message = "" + new HttpResponseMessage(System.Net.HttpStatusCode.OK);
                    _response.AddResponse(true, 0, _checkout, message);
                    return _response;
                }

            }
            catch (Exception ex)
            {
                string message = " exception occured" + new HttpResponseMessage(System.Net.HttpStatusCode.OK);
                _response.AddResponse(false, 0, null, message);
                _log.Error("log4net : error in the post controller", ex);
                return _response;
            }

        }
        #endregion

        #region Update Method for Users
        [HttpPut("CheckoutPut")]
        public IActionResult CheckoutPut([FromBody] Checkout checkout)
        {
            ResponseModel<string> _response = new ResponseModel<string>();
            try
            {
                _checkOutOperation.Edit(checkout);
                string message = "Updated" + new HttpResponseMessage(System.Net.HttpStatusCode.OK);
                _response.AddResponse(true, 0, _userMessages.Updated, message);
                return new JsonResult(_response);
            }
            catch (Exception ex)
            {
                string message ="Error occured" + new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError);
                _response.AddResponse(false, 0, _userMessages.ExceptionError, message);
                _log.Error("log4net : error in the post controller", ex);
                return new JsonResult(_response);
            }
        }
        #endregion
    }


}
