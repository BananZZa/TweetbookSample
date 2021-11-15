using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Application.Entities;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Settings;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace Application.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JwtSettings _jwtSettings;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly TokenValidationParameters _tokenValidationParameters;

        public AuthenticationService(UserManager<ApplicationUser> userManager, JwtSettings jwtSettings,
            IRefreshTokenRepository refreshTokenRepository, TokenValidationParameters tokenValidationParameters)
        {
            _userManager = userManager;
            _jwtSettings = jwtSettings;
            _refreshTokenRepository = refreshTokenRepository;
            _tokenValidationParameters = tokenValidationParameters;
        }

        public async Task<AuthenticationResult> GenerateAuthenticationResultForUserAsync(ApplicationUser user)
        {
            var tokenHander = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.Email),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(JwtRegisteredClaimNames.Email, user.Email),
                new("id", user.Id.ToString())
            };

            claims.AddRange(await _userManager.GetClaimsAsync(user));

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.Add(_jwtSettings.TokenLifetime),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHander.CreateToken(tokenDescriptor);

            var refreshToken = new RefreshToken()
            {
                JwtId = token.Id,
                UserId = user.Id,
                CreationDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddMonths(6),
            };

            await _refreshTokenRepository.AddAsync(refreshToken);

            return new AuthenticationResult()
            {
                Success = true,
                Token = tokenHander.WriteToken(token),
                RefreshToken = refreshToken.Token
            };
        }
    }
}