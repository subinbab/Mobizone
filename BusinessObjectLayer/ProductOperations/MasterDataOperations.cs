using DomainLayer.ProductModel.Master;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        public void Add(MasterTable data)
        {
            _repo.Add(data);
            _repo.Save();
        }
        public IEnumerable<MasterTable> GetAll()
        {
            _masterDatas = _repo.Get().ToList();
            return _masterDatas;
        }
    }
}
