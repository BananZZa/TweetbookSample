using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces.Repositories;
using AutoMapper;
using MediatR;

namespace Application.CQRS.Posts.Queries.GetPost
{
    public class GetPostQuery : IRequest<PostDto>
    {
        public GetPostQuery(long id)
        {
            Id = id;
        }
        public long Id { get; }  
    }
    
    public class GetPostQueryHandler : IRequestHandler<GetPostQuery, PostDto>
    {
        private readonly IPostRepository _postRepository;
        private readonly IMapper _mapper;

        public GetPostQueryHandler(IPostRepository postRepository, IMapper mapper)
        {
            _postRepository = postRepository;
            _mapper = mapper;
        }

        public Task<PostDto> Handle(GetPostQuery request, CancellationToken cancellationToken)
        {
            var post = _postRepository.GetQuery().Single(p => p.Id == request.Id);
            var postDto = _mapper.Map<PostDto>(post);
            return Task.FromResult(postDto);
        }
    }
}