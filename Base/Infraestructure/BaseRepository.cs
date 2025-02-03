using System.Linq.Expressions;
using BaseContract;
using MongoDB.Driver;

namespace BaseInfraestructure;

public abstract class BaseRepository<TDocument>(IMongoContext context) : ReadOnlyBaseRepository<TDocument> (context) where TDocument: IId
{
    public IMongoCollection<TDocument> GetCollectionWrite()
    {
        return BaseMongoContext.GetCollectionWrite<TDocument>();
    }

    public IMongoCollection<TEntity> GetCollectionWrite<TEntity>()
    {
        return BaseMongoContext.GetCollectionWrite<TEntity>();
    }

    public virtual async Task<bool> Save(TDocument obj, CancellationToken cancellationToken = default)
    {
        await GetCollectionWrite().InsertOneAsync(obj, cancellationToken: cancellationToken);
        return true;
    }

    public virtual async Task<TDocument> SaveAndReturn(TDocument obj, CancellationToken cancellationToken = default)
    {
        await GetCollectionWrite().InsertOneAsync(obj, cancellationToken: cancellationToken);
        return obj;
    }

    public virtual async Task SaveMany(List<TDocument> listObj, CancellationToken cancellationToken = default)
    {
        await GetCollectionWrite().InsertManyAsync(listObj, cancellationToken: cancellationToken);
    }

    public virtual async Task<TDocument> SaveOrReplaceOne(Expression<Func<TDocument, bool>> filter, TDocument obj, CancellationToken cancellationToken = default)
    {
        var replaced = await ReplaceOne(filter, obj, cancellationToken);
        if (replaced)
            return obj;

        return await SaveAndReturn(obj, cancellationToken);
    }

    public virtual async Task<bool> DeleteOne(Expression<Func<TDocument, bool>> filter, CancellationToken cancellationToken = default)
    {
        var deletedCount = (await GetCollectionWrite().DeleteOneAsync(filter, cancellationToken: cancellationToken)).DeletedCount;
        return deletedCount > 0;
    }

    public virtual async Task<bool> DeleteOne(FilterDefinition<TDocument> filter, CancellationToken cancellationToken = default)
    {
        var deletedCount = (await GetCollectionWrite().DeleteOneAsync(filter, cancellationToken: cancellationToken)).DeletedCount;
        return deletedCount > 0;
    }

    public virtual async Task<bool> DeleteMany(Expression<Func<TDocument, bool>> filter, CancellationToken cancellationToken = default)
    {
        var deletedCount = await GetCollectionWrite().DeleteManyAsync(filter, cancellationToken: cancellationToken);
        return deletedCount?.DeletedCount > 0;
    }

    public virtual async Task<bool> DeleteMany(FilterDefinition<TDocument> filter, CancellationToken cancellationToken = default)
    {
        var deletedCount = await GetCollectionWrite().DeleteManyAsync(filter, cancellationToken: cancellationToken);
        return deletedCount?.DeletedCount > 0;
    }

    public virtual async Task<bool> ReplaceOne(Expression<Func<TDocument, bool>> filter, TDocument record, CancellationToken cancellationToken = default)
    {
        var result = await GetCollectionWrite().ReplaceOneAsync(filter, record, cancellationToken: cancellationToken);
        return result.IsAcknowledged && result.ModifiedCount > 0;
    }

    public virtual async Task<bool> Update(
        Expression<Func<TDocument, bool>> filter, 
        CancellationToken cancellationToken = default, 
        params (Expression<Func<TDocument, object>>, object)[] updateDefinitions)
    {
        var update = Builders<TDocument>.Update;
        var definitions = updateDefinitions.Aggregate(
            (UpdateDefinition<TDocument>?)null,
            (current, ud) => current?.Set(ud.Item1, ud.Item2) ?? update.Set(ud.Item1, ud.Item2)
        );

        var updateResult = await GetCollectionWrite().UpdateOneAsync(filter, definitions, cancellationToken: cancellationToken);

        return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
    }

    public virtual async Task<bool> Update(
        ISpecificationQuery<TDocument> filter,
        CancellationToken cancellationToken = default,
        params (Expression<Func<TDocument, object>>, object)[] updateDefinitions
    )
    {
        return await Update(filter.GetSpecificationExpression(), cancellationToken, updateDefinitions);
    }

    public virtual async Task<bool> UpdateMany(
        Expression<Func<TDocument, bool>> filter,
        CancellationToken cancellationToken = default,
        params (Expression<Func<TDocument, object>>, object)[] updateDefinitions
    )
    {
        var update = Builders<TDocument>.Update;

        var definitions = updateDefinitions.Aggregate(
            (UpdateDefinition<TDocument>?)null,
            (current, ud) => current?.Set(ud.Item1, ud.Item2) ?? update.Set(ud.Item1, ud.Item2)
        );

        var updateResult = await GetCollectionWrite()
            .UpdateManyAsync(filter, definitions, cancellationToken: cancellationToken);

        return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
    }

    public virtual async Task<bool> UpdateMany(
        ISpecificationQuery<TDocument> filter, CancellationToken cancellationToken = default,
        params (Expression<Func<TDocument, object>>, object)[] updateDefinitions
    )
    {
        return await UpdateMany(filter.GetSpecificationExpression(), cancellationToken, updateDefinitions);
    }
}