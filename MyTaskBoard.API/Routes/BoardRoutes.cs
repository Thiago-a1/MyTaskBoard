using Microsoft.AspNetCore.Http.Headers;
using MyTaskBoard.API.DTOs.Requests;
using MyTaskBoard.API.DTOs.Responses;
using MyTaskBoard.API.Models;
using MyTaskBoard.API.Services;
using MyTaskBoard.Domain.Entities;
using Org.BouncyCastle.Asn1.Ocsp;

namespace MyTaskBoard.API.Routes;

public static class BoardRoutes
{
    public static void MapBoardRoutes(this WebApplication app)
    {
        app.MapGet("/Board", async (HttpRequest requestHeaders, IBoardRepository _service) => {

            string Token = requestHeaders.Headers.Authorization.ToString()["Bearer ".Length..];

            var response = await _service.GetAllBoardsByUserId(Token);

            return Results.Ok(response);
        })
            .RequireAuthorization("user")
            .Produces<ResponseModel<GetBoardResponse>>(StatusCodes.Status200OK)
            .WithTags("Boards")
            .WithSummary("Retorna todos os quadros do usuário logado.");

        app.MapPost("/Board", async (HttpRequest requestHeaders, CreateBoardRequest request, IBoardRepository _service) =>
        {
            string Token = requestHeaders.Headers.Authorization.ToString()["Bearer ".Length..];

            var response = await _service.CreateBoard(Token, request);

            return response.Status ? Results.Created(string.Empty, response) : Results.BadRequest();
        })
            .RequireAuthorization("user")
            .Produces<ResponseModel<CreateBoardResponse>>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .WithTags("Boards")
            .WithSummary("Cria um novo quadro para o usuário logado.");

        app.MapPatch("/Board/{boardId}", async (HttpRequest requestHeaders, int boardId, UpdateBoardRequest request, IBoardRepository _service) =>
        {
            string Token = requestHeaders.Headers.Authorization.ToString()["Bearer ".Length..];

            var response = await _service.UpdateBoard(Token, boardId, request);

            return response.Status ? Results.Ok(response) : Results.BadRequest();
        })
            .RequireAuthorization("user")
            .Produces<ResponseModel<UpdateBoardResponse>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .WithTags("Boards")
            .WithSummary("Atualiza um quadro com o Id passado e compativel com o usuário logado.");

        app.MapDelete("/Board/{boardId}", async (HttpRequest requestHeaders, int boardId, IBoardRepository _service) =>
        {
            string Token = requestHeaders.Headers.Authorization.ToString()["Bearer ".Length..];

            var response = await _service.DeleteBoard(Token, boardId);

            return response.Status ? Results.Accepted(string.Empty, response) : Results.BadRequest();
        })
            .RequireAuthorization("user")
            .Produces<ResponseModel<string>>(StatusCodes.Status202Accepted)
            .Produces(StatusCodes.Status400BadRequest)
            .WithTags("Boards")
            .WithSummary("Deleta o quadro com o Id passado e compativel com o usuário logado.");
    }
}
