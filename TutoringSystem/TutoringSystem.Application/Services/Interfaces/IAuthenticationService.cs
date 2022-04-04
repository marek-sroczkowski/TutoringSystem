using System.Threading.Tasks;
using TutoringSystem.Application.Dtos.AccountDtos;

namespace TutoringSystem.Application.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<AuthenticationResposneDto> AuthenticateAsync(AuthenticationDto authentication);
    }
}