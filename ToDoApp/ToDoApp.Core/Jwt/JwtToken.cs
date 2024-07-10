using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ToDoApp.Core.Jwt;

public class JwtToken(JwtOptions jwtOptions)
{
    private readonly JwtOptions _jwtOptions = jwtOptions;
    private readonly ClaimsIdentity _claimsIdentity;
    private readonly JwtSecurityTokenHandler _tokenHandler = new JwtSecurityTokenHandler();

    public string generateJwtToken(string userId)
    {
        try
        {
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _jwtOptions!.Issuer,
                Audience = _jwtOptions.Audience,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SigningKey)),
                SecurityAlgorithms.HmacSha256),
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new (ClaimTypes.NameIdentifier , userId)
                })
            };

            var securtyToken = _tokenHandler.CreateToken(tokenDescriptor);
            string accessToken = _tokenHandler.WriteToken(securtyToken);

            return accessToken;
        }
        catch
        {
            throw new Exception("there is problem in generated Token");
        }
    }



    public ClaimsPrincipal ValidateJwtToken(string token , out SecurityToken OutvalidatedToken)
    {

        try
        {
            var claimsPrincipal = _tokenHandler.ValidateToken
                (token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _jwtOptions!.Issuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SigningKey)),
                    ValidateAudience = true,
                    ValidAudience = _jwtOptions?.Audience,
                }, out SecurityToken validatedToken);
            OutvalidatedToken = validatedToken;

            string userId = claimsPrincipal.FindFirst("Id")?.Value!;


            return claimsPrincipal;
        }
        catch (SecurityTokenExpiredException)
        {
            throw new ApplicationException("Token has expired.");
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    //public int GetUserId(HttpRequest requestHeader)
    //{
    //    var token = requestHeader.Headers["Authorization"].ToString().Replace("Bearer ", "");
    //    var payload = ValidateJwtToken(token);
    //    int user_id = short.Parse(payload["User"]);
    //    return user_id;
    //}
}
