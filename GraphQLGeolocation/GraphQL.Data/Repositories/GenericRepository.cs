using System;
using System.Collections.Generic;
using System.Linq;
using GraphQL.Core.Classes;
using GraphQL.Core.Data;
using GraphQL.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace GraphQL.Data.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        protected readonly ApplicationContext _context;

        protected readonly DbSet<T> _entities;

        string ErrorMessage = string.Empty;

        public GenericRepository(ApplicationContext context)
        {
            _context = context;
            _entities = context.Set<T>();
        }
        
        public IEnumerable<T> GetAll()
        {
            return _entities.AsEnumerable();
        }

        public T GetById(int id)
        {
            return _entities.SingleOrDefault(e => e.Id == id);
        }

        public T Insert(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            _entities.Add(entity);
            _context.SaveChanges();
            return entity;
        }

        public T Update(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            _context.SaveChanges();
            return entity;
        }

        public void Delete(int id)
        {
            T entity = _entities.SingleOrDefault(e => e.Id == id);
            _entities.Remove(entity);
            _context.SaveChanges();
        }
    }
}