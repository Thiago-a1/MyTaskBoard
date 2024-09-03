namespace MyTaskBoard.API.Models;

public class ResponseModel<T>
{
    public List<T>? Data { get; set; }
    public string Message { get; set; } = string.Empty;
    public bool Status { get; set; } = false;
}
