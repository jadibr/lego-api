using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace lego_api;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<BrickEntity> Bricks { get; set; }
    public DbSet<UserEntity> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<BrickEntity>()
            .Property(b => b.Id)
            .HasDefaultValueSql("NEWID()");

        modelBuilder.Entity<UserEntity>(entity =>
        {
            entity.Property(u => u.Id)
                .HasDefaultValueSql("NEWID()");

            entity.HasIndex(u => u.Email)
                .IsUnique();
        });
    }

    public static async Task SeedData(AppDbContext context)
    {
        if (context.Users.Any()) return;

        var passwordHasher = new PasswordHasher<string>();

        var email = "tester@mail.com";
        var password = passwordHasher.HashPassword(email, "1234");

        context.Users.Add(
            new UserEntity{ Email = email, Password = password }
        );
        await context.SaveChangesAsync();
    }
}