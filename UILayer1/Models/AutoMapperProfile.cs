using AutoMapper;
using DomainLayer.ProductModel;
using DTOLayer.Product;
using System.Collections.Generic;

namespace UILayer.Models
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ProductViewModel, ProductEntity>();
            CreateMap<ProductEntity, ProductListViewModel>();
        }
    }
}
