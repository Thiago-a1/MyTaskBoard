using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MyTaskBoard.API.DTOs.Requests;

public class UpdateTaskRequest
{
    [JsonPropertyName("Task_Name")]
    [Required(ErrorMessage = "O nome da Tarefa é obrigatorio.")]
    [StringLength(50, MinimumLength = 5, ErrorMessage = "O Nome da Tarefa deve ter no minimo 6 caracteres e no maximo 50.")]
    public required string TaskName { get; set; }
    [JsonPropertyName("Task_Description")]
    [Required(ErrorMessage = "A descrição da Tarefa é obrigatoria.")]
    [StringLength(250, MinimumLength = 5, ErrorMessage = "A descrição da Tarefa deve ter no minimo 5 caracteres e no maximo 250.")]
    public required string Description { get; set; }
    [JsonPropertyName("Task_Status")]
    [Range(0, 2)]
    public required int Status { get; set; }
    [JsonPropertyName("Task_Icon")]
    [Range(0, 6)]
    public required int Icon { get; set; }
}
