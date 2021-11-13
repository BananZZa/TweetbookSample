using System.Drawing;
using Application.CQRS.Tags;
using AutoMapper;
using Domain.Entities;

namespace Application.MappingProfiles
{
    public class TagProfile : Profile
    {
        public TagProfile()
        {
            CreateMap<Tag, TagDto>()
                .ForMember(p => p.TagId, x => x.MapFrom(e => e.TagId));
        }
    }
}