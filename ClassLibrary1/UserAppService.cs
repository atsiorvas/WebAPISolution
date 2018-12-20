using Common;
using Common.Interface;
using System;
using System.Threading.Tasks;
using MediatR;
using Repository;
using System.Reactive.Subjects;
using System.Linq.Expressions;
using AutoMapper;
using System.IO;
using System.Collections.Generic;

namespace Service {
    public class UserAppService : IUserAppService {

        private readonly IUserRepository _userRepository;
        private readonly IMediator _mediator;
        private readonly UnitOfWork _unitOfWork;
        private const bool Eager = true;
        private readonly IConfigurationProvider _cfg;
        private readonly IMapper _mapper;

        public UserAppService(IUserRepository userRepository,
            IMediator mediator,
            UnitOfWork unitOfWork,
            IMapper mapper) {
            _userRepository = userRepository
                ?? throw new ArgumentNullException("userRepository");
            _mediator = mediator ?? throw new ArgumentNullException("mediator");
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException("unitOfWork");
            _mapper = mapper ?? throw new ArgumentNullException("mapper");
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
            var user = await _unitOfWork
                .UserRepository.GetAsync(true, filter: u => u.Email == email);
            return _mapper.Map<UserModel>(user);
        }
        public async Task<bool> RemoveUserAsync(long id) {
            return await _unitOfWork.UserRepository.Delete(id);
        }

        public async Task<long> FindIdByBkAsync(string bk) {
            return await _unitOfWork.UserRepository
                .FindIdByBkAsync(filter: u => u.Email == (string)bk);
        }

       
    }
}