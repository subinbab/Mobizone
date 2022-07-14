using DomainLayer.ProductModel;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UILayer.Data.ApiServices;

namespace UILayer.Data
{
    public class CacheData
    {
        private readonly IDistributedCache _distributedCache;
        public IEnumerable<ProductEntity> _products { get; set; }
        IProductOpApi _productOpApi;
        public CacheData(IDistributedCache distributedCache,IProductOpApi productOpApi)
        {
            _distributedCache = distributedCache;
            _productOpApi = productOpApi;
            SetProducts();
        }

        private async Task SetProducts()
        {
            _products = await _productOpApi.GetAll();
            _distributedCache.SetString("Products", JsonConvert.SerializeObject(_products));
        }
    }
}
