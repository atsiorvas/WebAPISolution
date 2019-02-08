using Microsoft.EntityFrameworkCore;
using Common;
using MediatR;
using System.Threading.Tasks;
using System;
using Common.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore.Design;
using Entity = System.Data.Entity;
using System.Threading;
using System.IO;
using Common.Data;
using System.Linq;

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

        public virtual DbSet<Alert>
            Alert { get; set; }

        public virtual DbSet<Order>
            Order { get; set; }

        public virtual DbSet<OrderAlert>
            OrderAlert { get; set; }

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

        protected override void OnModelCreating(ModelBuilder modelBuilder) {


            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new NoteConfiguration());
            modelBuilder.ApplyConfiguration(new AlertEntityConfiguration());
            modelBuilder.ApplyConfiguration(new OrderConfiguration());
            modelBuilder.ApplyConfiguration(new OrderAlertConfiguration());

            foreach (
                var relationship in modelBuilder.Model.GetEntityTypes()
                .SelectMany(e => e.GetForeignKeys())
                ) {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
            base.OnModelCreating(modelBuilder);
        }
        public override async Task<int> SaveChangesAsync(
            CancellationToken cancellationToken = default(CancellationToken)) {

            //Audit
            this.ApplyAuditInformation();

            //return base.SaveChanges
            return (await base.SaveChangesAsync(true, cancellationToken));
        }

        private void ApplyAuditInformation() {

            var entries = ChangeTracker.Entries();

            foreach (var entry in entries) {

                if (entry.Entity is ITrack track) {

                    switch (entry.State) {
                        case EntityState.Added:
                            track.CreatedOn = DateTime.UtcNow;
                            track.CreatedBy = "Admin";

                            break;
                        case EntityState.Modified:
                            track.UpdatedOn = DateTime.UtcNow;
                            break;
                    }
                }
            }
        }
    }

    public class UserDbContextFactory
        : IDesignTimeDbContextFactory<UserContext> {

        private readonly IConfigurationRoot _configuration;

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