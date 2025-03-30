
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SurveyBasket.Api.Authentication;

public class JwtProvider : IJwtProvider
{
    public (string token, int ExpiresIn) GenerateToken(ApplicationUser user)
    {
        Claim[] claims = [
                new(JwtRegisteredClaimNames.Sub,user.Id),
                new(JwtRegisteredClaimNames.Email,user.Email),
                new(JwtRegisteredClaimNames.GivenName,user.FirstName),
                new(JwtRegisteredClaimNames.FamilyName,user.LastName),
                new(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
        ];
        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("z19AnpfklH2VYoJze2MdtmW8pGTHgw2a"));
        var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
        var expiresIn = 30;
        var expirationDate = DateTime.UtcNow.AddMinutes(expiresIn);
        var token = new JwtSecurityToken(
            issuer: "SurveyBasketApp",
            audience: "SurveyBasketAppUsers",
            claims: claims,
            expires: expirationDate,
            signingCredentials: signingCredentials
            );
        return (token: new JwtSecurityTokenHandler().WriteToken(token), ExpiresIn: expiresIn);
    }
}
