using System.Threading.Tasks;
using TutoringSystem.Application.Models.Dtos.Authentication;

namespace TutoringSystem.Application.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<AuthenticationResposneDto> AuthenticateAsync(AuthenticationDto authentication, string clientIp);
    }
}