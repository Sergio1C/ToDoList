using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ToDo.Services.DataBase
{
    public interface IDbService : IDisposable
    {
        IQueryable<TEntity> GetAll<TEntity>() where TEntity : class;

        IQueryable<TEntity> GetAll<TEntity>(Expression<Func<TEntity, bool>> expression) where TEntity : class;

        Task<TEntity> CreateOrUpdateAsync<TEntity>(TEntity entity) where TEntity : class;

        Task<bool> DeleteAsync<TEntity>(TEntity entity) where TEntity : class;
    }
}
