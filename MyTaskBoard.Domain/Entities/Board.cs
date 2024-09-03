namespace MyTaskBoard.Domain.Entities;
public class Board
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime Created_At {  get; set; }

    public Guid User_Id { get; set; }
}
