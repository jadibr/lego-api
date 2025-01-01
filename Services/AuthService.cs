using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace lego_api;

public class AuthService(UserService userService)
{
  public async Task<Tokens> GenerateTokens(LoginInput loginInput)
  {
    var user = await userService.GetByEmailAndPassword(loginInput) ??
      throw new GraphQLException("Invalid credentials");

    var accessToken = GenerateAccessToken(user.Id.ToString());
    var refreshToken = GenerateRefreshToken(user.Id.ToString());

    return new Tokens
    {
      AccessToken = accessToken,
      RefreshToken = refreshToken
    };
  }

  public async Task<Tokens> RefreshToken(RefreshTokenInput refreshTokenInput)
  {
    string tokenSecret = Environment.GetEnvironmentVariable("REFRESH_TOKEN_SECRET") ??
      throw new InvalidOperationException("Missing refresh_token_secret env variable");

    string userId;

    try
    {
      var tokenHandler = new JwtSecurityTokenHandler();
      var key = Encoding.ASCII.GetBytes(tokenSecret);

      var tokenValidationParameters = new TokenValidationParameters
      {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
      };

      ClaimsPrincipal principal = tokenHandler.ValidateToken(
          refreshTokenInput.refreshToken,
          tokenValidationParameters,
          out SecurityToken validatedToken);

      var userIdClaim = principal.FindFirst(JwtRegisteredClaimNames.Sub) ??
        throw new SecurityTokenException("Token does not contain user id claim");

      userId = userIdClaim.Value;
    }
    catch (Exception)
    {
      throw new SecurityTokenException("Invalid refresh token");
    }

    if (await userService.GetById(userId) == null) throw new GraphQLException("User for which the token was generated does not exist");

    return new Tokens
    {
      AccessToken = GenerateAccessToken(userId)
    };
  }

  private static string GenerateAccessToken(string userId)
  {
    string tokenSecret = Environment.GetEnvironmentVariable("ACCESS_TOKEN_SECRET") ??
      throw new InvalidOperationException("Missing access_token_secret env variable");

    var tokenHandler = new JwtSecurityTokenHandler();
    var key = Encoding.ASCII.GetBytes(tokenSecret);

    var tokenDescriptor = new SecurityTokenDescriptor
    {
      Subject = new ClaimsIdentity(
      [
        new Claim(JwtRegisteredClaimNames.Sub, userId)
      ]),
      Expires = DateTime.UtcNow.AddMinutes(30),
      SigningCredentials = new SigningCredentials(
        new SymmetricSecurityKey(key),
        SecurityAlgorithms.HmacSha256Signature
      )
    };

    var token = tokenHandler.CreateToken(tokenDescriptor);
    return tokenHandler.WriteToken(token);
  }

  private static string GenerateRefreshToken(string userId)
  {
    string tokenSecret = Environment.GetEnvironmentVariable("REFRESH_TOKEN_SECRET") ??
      throw new InvalidOperationException("Missing refresh_token_secret env variable");

    var tokenHandler = new JwtSecurityTokenHandler();
    var key = Encoding.ASCII.GetBytes(tokenSecret);

    var tokenDescriptor = new SecurityTokenDescriptor
    {
      Subject = new ClaimsIdentity(
      [
        new Claim(JwtRegisteredClaimNames.Sub, userId)
      ]),
      Expires = DateTime.UtcNow.AddDays(30),
      SigningCredentials = new SigningCredentials(
        new SymmetricSecurityKey(key),
        SecurityAlgorithms.HmacSha256Signature
      )
    };

    var token = tokenHandler.CreateToken(tokenDescriptor);
    return tokenHandler.WriteToken(token);
  }

}