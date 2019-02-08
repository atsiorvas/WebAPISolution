using AutoMapper;
using Common.Data;
using Common.Info;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mapper {
    public class AlertMapper : Profile {
        public AlertMapper() {
            this.Apply();
        }

        public override string ProfileName {
            get { return "AlertMappings"; }
        }

        public void Apply() {

            CreateMap<Alert, AlertInfo>()
                .ForMember(dest => dest.Text, src => src.MapFrom<string>(opt => opt.Text))
                .ForMember(dest => dest.Arguments, src => src.MapFrom<IList<string>>(opt =>
                    opt.Arguments.Split(',', StringSplitOptions.None).ToList()))
                .ForMember(dest => dest.DateCreated, src => src.MapFrom<DateTime>(opt => opt.DateCreated))
                .ForMember(dest => dest.DateSent, src => src.MapFrom<DateTime>(opt => opt.DateSent))
                .ForMember(dest => dest.UserName, src => src.MapFrom<string>(opt => opt.User != null
                    ? opt.User.Email : string.Empty))
                .ReverseMap()
                .ForMember(dest => dest.Text, src => src.MapFrom<string>(opt => opt.Text))
                .ForMember(dest => dest.Arguments, src => src.MapFrom<string>(opt =>
                    string.Join(',', opt.Arguments)))
                .ForMember(dest => dest.DateCreated, src => src.MapFrom<DateTime>(opt => opt.DateCreated))
                .ForMember(dest => dest.DateSent, src => src.MapFrom<DateTime>(opt => opt.DateSent))
                .ForMember(dest => dest.AuditedEntity, src => src.Ignore())
                .ForMember(dest => dest.User,
                    opt => opt.MapFrom<UserResolver>())
                .ForMember(dest => dest.UserId, src => src.Ignore());

            CreateMap<PaginatedList<Alert>, PaginatedList<AlertInfo>>()
                .ForMember(dest => dest.Elements, opt => opt.MapFrom<IList<Alert>>(src => src.Elements))
                .ForMember(dest => dest.PageInfo, opt => opt.MapFrom<PageInfo>(src => src.PageInfo));
        }
    }
}
