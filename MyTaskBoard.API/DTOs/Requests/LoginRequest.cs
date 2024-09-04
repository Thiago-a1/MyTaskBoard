using MyTaskBoard.API.Utils;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MyTaskBoard.API.DTOs.Requests;

public class LoginRequest
{
    [JsonPropertyName("User_Email")]
    [Required(ErrorMessage = "O E-email é obrigatorio.")]
    [EmailAddress, EmailValidator(ErrorMessage = "E-mail invalido.")]
    public required string Email { get; set; }
    [JsonPropertyName("User_Password")]
    [Required(ErrorMessage = "A senha é obrigatoria.")]
    [MinLength(8, ErrorMessage = "A Senha deve conter pelo menos 8 caracteres.")]
    public required string Password { get; set; }
}
