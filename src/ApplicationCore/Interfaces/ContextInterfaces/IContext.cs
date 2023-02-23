using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ApplicationCore.Entities;

namespace ApplicationCore.Interfaces.ContextInterfaces
{
    public interface IContext
    {
        public IQueryable<T> GetAll<T>() where T : BaseEntity;
        public Task<T> FindAsync<T>(int id) where T : BaseEntity;
        public Task<T> FindAsync<T>(Expression<Func<T, bool>> expression) where T : BaseEntity;
        public Task<bool> AnyAsync<T>(Expression<Func<T, bool>> expression) where T : BaseEntity;
        public Task UpdateAsync<T>(T entity) where T : BaseEntity;
        public Task AddAsync<T>(T entity) where T : BaseEntity;
        public Task RemoveAsync<T>(T entity) where T : BaseEntity;

    }
}