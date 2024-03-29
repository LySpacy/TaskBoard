using System.Security.Cryptography;
using System.Text;

namespace TaskBoard.Domain.Helpers
{
    public class PasswordHelper
    {
        public static string GetHashPassword(string password)
        {
            var hash = SHA256.Create();
            return Convert.ToHexString(hash.ComputeHash(Encoding.ASCII.GetBytes(password)));
        }

        public static bool Verify(string password, string passwordHash)
        {
            var hash = SHA256.Create();
            return passwordHash == Convert.ToHexString(hash.ComputeHash(Encoding.ASCII.GetBytes(password)));
        }

        public static string GeneratePassword()
        {
            string numbers = "0123456789";
            string alphabetLow = "abcdefghijklmnopqrstuvwxyz";
            string alphaberHith = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string symbols = "~`@#$%^&*()_+-=[]{};'\\:\"|,./<>?";

            var builderPassword = new StringBuilder();
            var random = new Random();
            int lengthPassword = random.Next(6, 18);

            for (int i = 0; i < lengthPassword; i++)
            {
                switch (random.Next(0, 4))
                {
                    case 0:
                        {
                            builderPassword.Append(numbers[random.Next(0, numbers.Length)]);
                        }
                        break;
                    case 1:
                        {
                            builderPassword.Append(alphabetLow[random.Next(0, alphabetLow.Length)]);
                        }
                        break;
                    case 2:
                        {
                            builderPassword.Append(alphaberHith[random.Next(0, alphaberHith.Length)]);
                        }
                        break;
                    case 3:
                        {
                            builderPassword.Append(symbols[random.Next(0, symbols.Length)]);
                        }
                        break;
                }
            }

            return builderPassword.ToString();
        }
    }
}
