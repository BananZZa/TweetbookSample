using System.Reflection;
using System.Text;
using Application.Behaviours;
using Application.Interfaces.Services;
using Application.Services;
using Application.Settings;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Application
{
    public static class DependencyInjection
    {
        public static void AddApplication(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddAutoMapper(Assembly.GetExecutingAssembly());
            serviceCollection.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            serviceCollection.AddMediatR(Assembly.GetExecutingAssembly());

            serviceCollection.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

            var jwtSettings = new JwtSettings();
            configuration.Bind(nameof(jwtSettings), jwtSettings);
            serviceCollection.AddSingleton(jwtSettings);

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey =
                    new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.Secret)),
                ValidateIssuer = false,
                ValidateAudience = false,
                RequireExpirationTime = false,
                ValidateLifetime = true
            };

            serviceCollection.AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(x =>
                {
                    x.SaveToken = true;
                    x.TokenValidationParameters = tokenValidationParameters;
                });

            serviceCollection.AddSingleton(tokenValidationParameters);

            serviceCollection.AddScoped<IAuthenticationService, AuthenticationService>();
        }
    }
}