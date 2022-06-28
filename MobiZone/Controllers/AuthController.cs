using ApiLayer.Messages;
using ApiLayer.Models;
using AutoMapper;
using BusinessObjectLayer;
using BusinessObjectLayer.User;
using DomainLayer;
using DomainLayer.Users;
using DTOLayer.Test;
using DTOLayer.UserModel;
using log4net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Repository;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
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
        ICartOperations _cartOperations;
        ITokenManager _tokenManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        JwtSecurityToken token;
        public AuthController(ProductDbContext userContext, IUserCreate userCreate, IMapper mapper, IWebHostEnvironment web, ILoginOperations loginOperations, UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration, ITokenManager tokenManager, ICartOperations cartOperations)
        {
            _cartOperations = cartOperations;
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
            _tokenManager = tokenManager;
        }
        [HttpPost]
        public async Task<IActionResult> post(Login data)
        {
            try
            {
                ResponseModel<Login> _response = new ResponseModel<Login>();
                string message;
                string password = _sec.Encrypt("admin", data.password);
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
        public async Task<ResponseModel<Login>> admin(Login data)
        {
            Login check = new Login();
            try
            {
                ResponseModel<Login> _response = new ResponseModel<Login>();
                string message;
                string password = _sec.Encrypt("admin", data.password);
                var list = await _loginOperations.Get();
                check = list.Where(c => c.username.Equals(data.username) && c.password.Equals(password)).FirstOrDefault();
                
                if (check != null)
                {
                    /*try
                    {
                        var cart = _cartOperations.Get().Result.Where(c => c.sessionId.Equals(check.sessionId)).FirstOrDefault();
                        //cart.sessionId = data.sessionId;
                        //_cartOperations.Edit(cart);
                    }
                    catch (Exception ex)
                    {

                    }*/



                    //UserRegistration check = _userCreate.Authenticate(data.userName, password);
                    check.sessionId = data.sessionId;
                    _loginOperations.Edit(check);
                    message = "" + new HttpResponseMessage(System.Net.HttpStatusCode.OK);
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
        /*private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
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
            await _userManager.AddToRoleAsync(user, UserRoles.User);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });

            return Ok(new Response { Status = "Success", Message = "User created successfully!" });
        }
        [HttpPost]
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
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                 token = GetToken(authClaims);

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                });
            }
            return Unauthorized();
        }
        [HttpPost("logout")]
        public async Task<IActionResult> CancelAccessToken()
        {
            SignOut();

            return NoContent();
        }*/
    }
            }
