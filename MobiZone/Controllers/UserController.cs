using ApiLayer.Messages;
using ApiLayer.Models;
using AutoMapper;
using BusinessObjectLayer.User;
using DomainLayer.Users;
using DTOLayer.UserModel;
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
        ResponseModel<UserDataViewModel> _userResponse;
        IMapper _mapper;
        IEnumerable<UserRegistration> _userList;
        List<UserDataViewModel> _userDataList;
        public UserController(ProductDbContext userContext, IUserCreate userCreate, IMapper mapper)
        {
            _userContext = userContext;
            _userCreate = userCreate;
            _user = new UserRegistration();
            _userMessages = new UserMessages();
            _userResponse = new ResponseModel<UserDataViewModel>();
            _log = LogManager.GetLogger(typeof(UserController));
            _mapper = mapper;
            _userDataList = new List<UserDataViewModel>();
        }
        [HttpPost]

        public IActionResult Post([FromBody] UserViewModel users)
        {
            _user = (UserRegistration)_mapper.Map<UserRegistration>(users);
            string myDateTime = DateTime.Now.ToString();
            _user.createdOn = DateTime.Now;
            _user.modifiedOn = DateTime.Now;
            _user.createdBy = users.FirstName + " " + users.LastName;
            _user.modifiedBy = users.FirstName + " " + users.LastName;
            ResponseModel<string> _userResponse = new ResponseModel<string>();
            try
            {
                _userCreate.AddUserRegistration(_user);
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


        #region Get Method for Products
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                _userList = _userCreate.Get();
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
                string message = _userMessages.ExceptionError + new HttpResponseMessage(System.Net.HttpStatusCode.OK)+ex.Message;
                _response.AddResponse(false, 0, _userMessages.ExceptionError, message);
                _log.Error("log4net : error in the post controller", ex);
                return new JsonResult(_response);
            }

        }
        #endregion


    }
}
