using System.ComponentModel.DataAnnotations;

namespace TaskBoard.API.Contracts.User
{
    public record  ResetPasswordUserRequest(
        [Required] [EmailAddress]  string Email,
        [Required] [StringLength(100, ErrorMessage = "Пароль должен содержать как минимум 6 символов", MinimumLength = 6)]
        [DataType(DataType.Password)]
        string Password,
        [DataType(DataType.Password)]
        string ConfirmPassword
        );
    
}
