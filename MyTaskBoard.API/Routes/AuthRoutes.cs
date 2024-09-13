using MiniValidation;
using MyTaskBoard.API.DTOs.Requests;
using MyTaskBoard.API.Models;
using MyTaskBoard.API.Services;
using Org.BouncyCastle.Asn1.Ocsp;
using ZstdSharp.Unsafe;

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

        app.MapPost("/Auth/VerifyToken", (VerifyTokenRequest request, ILoginService _service) =>
        {
            if (!MiniValidator.TryValidate(request, out var errors))
            {
                return Results.ValidationProblem(errors);
            }

            var response = _service.VerifyToken(request);

            return Results.Ok(response);
        })
            .Produces<ResponseModel<object>>(StatusCodes.Status200OK)
            .WithTags("Auth")
            .WithSummary("Verifica validade e a assinatura do JWTToken enviado pelo Front-end.");
    }
}
