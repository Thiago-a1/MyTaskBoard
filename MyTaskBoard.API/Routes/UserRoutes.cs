using MiniValidation;
using MyTaskBoard.API.DTOs.Requests;
using MyTaskBoard.API.DTOs.Responses;
using MyTaskBoard.API.Models;
using MyTaskBoard.API.Services;

namespace MyTaskBoard.API.Routes;

public static class UserRoutes
{
    public static void MapUserRoutes(this WebApplication app)
    {
        app.MapGet("/User", async (IUserRepository _service) => {

            var response = await _service.GetAllUser();

            return Results.Ok(response);
        })
            .Produces<ResponseModel<GetUserResponse>>(StatusCodes.Status200OK)
            .WithTags("User")
            .WithSummary("Retorna todos os usuários");

        app.MapGet("/User/{Id}", async (Guid Id, IUserRepository _service) => {

            var response = await _service.GetUserById(Id);

            return response.Status == true ? Results.Ok(response) : Results.BadRequest(response);
        })
            .Produces<ResponseModel<GetUserResponse>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .WithTags("User")
            .WithSummary("Retorna um usuario pelo ID");

        app.MapPost("/User", async (CreateUserRequest request, IUserRepository _service) => {

            if (!MiniValidator.TryValidate(request, out var errors))
            {
                return Results.ValidationProblem(errors);
            }

            var response = await _service.CreateUser(request);

            return response.Status == true ? Results.Created(string.Empty, response) : Results.BadRequest(response);
        })
            .Produces<ResponseModel<CreateUserResponse>>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .WithTags("User")
            .WithSummary("Cria um novo usuario.");

        app.MapDelete("/User", async (HttpRequest requestHeaders, IUserRepository _service) =>
        {
            string Token = requestHeaders.Headers.Authorization.ToString()["Bearer ".Length..];

            var response = await _service.DeleteUser(Token);

            return response.Status == true ? Results.Accepted(string.Empty, response) : Results.BadRequest(response);
        })
            .RequireAuthorization("user")
            .Produces<ResponseModel<string>>(StatusCodes.Status202Accepted)
            .Produces(StatusCodes.Status400BadRequest)
            .WithTags("User")
            .WithSummary("Exclui o usuario logado.");
    }
}
