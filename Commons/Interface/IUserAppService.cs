using Common;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Common.Interface {
    public interface IUserAppService {

        Task<UserModel> RegisterAsync(UserModel userRegister);
        Task<UserModel> LoginAsync(LoginModel userLogin);
        Task<UserModel> GetUserAsync(string userLogin);
        Task<UserModel> GetUserWithNotesAsync(string email);
        Task<UserModel> SaveUserAsync(UserModel userModel);
        Task<bool> RemoveUserAsync(int id);
        Task<int> FindIdByBkAsync(string bk);
    }
}