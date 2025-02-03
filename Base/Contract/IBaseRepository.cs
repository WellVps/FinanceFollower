using MongoDB.Driver;
using System.Linq.Expressions;

namespace BaseContract;

public interface IBaseRepository<T> : IReadOnlyBaseRepository<T> where T : IId
{
    Task<bool> Save(T record, CancellationToken cancellationToken = default);
    Task<T> SaveAndReturn(T record, CancellationToken cancellationToken = default);
    Task SaveMany(List<T> listObj, CancellationToken cancellationToken = default);
    Task<T> SaveOrReplaceOne(Expression<Func<T, bool>> filter, T obj, CancellationToken cancellationToken = default);
    Task<bool> ReplaceOne(Expression<Func<T, bool>> filter, T record, CancellationToken cancellationToken = default);

    Task<bool> Update(Expression<Func<T, bool>> filter, CancellationToken cancellationToken = default, 
        params (Expression<Func<T, object>>, object)[] updateDefinition);

    Task<bool> UpdateMany(Expression<Func<T, bool>> filter, CancellationToken cancellationToken = default, 
        params (Expression<Func<T, object>>, object)[] updateDefinition);

    Task<bool> DeleteOne(Expression<Func<T, bool>> filter, CancellationToken cancellationToken = default);
    Task<bool> DeleteMany(Expression<Func<T, bool>> filter, CancellationToken cancellationToken = default);
}