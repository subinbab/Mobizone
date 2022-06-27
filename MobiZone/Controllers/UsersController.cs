 using ApiLayer.Messages;
using ApiLayer.Models;
using AutoMapper;
using BusinessObjectLayer;
using BusinessObjectLayer.ProductOperations;
using BusinessObjectLayer.User;
using DomainLayer;
using DomainLayer.ProductModel;
using DomainLayer.Users;
using DTOLayer.UserModel;
using Firebase.Auth;
using log4net;
using Microsoft.AspNetCore.Authorization;
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
//using System.Web.Http;

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
        IProductOperations _productOperations;
        Address _addresData;
        ICartOperations _cartOperations;
        IForgotPassword _forgotPassword;
        ICartDetailsOperation _cartDetailsOperation;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        public UsersController(ProductDbContext userContext, IUserCreate userCreate, IMapper mapper, IWebHostEnvironment web, ILoginOperations loginOperations,IAddressOperations addressOperations,IForgotPassword forgotPassword,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration, ICheckOutOperation checkOutOperation, IProductOperations productOperations, ICartOperations cartOperations, ICartDetailsOperation cartDetailsOperation)
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
            _forgotPassword = forgotPassword;
            _checkOutOperation = checkOutOperation;
            _productOperations = productOperations;
            _cartOperations = cartOperations;
            _cartDetailsOperation = cartDetailsOperation;
        }
        [HttpPost("UserCreate")]
        public async Task<ResponseModel<UserViewModel>> post([FromBody] UserViewModel users)
        {
            _user = (UserRegistration)_mapper.Map<UserRegistration>(users);
            string myDateTime = DateTime.Now.ToString();
            _user.createdOn = DateTime.Now;
            _user.modifiedOn = DateTime.Now;
            _user.createdBy = users.FirstName + " " + users.LastName;
            _user.modifiedBy = users.FirstName + " " + users.LastName;
            _user.Password = _sec.Encrypt("admin", users.Password);

            //_user.Password = users.Password;
            _login.username = users.Email;
            _login.password = _sec.Encrypt("admin", users.Password);
            //_login.password = users.Password;

            _login.createdOn = DateTime.Now;
            _login.createdBy = users.FirstName + " " + users.LastName;
            _login.modifiedOn = DateTime.Now;
            _login.modifiedBy = users.FirstName + " " + users.LastName;
            _login.rolesId = 1;
            ResponseModel<UserViewModel> _userResponse = new ResponseModel<UserViewModel>();
            try
            {
                await _userCreate.AddUserRegistration(_user);
                await Task.Delay(1000);
                await _loginOperations.Add(_login);
                string message = _userMessages.Added + ",Response Message : " + new HttpResponseMessage(System.Net.HttpStatusCode.OK);
                _userResponse.AddResponse(true, 0, null, message);
                return _userResponse;
            }
            catch (Exception ex)
            {
                string message = _userMessages.ExceptionError + ",Respons Message : " + new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError);
                _userResponse.AddResponse(false, 0, null, message);
                _log.Error("log4net:Error in post controller", ex);
                return _userResponse;
            }


        }


        #region Get Method for users
        [Authorize(AuthenticationSchemes = "BasicAuthentication")]
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
                Login check = _loginOperations.Get().Result.Where(c=> c.username.Equals(data.username)&&c.password.Equals(data.password)&&c.rolesId.Equals((int)RoleTypes.User)).FirstOrDefault();
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
                _log.Error("log4net:Error in post controller", ex);
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
       
      /*[HttpPost]
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
        }*/

        #region delete method for address
        [HttpDelete("AddressDelete/{id}")]
        public ResponseModel<Address> AddressDelete(int id)
        {
            ResponseModel<Address> _response = new ResponseModel<Address>();
            try
            {
                List<Address> data = new List<Address>();
                data = _addressOperations.get().Result.ToList();
                _addresData = data.Where(c => c.id.Equals(id)).FirstOrDefault();
                _addressOperations.delete(_addresData);
                string message = ""+new HttpResponseMessage(System.Net.HttpStatusCode.OK);
                _response.AddResponse(true, 0, null, message);
                return _response;
            }
            catch (Exception ex)
            {
                string message = "" + new HttpResponseMessage(System.Net.HttpStatusCode.OK);
                _response.AddResponse(false, 0, null, message);
                _log.Error("log4net : error in the post controller", ex);
                return _response;
            }
        }
        #endregion

        #region Update Method for Users
        [HttpPut]
        public IActionResult Put([FromBody] UserRegistration user)
        {
            
            string myDateTime = DateTime.Now.ToString();
            user.createdOn = DateTime.Now;
            user.modifiedOn = DateTime.Now;
            user.createdBy = user.FirstName + " " + user.LastName;
            user.modifiedBy = user.FirstName + " " + user.LastName;
            user.Password = user.Password;
            _login = _loginOperations.Get().Result.Where(c => c.username.Equals(user.Email)).FirstOrDefault();
            _login.username = user.Email;
            _login.password = user.Password;
            _login.createdOn = DateTime.Now;
            _login.createdBy = user.FirstName + " " + user.LastName;
            _login.modifiedOn = DateTime.Now;
            _login.modifiedBy = user.FirstName + " " + user.LastName;
            _login.rolesId = 1;
            ResponseModel<string> _response = new ResponseModel<string>();
            try
            {
                _userCreate.Edit(user);
                _loginOperations.Edit(_login);
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
                checkoutData.address = _addressOperations.get().Result.Where(c=> c.id.Equals(checkoutData.addressId)).FirstOrDefault();
                checkoutData.product = _productOperations.GetAll().Result.Where(c => c.id.Equals(checkoutData.productId)).FirstOrDefault();
                checkoutData.user = _userCreate.Get().Result.Where(c=> c.UserId.Equals(checkoutData.userId)).FirstOrDefault();  
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

        #region Update Method for CheckOut
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

        #region Post Method for Cart

        [HttpPost("CreateCart")]
        public IActionResult CreateCart([FromBody] MyCart cart)
        {
            ResponseModel<string> _response = new ResponseModel<string>();
            try
            {
              
                _cartOperations.Add(cart);
                string message = " Response Message : " + new HttpResponseMessage(System.Net.HttpStatusCode.OK);
                _response.AddResponse(true, 0, null, message);
                var data = Newtonsoft.Json.JsonConvert.SerializeObject(_response);
                return new JsonResult(_response);
            }
            catch (Exception ex)
            {
                string message = " Response Message : " + new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError);
                _response.AddResponse(false, 0, null, message);
                _log.Error("log4net : error in the post controller", ex);
                return new JsonResult(_response);
            }

        }
        #endregion

        #region Get Method for cart
        [HttpGet("GetCart")]
        public async Task<ResponseModel<IEnumerable<MyCart>>> GetCart()
        {
            IEnumerable<MyCart> result = null;
            ResponseModel<IEnumerable<MyCart>> _response = new ResponseModel<IEnumerable<MyCart>>();
            try
            {
                 result = await _cartOperations.Get();
                if (result == null)
                {
                    string message = " " + new HttpResponseMessage(System.Net.HttpStatusCode.OK);
                    _response.AddResponse(true, 0, null, message);
                    /*                    var json = JsonConvert.SerializeObject(_response, Formatting.Indented,
                              new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.None });*/
                    /*                    return Content(json, "application/json");*/
                    return _response;
                }
                else
                {
                    string message = "" + new HttpResponseMessage(System.Net.HttpStatusCode.OK);
                    _response.AddResponse(true, 0, result, message);

                    return _response;
                }

            }
            catch (Exception ex)
            {
                string message = " " + ex.Message + " : " + new HttpResponseMessage(System.Net.HttpStatusCode.OK);
                _response.AddResponse(false, 0, null, message);
                _log.Error("log4net : error in the post controller", ex);
                return _response;
            }

        }
        #endregion


        #region Update Method for Cart
        [HttpPut("UpdateCart")]
        public IActionResult UpdateCart([FromBody] MyCart cart)
        {
            ResponseModel<ProductCart> _response = new ResponseModel<ProductCart>();
            try
            {
                _cartOperations.Edit(cart);
                string message = "Updated" + new HttpResponseMessage(System.Net.HttpStatusCode.OK);
                _response.AddResponse(true, 0, null, message);
                return new JsonResult(_response);
            }
            catch (Exception ex)
            {
                string message = "Error occured" + new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError);
                _response.AddResponse(false, 0, null, message);
                _log.Error("log4net : error in the post controller", ex);
                return new JsonResult(_response);
            }
        }
        #endregion

        #region Get Method for users orders
        [HttpGet("GetUserOrders/{id}")]
        public async Task<ResponseModel<IEnumerable<Checkout>>> GetUserOrders(int id)
        {
            ResponseModel<IEnumerable<Checkout>> _response = new ResponseModel<IEnumerable<Checkout>>();
            try
            {
                var result =  _checkOutOperation.get().Result.ToList().Where(c=> c.userId.Equals(id));
                if (result == null)
                {
                    string message = " " + new HttpResponseMessage(System.Net.HttpStatusCode.OK);
                    _response.AddResponse(true, 0, null, message);
                    /*                    var json = JsonConvert.SerializeObject(_response, Formatting.Indented,
                              new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.None });*/
                    /*                    return Content(json, "application/json");*/
                    return _response;
                }
                else
                {
                    string message = "" + new HttpResponseMessage(System.Net.HttpStatusCode.OK);
                    _response.AddResponse(true, 0, result, message);

                    return _response;
                }

            }
            catch (Exception ex)
            {
                string message = " " + ex.Message + " : " + new HttpResponseMessage(System.Net.HttpStatusCode.OK);
                _response.AddResponse(false, 0, null, message);
                _log.Error("log4net : error in the post controller", ex);
                return _response;
            }

        }
        #endregion

        [HttpGet]
        public IActionResult ForgotPassword(int userId)
        {
            var data = _forgotPassword.forgotPassword(userId);
            if (data != null)
            {
                return Ok();
            }
            return BadRequest();
        }
        [HttpDelete("DeleteCartDetails/{id}")]
        public ResponseModel<MyCart> DeleteCartDetails(int id)
        {
            try
            {
                ResponseModel<MyCart> _response = new ResponseModel<MyCart>();
                /*  _addressList = _addressOperations.Get().Result;*/
                var data = _cartDetailsOperation.Get().Result;
                _cartDetailsOperation.Delete(data.Where(c => c.id.Equals(id)).FirstOrDefault());
                _response.AddResponse(true, 0, null, "deleted");
                return _response;

            }
            catch (Exception ex)
            {
                ResponseModel<MyCart> _response = new ResponseModel<MyCart>();
                string message = _userMessages.ExceptionError + new HttpResponseMessage(System.Net.HttpStatusCode.OK) + ex.Message;
                _response.AddResponse(false, 0,null, message);
                _log.Error("log4net : error in the post controller", ex);
                return _response;
            }
        }

        [HttpDelete("DeleteCart/{id}")]
        public ResponseModel<MyCart> DeleteCart(int id)
        {
            try
            {
                ResponseModel<MyCart> _response = new ResponseModel<MyCart>();
                /*  _addressList = _addressOperations.Get().Result;*/
                var data = _cartOperations.Get().Result;
                _cartOperations.Delete(data.Where(c => c.id.Equals(id)).FirstOrDefault());
                _response.AddResponse(true, 0, null, "deleted");
                return _response;

            }
            catch (Exception ex)
            {
                ResponseModel<MyCart> _response = new ResponseModel<MyCart>();
                string message = _userMessages.ExceptionError + new HttpResponseMessage(System.Net.HttpStatusCode.OK) + ex.Message;
                _response.AddResponse(false, 0, null, message);
                _log.Error("log4net : error in the post controller", ex);
                return _response;
            }
        }
    }


}



