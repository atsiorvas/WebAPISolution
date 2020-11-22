using Common;
using Common.Interface;
using System;
using System.Threading.Tasks;
using MediatR;
using Repository;
using AutoMapper;
using System.Linq;
using Common.Info;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Service {
    public class UserAppService : IUserAppService {

        private readonly IUserRepository _userRepository;
        private readonly IMediator _mediator;
        private readonly UnitOfWork _unitOfWork;
        private const bool Eager = true;
        private readonly IConfigurationProvider _cfg;
        private readonly IMapper _mapper;
        private readonly UserContext _context = null;
        private readonly ILogger<UserAppService> _logger;

        public UserAppService(
            IUserRepository userRepository,
            IMediator mediator,
            UnitOfWork unitOfWork,
            IMapper mapper,
            UserContext context,
            ILogger<UserAppService> logger) {
            _userRepository = userRepository
                ?? throw new ArgumentNullException("userRepository");
            _mediator = mediator ?? throw new ArgumentNullException("mediator");
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException("unitOfWork");
            _mapper = mapper ?? throw new ArgumentNullException("mapper");
            _context = context ?? throw new ArgumentNullException("context");
            _logger = logger ?? throw new ArgumentNullException("logger");
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

            //if (await _unitOfWork.
            //    UserRepository.IsExistsAsync(filter: q => q.Email == email)) {
            //    return null;
            //}
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

        public UserModel GetUser(string email) {
            User user = _unitOfWork.UserRepository.Get(filter: u => u.Email == email).FirstOrDefault();
            return _mapper.Map<UserModel>(user);
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
            return _unitOfWork.UserRepository.Update(userEntity);

        }
        public async Task<bool> ChangeUser(UserModel userModel) {
            try {
                User user = await _unitOfWork.UserRepository
                   .GetAsync(true, filter: u => u.Email == userModel.Email);

                var iuser = _mapper.Map<User>(userModel);
                user.FirstName = iuser.FirstName;
                user.ResetAnswer = iuser.ResetAnswer;
                _context.Entry(user.AuditedEntity).State = EntityState.Modified;
                _unitOfWork.UserRepository.Update(user);
                return true;
            } catch (Exception ex) {
                _logger.LogError("Exception: ", ex);
                return false;
            }
        }

        public PaginatedList<UserModel> GetUserPaging(string email,
        string searchString, string nameSort, int pageNumber, int pageSize) {
            try {
                IQueryable<User> query = null;

                //List<string> args
                //    = new List<string>(
                //        new[] {
                //        email,
                //        "kostas",
                //        "xristos"
                //        });

                //AlertsParameters alertsParameters =
                //new AlertsParameters.Builder()
                //.WithArguments(args)
                //.WithFromDate(DateTime.UtcNow)
                //.WithToDate(DateTime.UtcNow)
                //.build();

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
                _logger.LogError("Exception: ", ex);
                return null;
            }
        }
    }
}