using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface ITagRepository
    {
        IQueryable<Tag> GetQuery();
        Task<List<Tag>> GetAsync();
        Task<Tag> AddAsync(Tag entity);
        Task<Tag> UpdateAsync(Tag entity);
        Task DeleteAsync(long id);
    }
}