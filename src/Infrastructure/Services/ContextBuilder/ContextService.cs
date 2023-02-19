using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces.ContextInterfaces;
using Infrastructure.AppData.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.ContextBuilder
{
    public class ContextService : IContext
    {
        private readonly AppDbContext _db;

        public ContextService(AppDbContext db)
        {
            _db = db;
        }
        
        public  IQueryable<T> GetAll<T>() where T : BaseEntity =>
            _db.Set<T>();

        public async Task<T> FindAsync<T>(int id) where T : BaseEntity =>
            await _db.Set<T>().FirstOrDefaultAsync(p => p.Id == id);
        public async Task<T> FindAsync<T>(Expression< Func<T, bool>> expression) where T : BaseEntity  =>
            await _db.Set<T>().FirstOrDefaultAsync(expression);

        public async Task UpdateAsync<T>(T entity) where T : BaseEntity 
        {
            _db.Set<T>().Update(entity);
            await _db.SaveChangesAsync();
        }    
        public async Task AddAsync<T>(T entity) where T : BaseEntity 
        {
            await _db.Set<T>().AddAsync(entity);
            await _db.SaveChangesAsync();
        }     
        public async Task<bool> AnyAsync<T>(Expression< Func<T, bool>> expression) where T : BaseEntity  =>
            await _db.Set<T>().AnyAsync(expression); 
        
        public async Task RemoveAsync<T>(T entity) where T : BaseEntity 
        {
            _db.Set<T>().Remove(entity);
            await _db.SaveChangesAsync();
        }
    }
}