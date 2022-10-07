using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq.Expressions;
using ToDo.DataAccess;

namespace ToDo.Services.DataBase
{
    public class DbService : IDbService
    {
        private ToDoDbContext _dbContext;

        public DbService(DbContext dbContext)
        {
            _dbContext = (ToDoDbContext)dbContext;
        }

        public IQueryable<TEntity> GetAll<TEntity>() where TEntity : class
        {
            return _dbContext.Set<TEntity>().AsNoTrackingWithIdentityResolution();
        }

        public IQueryable<TEntity> GetAll<TEntity>(Expression<Func<TEntity, bool>> expression) where TEntity : class
        {
            return GetAll<TEntity>().Where(expression);
        }

        public async Task<TEntity> CreateOrUpdateAsync<TEntity>(TEntity entity) where TEntity : class
        {

           var entry = _dbContext.Update(entity);
            
           try
           {
                await _dbContext.SaveChangesAsync();
           }
           catch (Exception ex)
           {
                //todo log
                throw;
           }
            await _dbContext.Entry(entity).ReloadAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync<TEntity>(TEntity entity) where TEntity : class
        {

            var entry = _dbContext.Remove(entity);

            try
            {
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch(Exception ex)
            {
                //todo log
                throw;
            }            
        }

        public void Dispose()
        {
            if (_dbContext != null)
            {
                Debug.WriteLine("DbContext: " + _dbContext.ContextId + " dispose");
                _dbContext.Dispose();                
            }
        }
    }
}
