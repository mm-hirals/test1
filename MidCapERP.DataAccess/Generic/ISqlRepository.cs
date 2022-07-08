using System.Linq.Expressions;

namespace MidCapERP.DataAccess.Generic
{
    /// <summary>
    /// Represents an entity repository
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    public partial interface ISqlRepository<TEntity>
    {
        #region Methods

        /// <summary>
        /// Get entity by identifier
        /// </summary>
        /// <param name="id">Identifier</param>
        /// <returns>Entity</returns>
        TEntity GetById(object id, CancellationToken cancellationToken);

        Task<TEntity> GetByIdAsync(object id, CancellationToken cancellationToken);

        /// <summary>
        /// Insert entity
        /// </summary>
        /// <param name="entity">Entity</param>
        TEntity Insert(TEntity entity, CancellationToken cancellationToken);

        Task<TEntity> InsertAsync(TEntity entity, CancellationToken cancellationToken);

        /// <summary>
        /// Insert entities
        /// </summary>
        /// <param name="entities">Entities</param>
        IEnumerable<TEntity> Insert(IEnumerable<TEntity> entities, CancellationToken cancellationToken);

        Task<IEnumerable<TEntity>> InsertAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken);

        /// <summary>
        /// Update entity
        /// </summary>
        /// <param name="entity">Entity</param>
        TEntity Update(TEntity entity, CancellationToken cancellationToken);

        Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken);

        /// <summary>
        /// Update entities
        /// </summary>
        /// <param name="entities">Entities</param>
        IEnumerable<TEntity> Update(IEnumerable<TEntity> entities, CancellationToken cancellationToken);

        Task<IEnumerable<TEntity>> UpdateAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken);

        /// <summary>
        /// Delete entity
        /// </summary>
        /// <param name="entity">Entity</param>
        TEntity Delete(TEntity entity, CancellationToken cancellationToken);

        Task<TEntity> DeleteAsync(TEntity entity, CancellationToken cancellationToken);

        /// <summary>
        /// Delete entities
        /// </summary>
        /// <param name="entities">Entities</param>
        IEnumerable<TEntity> Delete(IEnumerable<TEntity> entities, CancellationToken cancellationToken);

        Task<IEnumerable<TEntity>> DeleteAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken);

        /// <summary>
        /// Get WIth included table values
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="includeProperties"></param>
        /// <returns></returns>
        IQueryable<TEntity> Get(CancellationToken cancellationToken, Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "");

        Task<IQueryable<TEntity>> GetAsync(CancellationToken cancellationToken, Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "");

        /// <summary>
        /// Get data from raw SQL
        /// </summary>
        /// <param name="query">QUERY </param>
        /// <param name="parameters">Param to send</param>
        /// <returns></returns>
        IQueryable<TEntity> GetWithRawSql(string query, CancellationToken cancellationToken, params object[] parameters);

        Task<IQueryable<TEntity>> GetWithRawSqlAsync(string query, CancellationToken cancellationToken, params object[] parameters);

        #endregion Methods

        #region Properties

        /// <summary>
        /// Gets a table
        /// </summary>
        IQueryable<TEntity> Table { get; }

        /// <summary>
        /// Gets a table with "no tracking" enabled (EF feature) Use it only when you load record(s) only for read-only operations
        /// </summary>
        IQueryable<TEntity> TableNoTracking { get; }

        #endregion Properties
    }
}