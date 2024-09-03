using Microsoft.OpenApi.Models;

namespace MyTaskBoard.API.Services;

public static class SwaggerService
{
    public static void AddSwaggerService(this IServiceCollection service)
    {
        service.Swagger();
    }

    public static void Swagger(this IServiceCollection service)
    {
        service.AddEndpointsApiExplorer();
        service.AddSwaggerGen(conn =>
        {
            conn.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "MyTaskBoard.API",
                Contact = new OpenApiContact
                {
                    Name = "Thiago Araujo",
                    Email = "Thiago.araujo.r11@gmail.com"
                },
                Version = "v1",
            });

            conn.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Name = "Authorization",
                In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            conn.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header
                    },
                    new List<string>()
                }
            });
        });
    }
}
