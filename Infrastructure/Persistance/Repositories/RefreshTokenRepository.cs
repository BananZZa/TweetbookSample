using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistance.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public RefreshTokenRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        
        public IQueryable<RefreshToken> GetQuery()
        {
            return _applicationDbContext.RefreshTokens.AsQueryable();
        }

        public async Task<List<RefreshToken>> GetAsync()
        {
            return await _applicationDbContext.RefreshTokens.ToListAsync();
        }

        public async Task<RefreshToken> AddAsync(RefreshToken entity)
        {
            _applicationDbContext.RefreshTokens.Add(entity);
            await _applicationDbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<RefreshToken> UpdateAsync(RefreshToken entity)
        {
            _applicationDbContext.RefreshTokens.Update(entity);
            await _applicationDbContext.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(long id)
        {
            var entity = await _applicationDbContext.RefreshTokens.FindAsync(id);
            _applicationDbContext.RefreshTokens.Remove(entity);
            await _applicationDbContext.SaveChangesAsync();
        }
    }
}