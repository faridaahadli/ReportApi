using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Report.Data.Repositories;

public interface IBaseRepository<TEntity> where TEntity : class
{
    Task BeginTransaction();
    Task Commit();
    Task Rollback();

    DbSet<TEntity> Table { get; }
    Task DeleteAsync();
    Task<int> SaveChangesAsync();
    Task<TEntity?> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);
    Task<TEntity> GetFirstAsync();
    Task<TEntity> GetFirstAsync(Expression<Func<TEntity, bool>> predicate);
    Task<TEntity> GetFirstAsyncWithInclude(Expression<Func<TEntity, bool>> predicate, string include);
    Task<TEntity> GetByIdAsyncWithInclude(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties);
    IQueryable<TEntity> GetQuery();
    IQueryable<TEntity> GetQuery(Expression<Func<TEntity, bool>> predicate);
    Task<List<TEntity>> GetAllAsync();
    Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate);
    Task<int> AddRangeAsync(List<TEntity> entity, CancellationToken cancellationToken = default);
    Task<TEntity> AddAsync(TEntity entity);
    Task<TEntity> AddToDbSet(TEntity entity);
    Task UpdateAsync(TEntity entity);
    Task UpdateRangeAsync(List<TEntity> entity, CancellationToken cancellationToken = default); 
    Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);
   
    Task AddRangeToDbSetAsync(List<TEntity> entity);
}

