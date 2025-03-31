using System.IdentityModel.Tokens.Jwt;

namespace SurveyBasket.Api.Authentication;

public interface IJwtProvider
{
    (string token, int ExpiresIn) GenerateToken(ApplicationUser user);
    string? ValidateToken(string token);

}