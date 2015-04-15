namespace RestDoc.Models
{
    using AutoMapper;
    using Data;
    using View.Api;

    public class VerbViewModelProfile : Profile
    {
        protected override void Configure()
        {
            base.Configure();
            CreateMap<ApiVerb, VerbViewModel>()
                .ForMember(i => i.StatusCodes, opt => opt.MapFrom(i => i.ApiStatusCodes))
                .ForMember(i => i.RequestBody, opt => opt.MapFrom(x => x.RequestBody))
                .ForMember(i => i.ResponseBody, opt => opt.MapFrom(x => x.ResponseBody))
                .ForMember(i => i.Path, opt => opt.MapFrom(x => x.ApiPath))
                .ReverseMap()
                .ForMember(i => i.Id, opt => opt.Ignore())
                .ForMember(i => i.ApiStatusCodes, opt => opt.Ignore())
                .ForMember(i => i.Parameters, opt => opt.Ignore())
                .ForMember(i => i.ApiPath, opt => opt.Ignore())
                .ForMember(i => i.ApiPathId, opt => opt.Ignore())
                .ForMember(i => i.RequestBodyId, opt => opt.Ignore())
                .ForMember(i => i.ResponseBodyId, opt => opt.Ignore());

        }
    }

    public class BodyViewModelProfile : Profile
    {
        protected override void Configure()
        {
            base.Configure();
            CreateMap<ApiBody, BodyViewModel>()
                .ReverseMap()
                .ForMember(i => i.Id, opt => opt.Ignore());
        }
    }
}