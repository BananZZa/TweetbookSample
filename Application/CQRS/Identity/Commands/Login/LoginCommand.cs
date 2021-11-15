using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces.Services;
using Application.Resources;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.CQRS.Identity.Commands.Login
{
    public class LoginCommand : IRequest<AuthenticationResultDto>
    {
        public LoginCommand(string email, string password)
        {
            Email = email;
            Password = password;
        }

        public string Email { get; }
        public string Password { get; }
    }

    public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthenticationResultDto>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAuthenticationService _authenticationService;
        private readonly IMapper _mapper;

        public LoginCommandHandler(UserManager<ApplicationUser> userManager,
            IAuthenticationService authenticationService, IMapper mapper)
        {
            _userManager = userManager;
            _authenticationService = authenticationService;
            _mapper = mapper;
        }

        public async Task<AuthenticationResultDto> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return new AuthenticationResultDto()
                {
                    Errors = new[] { ErrorMessages.UserDoesNotExist },
                };
            }

            var userHasValidPassword = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!userHasValidPassword)
            {
                return new AuthenticationResultDto()
                {
                    Errors = new[] { ErrorMessages.WrongPassword },
                };
            }

            var result = await _authenticationService.GenerateAuthenticationResultForUserAsync(user);
            return _mapper.Map<AuthenticationResultDto>(result);
        }
    }
}