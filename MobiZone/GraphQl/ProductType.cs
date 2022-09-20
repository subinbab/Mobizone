using DomainLayer.ProductModel;
using HotChocolate.Types;

namespace ApiLayer.GraphQl
{
    public class ProductType : ObjectType<ProductEntity>
    {
        protected override void Configure(IObjectTypeDescriptor<ProductEntity> descriptor)
        {
            descriptor.Field(a => a.id).Type<IdType>();
            descriptor.Field(a => a.name).Type<StringType>();
            descriptor.Field(a => a.description).Type<StringType>();
            descriptor.Field(a=> a.productType).Type<StringType>();
            descriptor.Field(a=>a.colors).Type<StringType>();
            descriptor.Field(a=>a.price).Type<StringType>();
            descriptor.Field(a=>a.productBrand).Type<StringType>();
            descriptor.Field(a=>a.purchasedNumber).Type<StringType>();
        }
    }
}
