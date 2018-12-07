using AutoMapper;
using Common;
using Common.Interface;

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
            .ForMember(dest => dest.Note, src => src.MapFrom(sourceMember: srcUser => srcUser.Note))
            .ReverseMap()
            .ForMember(dest => dest.FirstName, src => src.MapFrom<string>(srcUser => srcUser.FirstName))
            .ForMember(dest => dest.LastName, src => src.MapFrom<string>(srcUser => srcUser.LastName))
            .ForMember(dest => dest.RememberMe, src => src.MapFrom<bool>(srcUser => srcUser.RememberMe))
            .ForMember(dest => dest.Password, src => src.MapFrom<string>(srcUser => srcUser.Password))
            .ForMember(dest => dest.Note, src =>
                src.MapFrom(srcUser => srcUser.Note))
            .ForMember(dest => dest.IsAdmin, src => src.Ignore())
            .ForMember(dest => dest.UserId, src => src.Ignore());
        }
    }
}
