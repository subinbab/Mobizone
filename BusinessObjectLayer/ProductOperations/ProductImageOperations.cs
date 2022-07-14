using DomainLayer.ProductModel;
using Repository;
using System.Collections.Generic;
using System.Linq;
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
            _repo.Update(data);
            _repo.Save();
        }
        public async Task<IEnumerable<Images>> get()
        {
            var result = _repo.Get();
            return result.Result.ToList().Where(c => c.IsActive.Equals(0));
        }
    }
}