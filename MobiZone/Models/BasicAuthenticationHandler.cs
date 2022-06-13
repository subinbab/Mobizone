using BusinessObjectLayer;
using BusinessObjectLayer.User;
using DomainLayer;
using DomainLayer.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace ApiLayer.Models
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        ILoginOperations _loginOperations;
        private readonly IUserCreate _userService;
        IEnumerable<UserRegistration> _userList;
        Security _security;
        IConfiguration _configuration;

        public BasicAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IUserCreate userService, ILoginOperations loginOperations,IConfiguration config)
            : base(options, logger, encoder, clock)
        {
            _userService = userService;
            _security = new Security();
       _loginOperations = loginOperations;
            _configuration = config;
    }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            string username;
            string password;
            string configUsername;
            string configPassword;
            string encryptUsername;
            string encryptPassword;
            Login userData = null;
            // skip authentication if endpoint has [AllowAnonymous] attribute
            var endpoint = Context.GetEndpoint();
            if (endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() != null)
                return AuthenticateResult.NoResult();

            if (!Request.Headers.ContainsKey("Authorization"))
                return AuthenticateResult.Fail("Missing Authorization Header");

            UserRegistration user = new UserRegistration();
            user = null;
            try
            {
                var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
                var credentialBytes = Convert.FromBase64String(authHeader.Parameter);
                var credentials = Encoding.UTF8.GetString(credentialBytes).Split(new[] { ':' }, 2);
                username = credentials[0];
                password = credentials[1];
                var decryptPass = _security.Encrypt("admin", password);
                _userList = _userService.Get().Result;
                /*user = _userList.Where(c=> c.Email.Equals(username)&& c.Password.Equals(decryptPass)).FirstOrDefault();*/
                configUsername = _configuration.GetSection("AuthenticationCredentials:username").Value;
                configPassword = _configuration.GetSection("AuthenticationCredentials:password").Value;
                userData = _loginOperations.Get().Result.Where(c=> c.username.Equals(username) && c.password.Equals(decryptPass)).FirstOrDefault();
            }
            catch(Exception ex)
            {
                return AuthenticateResult.Fail("Invalid Authorization Header");
            }

            if (username != configUsername && password != configPassword)
                return AuthenticateResult.Fail("Invalid Username or Password");

            var claims = new[] {
                new Claim(ClaimTypes.NameIdentifier, configUsername),
                new Claim(ClaimTypes.Name,configUsername),
            };
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return AuthenticateResult.Success(ticket);
        }
        
    }
}