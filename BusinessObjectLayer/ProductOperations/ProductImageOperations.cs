using DomainLayer.ProductModel;
using Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessObjectLayer.ProductOperations
{
    public class ProductImageOperations : IProductImageOperations
    {
        IRepositoryOperations<Images> _repo;
        public ProductImageOperations(IRepositoryOperations<Images> repo)
        {
            _repo = repo;
        }
        public void delete(Images data)
        {
            data.IsActive=1;
            _repo.update(data);
            _repo.Save();
        }
        public Task<IEnumerable<Images>> get()
        {
            return _repo.Get();
        }
    }
}