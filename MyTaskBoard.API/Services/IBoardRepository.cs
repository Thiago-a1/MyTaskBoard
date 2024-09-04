using MyTaskBoard.API.DTOs.Requests;
using MyTaskBoard.API.DTOs.Responses;
using MyTaskBoard.API.Models;

namespace MyTaskBoard.API.Services;

public interface IBoardRepository
{
    public Task<ResponseModel<GetBoardResponse>> GetAllBoardsByUserId(string bearerToken);
    public Task<ResponseModel<CreateBoardResponse>> CreateBoard(string bearerToken, CreateBoardRequest request);
    public Task<ResponseModel<UpdateBoardResponse>> UpdateBoard(string bearerToken, int boardId, UpdateBoardRequest request);
    public Task<ResponseModel<string>> DeleteBoard(string bearerToken, int boardId);
}
