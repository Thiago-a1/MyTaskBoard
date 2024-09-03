using System.ComponentModel.DataAnnotations;
using System.Net.Mail;

namespace MyTaskBoard.API.Utils;

public class EmailValidator : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return ValidationResult.Success;
        }

        try
        {
            var email = value.ToString();
            var mailAddress = new MailAddress(email!);

            return ValidationResult.Success;
        }
        catch (FormatException)
        {
            return new ValidationResult("Endereço de e-mail inválido");
        }
    }
}
