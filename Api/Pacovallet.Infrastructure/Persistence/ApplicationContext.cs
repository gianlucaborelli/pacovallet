using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Pacovallet.Domain.Entities;
using Pacovallet.Application.Ports;
using Pacovallet.Core.Models;

namespace Pacovallet.Infrastructure.Persistence
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

            ConfigureEntityMappings(modelBuilder);

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

        private void ConfigureEntityMappings(ModelBuilder modelBuilder)
        {
            // Person configuration
            modelBuilder.Entity<Person>(entity =>
            {
                entity.ToTable("Persons");
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Id).ValueGeneratedNever();
                entity.Property(p => p.Name).IsRequired().HasMaxLength(200);
                entity.Property(p => p.BirthDate).IsRequired();
            });

            // Category configuration  
            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Categories");
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Id).ValueGeneratedNever();
                entity.Property(c => c.Description).IsRequired().HasMaxLength(200);
                entity.Property(c => c.Purpose).IsRequired();
            });

            // Transaction configuration
            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.ToTable("Transactions");
                entity.HasKey(t => t.Id);
                entity.Property(t => t.Id).ValueGeneratedNever();
                entity.Property(t => t.Description).HasMaxLength(500);
                entity.Property(t => t.Amount).HasPrecision(18, 2).IsRequired();

                entity.HasOne(t => t.Category)
                    .WithMany()
                    .HasForeignKey(t => t.CategoryId);

                entity.HasOne(t => t.Person)
                    .WithMany()
                    .HasForeignKey(t => t.PersonId);

                entity.HasOne(t => t.ParentTransaction)
                    .WithMany(t => t.ChildTransactions)
                    .HasForeignKey(t => t.ParentTransactionId);
            });


        }

        public override int SaveChanges()
        {
            SetUserId();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
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