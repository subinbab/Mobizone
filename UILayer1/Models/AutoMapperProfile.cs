using AutoMapper;
using DomainLayer;
using DomainLayer.ProductModel;
using DomainLayer.Users;
using DTOLayer.Product;
using System.Collections.Generic;

namespace UILayer.Models
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ProductViewModel, ProductEntity>()
                .ForMember(d => d.status,
                op => op.MapFrom("status"));
            CreateMap<ProductEntity, ProductListViewModel>();
            CreateMap<ProductEntity, ProductViewModel>();
            CreateMap<ProductListViewModel, ProductViewModel>();
            CreateMap<LoginViewModel, Login>();
        }
    }
}
