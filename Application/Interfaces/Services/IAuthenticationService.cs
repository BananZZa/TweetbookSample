using System.Threading.Tasks;
using Application.Entities;
using Domain.Entities;

namespace Application.Interfaces.Services
{
    public interface IAuthenticationService
    {
        Task<AuthenticationResult> GenerateAuthenticationResultForUserAsync(ApplicationUser user);
    }
}