using Common;
using Common.Interface;
using System;
using System.Threading.Tasks;
using MediatR;
using Repository;
using System.Reactive.Subjects;

namespace Service {
    public class UserAppService : IUserAppService {

        private readonly IUserRepository _userRepository;
        private readonly IMediator _mediator;
        private readonly UnitOfWork _unitOfWork;

        public UserAppService(IUserRepository userRepository,
            IMediator mediator,
            UnitOfWork unitOfWork) {
            _userRepository = userRepository
                ?? throw new ArgumentNullException("userRepository");
            _mediator = mediator ?? throw new ArgumentNullException("mediator");
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException("unitOfWork");
        }

        public async Task<UserModel> RegisterAsync(UserModel userRegister) {

            var email = userRegister.Email;

            if (await _unitOfWork.UserRepository
                .IsExistsAsync(filter: q => q.Email == email)) {
                return null;
            }

            UserModel user = await _userRepository.SaveUserAsync(userRegister);

            return user;
        }
        public async Task<UserModel> LoginAsync(LoginModel userLogin) {

            var email = userLogin.Email;
            UserModel user = await _userRepository.GetUserAsync(email);
            return user;
        }

        public async Task<UserModel> SaveUserAsync(UserModel userModel) {
            var email = userModel.Email;

            if (await _unitOfWork.
                UserRepository.IsExistsAsync(filter: q => q.Email == email)) {
                return null;
            }
            UserModel user = await _userRepository.SaveUserAsync(userModel);

            return user;
        }

        public async Task<UserModel> GetUserAsync(string email) {

            UserModel user = await _userRepository.GetUserAsync(email);

            return user;
        }

        public async Task<UserModel> GetUserWithNotesAsync(string email) {
            UserModel user = await _userRepository.GetUserWithNotesAsync(email);
            return user;
        }
        public async Task<bool> RemoveUserAsync(string email) {
            return await _userRepository.DeleteUserAsync(email);
        }
    }
}