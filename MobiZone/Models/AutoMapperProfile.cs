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
            CreateMap<ProductEntity, ProductViewModel>();
            CreateMap<ProductEntity, ProductListViewModel>();
            CreateMap<UserViewModel, UserRegistration>();
            CreateMap<UserDataViewModel, UserRegistration>()
                .ForPath(prop => prop.FirstName, opt => opt.MapFrom(src => src.name))
                .ForPath(prop=> prop.createdOn,opt=> opt.MapFrom(src=>src.createdOn));
        }
    }
}
