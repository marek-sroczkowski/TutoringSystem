using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TutoringSystem.Domain.Entities.Base;
using TutoringSystem.Domain.Repositories;
using TutoringSystem.Infrastructure.Data;
using TutoringSystem.Infrastructure.Extensions;

namespace TutoringSystem.Infrastructure.Repositories
{
    public abstract class RepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : Entity
	{
		protected AppDbContext DbContext { get; set; }

        protected RepositoryBase(AppDbContext dbContext)
        {
            DbContext = dbContext;
        }

		public IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> expression)
		{
			return expression == null
				? DbContext.Set<TEntity>().AsNoTracking()
				: DbContext.Set<TEntity>().AsNoTracking().Where(expression);
		}

		public void Create(TEntity entity)
		{
			DbContext.Set<TEntity>().Add(entity);
		}

		public void CreateRange(IEnumerable<TEntity> entities)
        {
			DbContext.Set<TEntity>().AddRange(entities);
		}

		public void Update(TEntity entity)
		{
			DbContext.DetachLocal(entity, entity.Id);
			DbContext.Set<TEntity>().Update(entity);
		}

		public void UpdateRange(IEnumerable<TEntity> entities)
		{
			DbContext.Set<TEntity>().UpdateRange(entities);
		}

		public void Delete(TEntity entity)
		{
			DbContext.DetachLocal(entity, entity.Id);
			DbContext.Set<TEntity>().Remove(entity);
		}

		public bool Contains(Expression<Func<TEntity, bool>> expression)
        {
			return expression == null
				? DbContext.Set<TEntity>().Any()
				: DbContext.Set<TEntity>().Any(expression);
        }

		public async Task<bool> SaveChangedAsync()
        {
			var changesCount = await DbContext.SaveChangesAsync();

			return changesCount > 0;
        }
	}
}
