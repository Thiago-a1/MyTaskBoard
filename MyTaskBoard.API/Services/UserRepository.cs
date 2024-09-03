using Dapper;
using MySql.Data.MySqlClient;
using MyTaskBoard.API.DTOs.Requests;
using MyTaskBoard.API.DTOs.Responses;
using MyTaskBoard.API.Models;
using MyTaskBoard.Domain.Entities;

namespace MyTaskBoard.API.Services;

public class UserRepository : IUserRepository
{
    private readonly string _connectionString;

    public UserRepository()
    {
        this._connectionString = Environment.GetEnvironmentVariable("ASPNET_CONNECTION_STRING")!;
    }

    public async Task<ResponseModel<GetUserResponse>> GetAllUser()
    {
        using (var conn = new MySqlConnection(_connectionString))
        {
            string query = $"SELECT ID, NAME, EMAIL, CREATED_AT FROM Users";

            var responseData = await conn.QueryAsync<GetUserResponse>(query);

            if (responseData.Count() == 0) 
            {
                return new ResponseModel<GetUserResponse>
                {
                    Data = [],
                    Message = "Nenhum usuário encontrado.",
                    Status = false
                };
            }

            return new ResponseModel<GetUserResponse>
            {
                Data = responseData.ToList(),
                Message = "Usuários localizados com sucesso.",
                Status = true
            }; ;
        }
    }

    public async Task<ResponseModel<GetUserResponse>> GetUserById(Guid Id)
    {
        ResponseModel<GetUserResponse> response = new ResponseModel<GetUserResponse>();

        using (var conn = new MySqlConnection(_connectionString))
        {
            string query = $"SELECT ID, NAME, EMAIL, CREATED_AT FROM Users WHERE ID = @ID";

            var responseData = await conn.QueryFirstOrDefaultAsync<GetUserResponse>(query, new { ID = Id });

            if (responseData == null)
            {
                return new ResponseModel<GetUserResponse>
                {
                    Data = [],
                    Message = "Nenhum usuário encontrado.",
                    Status = false
                };
            }

            return new ResponseModel<GetUserResponse>
            {
                Data = [responseData],
                Message = "Usuário localizado com sucesso.",
                Status = true
            }; ;
        }
    }

    public async Task<ResponseModel<CreateUserResponse>> CreateUser(CreateUserRequest request)
    {
        using (var conn = new MySqlConnection(_connectionString))
        {
            string query = @"
                            INSERT INTO Users (ID, NAME, EMAIL, PASSWORD, CREATED_AT) 
                            VALUES (@ID, @NAME, @EMAIL, @PASSWORD, @CREATED_AT);";

            var newUser = new User
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Email = request.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
                Created_At = DateTime.Now,
            };

            var responseData = await conn.ExecuteAsync(query, newUser);

            if (responseData <= 0)
            {
                return new ResponseModel<CreateUserResponse>
                {
                    Data = [],
                    Message = "Erro ao registrar o usuário.",
                    Status = false
                };
            }

            query = $"SELECT ID, NAME, EMAIL, CREATED_AT FROM Users WHERE ID = @ID";

            var createdUserData = await conn.QueryFirstOrDefaultAsync<CreateUserResponse>(query, new { ID = newUser.Id });

            return new ResponseModel<CreateUserResponse>
            {
                Data = [createdUserData],
                Message = "Usuário registrado com sucesso.",
                Status = true
            };
        }
    }

    public async Task<ResponseModel<string>> DeleteUser(string bearerToken)
    {
        using (var conn = new MySqlConnection(_connectionString))
        {
            string query = @"DELETE FROM Users WHERE ID = @ID";

            string Id = TokenRepository.TakeIdByToken(bearerToken).ToString();

            var responseData = await conn.ExecuteAsync(query, new { ID = Id});

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
