using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Ballastlane.TaskManagement.Security
{
    public class CustomPasswordHasher : IPasswordHasher<IdentityUser>
    {
        private readonly string _connectionString;

        public CustomPasswordHasher(string connectionString)
        {
            _connectionString = connectionString;
        }

        public string HashPassword(IdentityUser user, string password)
        {
            return Hash(password);
        }

        public PasswordVerificationResult VerifyHashedPassword(IdentityUser user, string hashedPassword, string providedPassword)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT PasswordHash FROM AspNetUsers WHERE UserName = @UserName", connection);
                command.Parameters.AddWithValue("@UserName", user.UserName);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        var storedHash = reader.GetString(0);
                        if (Verify(providedPassword, storedHash))
                        {
                            return PasswordVerificationResult.Success;
                        }
                    }
                }
            }

            return PasswordVerificationResult.Failed;
        }

        private string Hash(string password)
        {

            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }

        private bool Verify(string password, string hashedPassword)
        {
            return Hash(password) == hashedPassword;
        }
    }

}
