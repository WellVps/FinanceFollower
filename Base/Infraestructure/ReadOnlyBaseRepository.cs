using BaseContract;
using BaseDomain.Extensions;
using BaseInfraestructure.Extensions;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace BaseInfraestructure;

public abstract class ReadOnlyBaseRepository<TDocument>(IMongoContext context) 
    : IReadOnlyBaseRepository<TDocument>
    where TDocument: IId
{
    protected readonly IMongoContext BaseMongoContext = context;

    public IMongoCollection<TDocument> GetCollectionRead()
    {
        return BaseMongoContext.GetCollectionRead<TDocument>();
    }

    public IMongoCollection<TEntity> GetCollectionRead<TEntity>()
    {
        return BaseMongoContext.GetCollectionRead<TEntity>();
    }
    
    public async Task<IEnumerable<TDocument>> GetAll(
        Expression<Func<TDocument, bool>>? filter = null,
        int skip = 0,
        int limit = int.MaxValue,
        SortDefinition<TDocument>? sort = null,
        CancellationToken cancellationToken = default
    )
    {
        filter ??= x => true;

        var data = await GetCollectionRead()
            .FindAsync(
                filter,
                FindOptionsExtensions.MakeFindOptions(limit, skip, sort), cancellationToken);

        return data.ToEnumerable(cancellationToken);
    }
    
    public async Task<IEnumerable<TDocument>> GetAll(
        FilterDefinition<TDocument>? filter = null,
        int skip = 0,
        int limit = int.MaxValue,
        SortDefinition<TDocument>? sort = null,
        CancellationToken cancellationToken = default
    )
    {
        var data = await GetCollectionRead().FindAsync(
            filter,
            FindOptionsExtensions.MakeFindOptions(limit, skip, sort),
            cancellationToken
        );

        return data.ToEnumerable(cancellationToken);
    }
    
    public async Task<IEnumerable<TDocument>> GetAll(
        ISpecificationQuery<TDocument>? filter = null,
        int skip = 0,
        int limit = int.MaxValue,
        SortDefinition<TDocument>? sort = null,
        CancellationToken cancellationToken = default
    )
    {
        Expression<Func<TDocument, bool>> filterExpression = x => true;
        if(filter!=null)
            filterExpression = filter.GetSpecificationExpression();

        var data = await GetCollectionRead()
            .FindAsync(
                filterExpression,
                FindOptionsExtensions.MakeFindOptions(limit, skip, sort), cancellationToken);

        return data.ToEnumerable(cancellationToken);
    }

    public async Task<TDestination?> GetFirstProjectedOrDefault<TDestination>(
        Expression<Func<TDocument, TDestination>> projection,
        Expression<Func<TDocument, bool>>? filter = null,
        CancellationToken cancellationToken = default)
    {
        filter ??= x => true;

        var projectionBuilder = Builders<TDocument>.Projection;
        var projectionDefinition = projectionBuilder.Expression(projection);
        var cursor = await GetCollectionRead().FindAsync(filter,
            options: new FindOptions<TDocument, TDestination> { Projection = projectionDefinition },
            cancellationToken: cancellationToken);

        return await cursor.FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IEnumerable<TDestination>> GetProjected<TDestination>(
        Expression<Func<TDocument, TDestination>> projection,
        Expression<Func<TDocument, bool>>? filter = null,
        int skip = 0,
        int limit = int.MaxValue,
        SortDefinition<TDocument>? sort = null,
        CancellationToken cancellationToken = default
    )
    {
        return await GetCollectionRead()
            .GetProjected(
                projection,
                filter,
                skip,
                limit,
                sort,
                cancellationToken
            );
    }

    public async Task<IEnumerable<TDestination>> GetProjected<TDestination>(
        Expression<Func<TDocument, TDestination>> projection,
        FilterDefinition<TDocument>? filter = null,
        int skip = 0,
        int limit = int.MaxValue,
        SortDefinition<TDocument>? sort = null,
        CancellationToken cancellationToken = default
    )
    {
        return await GetCollectionRead()
            .GetProjected(
                projection,
                filter,
                skip,
                limit,
                sort,
                cancellationToken
            );
    }

    public async Task<IEnumerable<TDestination>> GetPaginated<TDestination>(
        Expression<Func<TDocument, TDestination>> projection,
        Expression<Func<TDocument, bool>>? filter = null,
        int page = 1,
        int pageSize = int.MaxValue,
        SortDefinition<TDocument>? sort = null,
        CancellationToken cancellationToken = default
    )
    {
        return await GetProjected(
            projection,
            filter,
            skip: (page - 1) * pageSize,
            limit: pageSize,
            sort,
            cancellationToken
        );
    }

    public async Task<IEnumerable<TDestination>> GetPaginated<TDestination>(
        Expression<Func<TDocument, TDestination>> projection,
        FilterDefinition<TDocument>? filter = null,
        int page = 1,
        int pageSize = int.MaxValue,
        SortDefinition<TDocument>? sort = null,
        CancellationToken cancellationToken = default
    )
    {
        return await GetProjected(
            projection,
            filter,
            skip: (page - 1) * pageSize,
            limit: pageSize,
            sort,
            cancellationToken
        );
    }

    public async Task<bool> HasRecord(Expression<Func<TDocument, bool>> filter,
        CancellationToken cancellationToken = default)
    {
        var cursor = await GetCollectionRead().FindAsync(filter, cancellationToken: cancellationToken);
        return await cursor.AnyAsync(cancellationToken: cancellationToken);
    }

    public async Task<bool> HasRecord(ISpecificationQuery<TDocument> filter,
        CancellationToken cancellationToken = default)
    {
        var cursor = await GetCollectionRead()
            .FindAsync(filter.GetSpecificationExpression(), cancellationToken: cancellationToken);
        return await cursor.AnyAsync(cancellationToken: cancellationToken);
    }

    public async Task<bool> HasRecord(FilterDefinition<TDocument> filter,
        CancellationToken cancellationToken = default)
    {
        var cursor = await GetCollectionRead().FindAsync(filter, cancellationToken: cancellationToken);
        return await cursor.AnyAsync(cancellationToken: cancellationToken);
    }

    public async Task<bool> HasAnyRecord(CancellationToken cancellationToken = default)
    {
        var cursor = await GetCollectionRead().FindAsync(x => true, cancellationToken: cancellationToken);
        return await cursor.AnyAsync(cancellationToken: cancellationToken);
    }

    public async Task<TDocument?> GetFirstOrDefault(CancellationToken cancellationToken = default)
    {
        var cursor = await GetCollectionRead().FindAsync(_ => true, cancellationToken: cancellationToken);
        return await cursor.FirstOrDefaultAsync(cancellationToken: cancellationToken);
    }

    public async Task<TDocument?> GetFirstOrDefault(Expression<Func<TDocument, bool>> filter,
        CancellationToken cancellationToken = default)
    {
        var document = await GetCollectionRead()
            .Aggregate()
            .Match(filter)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);

        return document;
    }

    public async Task<TDocument?> GetFirstOrDefault(ISpecificationQuery<TDocument> filter,
        CancellationToken cancellationToken = default)
    {
        var document = await GetCollectionRead()
            .Aggregate()
            .Match(filter.GetSpecificationExpression())
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);

        return document;
    }

    public async Task<TDocument?> GetFirstOrDefault(FilterDefinition<TDocument> filter,
        CancellationToken cancellationToken = default)
    {
        var record = await GetCollectionRead()
            .Aggregate()
            .Match(filter)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);

        return record;
    }

    public async Task<long> CountAsync(Expression<Func<TDocument, bool>> filter,
        CancellationToken cancellationToken = default)
    {
        var count = await GetCollectionRead()
            .CountDocumentsAsync(filter, cancellationToken: cancellationToken);

        return count;
    }

    public async Task<long> CountAsync(ISpecificationQuery<TDocument> filter,
        CancellationToken cancellationToken = default)
    {
        var count = await GetCollectionRead()
            .CountDocumentsAsync(filter.GetSpecificationExpression(), cancellationToken: cancellationToken);

        return count;
    }

    public async Task<long> CountAsync(FilterDefinition<TDocument> filter,
        CancellationToken cancellationToken = default)
    {
        var count = await GetCollectionRead()
            .CountDocumentsAsync(filter, cancellationToken: cancellationToken);

        return count;
    }

    public async Task<long> CountAsync(CancellationToken cancellationToken = default)
    {
        var count = await GetCollectionRead()
            .CountDocumentsAsync(x => true, cancellationToken: cancellationToken);

        return count;
    }
}