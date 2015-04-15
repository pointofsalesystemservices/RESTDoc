namespace RestDoc.Models
{
    using AutoMapper;
    using Data;
    using View.Api;

    public class PathViewModelProfile : Profile
    {
        protected override void Configure()
        {
            base.Configure();
            CreateMap<ApiPath, PathViewModel>()
                .ForMember(i => i.PathGroup, opt => opt.MapFrom(i => i.ApiPathGroup))
                .ForMember(i => i.Verbs, opt => opt.MapFrom(x => x.ApiVerbs))
                .ReverseMap()
                .ForMember(i => i.ApiPathGroupId, opt => opt.Ignore())
                .ForMember(i => i.ApiPathGroup, opt => opt.Ignore())
                .ForMember(i => i.ApiVerbs, opt => opt.Ignore())
                .ForMember(i => i.Id, opt => opt.Ignore());
        }
    }
}