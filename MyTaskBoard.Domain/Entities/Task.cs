﻿namespace MyTaskBoard.Domain.Entities;
public class Task
{
    public int Id { get; set; }
    public string TaskName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Status { get; set; }
    public int Icon { get; set; }

    public int Board_Id { get; set; }
}