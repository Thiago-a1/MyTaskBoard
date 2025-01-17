﻿using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MyTaskBoard.API.Services;

public class TokenRepository
{
    public static object GenerateToken(string Id, string LoginType)
    {
        var key = Encoding.ASCII.GetBytes(Environment.GetEnvironmentVariable("ASPNET_SECRET_KEY")!);
        var tokenConfig = new SecurityTokenDescriptor
        {
            Subject = new System.Security.Claims.ClaimsIdentity(new Claim[]
            {
                new Claim("Id", Id),
                new Claim(ClaimTypes.Role, LoginType)
            }),
            Expires = DateTime.UtcNow.AddMinutes(5),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenConfig);
        var tokenString = tokenHandler.WriteToken(token);

        return new
        {
            token = tokenString,
        };
    }

    public static bool VerifyToken(string JWTToken)
    {
        var key = Encoding.ASCII.GetBytes(Environment.GetEnvironmentVariable("ASPNET_SECRET_KEY")!);
        var tokenHandler = new JwtSecurityTokenHandler();

        try
        {
            tokenHandler.ValidateToken(JWTToken, new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            return true;
        }
        catch 
        {
            return false;
        }
    }

    public static Guid TakeIdByToken(string JWTToken)
    {
        var token = new JwtSecurityTokenHandler().ReadJwtToken(JWTToken);

        return Guid.Parse(token.Claims.First(token => token.Type == "Id").Value);
    }
}
