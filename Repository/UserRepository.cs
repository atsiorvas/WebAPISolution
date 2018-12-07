using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common;
using Common.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Repository {
    [Obsolete("UserRepository-Change with Generic Repo", false)]
    public class UserRepository : IUserRepository {

        private readonly UserContext _context;
        private readonly IConfigurationProvider _cfg;
        private readonly IMapper _mapper;

        public ISaveChangesWarper SaveChangesWarper {

            get {
                return _context;
            }
        }

        public UserRepository(UserContext context, IConfigurationProvider cfg,
            IMapper mapper) {
            _context = context ?? throw new ArgumentNullException();
            _cfg = cfg ?? throw new ArgumentNullException();
            _mapper = mapper ?? throw new ArgumentNullException();
        }

        public async Task<bool> IsUserExistsAsync(string email) {
            return await _context.User.AnyAsync(user => user.Email == email);
        }


        public async Task<UserModel> SaveUserAsync(UserModel userModel) {
            try {

                var user = _mapper.Map<User>(userModel);

                var userToSave = _context.User.Add(user).Entity;
                //update db
                await _context.SaveChangesAsync();

                return userModel;

            } catch (Exception ex) {
                throw ex;
            }
        }

        public async Task<UserModel> GetUserWithNotesAsync(string email) {
            try {

                User userExt =
                    await _context.User
                   .Where(user => user.Email == email)
                   .Include(x => x.Note)
                   .FirstOrDefaultAsync();

                return _mapper.Map<UserModel>(userExt);
            } catch (Exception ex) {
                throw ex;
            }
        }

        public async Task<UserModel> GetUserAsync(string email) {
            try {

                UserModel user = await _context.User
                    .Where(u => u.Email == email)
                    .ProjectTo<UserModel>(_cfg)
                    .FirstOrDefaultAsync();
                user.Password = null;
                return user;
            } catch (Exception ex) {
                throw ex;
            }
        }



        public async Task<bool> ModifyUserByAsync(UserModel userToModify) {
            var user = _mapper.Map<User>(userToModify);

            try {
                User userExist = await _context.User.SingleOrDefaultAsync(x => x.Email == user.Email);

                if (userExist != null && user != userExist) {

                    var userToBeSave = new User {
                        Email = user.Email,
                        Password = user.Password
                    };
                    _context.Entry(userToBeSave).State = EntityState.Modified;

                    await _context.SaveChangesAsync();

                    return true;
                }
                return false;
            } catch (Exception exception) {
                throw exception;
            }
        }

        public async Task<bool> DeleteUserAsync(string email) {

            try {
                User userExist = await _context.User.SingleOrDefaultAsync(x => x.Email == email);
                if (userExist != null) {

                    _context.Remove(userExist);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            } catch (Exception ex) {
                throw ex;
            }
        }

    }
}