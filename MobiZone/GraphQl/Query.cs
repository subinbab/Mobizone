using BusinessObjectLayer.ProductOperations;
using DomainLayer.ProductModel;
using HotChocolate;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiLayer.GraphQl
{
    
    public class Query
    {
        //IProductOperations _productOperations;
        public async Task<List<ProductEntity>> GetProducts([Service] IProductOperations _productOperations)
        {
            return  _productOperations.GetAll().Result.ToList();
        }
    }
}
