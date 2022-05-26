
using BusinessObjectLayer;
using BusinessObjectLayer.ProductOperations;
using DTOLayer.Test;
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

            services.AddScoped(typeof(IAboutOperations), typeof(AboutOperations));
            services.AddScoped(typeof(IContactOperations), typeof(ContactOperations));
            services.AddScoped(typeof(ICheckOutOperation), typeof(CheckOutOperation));
            services.AddScoped(typeof(IAddressOperations), typeof(AddressOperations));
            services.AddScoped(typeof(IRamOperations), typeof(RamOperations));
            services.AddScoped(typeof(IStorageOperations), typeof(StorageOperations));
            services.AddScoped(typeof(IProductSubPartOperations), typeof(ProductSubPartOperations));
            services.AddScoped(typeof(ITokenManager), typeof(TokenManager));
            services.AddTransient(typeof(IMailService), typeof(MailService));

            services.AddScoped(typeof(ICartOperations), typeof(CartOperations));

            services.AddScoped(typeof(IForgotPassword), typeof(ForgotPassword));
            services.AddScoped(typeof(ICartDetailsOperation), typeof(CartDetailsOperation));


        }

    }
}
