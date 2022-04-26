
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
            services.AddScoped(typeof(IProductImageOperations),typeof(ProductImageOperations));
            services.AddScoped(typeof(IPrivacyOperation), typeof(PrivacyOperations));
<<<<<<< HEAD
            services.AddScoped(typeof(ICheckOutOperation), typeof(CheckOutOperation)); 
            services.AddScoped(typeof(IAddressOperations), typeof(AddressOperations));
        }
=======
            services.AddScoped(typeof(IAboutOperations), typeof(AboutOperations));
            services.AddScoped(typeof(IContactOperations), typeof(ContactOperations));
            services.AddScoped(typeof(ICheckOutOperation), typeof(CheckOutOperation));
    }
>>>>>>> d10c2c6dbdaff6377b74c5574236f9880451da2b
    }
}
