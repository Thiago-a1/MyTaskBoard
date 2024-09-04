using MiniValidation;
using MyTaskBoard.API.DTOs.Requests;
using MyTaskBoard.API.Models;
using MyTaskBoard.API.Services;

namespace MyTaskBoard.API.Routes;

public static class AuthRoutes
{
    public static void MapAuthRoutes(this WebApplication app)
    {
        app.MapPost("/Auth", async (LoginRequest request, ILoginService _service) =>
        {
            if (!MiniValidator.TryValidate(request, out var errors))
            {
                return Results.ValidationProblem(errors);
            }

            var response = await _service.LoginUser(request);

            return response.Status ? Results.Ok(response) : Results.BadRequest(response);
        })
            .Produces<ResponseModel<Object>>(StatusCodes.Status200OK)
            .Produces<ResponseModel<Object>>(StatusCodes.Status400BadRequest)
            .WithTags("Auth")
            .WithSummary("Realiza o login do usuário e retorna um JWTToken.");
    }
}
