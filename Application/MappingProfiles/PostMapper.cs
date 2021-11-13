using System.Linq;
using Application.CQRS.Posts;
using AutoMapper;
using Domain.Entities;

namespace Application.MappingProfiles
{
    public class PostMapper : Profile
    {
        public PostMapper()
        {
            CreateMap<Post, PostDto>()
                .ForMember(p => p.Content, x => x.MapFrom(e => e.Content))
                .ForMember(p => p.Id, x => x.MapFrom(e => e.Id))
                .ForMember(p => p.Tags, x => x.MapFrom(e => e.Tags.Select(p => p.TagId).ToArray()))
                .ForMember(p => p.Title, x => x.MapFrom(e => e.Title));
        }
    }
}