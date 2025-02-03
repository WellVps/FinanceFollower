using System.Linq.Expressions;
using MongoDB.Driver;

namespace BaseContract;

public interface IReadOnlyBaseRepository<T> where T: IId
{
    IMongoCollection<TEntity> GetCollectionRead<TEntity>();
    IMongoCollection<T> GetCollectionRead();

    Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>>? filter = null, 
        int skip = 0, int limit = int.MaxValue, SortDefinition<T>? sort = null,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<T>> GetAll(FilterDefinition<T>? filter = null, 
        int skip = 0, int limit = int.MaxValue, SortDefinition<T>? sort = null,
        CancellationToken cancellationToken = default);

    Task<TDestination?> GetFirstProjectedOrDefault<TDestination>(
        Expression<Func<T, TDestination>> projection,
        Expression<Func<T, bool>>? filter = null,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<TDestination>> GetProjected<TDestination>(
        Expression<Func<T, TDestination>> projection,
        FilterDefinition<T>? filter = null,
        int skip = 0, int limit = int.MaxValue, SortDefinition<T>? sort = null,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<TDestination>> GetPaginated<TDestination>(
        Expression<Func<T, TDestination>> projection,
        FilterDefinition<T>? filter = null,
        int page = 1, int pageSize = int.MaxValue, SortDefinition<T>? sort = null,
        CancellationToken cancellationToken = default);

    Task<bool> HasRecord(Expression<Func<T, bool>> filter, CancellationToken cancellationToken = default);

    Task<bool> HasAnyRecord(CancellationToken cancellationToken = default);

    Task<T?> GetFirstOrDefault(CancellationToken cancellationToken = default);
    Task<T?> GetFirstOrDefault(Expression<Func<T, bool>> filter, CancellationToken cancellationToken = default);
    Task<T?> GetFirstOrDefault(FilterDefinition<T> filter, CancellationToken cancellationToken = default);

    Task<long> CountAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken = default);
    Task<long> CountAsync(FilterDefinition<T> filter, CancellationToken cancellationToken = default);
    Task<long> CountAsync(CancellationToken cancellationToken = default);
}