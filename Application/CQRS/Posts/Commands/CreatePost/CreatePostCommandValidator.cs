using FluentValidation;

namespace Application.CQRS.Posts.Commands.CreatePost
{
    public class CreatePostCommandValidator : AbstractValidator<CreatePostCommand>
    {
        public CreatePostCommandValidator()
        {
            RuleFor(p => p.Title)
                .NotEmpty();

            RuleFor(p => p.Content)
                .NotEmpty();
        }
    }
}