using DomainLayer.ProductModel;
using DomainLayer.ProductModel.Master;
using DomainLayer.Users;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UILayer.Models;

namespace UILayer.Data.ApiServices
{
    public class MasterApi
    {
        private IConfiguration Configuration { get; }
        public MasterApi(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        #region Add method for master data
        public bool Add(MasterTable masterData)
        {
            try
            {
                RequestHandler<MasterTable> request = new RequestHandler<MasterTable>(Configuration);
                request.url = "api/Master";
                var result = request.Post(masterData);
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
            catch(Exception ex)
            {
                return false;
            }
        }
        #endregion


        #region Get Method for Master Data
        public List<string> GetList(int id)
        {
            try
            {
                List<string> brandList = new List<string>();
                RequestHandler<IEnumerable<MasterTable>> _requestHandler = new RequestHandler<IEnumerable<MasterTable>>(Configuration);
                _requestHandler.url = "api/master";
                var result = _requestHandler.Get();
                if(result != null)
                {
                    var data = result.result.ToList().Where(s => s.parantId == id);
                    foreach (var item in data)
                    {
                        brandList.Add(item.masterData.ToString());
                    }
                }
                else
                {
                    brandList = null;
                }
                return brandList;
            }
            catch (Exception ex)
            {
                return null;
            }

           
        }
        #endregion

        #region Get Method for Master Data
        public IEnumerable<MasterTable> GetAll()
        {
            try
            {
                RequestHandler<IEnumerable<MasterTable>> _requestHandler = new RequestHandler<IEnumerable<MasterTable>>(Configuration);
                _requestHandler.url = "api/master";
                return _requestHandler.Get().result;
            }
           catch (Exception ex)
            {
                return null;
            }

           
        }
        #endregion


        public bool Delete(int id)
        {
            try
            {
                RequestHandler<string> requestHandler = new RequestHandler<string>(Configuration);
                requestHandler.url = "api/master/";
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
            catch(Exception ex)
            {
                return false;
            }
        }
        public bool Edit(MasterTable product)
        {
            try
            {
                RequestHandler<MasterTable> requestHandler = new RequestHandler<MasterTable>(Configuration);
                requestHandler.url = "api/master";
                var result = requestHandler.Edit(product);
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
            catch (Exception ex)
            {
                return false;
            }
        }

    }
}
