using System.ComponentModel.DataAnnotations;
using ToDoApp.Core.Constant;

namespace ToDoApp.Core.DTOs;
public record class LoginDTO
{
    [EmailAddress(ErrorMessage = RegularExpressionsConstant.EmailMessage)]
    public required string Email { get; set; }

    [RegularExpression(RegularExpressionsConstant.Password,
      ErrorMessage = RegularExpressionsConstant.PasswordMessage)]
    public required string Password { get; set; }


}

public record class SignUpDTO
{
    public required string Username { get; set; }
    
    [EmailAddress(ErrorMessage = RegularExpressionsConstant.EmailMessage)]
    public required string Email { get; set; }

    [RegularExpression(RegularExpressionsConstant.Password,
  ErrorMessage = RegularExpressionsConstant.PasswordMessage)]
    public required string Password { get; set; }
}
