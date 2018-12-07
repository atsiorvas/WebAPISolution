using Common.Interface;
using System;
using System.Threading.Tasks;

namespace Common.Interface {

    [Obsolete("UserRepository-Change with Generic Repo", false)]
    public interface IUserRepository : IRepository<User> {

        Task<UserModel> SaveUserAsync(UserModel user);

        Task<UserModel> GetUserAsync(string email);

        Task<UserModel> GetUserWithNotesAsync(string email);

        Task<bool> ModifyUserByAsync(UserModel userToDelete);

        Task<bool> DeleteUserAsync(string email);

        Task<bool> IsUserExistsAsync(string email);
    }
}

