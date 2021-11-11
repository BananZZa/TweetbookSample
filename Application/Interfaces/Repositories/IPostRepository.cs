using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface IPostRepository
    {
        IQueryable<Post> GetQuery();
        Task<List<Post>> GetAsync();
        Task<Post> AddAsync(Post entity);
        Task<Post> UpdateAsync(Post entity);
        Task DeleteAsync(long id);
    }
}