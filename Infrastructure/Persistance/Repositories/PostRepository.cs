using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistance.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public PostRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public IQueryable<Post> GetQuery()
        {
            return _applicationDbContext.Posts.AsQueryable();
        }

        public async Task<List<Post>> GetAsync()
        {
            return await _applicationDbContext.Posts.ToListAsync();
        }

        public async Task<Post> AddAsync(Post entity)
        {
            _applicationDbContext.Posts.Add(entity);
            await _applicationDbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<Post> UpdateAsync(Post entity)
        {
            _applicationDbContext.Posts.Update(entity);
            await _applicationDbContext.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(long id)
        {
            var entity = await _applicationDbContext.Posts.FindAsync(id);
            _applicationDbContext.Posts.Remove(entity);
            await _applicationDbContext.SaveChangesAsync();
        }
    }
}