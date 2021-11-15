using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces.Services;
using Application.Resources;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.CQRS.Identity.Commands.Register
{
    public sealed class RegisterCommand : IRequest<AuthenticationResultDto>
    {
        public RegisterCommand(string email, string password)
        {
            Email = email;
            Password = password;
        }

        public string Email { get; }
        public string Password { get; }
    }
    
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthenticationResultDto>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAuthenticationService _authenticationService;
        private readonly IMapper _mapper;

        public RegisterCommandHandler(IAuthenticationService authenticationService,
            UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _authenticationService = authenticationService;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<AuthenticationResultDto> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser != null)
            {
                return new AuthenticationResultDto()
                {
                    Errors = new[] { ErrorMessages.EmailAlreadyExist },
                };
            }

            var newUser = new ApplicationUser()
            {
                Email = request.Email,
                UserName = request.Email,
            };
            var createdUser = await _userManager.CreateAsync(newUser, request.Password);


            if (!createdUser.Succeeded)
            {
                return new AuthenticationResultDto()
                {
                    Errors = createdUser.Errors.Select(p => p.Description)
                };
            }

            var result = await _authenticationService.GenerateAuthenticationResultForUserAsync(newUser);
            return _mapper.Map<AuthenticationResultDto>(result);
        }
    }
}