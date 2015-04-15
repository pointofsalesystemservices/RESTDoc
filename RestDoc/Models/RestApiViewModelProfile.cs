using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestDoc.Models
{
    using AutoMapper;
    using Data;
    using View.Api;

    public class RestApiViewModelProfile : Profile
    {
        protected override void Configure()
        {
            base.Configure();

            CreateMap<RestApi, RestApiViewModel>()
                .ReverseMap()
                .ForMember(i => i.Id, opt => opt.Ignore())
                .ForMember(i => i.LastModified, opt => opt.MapFrom(x => DateTimeOffset.Now))
                .ForMember(i => i.Created, opt => opt.Ignore())
                .ForMember(i => i.Creator, opt => opt.Ignore())
                .ForMember(i => i.PathGroups, opt => opt.Ignore());
        }
    }
}
