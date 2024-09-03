using MyTaskBoard.API.Utils;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MyTaskBoard.API.DTOs.Requests;

public class CreateUserRequest
{
    [JsonPropertyName("User_Name")]
    [Required(ErrorMessage = "O Nome do usuário é obrigatorio.")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "O Nome deve ter no minimo 6 caracteres e no maximo 100.")]
    public required string Name { get; set; }
    [JsonPropertyName("User_E-mail")]
    [Required(ErrorMessage = "O E-mail do usuário é obrigatorio.")]
    [EmailAddress, EmailValidator(ErrorMessage = "E-mail Invalido.")]
    public required string Email { get; set; }
    [JsonPropertyName("User_Password")]
    [Required(ErrorMessage = "A Senha do usuário não pode ser vazia.")]
    [MinLength(8, ErrorMessage = "A Senha deve conter pelo menos 8 caracteres.")]
    public required string Password { get; set; }
}
