﻿using DomainLayer.ProductModel;
using Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjectLayer.ProductOperations
{
    public class StorageOperations : IStorageOperations
    {
        IRepositoryOperations<Storage> _repo;
        Storage _ram;
        IEnumerable<Storage> _rams;
        public StorageOperations(IRepositoryOperations<Storage> repo)
        {
            _repo = repo;
        }
        public async Task Add(Storage entity)
        {
            await _repo.Add(entity);
            await _repo.Save();
        }

        public async Task DeleteProduct(Storage entity)
        {
            await _repo.Delete(entity);
            await _repo.Save();
        }

        public async Task Edit(Storage entity)
        {
            await _repo.Update(entity);
            await _repo.Save();
        }

        public async Task<Storage> Get(int id)
        {
            return await _repo.GetById(id);
        }

        public async Task<IEnumerable<Storage>> Get()
        {
            return await _repo.Get();
        }
    }
}
