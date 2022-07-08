using DomainLayer.ProductModel.Master;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjectLayer.ProductOperations
{
    public class MasterDataOperations : IMasterDataOperations
    {
        IRepositoryOperations<MasterTable> _repo;
        List<MasterTable> _masterDatas;
        public MasterDataOperations(IRepositoryOperations<MasterTable> repo)
        {
            _repo = repo;
        }
        public async Task Add(MasterTable data)
        {
            data.IsActive = 0;
            _repo.Add(data);
            _repo.Save();
        }

        public async Task Delete(MasterTable entity)
        {
            entity.IsActive = 1;
            await _repo.Update(entity);
            await _repo.Save();
        }

        public async Task Edit(MasterTable entity)
        {
            await _repo.Update(entity);
            await _repo.Save();
        }

        public async Task<IEnumerable<MasterTable>> GetAll()
        {
            var result = await _repo.Get();
            return result.Where(c=> c.IsActive.Equals(0));
        }
    }
}
