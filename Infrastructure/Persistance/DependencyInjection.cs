using Microsoft.Extensions.DependencyInjection;
using System;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistance.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Persistance
{
    public static class DependencyInjection
    {
        public static void AddPersistance(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<ApplicationDbContext>(opt =>
                {
                    opt.UseInMemoryDatabase(Guid.NewGuid().ToString());
                });
            }
            else
            {
                services.AddDbContext<ApplicationDbContext>(opt =>
                {
                    opt.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
                });
            }

            services.AddScoped<ITagRepository, TagRepository>();
            services.AddScoped<IPostRepository, PostRepository>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            
            services.AddIdentity<ApplicationUser, IdentityRole<long>>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
        }
    }
}
