using AutoMapper;
using Common;
using Common.Data;
using Common.Info;
using Common.Interface;

namespace Mapper {

    //MyPropertyResolver
    public class UserResolver : IValueResolver<AlertInfo, object, User> {

        private readonly IUserAppService _userAppService;
        private readonly IMapper _mapper;

        public UserResolver(IUserAppService userAppService, IMapper mapper) {
            _userAppService = userAppService;
            _mapper = mapper;
        }

        /*
         * sourceMember - > email
         * 
         */
        public User Resolve(AlertInfo source, object destination,
            User destMember, ResolutionContext context) {

            UserModel userModel = _userAppService.GetUser(source.UserName);
            return _mapper.Map<User>(userModel);
        }
    }
}