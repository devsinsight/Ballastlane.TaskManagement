using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ballastlane.TaskManagement.Security
{
    public class CustomRoleStore : IRoleStore<IdentityRole>
    {
        private readonly string _connectionString;

        public CustomRoleStore(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IdentityResult> CreateAsync(IdentityRole role, CancellationToken cancellationToken)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync(cancellationToken);

                var command = new SqlCommand(
                    "INSERT INTO AspNetRoles (Id, Name, NormalizedName) " +
                    "VALUES (@Id, @Name, @NormalizedName)",
                    connection
                );

                command.Parameters.AddWithValue("@Id", role.Id);
                command.Parameters.AddWithValue("@Name", role.Name);
                command.Parameters.AddWithValue("@NormalizedName", role.NormalizedName);

                await command.ExecuteNonQueryAsync(cancellationToken);
            }

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> UpdateAsync(IdentityRole role, CancellationToken cancellationToken)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync(cancellationToken);

                var command = new SqlCommand(
                    "UPDATE AspNetRoles SET Name = @Name, NormalizedName = @NormalizedName WHERE Id = @Id",
                    connection
                );

                command.Parameters.AddWithValue("@Id", role.Id);
                command.Parameters.AddWithValue("@Name", role.Name);
                command.Parameters.AddWithValue("@NormalizedName", role.NormalizedName);

                await command.ExecuteNonQueryAsync(cancellationToken);
            }

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(IdentityRole role, CancellationToken cancellationToken)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync(cancellationToken);

                var command = new SqlCommand(
                    "DELETE FROM AspNetRoles WHERE Id = @Id",
                    connection
                );

                command.Parameters.AddWithValue("@Id", role.Id);

                await command.ExecuteNonQueryAsync(cancellationToken);
            }

            return IdentityResult.Success;
        }

        public Task<string> GetRoleIdAsync(IdentityRole role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.Id);
        }

        public Task<string> GetRoleNameAsync(IdentityRole role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.Name);
        }

        public Task SetRoleNameAsync(IdentityRole role, string roleName, CancellationToken cancellationToken)
        {
            role.Name = roleName;
            return Task.CompletedTask;
        }

        public Task<string> GetNormalizedRoleNameAsync(IdentityRole role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.NormalizedName);
        }

        public Task SetNormalizedRoleNameAsync(IdentityRole role, string normalizedName, CancellationToken cancellationToken)
        {
            role.NormalizedName = normalizedName;
            return Task.CompletedTask;
        }

        public async Task<IdentityRole> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync(cancellationToken);

                var command = new SqlCommand(
                    "SELECT * FROM AspNetRoles WHERE Id = @Id",
                    connection
                );

                command.Parameters.AddWithValue("@Id", roleId);

                using (var reader = await command.ExecuteReaderAsync(cancellationToken))
                {
                    if (await reader.ReadAsync(cancellationToken))
                    {
                        return new IdentityRole
                        {
                            Id = reader["Id"].ToString(),
                            Name = reader["Name"].ToString(),
                            NormalizedName = reader["NormalizedName"].ToString()
                        };
                    }
                }
            }

            return null;
        }

        public async Task<IdentityRole> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync(cancellationToken);

                var command = new SqlCommand(
                    "SELECT * FROM AspNetRoles WHERE NormalizedName = @NormalizedName",
                    connection
                );

                command.Parameters.AddWithValue("@NormalizedName", normalizedRoleName);

                using (var reader = await command.ExecuteReaderAsync(cancellationToken))
                {
                    if (await reader.ReadAsync(cancellationToken))
                    {
                        return new IdentityRole
                        {
                            Id = reader["Id"].ToString(),
                            Name = reader["Name"].ToString(),
                            NormalizedName = reader["NormalizedName"].ToString()
                        };
                    }
                }
            }

            return null;
        }

        public void Dispose()
        {
            // Implementa la lógica de disposición si es necesaria
        }
    }

}
