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

        modelBuilder.Entity<UserEntity>(entity =>
        {
            entity.HasKey(u => u.Id);

            entity.Property(u => u.Id)
                .HasDefaultValueSql("NEWID()");

            entity.HasIndex(u => u.Email)
                .IsUnique();

            entity.Property(u => u.Email)
                .IsRequired();

            entity.Property(u => u.Password)
                .IsRequired();
        });

        modelBuilder.Entity<BrickEntity>(entity =>
        {
            entity.HasKey(b => b.Id);

            entity.Property(b => b.Id)
                .HasDefaultValueSql("NEWID()");

            entity.Property(b => b.PartNumber)
                .IsRequired();

            entity.Property(b => b.Color)
                .IsRequired();

            entity.Property(b => b.InStockCount)
                .IsRequired();
        });

        modelBuilder.Entity<SetEntity>(entity =>
        {
            entity.HasKey(s => s.Id);

            entity.Property(u => u.Id)
                .HasDefaultValueSql("NEWID()");
        });

        modelBuilder.Entity<SetEntity>()
            .HasMany(s => s.Bricks)
            .WithMany(b => b.Sets)
            .UsingEntity<SetBrickEntity>(
                j => j.HasOne(sb => sb.Brick)
                    .WithMany()
                    .HasForeignKey(sb => sb.BrickId),
                j => j.HasOne(sb => sb.Set)
                    .WithMany()
                    .HasForeignKey(sb => sb.SetId),
                j =>
                {
                    j.HasKey(sb => new { sb.SetId, sb.BrickId });
                });
    }

    public static async Task SeedData(AppDbContext context)
    {
        if (context.Users.Any()) return;

        var passwordHasher = new PasswordHasher<string>();

        var email = "tester@mail.com";
        var password = passwordHasher.HashPassword(email, "1234");

        context.Users.Add(
            new UserEntity { Email = email, Password = password }
        );
        await context.SaveChangesAsync();
    }
}