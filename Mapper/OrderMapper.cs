using AutoMapper;
using Common;
using Common.Info;
using System;

namespace Mapper {
    public class OrderMapper : Profile {
        public OrderMapper() {
            this.Apply();
        }

        public override string ProfileName {
            get { return "OrderMappings"; }
        }

        public void Apply() {

            CreateMap<Order, OrderInfo>()
                .ForMember(dest => dest.DateSent, src => src.MapFrom<DateTime>(opt => opt.DateSent))
                .ForMember(dest => dest.Description, src => src.MapFrom<string>(opt => opt.Description))
                .ReverseMap()
                .ForMember(dest => dest.DateSent, src => src.MapFrom<DateTime>(opt => opt.DateSent))
                .ForMember(dest => dest.Description, src => src.MapFrom<string>(opt => opt.Description))
                .ForMember(dest => dest.AuditedEntity, src => src.Ignore())
                .ForMember(dest => dest.OrderAlert, src => src.Ignore())
                .ForMember(dest => dest.User, src => src.Ignore())
                .ForMember(dest => dest.UserId, src => src.Ignore());
        }
    }
}
