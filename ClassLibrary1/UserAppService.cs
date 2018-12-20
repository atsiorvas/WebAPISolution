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
using System.Linq;

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
            var userEntry = _mapper.Map<User>(userModel);

            //AuditedEntity audit = new AuditedEntity() {
            //    CreatedBy = "Admin",
            //    CreatedOn = DateTime.UtcNow,
            //    UpdatedOn = DateTime.UtcNow
            //};
            //userEntry.AuditedEntity = audit;

            //foreach (var note in userEntry.Note) {
            //    note.AuditedEntity = audit;
            //}

            await _unitOfWork.UserRepository.SaveAsync(userEntry);

            return userModel;
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
        public async Task<bool> UpdateUser(UserModel userModel) {
            var userEntity = _mapper.Map<User>(userModel);
            userEntity.Id = 1;
            return await _unitOfWork.UserRepository.UpdateAsync(userEntity);

        }
        public async Task<bool> ChangeUser(UserModel userModel) {
            try {
                User user = await _unitOfWork.UserRepository
                   .GetAsync(true, filter: u => u.Email == userModel.Email);

                user = _mapper.Map<User>(userModel);
                if (user.AuditedEntity == null) {
                    user.AuditedEntity = new AuditedEntity();
                }
                await _unitOfWork.UserRepository.UpdateAsync(user);
                await _unitOfWork.UserRepository.SaveAsync(user);
                return true;
            } catch (Exception ex) {
                return false;
            }
        }

        public PaginatedList<UserModel> GetUserPaging(string email,
            string searchString, string nameSort, int pageNumber, int pageSize) {
            try {
                IQueryable<User> query = null;

                if (!string.IsNullOrEmpty(searchString)) {
                    query = _unitOfWork.UserRepository.GetQuery(true,
                        filter: s => (s.LastName.Contains(searchString)
                                               || s.FirstName.Contains(searchString)));
                } else {
                    query = _unitOfWork.UserRepository
                            .GetQuery(true, filter: u => u.Email == email);
                }

                switch (nameSort) {
                    case "last_name_desc":
                        query = query?.OrderByDescending(s => s.LastName);
                        break;
                    case "first_name_desc":
                        query = query?.OrderByDescending(s => s.FirstName);
                        break;
                    case "email_desc":
                        query = query?.OrderByDescending(s => s.Email);
                        break;
                    default:
                        query = query?.OrderBy(s => s.LastName);
                        break;
                }

                PaginatedList<User> pagination
                    = new PaginatedList<User>(query, pageNumber, pageSize);

                PaginatedList<UserModel> paginated
                    = _mapper.Map<PaginatedList<UserModel>>(pagination);
                return paginated;
            } catch (Exception ex) {
                return null;
            }
        }
    }
}