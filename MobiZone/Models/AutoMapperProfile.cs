using AutoMapper;
using DomainLayer.ProductModel;
using DomainLayer.Users;
using DTOLayer.Product;
using DTOLayer.UserModel;
using System.Collections.Generic;

namespace UILayer.Models
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ProductViewModel, ProductEntity>();
            CreateMap<ProductEntity, ProductListViewModel>();
            CreateMap<UserViewModel, UserRegistration>();
        }
    }
}
