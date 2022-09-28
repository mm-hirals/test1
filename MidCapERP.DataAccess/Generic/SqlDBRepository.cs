using MagnusMinds.Utility;
using Microsoft.EntityFrameworkCore;
using MidCapERP.DataEntities;
using System.Linq.Expressions;

namespace MidCapERP.DataAccess.Generic
{
    /// <summary>
    /// Represents the Entity Framework repository
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    public partial class SqlDBRepository<TEntity> : ISqlRepository<TEntity> where TEntity : class
    {
        #region Fields

        private readonly ApplicationDbContext _context;

        private DbSet<TEntity> _entities;

        #endregion Fields

        #region Ctor

        public SqlDBRepository(ApplicationDbContext context)
        {
            this._context = context;
        }

        #endregion Ctor

        #region Utilities

        /// <summary>
        /// Rollback of entity changes and return full error message
        /// </summary>
        /// <param name="exception">Exception</param>
        /// <returns>Error message</returns>
        protected string GetFullErrorTextAndRollbackEntityChanges(DbUpdateException exception)
        {
            //rollback entity changes
            if (_context is DbContext dbContext)
            {
                var entries = dbContext.ChangeTracker.Entries()
                    .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified).ToList();

                entries.ForEach(entry => entry.State = EntityState.Unchanged);
            }

            _context.SaveChanges();
            return exception.ToString();
        }

        #endregion Utilities

        #region Methods

        /// <summary>
        /// Get entity by identifier
        /// </summary>
        /// <param name="id">Identifier</param>
        /// <returns>Entity</returns>
        public virtual TEntity GetById(object id, CancellationToken cancellationToken)
        {
            return GetEntities().Find(id);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="includeProperties"></param>
        /// <returns></returns>
        public IQueryable<TEntity> Get(CancellationToken cancellationToken, Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "")
        {
            IQueryable<TEntity> query = GetEntities();

            if (!filter.CheckIsNull())
            {
                query = query.Where(filter);
            }

            if (!includeProperties.CheckIsNull())
            {
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            if (!orderBy.CheckIsNull())
            {
                return orderBy(query);
            }
            else
            {
                return query;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="query"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public IQueryable<TEntity> GetWithRawSql(string query, CancellationToken cancellationToken, params object[] parameters)
        {
            return GetEntities().FromSqlRaw(query, parameters);
        }

        /// <summary>
        /// Insert entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual TEntity Insert(TEntity entity, CancellationToken cancellationToken)
        {
            if (entity.CheckIsNull())
            {
                throw new ArgumentNullException(nameof(entity));
            }

            try
            {
                GetEntities().Add(entity);
                _context.SaveChanges();
                return entity;
            }
            catch (DbUpdateException exception)
            {
                //ensure that the detailed error text is saved in the Log
                throw new Exception(GetFullErrorTextAndRollbackEntityChanges(exception), exception);
            }
        }

        /// <summary>
        /// Insert entities
        /// </summary>
        /// <param name="entities">Entities</param>
        public virtual IEnumerable<TEntity> Insert(IEnumerable<TEntity> entities, CancellationToken cancellationToken)
        {
            if (entities.CheckIsNull())
            {
                throw new ArgumentNullException(nameof(entities));
            }

            try
            {
                GetEntities().AddRange(entities);
                _context.SaveChanges();
                return entities;
            }
            catch (DbUpdateException exception)
            {
                //ensure that the detailed error text is saved in the Log
                throw new Exception(GetFullErrorTextAndRollbackEntityChanges(exception), exception);
            }
        }

        /// <summary>
        /// Update entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual TEntity Update(TEntity entity, CancellationToken cancellationToken)
        {
            if (entity.CheckIsNull())
            {
                throw new ArgumentNullException(nameof(entity));
            }

            try
            {
                GetEntities().Update(entity);
                _context.SaveChanges();
                return entity;
            }
            catch (DbUpdateException exception)
            {
                //ensure that the detailed error text is saved in the Log
                throw new Exception(GetFullErrorTextAndRollbackEntityChanges(exception), exception);
            }
        }

        /// <summary>
        /// Update entities
        /// </summary>
        /// <param name="entities">Entities</param>
        public virtual IEnumerable<TEntity> Update(IEnumerable<TEntity> entities, CancellationToken cancellationToken)
        {
            if (entities.CheckIsNull())
            {
                throw new ArgumentNullException(nameof(entities));
            }

            try
            {
                GetEntities().UpdateRange(entities);
                _context.SaveChanges();
                return entities;
            }
            catch (DbUpdateException exception)
            {
                //ensure that the detailed error text is saved in the Log
                throw new Exception(GetFullErrorTextAndRollbackEntityChanges(exception), exception);
            }
        }

        /// <summary>
        /// Delete entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual TEntity Delete(TEntity entity, CancellationToken cancellationToken)
        {
            if (entity.CheckIsNull())
            {
                throw new ArgumentNullException(nameof(entity));
            }

            try
            {
                GetEntities().Remove(entity);
                _context.SaveChanges();
                return entity;
            }
            catch (DbUpdateException exception)
            {
                //ensure that the detailed error text is saved in the Log
                throw new Exception(GetFullErrorTextAndRollbackEntityChanges(exception), exception);
            }
        }

        /// <summary>
        /// Delete entities
        /// </summary>
        /// <param name="entities">Entities</param>
        public virtual IEnumerable<TEntity> Delete(IEnumerable<TEntity> entities, CancellationToken cancellationToken)
        {
            if (entities.CheckIsNull())
            {
                throw new ArgumentNullException(nameof(entities));
            }

            try
            {
                GetEntities().RemoveRange(entities);
                _context.SaveChanges();
                return entities;
            }
            catch (DbUpdateException exception)
            {
                //ensure that the detailed error text is saved in the Log
                throw new Exception(GetFullErrorTextAndRollbackEntityChanges(exception), exception);
            }
        }

        #endregion Methods

        #region Async Methods

        /// <summary>
        /// Get entity by identifier
        /// </summary>
        /// <param name="id">Identifier</param>
        /// <returns>Entity</returns>
        public virtual async Task<TEntity> GetByIdAsync(object id, CancellationToken cancellationToken)
        {
            return await GetEntities().FindAsync(new object?[] { id }, cancellationToken: cancellationToken);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="includeProperties"></param>
        /// <returns></returns>
        public async Task<IQueryable<TEntity>> GetAsync(CancellationToken cancellationToken, Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "")
        {
            try
            {
                IQueryable<TEntity> query = GetEntities();

                if (!filter.CheckIsNull())
                {
                    query = query.Where(filter);
                }

                if (!includeProperties.CheckIsNull())
                {
                    foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        query = query.Include(includeProperty);
                    }
                }

                if (!orderBy.CheckIsNull())
                {
                    return orderBy(query);
                }
                else
                {
                    return query;
                }
            }
            catch (Exception e)
            {

                throw;
            }
            
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="query"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<IQueryable<TEntity>> GetWithRawSqlAsync(string query, CancellationToken cancellationToken, params object[] parameters)
        {
            return GetEntities().FromSqlRaw(query, parameters);
        }

        /// <summary>
        /// Insert entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual async Task<TEntity> InsertAsync(TEntity entity, CancellationToken cancellationToken)
        {
            if (entity.CheckIsNull())
            {
                throw new ArgumentNullException(nameof(entity));
            }

            try
            {
                await GetEntities().AddAsync(entity, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                return entity;
            }
            catch (DbUpdateException exception)
            {
                //ensure that the detailed error text is saved in the Log
                throw new Exception(GetFullErrorTextAndRollbackEntityChanges(exception), exception);
            }
        }

        /// <summary>
        /// Insert entities
        /// </summary>
        /// <param name="entities">Entities</param>
        public virtual async Task<IEnumerable<TEntity>> InsertAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken)
        {
            if (entities.CheckIsNull())
            {
                throw new ArgumentNullException(nameof(entities));
            }

            try
            {
                await GetEntities().AddRangeAsync(entities, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                return entities;
            }
            catch (DbUpdateException exception)
            {
                //ensure that the detailed error text is saved in the Log
                throw new Exception(GetFullErrorTextAndRollbackEntityChanges(exception), exception);
            }
        }

        /// <summary>
        /// Update entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual async Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken)
        {
            if (entity.CheckIsNull())
            {
                throw new ArgumentNullException(nameof(entity));
            }

            try
            {
                GetEntities().Update(entity);
                await _context.SaveChangesAsync(cancellationToken);
                return entity;
            }
            catch (DbUpdateException exception)
            {
                //ensure that the detailed error text is saved in the Log
                throw new Exception(GetFullErrorTextAndRollbackEntityChanges(exception), exception);
            }
        }

        /// <summary>
        /// Update entities
        /// </summary>
        /// <param name="entities">Entities</param>
        public virtual async Task<IEnumerable<TEntity>> UpdateAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken)
        {
            if (entities.CheckIsNull())
            {
                throw new ArgumentNullException(nameof(entities));
            }

            try
            {
                GetEntities().UpdateRange(entities);
                await _context.SaveChangesAsync(cancellationToken);
                return entities;
            }
            catch (DbUpdateException exception)
            {
                //ensure that the detailed error text is saved in the Log
                throw new Exception(GetFullErrorTextAndRollbackEntityChanges(exception), exception);
            }
        }

        /// <summary>
        /// Delete entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual async Task<TEntity> DeleteAsync(TEntity entity, CancellationToken cancellationToken)
        {
            if (entity.CheckIsNull())
            {
                throw new ArgumentNullException(nameof(entity));
            }

            try
            {
                GetEntities().Remove(entity);
                await _context.SaveChangesAsync(cancellationToken);
                return entity;
            }
            catch (DbUpdateException exception)
            {
                //ensure that the detailed error text is saved in the Log
                throw new Exception(GetFullErrorTextAndRollbackEntityChanges(exception), exception);
            }
        }

        /// <summary>
        /// Delete entities
        /// </summary>
        /// <param name="entities">Entities</param>
        public virtual async Task<IEnumerable<TEntity>> DeleteAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken)
        {
            if (entities.CheckIsNull())
            {
                throw new ArgumentNullException(nameof(entities));
            }

            try
            {
                GetEntities().RemoveRange(entities);
                await _context.SaveChangesAsync(cancellationToken);
                return entities;
            }
            catch (DbUpdateException exception)
            {
                //ensure that the detailed error text is saved in the Log
                throw new Exception(GetFullErrorTextAndRollbackEntityChanges(exception), exception);
            }
        }

        #endregion Async Methods

        #region Properties

        /// <summary>
        /// Gets a table
        /// </summary>
        public virtual IQueryable<TEntity> Table => GetEntities();

        /// <summary>
        /// Gets a table with "no tracking" enabled (EF feature) Use it only when you load record(s) only for read-only operations
        /// </summary>
        public virtual IQueryable<TEntity> TableNoTracking => GetEntities().AsNoTracking();

        /// <summary>
        /// Gets an entity set
        /// </summary>
        protected virtual DbSet<TEntity> GetEntities()
        {
            if (_entities.CheckIsNull())
                if (_entities.CheckIsNull())
                {
                    _entities = _context.Set<TEntity>();
                }

            return _entities;
        }

        #endregion Properties
    }
}