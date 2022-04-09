
using BusinessObjectLayer.ProductOperations;
using Microsoft.Extensions.DependencyInjection;

namespace ApiLayer.DI
{
    public class AddSerivices
    {
        
        public AddSerivices()
        {
            
        }
        public void Initialize(IServiceCollection services)
        {
            services.AddTransient(typeof(IMasterDataOperations), typeof(MasterDataOperations));
            services.AddTransient(typeof(IProductOperations),typeof(ProductOperations));
        }
    }
}
