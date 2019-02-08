using AutoMapper;
using Common;
using Common.Info;
using Common.Interface;
using System.Collections.Generic;

namespace Mapper {
    public class UserMapper : Profile, IUserMapper {
        public UserMapper() {
            this.Apply();
        }

        public override string ProfileName {
            get { return "UserMappings"; }
        }

        public void Apply() {

            CreateMap<User, UserModel>()
            .ForMember(dest => dest.Email, src => src.MapFrom<string>(srcUser => srcUser.Email))
            .ForMember(dest => dest.LastName, src => src.MapFrom<string>(srcUser => srcUser.LastName))
            .ForMember(dest => dest.FirstName, src => src.MapFrom<string>(srcUser => srcUser.FirstName))
            .ForMember(dest => dest.RememberMe, src => src.MapFrom<bool>(srcUser => srcUser.RememberMe))
            .ForMember(dest => dest.ResetAnswer, src => src.Ignore())
            .ForMember(dest => dest.Note, src => src.MapFrom(srcUser => srcUser.Note))
            .ReverseMap()
            .ForMember(dest => dest.FirstName, src => src.MapFrom<string>(srcUser => srcUser.FirstName))
            .ForMember(dest => dest.LastName, src => src.MapFrom<string>(srcUser => srcUser.LastName))
            .ForMember(dest => dest.RememberMe, src => src.MapFrom<bool>(srcUser => srcUser.RememberMe))
            .ForMember(dest => dest.Password, src => src.MapFrom<string>(srcUser => srcUser.Password))
            .ForMember(dest => dest.Note, src =>
                src.MapFrom(srcUser => srcUser.Note))
            .ForMember(dest => dest.IsAdmin, src => src.Ignore())
            .ForMember(dest => dest.Id, src => src.Ignore());

            CreateMap<PaginatedList<User>, PaginatedList<UserModel>>()
                .ForMember(dest => dest.Elements, opt => opt.MapFrom<IList<User>>(src => src.Elements))
                .ForMember(dest => dest.PageInfo, opt => opt.MapFrom<PageInfo>(src => src.PageInfo));
        }
    }
}