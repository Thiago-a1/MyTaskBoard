using MyTaskBoard.API.DTOs.Requests;
using MyTaskBoard.API.DTOs.Responses;
using MyTaskBoard.API.Models;

namespace MyTaskBoard.API.Services;

public interface ITaskRepository
{
    public Task<ResponseModel<GetTaskResponse>> GetAllTaskByBoardId(int boardId);
    public Task<ResponseModel<GetTaskResponse>> GetTaskById(int id);
    public Task<ResponseModel<CreateTaskResponse>> CreateTask(int boardId, CreateTaskRequest request);
    public Task<ResponseModel<UpdateTaskResponse>> UpdateTask(int id, int boardId, UpdateTaskRequest request);
    public Task<ResponseModel<string>> DeleteTask(int id, int boardId);
}
