using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Asn1.Ocsp;
using static Mysqlx.Error.Types;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Collections.Generic;

namespace ToDoApp.Helpers
{

    public class JwtSecurityTokenHandlerWrapper
    {
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;

        public JwtSecurityTokenHandlerWrapper()
        {
            _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        }


        // Generate a JWT token based on user ID and role
        public static string GenerateJwtToken(IConfiguration Configuration, string userId)
        {
            try
            {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
                var Sectoken = new JwtSecurityToken(Configuration["Jwt:Issuer"], Configuration["Jwt:Issuer"],

                 claims: new List<Claim>() { new Claim("id", userId), },
                  expires: DateTime.Now.AddMinutes(120),
                  signingCredentials: credentials);

                var token = new JwtSecurityTokenHandler().WriteToken(Sectoken);
                return token;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // Validate a JWT token

        public static IDictionary<string, string> ValidateJwtToken(IConfiguration Configuration, string token)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]));

            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();

                // Add logging for debugging purposes

                var claimsPrincipal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    IssuerSigningKey = securityKey,
                    ValidateAudience = true,
                    ValidAudience = Configuration["Jwt:Issuer"]
                }, out SecurityToken validatedToken);

                string userId = claimsPrincipal.FindFirst("id")?.Value ?? "0";

                Dictionary<string, string> payload =
                new Dictionary<string, string>(){
                                  {"user_id", userId},
                             };

                return payload;
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


    }

}