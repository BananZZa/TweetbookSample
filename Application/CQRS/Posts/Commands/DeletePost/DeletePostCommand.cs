using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces.Repositories;
using MediatR;

namespace Application.CQRS.Posts.Commands.DeletePost
{
    public class DeletePostCommand : IRequest
    {
        public DeletePostCommand(long id)
        {
            this.Id = id;
        }
        public long Id { get; }
    }

    public class DeletePostCommandHandler : IRequestHandler<DeletePostCommand>
    {
        private readonly IPostRepository _postRepository;
        public DeletePostCommandHandler(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        public async Task<Unit> Handle(DeletePostCommand request, CancellationToken cancellationToken)
        {
            await _postRepository.DeleteAsync(request.Id);
            return Unit.Value;
        }
    }
}