using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface IRefreshTokenRepository
    {
        IQueryable<RefreshToken> GetQuery();
        Task<List<RefreshToken>> GetAsync();
        Task<RefreshToken> AddAsync(RefreshToken entity);
        Task<RefreshToken> UpdateAsync(RefreshToken entity);
        Task DeleteAsync(long id);
    }
}