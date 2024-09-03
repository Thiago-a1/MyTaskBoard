using MyTaskBoard.API.DTOs.Requests;
using MyTaskBoard.API.DTOs.Responses;
using MyTaskBoard.API.Models;

namespace MyTaskBoard.API.Services;

public interface IUserRepository
{
    public Task<ResponseModel<GetUserResponse>> GetAllUser();
    public Task<ResponseModel<GetUserResponse>> GetUserById(Guid Id);
    public Task<ResponseModel<CreateUserResponse>> CreateUser(CreateUserRequest request);
    public Task<ResponseModel<string>> DeleteUser(string bearerToken);
}
