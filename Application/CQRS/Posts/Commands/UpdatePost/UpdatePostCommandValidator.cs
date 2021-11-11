using System.Linq;
using Application.Interfaces.Repositories;
using Application.Resources;
using FluentValidation;

namespace Application.CQRS.Posts.Commands.UpdatePost
{
    public class UpdatePostCommandValidator : AbstractValidator<UpdatePostCommand>
    {
        private readonly IPostRepository _postRepository;

        public UpdatePostCommandValidator(IPostRepository postRepository)
        {
            _postRepository = postRepository;
            
            RuleFor(p => p.Title)
                .NotEmpty();

            RuleFor(p => p.Content)
                .NotEmpty();
            
            RuleFor(v => v.UserId)
                .Must(BeEqualWithCurrent).WithMessage(ErrorMessages.Post_UserDoNotOwnPost);
        }

        private bool BeEqualWithCurrent(UpdatePostCommand command, long userId)
        {
            var post = _postRepository.GetQuery().SingleOrDefault(p => p.UserId == userId);
            return post != null;
        }
    }
}