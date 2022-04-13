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
            _repo.Add(data);
            _repo.Save();
        }
        public async Task<IEnumerable<MasterTable>> GetAll()
        {
            return await _repo.Get();
        }
    }
}
