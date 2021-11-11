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
        public UpdatePostCommand(long postId, long userId, string title, string content, string[] tags)
        {
            PostId = postId;
            UserId = userId;
            Title = title;
            Content = content;
            Tags = tags;
        }

        public long PostId { get; }
        public long UserId { get; }
        public string Title { get; }
        public string Content { get; }
        public string[] Tags { get; }
    }
    
    public class UpdatePostCommandHandler : IRequestHandler<UpdatePostCommand, PostDto>
    {
        private readonly IPostRepository _postRepository;
        private readonly ITagsRepository _tagsRepository;
        private readonly IMapper _mapper;

        public UpdatePostCommandHandler(IPostRepository postRepository, IMapper mapper, ITagsRepository tagsRepository)
        {
            _postRepository = postRepository;
            _mapper = mapper;
            _tagsRepository = tagsRepository;
        }

        public async Task<PostDto> Handle(UpdatePostCommand request, CancellationToken cancellationToken)
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
            
            var updatedPost = await _postRepository.AddAsync(new Post
            {
                Id = request.PostId,
                UserId = request.UserId,
                Title = request.Title,
                Content = request.Content,
                Tags = existedTags.Concat(newTags).ToList()
            });

            return _mapper.Map<PostDto>(updatedPost);
        }
    }
}