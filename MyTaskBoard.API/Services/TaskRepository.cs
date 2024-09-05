using Dapper;
using MySql.Data.MySqlClient;
using MyTaskBoard.API.DTOs.Requests;
using MyTaskBoard.API.DTOs.Responses;
using MyTaskBoard.API.Models;
using MyTaskBoard.Domain.Entities;

namespace MyTaskBoard.API.Services;

public class TaskRepository : ITaskRepository
{
    private readonly string _connectionString;
    public TaskRepository()
    {
        this._connectionString = Environment.GetEnvironmentVariable("ASPNET_CONNECTION_STRING")!;
    }

    public async Task<ResponseModel<GetTaskResponse>> GetAllTaskByBoardId(int boardId)
    {
        using (var conn = new MySqlConnection(_connectionString))
        {
            string query = $"SELECT * FROM Tasks WHERE BOARD_ID = @BOARD_ID;";

            var responseData = await conn.QueryAsync<GetTaskResponse>(query, new { BOARD_ID = boardId });

            if (responseData.Count() == 0)
            {
                return new ResponseModel<GetTaskResponse>
                {
                    Data = [],
                    Message = "Nenhuma tarefa foi encontrada.",
                    Status = false
                };
            }

            return new ResponseModel<GetTaskResponse>
            {
                Data = responseData.ToList(),
                Message = "Tarefas localizadas com sucesso.",
                Status = true
            }; ;
        }
    }

    public async Task<ResponseModel<GetTaskResponse>> GetTaskById(int id)
    {
        using (var conn = new MySqlConnection(_connectionString))
        {
            string query = $"SELECT * FROM Tasks WHERE ID = @ID;";

            var responseData = await conn.QueryAsync<GetTaskResponse>(query, new { ID = id });

            if (responseData.Count() == 0)
            {
                return new ResponseModel<GetTaskResponse>
                {
                    Data = [],
                    Message = "Nenhuma tarefa foi encontrada.",
                    Status = false
                };
            }

            return new ResponseModel<GetTaskResponse>
            {
                Data = responseData.ToList(),
                Message = "Tarefa localizada com sucesso.",
                Status = true
            }; ;
        }
    }

    public async Task<ResponseModel<CreateTaskResponse>> CreateTask(int boardId, CreateTaskRequest request)
    {
        using (var conn = new MySqlConnection(_connectionString))
        {
            string query = @"
                            INSERT INTO Tasks (TASKNAME, DESCRIPTION, STATUS, ICON, BOARD_ID) 
                            VALUES (@TASKNAME, @DESCRIPTION, @STATUS, @ICON, @BOARD_ID);
                            SELECT LAST_INSERT_ID();";

            var insertedId = await conn.ExecuteScalarAsync<int>(query, new Domain.Entities.Task 
            {
                TaskName = request.TaskName,
                Description = request.Description,
                Status = request.Status + 1,
                Icon = request.Icon + 1,
                Board_Id = boardId
            });

            if (insertedId == 0)
            {
                return new ResponseModel<CreateTaskResponse>
                {
                    Data = [],
                    Message = "Erro ao criar a tarefa.",
                    Status = false
                };
            }

            query = "SELECT * FROM Tasks WHERE ID = @ID";

            var responseData = await conn.QueryFirstOrDefaultAsync<CreateTaskResponse>(query, new { ID = insertedId });

            return new ResponseModel<CreateTaskResponse>
            {
                Data = [responseData],
                Message = "Tarefa criada com sucesso.",
                Status = true
            }; ;
        }
    }

    public async Task<ResponseModel<UpdateTaskResponse>> UpdateTask(int id, int boardId, UpdateTaskRequest request)
    {
        using (var conn = new MySqlConnection(_connectionString))
        {
            string query = @"
                            UPDATE Tasks SET 
                            TASKNAME = @TASKNAME, DESCRIPTION = @DESCRIPTION, STATUS = @STATUS, ICON = @ICON
                            WHERE ID = @ID AND BOARD_ID = @BOARD_ID;";

            var result = await conn.ExecuteAsync(query, new
            {
                ID = id,
                TASKNAME = request.TaskName,
                DESCRIPTION = request.Description,
                STATUS = request.Status + 1,
                ICON = request.Icon + 1,
                BOARD_ID = boardId
            });

            if (result <= 0)
            {
                return new ResponseModel<UpdateTaskResponse>
                {
                    Data = [],
                    Message = "Erro ao atualizar a tarefa.",
                    Status = false
                };
            }

            query = "SELECT * FROM Tasks WHERE ID = @ID";

            var responseData = await conn.QueryFirstOrDefaultAsync<UpdateTaskResponse>(query, new { ID = id });

            return new ResponseModel<UpdateTaskResponse>
            {
                Data = [responseData],
                Message = "Tarefa atualizada com sucesso.",
                Status = true
            };
        }
    }

    public async Task<ResponseModel<string>> DeleteTask(int id, int boardId)
    {
        using (var conn = new MySqlConnection(_connectionString))
        {
            string query = @"DELETE FROM Tasks WHERE ID = @ID AND BOARD_ID = @BOARD_ID;";

            var responseData = await conn.ExecuteAsync(query, new { ID = id, BOARD_ID = boardId });

            if (responseData <= 0)
            {
                return new ResponseModel<string>
                {
                    Data = [],
                    Message = "Erro ao realizar a operação.",
                    Status = false
                };
            }

            return new ResponseModel<string>
            {
                Data = [],
                Message = "Operação realizada com sucesso.",
                Status = true
            };
        }
    }
}
