using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Pacovallet.Api.Data.Mappings;
using Pacovallet.Api.Models;
using Pacovallet.Core.Models;

namespace Pacovallet.Api.Data
{
    public class ApplicationContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {
        private readonly ICurrentUser? _currentUser;
        public Guid? CurrentUserId => _currentUser?.UserId;

        public ApplicationContext(
        DbContextOptions<ApplicationContext> options,
        ICurrentUser? currentUser = null)
        : base(options)
        {
            _currentUser = currentUser;
            ConfigureChangeTracker();
        }

        private void ConfigureChangeTracker()
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            ChangeTracker.AutoDetectChangesEnabled = false;
            ChangeTracker.LazyLoadingEnabled = false;
        }

        public DbSet<User> AppUsers { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new PersonMap());
            modelBuilder.ApplyConfiguration(new CategoryMap());
            modelBuilder.ApplyConfiguration(new TransactionMap());

            modelBuilder.Entity<Person>()
                .HasQueryFilter(e => e.UserId == CurrentUserId);
            modelBuilder.Entity<Category>()
                .HasQueryFilter(e => e.UserId == CurrentUserId);
            modelBuilder.Entity<Transaction>()
                .HasQueryFilter(e => e.UserId == CurrentUserId);

            modelBuilder.HasDefaultSchema("Pacovallet");

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable(name: "User");
            });
            modelBuilder.Entity<IdentityRole<Guid>>(entity =>
            {
                entity.ToTable(name: "Role");
            });
            modelBuilder.Entity<IdentityUserRole<Guid>>(entity =>
            {
                entity.ToTable("UserRoles");
            });
            modelBuilder.Entity<IdentityUserClaim<Guid>>(entity =>
            {
                entity.ToTable("UserClaims");
            });
            modelBuilder.Entity<IdentityUserLogin<Guid>>(entity =>
            {
                entity.ToTable("UserLogins");
            });
            modelBuilder.Entity<IdentityRoleClaim<Guid>>(entity =>
            {
                entity.ToTable("RoleClaims");
            });
            modelBuilder.Entity<IdentityUserToken<Guid>>(entity =>
            {
                entity.ToTable("UserTokens");
            });
        }

        public override int SaveChanges()
        {
            SetUserId();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(
            CancellationToken cancellationToken = default)
        {
            SetUserId();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void SetUserId()
        {
            if (_currentUser == null)
                return;

            foreach (var entry in ChangeTracker.Entries<Entity>())
            {
                if (entry.State == EntityState.Added)
                {
                    if (CurrentUserId != null)
                        entry.Entity.UserId = CurrentUserId.Value;
                }
            }
        }
    }
}
