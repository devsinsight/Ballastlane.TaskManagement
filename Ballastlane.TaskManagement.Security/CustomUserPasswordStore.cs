using Microsoft.AspNetCore.Identity;
using System.Data.SqlClient;

namespace Ballastlane.TaskManagement.Security
{
    public class CustomUserPasswordStore : IUserPasswordStore<IdentityUser>
    {
        private readonly string _connectionString;

        public CustomUserPasswordStore(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<string> GetPasswordHashAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync(cancellationToken);

                var command = new SqlCommand(
                    "SELECT PasswordHash FROM AspNetUsers WHERE Id = @Id",
                    connection
                );

                command.Parameters.AddWithValue("@Id", user.Id);

                var passwordHash = await command.ExecuteScalarAsync(cancellationToken);

                return passwordHash?.ToString();
            }
        }

        public async Task<bool> HasPasswordAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync(cancellationToken);

                var command = new SqlCommand(
                    "SELECT COUNT(*) FROM AspNetUsers WHERE Id = @Id AND PasswordHash IS NOT NULL",
                    connection
                );

                command.Parameters.AddWithValue("@Id", user.Id);

                var count = (int)await command.ExecuteScalarAsync(cancellationToken);

                return count > 0;
            }
        }

        public async Task SetPasswordHashAsync(IdentityUser user, string passwordHash, CancellationToken cancellationToken)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync(cancellationToken);

                var command = new SqlCommand(
                    "UPDATE AspNetUsers SET PasswordHash = @PasswordHash WHERE Id = @Id",
                    connection
                );

                command.Parameters.AddWithValue("@Id", user.Id);
                command.Parameters.AddWithValue("@PasswordHash", passwordHash);

                await command.ExecuteNonQueryAsync(cancellationToken);
            }
        }

        public void Dispose()
        {
            // Implementa la lógica de disposición si es necesaria
        }

        public Task<string> GetUserIdAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string?> GetUserNameAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetUserNameAsync(IdentityUser user, string? userName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string?> GetNormalizedUserNameAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetNormalizedUserNameAsync(IdentityUser user, string? normalizedName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> CreateAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> UpdateAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> DeleteAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityUser?> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityUser?> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
