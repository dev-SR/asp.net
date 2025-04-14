using HMS.Entities.Areas.ACC;
using HMS.Entities.Core;
using HMS.Entities.NotMapped;
using HMS.Repository.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace HMS.Repository.Core
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity
    {
        private readonly ApplicationDbContext dbContext;

        public Repository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public virtual async Task<QueryResult<TEntity>> GetAllAsync(
            bool includeRelated = false,
            IQueryObject queryObj = null,
            Dictionary<string, Expression<Func<TEntity, object>>> columnsMap = null)
        {
            var query = this.dbContext.Set<TEntity>().Where(x => !x.IsDeleted).AsQueryable();
            var result = new QueryResult<TEntity>();
            result.TotalItems = await query.CountAsync();
            query = query.ApplyOrdering(queryObj, columnsMap);
            query = query.ApplyPaging(queryObj);

            if (includeRelated)
            {
                foreach (var property in this.dbContext.Model.FindEntityType(typeof(TEntity)).GetNavigations())
                    query = query.Include(property.Name);
            }
            result.Items = await query.AsNoTracking().ToListAsync();

            return result;
        }

        public virtual async Task<QueryResult<TEntity>> GetAllAsync(
            Expression<Func<TEntity, bool>> predicate,
            bool includeRelated = false,
            IQueryObject queryObj = null,
            Dictionary<string, Expression<Func<TEntity, object>>> columnsMap = null)
        {
            var query = this.dbContext.Set<TEntity>().Where(x => !x.IsDeleted).AsQueryable();
            var result = new QueryResult<TEntity>();
            query = query.Where(predicate);
            result.TotalItems = await query.CountAsync();
            query = query.ApplyOrdering(queryObj, columnsMap);
            query = query.ApplyPaging(queryObj);

            if (includeRelated)
            {
                foreach (var property in this.dbContext.Model.FindEntityType(typeof(TEntity)).GetNavigations())
                    query = query.Include(property.Name);
            }
            result.Items = await query.AsNoTracking().ToListAsync();

            return result;
        }

        public virtual async Task<QueryResult<TEntity>> GetAllAsync(
            IQueryObject queryObj = null,
            Dictionary<string, Expression<Func<TEntity, object>>> columnsMap = null,
            params Expression<Func<TEntity, object>>[] properties)
        {
            var query = this.dbContext.Set<TEntity>().Where(x => !x.IsDeleted).AsQueryable();
            var result = new QueryResult<TEntity>();
            result.TotalItems = await query.CountAsync();
            query = query.ApplyOrdering(queryObj, columnsMap);
            query = query.ApplyPaging(queryObj);

            if (properties.Any())
                query = properties.Aggregate(query, (current, property) => current.Include(property));
            result.Items = await query.AsNoTracking().ToListAsync();

            return result;
        }

        public virtual async Task<QueryResult<TEntity>> GetAllAsync(
            Expression<Func<TEntity, bool>> predicate,
            IQueryObject queryObj = null,
            Dictionary<string, Expression<Func<TEntity, object>>> columnsMap = null,
            params Expression<Func<TEntity, object>>[] properties)
        {
            var query = this.dbContext.Set<TEntity>().Where(x => !x.IsDeleted).AsQueryable();
            var result = new QueryResult<TEntity>();
            query = query.Where(predicate);
            result.TotalItems = await query.CountAsync();
            query = query.ApplyOrdering(queryObj, columnsMap);
            query = query.ApplyPaging(queryObj);
            if (properties.Any())
                query = properties.Aggregate(query, (current, property) => current.Include(property));
            result.Items = await query.AsNoTracking().ToListAsync();
            return result;
        }

        public virtual async Task<long> GetCountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            var query = this.dbContext.Set<TEntity>().AsQueryable();

            return await query.Where(predicate).CountAsync();
        }

        public virtual async Task<TEntity> GetByIdAsync(Guid id, bool includeRelated = false)
        {
            var query = this.dbContext.Set<TEntity>().Where(s => s.Id == id).AsQueryable();

            if (includeRelated)
            {
                foreach (var property in this.dbContext.Model.FindEntityType(typeof(TEntity)).GetNavigations())
                    query = query.Include(property.Name);
            }
            return await query.AsNoTracking().FirstOrDefaultAsync();
        }

        public virtual async Task<TEntity> GetByIdAsync(Guid id, params Expression<Func<TEntity, object>>[] properties)
        {
            var query = this.dbContext.Set<TEntity>().Where(s => s.Id == id).AsQueryable();

            if (properties.Any())
                query = properties.Aggregate(query, (current, property) => current.Include(property));

            return await query.AsNoTracking().FirstOrDefaultAsync();
        }

        public virtual async Task AddAsync(TEntity entity)
        {
            await this.dbContext.Set<TEntity>().AddAsync(entity);
        }

        public virtual async Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
            await this.dbContext.Set<TEntity>().AddRangeAsync(entities);
        }
        public virtual async Task UpdateAsync(TEntity entity)
        {
            // this.dbContext.Entry(entity).State = EntityState.Modified;
            this.dbContext.Set<TEntity>().Update(entity);
        }
        public virtual async Task UpdateRangeAsync(IEnumerable<TEntity> entities)
        {
            this.dbContext.Set<TEntity>().UpdateRange(entities);
        }
        public virtual async Task ActiveInactiveAsync(Guid id)
        {
            var entity = await GetByIdAsync(id);
            await this.ActiveInactiveAsync(entity);
        }

        public virtual async Task ActiveInactiveAsync(TEntity entity)
        {
            entity.IsActive = !entity.IsActive;
            await this.UpdateAsync(entity);

        }

        public virtual async Task DeleteAsync(Guid id)
        {
            var entity = await GetByIdAsync(id);
            await this.DeleteAsync(entity);
        }

        public virtual async Task DeleteAsync(TEntity entity)
        {
            entity.IsDeleted = true;
            await this.UpdateAsync(entity);

        }

        public virtual async Task DeleteFromDBAsync(Guid id)
        {
            var entity = await this.dbContext.Set<TEntity>().FirstOrDefaultAsync(x => x.Id == id);
            await Task.Run(() =>
            {
                this.dbContext.Set<TEntity>().Remove(entity);
            });
        }

        public virtual async Task DeleteFromDBAsync(TEntity entity)
        {
            await Task.Run(() =>
            {
                this.dbContext.Set<TEntity>().Remove(entity);
            });

        }
        public virtual async Task DeleteRangeFromDBAsync(IEnumerable<TEntity> entities)
        {
            await Task.Run(() =>
            {
                this.dbContext.Set<TEntity>().RemoveRange(entities);
            });

        }
        public virtual async Task<bool> IsExistsAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await this.dbContext.Set<TEntity>().AnyAsync(predicate);
        }
        public virtual async Task<TEntity> GetSingleItemAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] properties)
        {
            var query = this.dbContext.Set<TEntity>().Where(predicate).AsQueryable();

            if (properties.Any())
                query = properties.Aggregate(query, (current, property) => current.Include(property));

            var entity = await query.FirstOrDefaultAsync();

            return entity;
        }
        public async Task<ChartOfAcc> GetAccGrpByCode(string Code)
        {
            return await this.dbContext.ChartOfAcces.FirstOrDefaultAsync(x => !x.IsDeleted && x.IsActive && x.AccNo == Code);
        }
    }
}
