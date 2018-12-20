using Microsoft.EntityFrameworkCore;
using Common;
using MediatR;
using System.Threading.Tasks;
using System;
using Common.Interface;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore.Design;
using Entity = System.Data.Entity;
using System.Threading;
using System.Linq;
using System.IO;
using System.Globalization;

namespace Repository {

    [Entity
        .DbConfigurationType(
            typeof(MyDbConfiguration)
        )
    ]
    public class UserContext
        : DbContext, ISaveChangesWarper {

        private const string DateTimeFormatConst = "yyyy-MM-dd";


        private readonly IMediator _mediator;

        public virtual DbSet<User>
           User { get; set; }

        public virtual DbSet<Notes>
            Notes { get; set; }

        //ctro #1 -- default ctro 
        public UserContext() { }


        //ctro #2
        public UserContext(
            DbContextOptions<UserContext> options)
            : base(options) { }

        //ctro #3
        public UserContext(DbContextOptions<UserContext> options,
            IMediator mediator)
            : base(options) {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

        }

        //public async Task<bool> SaveChangesAsync() {
        //    //await _mediator.DispatchDomainEventsAsync(this);
        //    var result = await base.SaveChangesAsync();

        //    return true;
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new NoteConfiguration());
            //modelBuilder.ApplyConfiguration(new AuditedEntityConfiguration());
        }
        public override async Task<int> SaveChangesAsync(
            CancellationToken cancellationToken = default(CancellationToken)) {

            DateTime dt = DateTime.UtcNow;

            this.ChangeTracker.DetectChanges();
            var changes = this.ChangeTracker.Entries();

            //Added audit
            var added = changes
                .Where(t => t.State == EntityState.Added)
                .Select(t => t.Entity)
                .ToArray();

            foreach (var entity in added) {

                var entityName = entity.GetType();

                if (entityName == typeof(User)) {
                    var track = entity as User;
                    track.AuditedEntity = new AuditedEntity {
                        CreatedOn = dt,
                        CreatedBy = track.Email
                    };
                } else {
                    var track = entity as Notes;
                    track.AuditedEntity = new AuditedEntity {
                        CreatedOn = dt,
                        CreatedBy = track.User.Email
                    };
                }
            }

            //Modified audit
            var updated = this.ChangeTracker.Entries()
               .Where(t => t.State == EntityState.Modified)
               .Select(t => t.Entity)
               .ToArray();

            foreach (var entity in updated) {
                var entityName = entity.GetType();

                if (entityName == typeof(User)) {
                    var track = entity as User;
                    track.AuditedEntity.UpdatedOn = dt;
                } else {
                    var track = entity as Notes;
                    track.AuditedEntity.UpdatedOn = dt;
                }
                //if (entityName == typeof(Notes)) {
                //    var track = entity as Notes;
                //    if (track.AuditedEntity == null) {
                //        track.AuditedEntity = new AuditedEntity() {
                //            CreatedOn = dt,
                //            CreatedBy = track.User.Email
                //        };
                //    }
                //    track.User.AuditedEntity.UpdatedOn = dt;
                //}
            }

            //return base.SaveChanges
            return (await base.SaveChangesAsync(true, cancellationToken));
        }
    }

    public class UserDbContextFactory
        : IDesignTimeDbContextFactory<UserContext> {

        private IConfigurationRoot _configuration;

        public UserDbContextFactory() {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("C:\\Users\\atsio\\source\\repos\\Solution1\\UserService\\appsettings.json",
                optional: false, reloadOnChange: true);

            _configuration = builder.Build();
        }

        public UserContext CreateDbContext(string[] args) {
            var optionsBuilder =
                new DbContextOptionsBuilder<UserContext>();
            optionsBuilder
                .UseSqlServer(
                "Server=(localdb)\\MSSQLLocalDB;Database=UsersContext;ConnectRetryCount=0;MultipleActiveResultSets=True;",
                    m => { m.EnableRetryOnFailure(); });

            return new UserContext(optionsBuilder.Options);
        }
    }
}