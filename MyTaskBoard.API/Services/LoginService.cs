using Dapper;
using MySql.Data.MySqlClient;
using MyTaskBoard.API.DTOs.Requests;
using MyTaskBoard.API.Models;
using MyTaskBoard.Domain.Entities;

namespace MyTaskBoard.API.Services;

public class LoginService : ILoginService
{
    private readonly string _connectionString;

    public LoginService()
    {
        this._connectionString = Environment.GetEnvironmentVariable("ASPNET_CONNECTION_STRING")!;
    }

    public async Task<ResponseModel<object>> LoginUser(LoginRequest request)
    {
        using (var conn = new MySqlConnection(_connectionString))
        {
            var query = $"SELECT * FROM Users WHERE EMAIL = @EMAIL";

            var user = await conn.QueryFirstOrDefaultAsync<User>(query, new { request.Email });

            if (user == null)
            {
                return new ResponseModel<object>
                {
                    Data = [],
                    Message = "E-mail Invalido ou não encontrado.",
                    Status = false
                };
            }

            if (BCrypt.Net.BCrypt.Verify(request.Password, user.Password) == false)
            {
                return new ResponseModel<object>
                {
                    Data = [],
                    Message = "Senha Invalida.",
                    Status = false
                };
            }

            var BearerToken = TokenRepository.GenerateToken(user.Id.ToString(), "user");

            return new ResponseModel<object>
            {
                Data = [BearerToken],
                Message = "Usuário logado com sucesso.",
                Status = true
            };
        }
    }

    public ResponseModel<object> VerifyToken(VerifyTokenRequest request)
    {
        bool result = TokenRepository.VerifyToken(request.Token);

        return new ResponseModel<object>
        {
            Data = [],
            Message = result ? "Token Valido." : "Token Invalido.",
            Status = result 
        };
    }
}
