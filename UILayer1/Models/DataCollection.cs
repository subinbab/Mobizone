using DomainLayer.ProductModel.Master;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using UILayer.Data.ApiServices;

namespace UILayer.Models
{
    public class DataCollection
    {
        MasterApi _masterApi;
        IConfiguration _config;
        public DataCollection(IConfiguration configuration)
        {
            _config = configuration;
            _masterApi = new MasterApi(_config);
        }
        public List<string> FetchData(int id)
        {
            List<string> brandList = new List<string>();

            IEnumerable<MasterTable> data1 = _masterApi.GetAll().Where(s => s.parantId == id);
            foreach (var item in data1)
            {
                brandList.Add(item.masterData.ToString());
            }
            return brandList;
        } 
    }
}
