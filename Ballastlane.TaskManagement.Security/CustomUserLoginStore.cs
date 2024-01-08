using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ballastlane.TaskManagement.Security
{
    public class CustomUserLoginStore : IUserLoginStore<IdentityUser>
    {
        private readonly string _connectionString;

        public CustomUserLoginStore(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task AddLoginAsync(IdentityUser user, UserLoginInfo login, CancellationToken cancellationToken)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync(cancellationToken);

                var command = new SqlCommand(
                    "INSERT INTO UserLogins (LoginProvider, ProviderKey, ProviderDisplayName, UserId) " +
                    "VALUES (@LoginProvider, @ProviderKey, @ProviderDisplayName, @UserId)",
                    connection
                );

                command.Parameters.AddWithValue("@LoginProvider", login.LoginProvider);
                command.Parameters.AddWithValue("@ProviderKey", login.ProviderKey);
                command.Parameters.AddWithValue("@ProviderDisplayName", login.ProviderDisplayName);
                command.Parameters.AddWithValue("@UserId", user.Id);

                await command.ExecuteNonQueryAsync(cancellationToken);
            }
        }

        public async Task<IdentityUser> FindByLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync(cancellationToken);

                var command = new SqlCommand(
                    "SELECT u.* FROM AspNetUsers u " +
                    "JOIN UserLogins ul ON u.Id = ul.UserId " +
                    "WHERE ul.LoginProvider = @LoginProvider AND ul.ProviderKey = @ProviderKey",
                    connection
                );

                command.Parameters.AddWithValue("@LoginProvider", loginProvider);
                command.Parameters.AddWithValue("@ProviderKey", providerKey);

                using (var reader = await command.ExecuteReaderAsync(cancellationToken))
                {
                    if (await reader.ReadAsync(cancellationToken))
                    {
                        return new IdentityUser
                        {
                            Id = reader["Id"].ToString(),
                            UserName = reader["UserName"].ToString(),
                            // ... other properties...
                        };
                    }
                }
            }

            return null;
        }

        public async Task<IList<UserLoginInfo>> GetLoginsAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            var logins = new List<UserLoginInfo>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync(cancellationToken);

                var command = new SqlCommand(
                    "SELECT * FROM UserLogins WHERE UserId = @UserId",
                    connection
                );

                command.Parameters.AddWithValue("@UserId", user.Id);

                using (var reader = await command.ExecuteReaderAsync(cancellationToken))
                {
                    while (await reader.ReadAsync(cancellationToken))
                    {
                        var login = new UserLoginInfo(
                            reader["LoginProvider"].ToString(),
                            reader["ProviderKey"].ToString(),
                            reader["ProviderDisplayName"].ToString()
                        );

                        logins.Add(login);
                    }
                }
            }

            return logins;
        }

        public async Task<IdentityUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync(cancellationToken);

                var command = new SqlCommand(
                    "SELECT * FROM AspNetUsers WHERE Id = @Id",
                    connection
                );

                command.Parameters.AddWithValue("@Id", userId);

                using (var reader = await command.ExecuteReaderAsync(cancellationToken))
                {
                    if (await reader.ReadAsync(cancellationToken))
                    {
                        return new IdentityUser
                        {
                            Id = reader["Id"].ToString(),
                            UserName = reader["UserName"].ToString(),

                        };
                    }
                }
            }

            return null;
        }

        public async Task<IList<UserLoginInfo>> GetLoginsAsync(IdentityUser user)
        {
            return await GetLoginsAsync(user, CancellationToken.None);
        }

        // Otros métodos requeridos por la interfaz IUserLoginStore

        public void Dispose()
        {
            // Implementa la lógica de disposición si es necesaria
        }

        public Task RemoveLoginAsync(IdentityUser user, string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
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

        public Task<IdentityUser?> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }

}
