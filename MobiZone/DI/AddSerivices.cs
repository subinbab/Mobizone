
using BusinessObjectLayer;
using BusinessObjectLayer.ProductOperations;
using Microsoft.Extensions.DependencyInjection;
using System;

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
            services.AddTransient(typeof(IProductOperations), typeof(ProductOperations));
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddScoped(typeof(ILoginOperations), typeof(LoginOperations));
        
    }
    }
}
