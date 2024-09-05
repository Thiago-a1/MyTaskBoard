using MiniValidation;
using MyTaskBoard.API.DTOs.Requests;
using MyTaskBoard.API.DTOs.Responses;
using MyTaskBoard.API.Models;
using MyTaskBoard.API.Services;

namespace MyTaskBoard.API.Routes;

public static class TaskRoutes
{
    public static void MapTaskRoutes(this WebApplication app)
    {
        app.MapGet("/Board/{boardId}/Task", async (int boardId, ITaskRepository _service) => {

            var response = await _service.GetAllTaskByBoardId(boardId);

            return Results.Ok(response);
        })
            .RequireAuthorization("user")
            .Produces<ResponseModel<GetTaskResponse>>(StatusCodes.Status200OK)
            .WithTags("Task")
            .WithSummary("Retorna todas as Tarefas do quadro com id passado.");

        app.MapGet("/Board/Task/{taskId}", async (int taskId, ITaskRepository _service) => {

            var response = await _service.GetTaskById(taskId);

            return Results.Ok(response);
        })
            .RequireAuthorization("user")
            .Produces<ResponseModel<GetTaskResponse>>(StatusCodes.Status200OK)
            .WithTags("Task")
            .WithSummary("Retorna uma Tarefa pelo id passado.");

        app.MapPost("/Board/{boardId}/Task", async (int boardId, CreateTaskRequest request, ITaskRepository _service) => {

            if (!MiniValidator.TryValidate(request, out var errors))
            {
                return Results.ValidationProblem(errors);
            }

            var response = await _service.CreateTask(boardId, request);

            return response.Status ? Results.Created(string.Empty, response) : Results.BadRequest(response);
        })
            .RequireAuthorization("user")
            .Produces<ResponseModel<CreateTaskResponse>>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .WithTags("Task")
            .WithSummary("Cria uma nova Tarefa no Quadro passado por Id e o retorna.");

        app.MapPatch("/Board/{boardId}/Task/{taskId}", async (int taskId, int boardId, UpdateTaskRequest request, ITaskRepository _service) => {

            if (!MiniValidator.TryValidate(request, out var errors))
            {
                return Results.ValidationProblem(errors);
            }

            var response = await _service.UpdateTask(taskId, boardId, request);

            return response.Status ? Results.Ok(response) : Results.BadRequest(response);
        })
            .RequireAuthorization("user")
            .Produces<ResponseModel<UpdateTaskResponse>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .WithTags("Task")
            .WithSummary("Atualiza uma Tarefa de acordo com Id e no Quadro passado por Id.");

        app.MapDelete("/Board/{boardId}/Task/{taskId}", async (int taskId, int boardId, ITaskRepository _service) => {

            var response = await _service.DeleteTask(taskId, boardId);

            return response.Status ? Results.Accepted(string.Empty, response) : Results.BadRequest(response);
        })
            .RequireAuthorization("user")
            .Produces<ResponseModel<string>>(StatusCodes.Status202Accepted)
            .Produces(StatusCodes.Status400BadRequest)
            .WithTags("Task")
            .WithSummary("Deleta uma Tarefa de acordo com Id e no Quadro passado por Id.");
    }
}
