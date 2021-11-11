using System.Linq;
using Application.Interfaces.Repositories;
using Application.Resources;
using FluentValidation;

namespace Application.CQRS.Posts.Commands.DeletePost
{
    public class DeletePostCommandValidator : AbstractValidator<DeletePostCommand>
    {
        private readonly IPostRepository _postRepository;

        public DeletePostCommandValidator(IPostRepository postRepository)
        {
            _postRepository = postRepository;
            RuleFor(v => v.UserId)
                .Must(BeEqualWithCurrent).WithMessage(ErrorMessages.Post_UserDoNotOwnPost);
        }

        private bool BeEqualWithCurrent(DeletePostCommand command, long userId)
        {
            var post = _postRepository.GetQuery().SingleOrDefault(p => p.UserId == userId);
            return post != null;
        }
    }
}