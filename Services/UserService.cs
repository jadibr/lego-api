using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace lego_api;

public class UserService(IDbContextFactory<AppDbContext> ctxFactory, IMapper mapper)
{
  private readonly IDbContextFactory<AppDbContext> ctxFactory = ctxFactory;
  private readonly IMapper mapper = mapper;
  private readonly PasswordHasher<string> passwordHasher = new();

  public async Task<User?> GetById(string userId)
  {
    if (!Guid.TryParse(userId, out var guidId))
    {
      throw new ArgumentException("Invalid GUID format.", nameof(userId));
    }

    UserEntity? userEntity;
    try
    {
      using var dbContext = await ctxFactory.CreateDbContextAsync();
      userEntity = await dbContext.Users.FirstOrDefaultAsync(u =>
        u.Id == guidId);
    }
    catch
    {
      throw new Exception($"Failed to retrieve user by id {userId}");
    }

    if (userEntity == null) return null;

    return mapper.Map<User>(userEntity);
  }

  public async Task<User?> GetByEmailAndPassword(LoginInput loginInput)
  {
    UserEntity? userEntity;

    try
    {
      using var dbContext = await ctxFactory.CreateDbContextAsync();
      userEntity = await dbContext.Users.FirstOrDefaultAsync(u =>
        u.Email.ToLower().Equals(loginInput.email.ToLower()));
    }
    catch
    {
      throw new Exception($"Failed to retrieve user by email {loginInput.email}");
    }

    if (userEntity == null || !PasswordDoesNotMatch(userEntity, loginInput.password)) return null;

    return mapper.Map<User>(userEntity);
  }

  private bool PasswordDoesNotMatch(UserEntity user, string plainPassword)
  {
    var result = passwordHasher.VerifyHashedPassword(user.Email, user.Password, plainPassword);
    return result == PasswordVerificationResult.Success;
  }
}