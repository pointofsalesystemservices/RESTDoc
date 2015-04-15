namespace RestDoc.Models
{
    using AutoMapper;
    using Data;
    using View.Api;

    public class PathGroupViewModelProfile : Profile
    {
        protected override void Configure()
        {
            base.Configure();
            CreateMap<ApiPathGroup, PathGroupViewModel>()
                .ReverseMap()
                .ForMember(i => i.Id, opt => opt.Ignore())
                .ForMember(i => i.RestApi, opt => opt.Ignore())
                .ForMember(i => i.ApiPaths, opt => opt.Ignore())
                .ForMember(i => i.RestApiId, opt => opt.Ignore());
        }
    }
}