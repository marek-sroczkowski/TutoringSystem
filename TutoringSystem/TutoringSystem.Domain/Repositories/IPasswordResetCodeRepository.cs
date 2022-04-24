using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TutoringSystem.Domain.Entities;

namespace TutoringSystem.Domain.Repositories
{
    public interface IPasswordResetCodeRepository
    {
        Task<bool> AddCodeAsync(PasswordResetCode code);
        Task<bool> AddCodesCollectionAsync(IEnumerable<PasswordResetCode> codes);
        Task<PasswordResetCode> GetCodeAsync(Expression<Func<PasswordResetCode, bool>> expression, bool? isActive = true, bool isEagerLoadingEnabled = false);
        IQueryable<PasswordResetCode> GetCodesCollection(Expression<Func<PasswordResetCode, bool>> expression, bool? isActive = true, bool isEagerLoadingEnabled = false);
        Task<IEnumerable<PasswordResetCode>> GetCodesCollectionAsync(Expression<Func<PasswordResetCode, bool>> expression, bool? isActive = true, bool isEagerLoadingEnabled = false);
        bool CodeExists(Expression<Func<PasswordResetCode, bool>> expression, bool? isActive = true);
        Task<bool> RemoveCodeAsync(PasswordResetCode code);
        Task<bool> UpdateCodeAsync(PasswordResetCode code);
        Task<bool> UpdateCodesCollectionAsync(IEnumerable<PasswordResetCode> codes);
    }
}