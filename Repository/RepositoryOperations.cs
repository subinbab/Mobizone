using DomainLayer;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class RepositoryOperations<T> : IRepositoryOperations<T>where T:class
    {
        ProductDbContext _context;
        readonly DbSet<T> dbSet;
        IEnumerable<T> entities;
        IQueryable<T> query;
        T entity;
       
        public RepositoryOperations(ProductDbContext product)
        {
            _context = product;
            dbSet = _context.Set<T>();
           
        }
        public async Task Add(T entity)
        {
            try
            {
                dbSet.AddAsync(entity);
            }
            catch(SqlException ex)
            {

            }
            
        }

        public async Task Delete(T entity)
        {
            try
            {
                dbSet.Remove(entity);
            }
            catch (SqlException ex)
            {

            }
            
        }

        public async Task<IEnumerable<T>> Get()
        {
            try
            {
                entities = await dbSet.ToListAsync();
            }
            catch(SqlException ex)
            {

            }
            return entities;
        }
        public async Task<IQueryable<T>> Get(params Expression<Func<T, object>>[] includes)
        {
            try
            {
                IQueryable<T> result = dbSet;
                query = includes.Aggregate(result, (current, includeProperty) => current.Include(includeProperty));
            }
            catch (SqlException ex)
            {

            }
            return query;
        }

        public async Task<T> GetById(int Id)
        {
            try
            {
                entity = dbSet.Find(Id);
            }
            catch(SqlException ex)
            {

            }

            return entity;
        }
        public async Task<T> GetById(int id, params Expression<Func<T, object>>[] includes)
        {
            try
            {
                IQueryable<T> result = dbSet;
               /* query = includes.Aggregate(result, (current, includeProperty) => current.Include(includeProperty));*/
                foreach (var includeProperty in includes)
                {
                    dbSet.Include(includeProperty);
                }
                entity = dbSet.Find();
                return entity;
            }
            catch (SqlException ex)
            {
                return null;
            }

        }

        public async Task Save()
        {
            try
            {
                _context.SaveChanges();
            }
            catch(SqlException ex)
            {

            }
            
        }

        public async Task Update(T entity)
        {
            try
            {
                dbSet.Update(entity);
            }
            catch(SqlException ex)
            {

            }
            
        }
    }
}
