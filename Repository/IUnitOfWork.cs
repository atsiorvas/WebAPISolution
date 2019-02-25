using Common;
using System;

namespace Repository {
    public interface IUnitOfWork : IDisposable {

        bool Exists<T>(T entity) where T : Entity;

        GenericRepository<User> UserRepository { get; }
        GenericRepository<Notes> NoteRepository { get; }
    }
}