using DomainLayer.ProductModel;
using DomainLayer.ProductModel.Master;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using UILayer.Models;

namespace UILayer.Data.ApiServices
{
    public class ImageApi
    {
        private IConfiguration Configuration { get; }
        public ImageApi(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IEnumerable<Images> Get()
        {
            try
            {
                RequestHandler<IEnumerable<Images>> _requestHandler = new RequestHandler<IEnumerable<Images>>(Configuration);
                _requestHandler.url = "api/productop";
                return _requestHandler.Get().result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public bool Delete(int id)
        {
            try
            {
                RequestHandler<string> requestHandler = new RequestHandler<string>(Configuration);
                requestHandler.url = "api/imagesoperations/";
                var result = requestHandler.Delete(id);
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
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
