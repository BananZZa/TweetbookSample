using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistance.Repositories
{
    public class TagRepository : ITagRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public TagRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        
        public IQueryable<Tag> GetQuery()
        {
            return _applicationDbContext.Tags.AsQueryable();
        }

        public async Task<List<Tag>> GetAsync()
        {
            return await _applicationDbContext.Tags.ToListAsync();
        }

        public async Task<Tag> AddAsync(Tag entity)
        {
            _applicationDbContext.Tags.Add(entity);
            await _applicationDbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<Tag> UpdateAsync(Tag entity)
        {
            _applicationDbContext.Tags.Update(entity);
            await _applicationDbContext.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(long id)
        {
            var entity = await _applicationDbContext.Tags.FindAsync(id);
            _applicationDbContext.Tags.Remove(entity);
            await _applicationDbContext.SaveChangesAsync();
        }
    }
}