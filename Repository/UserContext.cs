using Microsoft.EntityFrameworkCore;
using Common;
using MediatR;
using System.Threading.Tasks;
using System;
using Common.Interface;

namespace Repository {

    [System.Data.Entity.DbConfigurationType(typeof(MyDbConfiguration))]
    public class UserContext : DbContext, ISaveChangesWarper {
        public UserContext(
            DbContextOptions<UserContext> options)
            : base(options) { }

        public UserContext(DbContextOptions<UserContext> options, IMediator mediator)
            : base(options) {

            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

        }

        public UserContext() {
        }

        public virtual DbSet<User>
            User { get; set; }
        public virtual DbSet<Notes>
            Notes { get; set; }

        private readonly IMediator _mediator;

        public async Task<bool> SaveChangesAsync() {
            //await _mediator.DispatchDomainEventsAsync(this);
            var result = await base.SaveChangesAsync();

            return true;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new NoteConfiguration());

        }
    }
}