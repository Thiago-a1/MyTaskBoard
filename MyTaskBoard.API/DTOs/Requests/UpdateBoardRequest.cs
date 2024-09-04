using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MyTaskBoard.API.DTOs.Requests;

public class UpdateBoardRequest
{
    [JsonPropertyName("Board_Name")]
    [Required(ErrorMessage = "O nome do quadro é obrigatorio.")]
    [StringLength(50, MinimumLength = 6, ErrorMessage = "O Nome deve ter no minimo 6 caracteres e no maximo 50.")]
    public required string Name { get; set; }
}
