using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Resources;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace Application.CQRS.Identity.Commands.Refresh
{
    public sealed class RefreshCommand : IRequest<AuthenticationResultDto>
    {
        public RefreshCommand(string token, string refreshToken)
        {
            Token = token;
            RefreshToken = refreshToken;
        }

        public string Token { get; }
        public string RefreshToken { get; }
    }

    public class RefreshCommandHandler : IRequestHandler<RefreshCommand, AuthenticationResultDto>
    {
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IAuthenticationService _authenticationService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public RefreshCommandHandler(TokenValidationParameters tokenValidationParameters,
            IRefreshTokenRepository refreshTokenRepository, IAuthenticationService authenticationService,
            UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _tokenValidationParameters = tokenValidationParameters;
            _refreshTokenRepository = refreshTokenRepository;
            _authenticationService = authenticationService;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<AuthenticationResultDto> Handle(RefreshCommand request, CancellationToken cancellationToken)
        {
            var validatedToken = GetPrincipalFromToken(request.Token);
            if (validatedToken == null)
                return new AuthenticationResultDto() { Errors = new[] { ErrorMessages.JwtInvalidToken } };

            var expiryDateUnix =
                long.Parse(validatedToken.Claims.Single(p => p.Type == JwtRegisteredClaimNames.Exp).Value);
            var expiryDateTimeUtc =
                new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(expiryDateUnix);
            if (expiryDateTimeUtc > DateTime.UtcNow)
            {
                return new AuthenticationResultDto() { Errors = new[] { ErrorMessages.JwtNotExpired } };
            }

            var jti = validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti)
                .Value;


            var storedRefreshToken =
                _refreshTokenRepository.GetQuery().SingleOrDefault(p => p.Token == request.RefreshToken);

            if (storedRefreshToken == null)
                return new AuthenticationResultDto() { Errors = new[] { ErrorMessages.JwtRefreshNotExist } };

            if (DateTime.UtcNow > storedRefreshToken.ExpiryDate)
                return new AuthenticationResultDto() { Errors = new[] { ErrorMessages.JwtRefreshExpired } };

            if (storedRefreshToken.Invalidated)
                return new AuthenticationResultDto() { Errors = new[] { ErrorMessages.JwtRefreshInvalidated } };

            if (storedRefreshToken.Used)
                return new AuthenticationResultDto() { Errors = new[] { ErrorMessages.JwtRefreshUsed } };

            if (storedRefreshToken.JwtId != jti)
                return new AuthenticationResultDto() { Errors = new[] { ErrorMessages.JwtRefreshNotMatchJwt } };


            storedRefreshToken.Used = true;
            await _refreshTokenRepository.UpdateAsync(storedRefreshToken);

            var user = await _userManager.FindByIdAsync(validatedToken.Claims
                .Single(x => x.Type == "id")
                .Value);

            var result = await _authenticationService.GenerateAuthenticationResultForUserAsync(user);
            return _mapper.Map<AuthenticationResultDto>(request);
        }
        
        
        private ClaimsPrincipal GetPrincipalFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var principal = tokenHandler.ValidateToken(token, _tokenValidationParameters,
                    out var validatedToken);
                if (!IsJwtWithValidSecurityAlgorithm(validatedToken))
                    return null;

                return principal;
            }
            catch
            {
                return null;
            }
        }
        
        private static bool IsJwtWithValidSecurityAlgorithm(SecurityToken validatedToken)
        {
            return (validatedToken is JwtSecurityToken jwtSecurityToken) &&
                   jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                       StringComparison.InvariantCultureIgnoreCase);
        }
    }
}