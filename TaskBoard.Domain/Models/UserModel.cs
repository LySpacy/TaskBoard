using CSharpFunctionalExtensions;
using System.Text.RegularExpressions;
using TaskBoard.Domain.Helpers;
using TaskBoard.Domain.Enum;
using System.Net.NetworkInformation;

namespace TaskBoard.Domain.Models
{
    public class UserModel
    {
        public UserModel()
        {
        }
        private UserModel(Guid id, string name, string password, string email)
        {
            Id = id;
            Name = name;
            PasswordHash = PasswordHelper.GetHashPassword(password);
            Email = email;
        }
        public Guid Id { get; set; }
        public string Name { get; private set; } = string.Empty;
        public string PasswordHash { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;
        public List<SprintModel> Sprints { get; private set; } = [];
        public UserRole Role { get; private set; } = UserRole.User;
        public BanModel? Ban { get; set; } = null;

        public void UpdateRole(UserRole newRole) => Role = newRole;
        public static Result<UserModel> Create(Guid id, string name, string password, string email)
        {
            if (string.IsNullOrEmpty(name))
            {
                return Result.Failure<UserModel>($"'{nameof(name)}' cannot be null or empty");
            }

            if (string.IsNullOrEmpty(password))
            {
                return Result.Failure<UserModel>($"'{nameof(password)}' cannot be null or empty");
            }

            if (!Regex.IsMatch(email, @"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$"))
            {
                return Result.Failure<UserModel>($"{nameof(email)} is not valided");
            }

            var user = new UserModel(id, name, password, email);

            return Result.Success<UserModel>(user);
        }

    }
}
