using MyTaskBoard.API.DTOs.Requests;
using MyTaskBoard.API.Models;

namespace MyTaskBoard.API.Services;

public interface ILoginService
{
    public Task<ResponseModel<object>> LoginUser(LoginRequest request);
    public ResponseModel<object> VerifyToken(VerifyTokenRequest token); 
}
