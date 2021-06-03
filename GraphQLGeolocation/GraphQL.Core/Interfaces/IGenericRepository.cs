using System.Collections.Generic;
using GraphQL.Core.Classes;

namespace GraphQL.Core.Data
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        IEnumerable<T> GetAll();

        T GetById(int id);
        
        T Insert(T entity);
        
        T Update(T entity);
        
        void Delete(int id);
    }
}