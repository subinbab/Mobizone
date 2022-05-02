using DomainLayer.ProductModel;
using Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjectLayer.ProductOperations
{
    public class RamOperations : IRamOperations
    {
        IRepositoryOperations<Ram> _repo;
        Ram _ram;
        IEnumerable<Ram> _rams;
        public RamOperations(IRepositoryOperations<Ram> repo)
        {
            _repo = repo;
        }
        public async Task Add(Ram entity)
        {
            await _repo.Add(entity);
            await _repo.Save();
        }

        public async Task DeleteProduct(Ram entity)
        {
            await _repo.Delete(entity);
            await _repo.Save();
        }

        public async Task Edit(Ram entity)
        {
            await _repo.Update(entity);
            await _repo.Save();
        }

        public async Task<Ram> Get(int id)
        {
            return await _repo.GetById(id);
        }

        public async Task<IEnumerable<Ram>> Get()
        {
            return await _repo.Get();
        }
    }
}
