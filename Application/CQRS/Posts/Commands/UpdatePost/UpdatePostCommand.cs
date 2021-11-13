using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.CQRS.Posts.Commands.UpdatePost
{
    public class UpdatePostCommand : IRequest<PostDto>
    {
        public UpdatePostCommand(long postId, string title, string content, string[] tags)
        {
            PostId = postId;
            Title = title;
            Content = content;
            Tags = tags;
        }

        public long PostId { get; }
        public string Title { get; }
        public string Content { get; }
        public string[] Tags { get; }
    }
    
    public class UpdatePostCommandHandler : IRequestHandler<UpdatePostCommand, PostDto>
    {
        private readonly IPostRepository _postRepository;
        private readonly ITagRepository _tagRepository;
        private readonly IMapper _mapper;

        public UpdatePostCommandHandler(IPostRepository postRepository, IMapper mapper, ITagRepository tagRepository)
        {
            _postRepository = postRepository;
            _mapper = mapper;
            _tagRepository = tagRepository;
        }

        public async Task<PostDto> Handle(UpdatePostCommand request, CancellationToken cancellationToken)
        {
            var existedTags = _tagRepository.GetQuery()
                .Where(p => request.Tags.Any(rt => rt == p.TagId));
            var existedTagIds = existedTags.Select(p => p.TagId);
            var newTags = request.Tags
                .Except(existedTagIds)
                .Select(tag => new Tag
                {
                    TagId = tag
                })
                .ToList();
            
            var updatedPost = await _postRepository.AddAsync(new Post
            {
                Id = request.PostId,
                Title = request.Title,
                Content = request.Content,
                Tags = existedTags.Concat(newTags).ToList()
            });

            return _mapper.Map<PostDto>(updatedPost);
        }
    }
}