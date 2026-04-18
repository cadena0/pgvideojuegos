using Microsoft.EntityFrameworkCore;
using PaginaVideojuegos.Models;

namespace PaginaVideojuegos.Data;

public class MysqlDbContext : DbContext
{
    public MysqlDbContext(DbContextOptions<MysqlDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<GameEntry> GameEntries { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasIndex(u => u.Email).IsUnique();
            entity.HasIndex(u => u.Username).IsUnique();
        });

        modelBuilder.Entity<GameEntry>(entity =>
        {
            entity.HasOne(g => g.User)
                  .WithMany(u => u.GameEntries)
                  .HasForeignKey(g => g.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
