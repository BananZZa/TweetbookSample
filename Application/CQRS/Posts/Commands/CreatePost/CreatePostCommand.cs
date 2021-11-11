using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.CQRS.Posts.Commands.CreatePost
{
    public sealed class CreatePostCommand : IRequest<PostDto>
    {
        public CreatePostCommand(long userId, string title, string content, string[] tags)
        {
            Title = title;
            Content = content;
            Tags = tags;
            UserId = userId;
        }
        
        public long UserId { get; }
        public string Title { get; }
        public string Content { get; }
        public string[] Tags { get; }
    }
    
    public class CreatePostCommandHandler : IRequestHandler<CreatePostCommand, PostDto>
    {
        private readonly IPostRepository _postRepository;
        private readonly ITagsRepository _tagsRepository;
        private readonly IMapper _mapper;

        public CreatePostCommandHandler(IPostRepository postRepository, IMapper mapper, ITagsRepository tagsRepository)
        {
            _postRepository = postRepository;
            _mapper = mapper;
            _tagsRepository = tagsRepository;
        }

        public async Task<PostDto> Handle(CreatePostCommand request, CancellationToken cancellationToken)
        {
            var existedTags = _tagsRepository.GetQuery()
                .Where(p => request.Tags.Any(rt => rt == p.TagId));
            var existedTagIds = existedTags.Select(p => p.TagId);
            var newTags = request.Tags
                .Except(existedTagIds)
                .Select(tag => new Tag
                {
                    TagId = tag
                })
                .ToList();
            
            var newPost = await _postRepository.AddAsync(new Post
            {
                UserId = request.UserId,
                Title = request.Title,
                Content = request.Content,
                Tags = existedTags.Concat(newTags).ToList()
            });
            
            return _mapper.Map<PostDto>(newPost);
        }
    }
}