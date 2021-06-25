namespace BattleCards.Data
{
    using BattleCards.Models;
    using Microsoft.EntityFrameworkCore;

    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Card> Cards { get; set; }

        public DbSet<UserCard> UserCards { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseSqlServer(DatabaseConfiguration.ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserCard>()
                .HasKey(x => new { x.UserId, x.CardId });

            modelBuilder.Entity<UserCard>()
                 .HasOne(x => x.User)
                 .WithMany(u => u.UserCard)
                 .HasForeignKey(x => x.UserId);

            modelBuilder.Entity<UserCard>()
                .HasOne(x => x.Card)
                .WithMany(x => x.UserCards)
                .HasForeignKey(x => x.CardId);

        }
    }
}
