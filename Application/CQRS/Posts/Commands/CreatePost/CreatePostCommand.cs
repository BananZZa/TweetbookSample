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
        public CreatePostCommand(string title, string content, string[] tags)
        {
            Title = title;
            Content = content;
            Tags = tags;
        }
        
        public string Title { get; }
        public string Content { get; }
        public string[] Tags { get; }
    }
    
    public class CreatePostCommandHandler : IRequestHandler<CreatePostCommand, PostDto>
    {
        private readonly IPostRepository _postRepository;
        private readonly ITagRepository _tagRepository;
        private readonly IMapper _mapper;

        public CreatePostCommandHandler(IPostRepository postRepository, IMapper mapper, ITagRepository tagRepository)
        {
            _postRepository = postRepository;
            _tagRepository = tagRepository;
            _mapper = mapper;
        }

        public async Task<PostDto> Handle(CreatePostCommand request, CancellationToken cancellationToken)
        {
            var existedTags = _tagRepository.GetQuery()
                .Where(p => request.Tags.Any(rt => rt == p.TagId))
                .ToList();
            var existedTagIds = existedTags.Select(p => p.TagId).ToList();
            var newTags = request.Tags
                .Except(existedTagIds)
                .Select(tag => new Tag
                {
                    TagId = tag
                })
                .ToList();
            
            var newPost = await _postRepository.AddAsync(new Post
            {
                Title = request.Title,
                Content = request.Content,
                Tags = existedTags.Concat(newTags).ToList()
            });
            
            return _mapper.Map<PostDto>(newPost);
        }
    }
}