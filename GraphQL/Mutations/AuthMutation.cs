namespace lego_api;

public class AuthMutation(AuthService authService)
{
  public async Task<Tokens> Login(LoginInput loginInput)
  {
    return await authService.GenerateTokens(loginInput);
  }

  public async Task<Tokens> Refresh(RefreshTokenInput refreshTokenInput)
  {
    return await authService.RefreshToken(refreshTokenInput);
  }
}