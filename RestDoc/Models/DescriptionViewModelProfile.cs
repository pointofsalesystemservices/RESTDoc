namespace RestDoc.Models
{
    using AutoMapper;
    using Data;
    using View.Api;

    public class DescriptionViewModelProfile : Profile
    {
        protected override void Configure()
        {
            base.Configure();
            CreateMap<RestApi, ObjectDescriptionViewModel>()
                .ForMember(i => i.Id, opt => opt.MapFrom(x => x.Id))
                .ForMember(i => i.Description, opt => opt.MapFrom(x => x.Name));

            CreateMap<ApiPathGroup, ObjectDescriptionViewModel>()
                .ForMember(i => i.Id, opt => opt.MapFrom(x => x.Id))
                .ForMember(i => i.Description, opt => opt.MapFrom(x => x.Description));

            CreateMap<ApiPath, ObjectDescriptionViewModel>()
                .ForMember(i => i.Id, opt => opt.MapFrom(x => x.Id))
                .ForMember(i => i.Description, opt => opt.MapFrom(x => x.Description));

            CreateMap<ApiVerb, ObjectDescriptionViewModel>()
                .ForMember(i => i.Id, opt => opt.MapFrom(x => x.Id))
                .ForMember(i => i.Description, opt => opt.MapFrom(x => x.Verb));

            CreateMap<ApiStatusCode, ObjectDescriptionViewModel>()
                .ForMember(i => i.Id, opt => opt.MapFrom(x => x.Id))
                .ForMember(i => i.Description, opt => opt.MapFrom(x => x.StatusCode + " - " + x.Description));

            CreateMap<ApiParameter, ObjectDescriptionViewModel>()
                .ForMember(i => i.Id, opt => opt.MapFrom(x => x.Id))
                .ForMember(i => i.Description, opt => opt.MapFrom(x => x.Name + " - " + x.DataType + " - Required? " + (x.Required ? "Yes" : "No")));


        }
    }
}