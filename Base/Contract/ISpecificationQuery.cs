using System.Linq.Expressions;

namespace BaseContract;

public interface ISpecificationQuery<TEntity>
{
    Expression<Func<TEntity, bool>> GetSpecificationExpression();
    bool IsSatisfied(TEntity obj);
}