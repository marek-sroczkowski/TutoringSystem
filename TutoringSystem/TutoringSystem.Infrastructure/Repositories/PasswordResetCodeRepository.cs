using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TutoringSystem.Application.Helpers;
using TutoringSystem.Domain.Entities;
using TutoringSystem.Domain.Repositories;
using TutoringSystem.Infrastructure.Data;

namespace TutoringSystem.Infrastructure.Repositories
{
    public class PasswordResetCodeRepository : RepositoryBase<PasswordResetCode>, IPasswordResetCodeRepository
    {
        public PasswordResetCodeRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<bool> AddCodeAsync(PasswordResetCode code)
        {
            Create(code);

            return await SaveChangedAsync();
        }

        public async Task<bool> AddCodesCollectionAsync(IEnumerable<PasswordResetCode> codes)
        {
            CreateRange(codes);

            return await SaveChangedAsync();
        }

        public async Task<PasswordResetCode> GetCodeAsync(Expression<Func<PasswordResetCode, bool>> expression, bool? isActive = true, bool isEagerLoadingEnabled = false)
        {
            if (isActive.HasValue)
            {
                ExpressionMerger.MergeExpression(ref expression, r => r.IsActive.Equals(isActive.Value));
            }

            var token = isEagerLoadingEnabled
                ? await GetCodeWithEagerLoading(expression)
                : await GetCodeWithoutEagerLoading(expression);

            return token;
        }

        public IQueryable<PasswordResetCode> GetCodesCollection(Expression<Func<PasswordResetCode, bool>> expression, bool? isActive = true, bool isEagerLoadingEnabled = false)
        {
            if (isActive.HasValue)
            {
                ExpressionMerger.MergeExpression(ref expression, r => r.IsActive.Equals(isActive.Value));
            }

            var tokens = isEagerLoadingEnabled
                ? GetCodesCollectionWithEagerLoading(expression)
                : Find(expression);

            return tokens;
        }

        public async Task<IEnumerable<PasswordResetCode>> GetCodesCollectionAsync(Expression<Func<PasswordResetCode, bool>> expression, bool? isActive = true, bool isEagerLoadingEnabled = false)
        {
            if (isActive.HasValue)
            {
                ExpressionMerger.MergeExpression(ref expression, r => r.IsActive.Equals(isActive.Value));
            }

            var tokens = isEagerLoadingEnabled
                ? GetCodesCollectionWithEagerLoading(expression)
                : Find(expression);

            return await tokens.ToListAsync();
        }

        public bool CodeExists(Expression<Func<PasswordResetCode, bool>> expression, bool? isActive = true)
        {
            if (isActive.HasValue)
            {
                ExpressionMerger.MergeExpression(ref expression, r => r.IsActive.Equals(isActive.Value));
            }

            bool exist = Contains(expression);

            return exist;
        }

        public async Task<bool> RemoveCodeAsync(PasswordResetCode code)
        {
            code.IsActive = false;

            return await UpdateCodeAsync(code);
        }

        public async Task<bool> UpdateCodeAsync(PasswordResetCode code)
        {
            Update(code);

            return await SaveChangedAsync();
        }

        public async Task<bool> UpdateCodesCollectionAsync(IEnumerable<PasswordResetCode> codes)
        {
            UpdateRange(codes);

            return await SaveChangedAsync();
        }

        private async Task<PasswordResetCode> GetCodeWithEagerLoading(Expression<Func<PasswordResetCode, bool>> expression)
        {
            var code = await Find(expression)
                .Include(t => t.User)
                .FirstOrDefaultAsync();

            return code;
        }

        private async Task<PasswordResetCode> GetCodeWithoutEagerLoading(Expression<Func<PasswordResetCode, bool>> expression)
        {
            var code = await Find(expression)
                .FirstOrDefaultAsync();

            return code;
        }

        private IQueryable<PasswordResetCode> GetCodesCollectionWithEagerLoading(Expression<Func<PasswordResetCode, bool>> expression)
        {
            var codes = Find(expression)
                .Include(t => t.User);

            return codes;
        }
    }
}