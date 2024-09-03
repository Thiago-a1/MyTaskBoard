using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace MyTaskBoard.TokenService;
public static class TokenService
{
    public static void AddServiceToken(this IServiceCollection service, IConfiguration configuration)
    {
        service.AddServiceAuthenticator(configuration);
        service.AddServiceAuthorization();
    }

    public static void AddServiceAuthenticator(this IServiceCollection services, IConfiguration configuration)
    {
        var key = Encoding.ASCII.GetBytes(Environment.GetEnvironmentVariable("ASPNET_SECRET_KEY")!);
        services.AddAuthentication(option =>
        {
            option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(option =>
        {
            option.RequireHttpsMetadata = false;
            option.SaveToken = true;
            option.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = false,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false
            };
        });
    }

    public static void AddServiceAuthorization(this IServiceCollection services)
    {
        services.AddAuthorization(option =>
        {
            option.AddPolicy("user", policy => policy.RequireRole("user"));
        });
    }

    public static void UseServiceApplicationAuth(this IApplicationBuilder app)
    {
        app.UseAuthentication();
        app.UseAuthorization();
    }
}
