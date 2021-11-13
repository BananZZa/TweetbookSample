using System.Linq;
using Application.Interfaces.Repositories;
using Application.Resources;
using FluentValidation;

namespace Application.CQRS.Posts.Commands.UpdatePost
{
    public class UpdatePostCommandValidator : AbstractValidator<UpdatePostCommand>
    {
        public UpdatePostCommandValidator()
        {
            RuleFor(p => p.Title)
                .NotEmpty();
            RuleFor(p => p.Content)
                .NotEmpty();
        }
    }
}