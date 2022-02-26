using System;
using System.Linq;
using System.Linq.Expressions;

namespace TutoringSystem.Domain.Repositories
{
    public interface IRepositoryBase<TEntity>
	{
        bool Contains(Expression<Func<TEntity, bool>> expression);
        void Create(TEntity entity);
        void Delete(TEntity entity);
        IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> expression);
        void Update(TEntity entity);
        void UpdateRange(System.Collections.Generic.IEnumerable<TEntity> entities);
    }
}