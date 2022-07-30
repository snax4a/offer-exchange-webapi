using System.Data;

namespace FSH.WebApi.Application.Common.Persistence;

public interface IDapperRepository : ITransientService
{
    /// <summary>
    /// Get an <see cref="IReadOnlyList{T}"/> using raw sql string with parameters.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <param name="sql">The sql string.</param>
    /// <param name="param">The paramters in the sql string.</param>
    /// <param name="transaction">The transaction to be performed.</param>
    /// <returns>Returns <see cref="Task"/> of <see cref="IReadOnlyCollection{T}"/>.</returns>
    Task<IEnumerable<T>> QueryAsync<T>(string sql, object? param = null, IDbTransaction? transaction = null)
    where T : class;

    /// <summary>
    /// Get a <typeparamref name="T"/> using raw sql string with parameters.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <param name="sql">The sql string.</param>
    /// <param name="param">The paramters in the sql string.</param>
    /// <param name="transaction">The transaction to be performed.</param>
    /// <returns>Returns <see cref="Task"/> of <typeparamref name="T"/>.</returns>
    Task<T?> QueryFirstOrDefaultAsync<T>(string sql, object? param = null, IDbTransaction? transaction = null)
    where T : class, IEntity;

    /// <summary>
    /// Get a <typeparamref name="T"/> using raw sql string with parameters.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <param name="sql">The sql string.</param>
    /// <param name="param">The paramters in the sql string.</param>
    /// <param name="transaction">The transaction to be performed.</param>
    /// <returns>Returns <see cref="Task"/> of <typeparamref name="T"/>.</returns>
    Task<T> QuerySingleAsync<T>(string sql, object? param = null, IDbTransaction? transaction = null)
    where T : class, IEntity;
}