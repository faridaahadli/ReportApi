using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Report.Core.Entities;
using Report.Core.Entities;
using Report.Data.Persistance;
using Report.Core.Exceptions;

namespace Report.Data.Repositories;

public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : BaseEntity
{ 
    protected readonly ReportContext Context;
    protected readonly DbSet<TEntity> DbSet;
    private DatabaseFacade Database => Context.Database;
    private IDbContextTransaction? Transaction => Database.CurrentTransaction;

    public DbSet<TEntity> Table => DbSet;

    public BaseRepository(ReportContext context)
    {
        Context = context ?? throw new ArgumentNullException(nameof(context));
        DbSet = context.Set<TEntity>();
    }

    public async Task BeginTransaction()
    {
        if (Transaction is null) await Database.BeginTransactionAsync();
    }

    public async Task Commit()
    {
        if (Transaction is not null) await Database.CommitTransactionAsync();
    }

    public async Task Rollback()
    {
        if (Transaction is not null) await Database.RollbackTransactionAsync();
    }

    public async Task<int> SaveChangesAsync() => await Context.SaveChangesAsync();

    public async Task DeleteAsync()
    {
        var tableName = Context.Model.FindEntityType(typeof(TEntity)).GetTableName();

        var sql = $@"
        DELETE FROM ""{tableName}"" 
        WHERE DATE_TRUNC('month', ""PackageCreateDate"") >= DATE_TRUNC('month', CURRENT_DATE) - INTERVAL '6 months';
        
        DO $$ 
        DECLARE 
            max_id bigint;
        BEGIN
            SELECT COALESCE(MAX(""Id""), 0) + 1 INTO max_id FROM ""{tableName}"";
            EXECUTE 'ALTER SEQUENCE ""{tableName}_Id_seq"" RESTART WITH ' || max_id;
        END $$;";

        await Context.Database.ExecuteSqlRawAsync(sql);
    }
    public IQueryable<TEntity> GetQuery()
    {
        return DbSet.AsQueryable();
    }

    public IQueryable<TEntity> GetQuery(Expression<Func<TEntity, bool>> predicate)
    {
        return DbSet.Where(predicate).AsQueryable();
    }

    public IQueryable<TEntity> GetQueryWithInclude(string include)
    {
        return DbSet.Include(include).AsQueryable();
    }

    public Task<List<TEntity>> GetAllAsync()
    {
        return DbSet.ToListAsync();
    }

    public Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return DbSet.Where(predicate).ToListAsync();
    }

    public async Task<TEntity> GetFirstAsync()
    {
        var entity = await DbSet.FirstOrDefaultAsync() 
            ?? throw new ResourceNotFoundException(typeof(TEntity));
        ;

        return entity;
    }
    public async Task<TEntity> GetFirstAsync(Expression<Func<TEntity, bool>> predicate)
    {
        var entity = await DbSet.Where(predicate).FirstOrDefaultAsync() 
            ?? throw new ResourceNotFoundException(typeof(TEntity));
        return entity;
    }

    public Task<TEntity?> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return DbSet.Where(predicate).FirstOrDefaultAsync();
    }

    public Task<TEntity> GetFirstAsyncWithInclude(Expression<Func<TEntity, bool>> predicate, string include)
    {
        var entity = DbSet.Where(predicate).Include(include).FirstOrDefaultAsync()
            ?? throw new ResourceNotFoundException(typeof(TEntity));
        return entity;
    }
    public async Task<TEntity> GetByIdAsyncWithInclude(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties)
    {

        var entity = DbSet.Where(predicate);

        foreach (var includeProperty in includeProperties)
        {
            entity = entity.Include(includeProperty);
        }

        if (entity == null) throw new ResourceNotFoundException(typeof(TEntity));

        return await entity.FirstOrDefaultAsync();
    }


    public async Task<int> AddRangeAsync(List<TEntity> entity, CancellationToken cancellationToken = default)
    {
        await AddRangeToDbSetAsync(entity);
        var count = await Context.SaveChangesAsync(cancellationToken);
        return count;
    }

    public async Task AddRangeToDbSetAsync(List<TEntity> entity)
    {
        await DbSet.AddRangeAsync(entity);
    }

    public async Task<TEntity> AddAsync(TEntity entity)
    {
        try
        {
            var addedEntity = await AddToDbSet(entity);
            await Context.SaveChangesAsync();
            return addedEntity;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

    }

    public async Task<TEntity> AddToDbSet(TEntity entity)
    {
        await DbSet.AddAsync(entity);
        return entity;
    }



    public async Task UpdateAsync(TEntity entity)
    {
        DbSet.Update(entity);
        await Context.SaveChangesAsync();
    }
  
    public async Task UpdateRangeAsync(List<TEntity> entities, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entities);

        if (entities.Count == 0)
            return;

        Context.UpdateRange(entities);
        await Context.SaveChangesAsync(cancellationToken);
    }

   
   
    public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await DbSet.AnyAsync(predicate);
    }
}
