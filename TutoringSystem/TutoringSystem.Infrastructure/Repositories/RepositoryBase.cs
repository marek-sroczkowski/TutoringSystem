using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TutoringSystem.Domain.Repositories;
using TutoringSystem.Infrastructure.Data;

namespace TutoringSystem.Infrastructure.Repositories
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
	{
		protected AppDbContext DbContext { get; set; }

        protected RepositoryBase(AppDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public IQueryable<T> FindAll()
		{
			return DbContext.Set<T>()
				.AsNoTracking();
		}

		public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
		{
			return DbContext.Set<T>()
				.Where(expression)
				.AsNoTracking();
		}

		public void Create(T entity)
		{
			DbContext.Set<T>().Add(entity);
		}

		public void Update(T entity)
		{
			DbContext.Set<T>().Update(entity);
		}

		public void Delete(T entity)
		{
			DbContext.Set<T>().Remove(entity);
		}

		public async Task<bool> SaveChangedAsync()
        {
			var changesCount = await DbContext.SaveChangesAsync();

			return changesCount > 0;
        }
	}
}
