using Microsoft.EntityFrameworkCore;
using Common;
using MediatR;
using System.Threading.Tasks;
using System;
using Common.Interface;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore.Design;

namespace Repository {

    [System.Data.Entity
        .DbConfigurationType(typeof(MyDbConfiguration))]
    public class UserContext
        : DbContext, ISaveChangesWarper {

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

    public class UserDbContextFactory
        : IDesignTimeDbContextFactory<UserContext> {

        private IConfigurationRoot _configuration;

        public UserDbContextFactory() {
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            _configuration = builder.Build();
        }

        public UserContext CreateDbContext(string[] args) {
            var optionsBuilder =
                new DbContextOptionsBuilder<UserContext>();
            optionsBuilder
                .UseSqlServer(
                _configuration.GetConnectionString("ConnectionDB"),
                    m => { m.EnableRetryOnFailure(); });

            return new UserContext(optionsBuilder.Options);
        }
    }
}