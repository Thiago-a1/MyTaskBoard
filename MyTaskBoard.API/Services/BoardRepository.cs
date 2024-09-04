using Dapper;
using MySql.Data.MySqlClient;
using MyTaskBoard.API.DTOs.Requests;
using MyTaskBoard.API.DTOs.Responses;
using MyTaskBoard.API.Models;
using MyTaskBoard.Domain.Entities;

namespace MyTaskBoard.API.Services;

public class BoardRepository : IBoardRepository
{
    private readonly string _connectionString;

    public BoardRepository()
    {
        this._connectionString = Environment.GetEnvironmentVariable("ASPNET_CONNECTION_STRING")!;
    }

    public async Task<ResponseModel<GetBoardResponse>> GetAllBoardsByUserId(string bearerToken)
    {
        using (var conn = new MySqlConnection(_connectionString))
        {
            string query = $"SELECT ID, NAME, CREATED_AT, USER_ID FROM Boards WHERE USER_ID = @USER_ID;";

            string userId = TokenRepository.TakeIdByToken(bearerToken).ToString();

            var responseData = await conn.QueryAsync<GetBoardResponse>(query, new { USER_ID = userId});

            if (responseData.Count() == 0)
            {
                return new ResponseModel<GetBoardResponse>
                {
                    Data = [],
                    Message = "Nenhum quadro foi encontrado.",
                    Status = false
                };
            }

            return new ResponseModel<GetBoardResponse>
            {
                Data = responseData.ToList(),
                Message = "Quadros localizados com sucesso.",
                Status = true
            }; ;
        }
    }

    public async Task<ResponseModel<CreateBoardResponse>> CreateBoard(string bearerToken, CreateBoardRequest request)
    {
        using (var conn = new MySqlConnection(_connectionString))
        {
            string query = @"
                            INSERT INTO Boards (NAME, CREATED_AT, USER_ID) 
                            VALUES (@NAME, @CREATED_AT, @USER_ID);
                            SELECT LAST_INSERT_ID();";

            Guid userId = TokenRepository.TakeIdByToken(bearerToken);

            var insertedId = await conn.ExecuteScalarAsync<int>(query, new Board 
            { 
                Name = request.Name,
                Created_At = DateTime.Now,
                User_Id = userId
            });

            if (insertedId <= 0)
            {
                return new ResponseModel<CreateBoardResponse>
                {
                    Data = [],
                    Message = "Erro ao criar o quadro.",
                    Status = false
                };
            }

            query = "SELECT * FROM Boards WHERE ID = @ID";

            var responseData = await conn.QueryFirstOrDefaultAsync<CreateBoardResponse>(query, new { ID = insertedId });

            return new ResponseModel<CreateBoardResponse>
            {
                Data = [responseData],
                Message = "Quadro criado com sucesso.",
                Status = true
            };
        }
    }

    public async Task<ResponseModel<UpdateBoardResponse>> UpdateBoard(string bearerToken, int boardId, UpdateBoardRequest request)
    {
        using (var conn = new MySqlConnection(_connectionString))
        {
            string query = @"
                            UPDATE Boards SET NAME = @NAME
                            WHERE ID = @ID AND USER_ID = @USER_ID;";

            Guid userId = TokenRepository.TakeIdByToken(bearerToken);

            var result = await conn.ExecuteAsync(query, new 
            {
                ID = boardId,
                NAME = request.Name,
                USER_ID = userId
            });

            if (result <= 0)
            {
                return new ResponseModel<UpdateBoardResponse>
                {
                    Data = [],
                    Message = "Erro ao atualizar o quadro.",
                    Status = false
                };
            }

            query = "SELECT * FROM Boards WHERE ID = @ID";

            var responseData = await conn.QueryFirstOrDefaultAsync<UpdateBoardResponse>(query, new { ID = boardId });

            return new ResponseModel<UpdateBoardResponse>
            {
                Data = [responseData],
                Message = "Quadro atualizado com sucesso.",
                Status = true
            };
        }
    }

    public async Task<ResponseModel<string>> DeleteBoard(string bearerToken, int boardId)
    {
        using (var conn = new MySqlConnection(_connectionString))
        {
            string query = @"DELETE FROM Boards WHERE ID = @ID AND USER_ID = @USER_ID;";

            string userId = TokenRepository.TakeIdByToken(bearerToken).ToString();

            var responseData = await conn.ExecuteAsync(query, new { ID = boardId, USER_ID = userId });

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
