using DomainLayer;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

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
        public void Add(T entity)
        {
            try
            {
                dbSet.Add(entity);
            }
            catch(SqlException ex)
            {

            }
            
        }

        public void Delete(T entity)
        {
            try
            {
                dbSet.Remove(entity);
            }
            catch (SqlException ex)
            {

            }
            
        }

        public IEnumerable<T> Get()
        {
            try
            {
                entities = dbSet.ToList();
            }
            catch(SqlException ex)
            {

            }
            return entities;
        }
        public IQueryable<T> Get(params Expression<Func<T, object>>[] includes)
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

        public T GetById(int Id)
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

        public void Save()
        {
            try
            {
                _context.SaveChanges();
            }
            catch(SqlException ex)
            {

            }
            
        }

        public void Update(T entity)
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
