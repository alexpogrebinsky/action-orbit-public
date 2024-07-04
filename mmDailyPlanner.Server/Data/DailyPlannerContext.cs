using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using mmDailyPlanner.Server.DTO;
using mmDailyPlanner.Server.Models;

namespace mmDailyPlanner.Server.Data
{
    public class DailyPlannerContext : DbContext
    {
        public DailyPlannerContext(DbContextOptions<DailyPlannerContext> options)
            : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<PlannerTask> PlannerTasks { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<SessionToken> SessionTokens { get; set; }
        public DbSet<CompletedTask> CompletedTasks { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                    .SetBasePath(System.IO.Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();

                optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Username).IsRequired().HasMaxLength(256);
                entity.Property(e => e.Password).IsRequired().HasMaxLength(256);
                entity.Property(e => e.Salt).IsRequired().HasMaxLength(256);
                entity.HasIndex(e => e.Username).IsUnique();
            });

            modelBuilder.Entity<Session>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.SessionId).IsRequired().HasMaxLength(128);
                entity.Property(e => e.UserId).IsRequired();
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.Property(e => e.IsActive).IsRequired();
                entity.HasIndex(e => e.SessionId);
                entity.HasOne(e => e.User)
                      .WithMany(u => u.Sessions)
                      .HasForeignKey(e => e.UserId);

                entity.HasOne(e => e.SessionToken)
                      .WithOne(st => st.Session)
                      .HasForeignKey<Session>(s => s.SessionTokenId)
                      .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<SessionToken>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Token).IsRequired().HasMaxLength(256);
                entity.Property(e => e.UserId).IsRequired();
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.Property(e => e.ExpiresAt).IsRequired();
                entity.HasIndex(e => e.Token).IsUnique();
                entity.HasOne(e => e.User)
                      .WithMany(u => u.SessionTokens)
                      .HasForeignKey(e => e.UserId);

                entity.HasOne(e => e.Session)
                      .WithOne(s => s.SessionToken)
                      .HasForeignKey<SessionToken>(st => st.Id);
            });
        }
    }
}
