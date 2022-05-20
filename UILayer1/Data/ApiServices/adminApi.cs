using AutoMapper;
using DomainLayer;
using DomainLayer.ProductModel;
using DomainLayer.Users;
using DTOLayer.Product;
using DTOLayer.UserModel;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UILayer.Models;

namespace UILayer.Data.ApiServices
{
    public class adminApi
    {
        Login _login;
        private readonly IMapper _mapper;
        private IConfiguration _configuration { get; }
        public adminApi(IConfiguration configuration, IMapper mapper)
        {
            _mapper = mapper;
            _configuration = configuration;
        }
        //Authenticate
        public Login Authenticate(LoginViewModel user)
        {
            try
            {
                _login = (Login)_mapper.Map<Login>(user);
                RequestHandler<Login> requestHandler = new RequestHandler<Login>(_configuration);
                requestHandler.url = "api/auth/admin";
                return requestHandler.Post(_login).result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public bool CreateAbout(About about)
        {
            try
            {
                RequestHandler<About> requestHandler = new RequestHandler<About>(_configuration);
                requestHandler.url = "api/Settings/AboutCreate";
                var result = requestHandler.Post(about);
                if (result != null)
                {
                    if (result.IsSuccess)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                return false;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public async Task<IEnumerable<About>> AboutGet()
        {
            try
            {
                RequestHandler<IEnumerable<About>> _requestHandler = new RequestHandler<IEnumerable<About>>(_configuration);
                _requestHandler.url = "api/Settings/AboutGet";
                return _requestHandler.Get().result;
            }
            catch(Exception ex)
            {
                return null;
            }

        }
        public bool EditAbout (About about)
        {
            RequestHandler<About> _requestHandler = new RequestHandler<About>(_configuration);
            try
            {
                _requestHandler.url = "api/Settings/AboutPut";
                var result = _requestHandler.Edit(about);
                if(result != null)
                {
                    if (result.IsSuccess)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool CreatePrivacy(PrivacyPolicy privacy)
        {
            try
            {
                RequestHandler<PrivacyPolicy> requestHandler = new RequestHandler<PrivacyPolicy>(_configuration);
                requestHandler.url = "api/Settings/PrivacyCreate";
                var result = requestHandler.Post(privacy);
                if(result != null)
                {
                    if (result.IsSuccess)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                return false;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public async Task<IEnumerable<PrivacyPolicy>> PrivacyGet()

        {
            try
            {
                RequestHandler<IEnumerable<PrivacyPolicy>> _requestHandler = new RequestHandler<IEnumerable<PrivacyPolicy>>(_configuration);
                _requestHandler.url = "api/Settings/PrivacyGet";
                return _requestHandler.Get().result;
            }
            catch(Exception ex)
            {
                return null;
            }

        }
        public bool EditPrivacy(PrivacyPolicy privacy)
        {
            RequestHandler<PrivacyPolicy> _requestHandler = new RequestHandler<PrivacyPolicy>(_configuration);
            try
            {
                _requestHandler.url = "api/Settings/PrivacyPut";
                var result = _requestHandler.Edit(privacy);
                if(result != null)
                {
                    if (result.IsSuccess)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool CreateContact(AdminContact contact)
        {
            try
            {
                RequestHandler<AdminContact> requestHandler = new RequestHandler<AdminContact>(_configuration);
                requestHandler.url = "api/Settings/ContactCreate";
                var result = requestHandler.Post(contact);
                if( result != null)
                {
                    if (result.IsSuccess)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                return false;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public async Task<IEnumerable<AdminContact>> ContactGet()

        {
            try
            {
                RequestHandler<IEnumerable<AdminContact>> _requestHandler = new RequestHandler<IEnumerable<AdminContact>>(_configuration);
                _requestHandler.url = "api/Settings/ContactGet";
                return _requestHandler.Get().result;
            }
            catch(Exception ex)
            {
                return null;
            }

        }
        public bool EditContact(AdminContact contact)
        {
            RequestHandler<AdminContact> _requestHandler = new RequestHandler<AdminContact>(_configuration);
            try
            {
                contact.country = "India";
                _requestHandler.url = "api/Settings/ContactPut";
                var result = _requestHandler.Edit(contact);
                if (result != null)
                {
                    if (result.IsSuccess)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            
        }
       
    }
}
