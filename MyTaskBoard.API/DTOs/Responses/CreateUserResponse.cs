namespace MyTaskBoard.API.DTOs.Responses;

public class CreateUserResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime Created_At { get; set; }
}
