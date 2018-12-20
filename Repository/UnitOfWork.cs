using Common;
using System.Threading.Tasks;
using System;
using Common.Interface;
using AutoMapper;
using System.Collections;
using System.Threading;

namespace Repository {
    public class UnitOfWork : ISaveChangesWarper {

        private readonly UserContext context;
        private GenericRepository<User> userRepository;
        private GenericRepository<Notes> noteRepository;
        private readonly IMapper mapper;

        public UnitOfWork(UserContext context,
            IMapper mapper) {
            this.context = context;
            this.mapper = mapper;
        }

        ~UnitOfWork() {
            // Finalizer calls Dispose(false)
            Dispose(false);
        }

        public GenericRepository<User> UserRepository {
            get {
                if (this.userRepository == null) {
                    this.userRepository =
                        new GenericRepository<User>(context, mapper);
                }
                return userRepository;
            }
        }

        public GenericRepository<Notes> NoteRepository {
            get {
                if (this.noteRepository == null) {
                    this.noteRepository =
                        new GenericRepository<Notes>(context, mapper);
                }
                return noteRepository;
            }
        }

        public async Task<int> SaveChangesAsync(
            CancellationToken cancellationToken
            = default(CancellationToken)
            ) {
            return await context.SaveChangesAsync(cancellationToken);
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing) {
            if (!this.disposed) {
                if (disposing) {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);

        }
    }
}