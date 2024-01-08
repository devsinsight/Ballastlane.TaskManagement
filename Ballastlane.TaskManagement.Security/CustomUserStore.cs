using Microsoft.AspNetCore.Identity;
using System.Data.SqlClient;
using System.Security.Claims;

namespace Ballastlane.TaskManagement.Security
{
    public class CustomUserStore : IUserStore<IdentityUser>,
                                   IUserClaimStore<IdentityUser>,
                                   IUserLoginStore<IdentityUser>,
                                   IUserRoleStore<IdentityUser>,
                                   IUserPasswordStore<IdentityUser>,
                                   IUserSecurityStampStore<IdentityUser>
    {
        private readonly string _connectionString;

        public CustomUserStore(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IdentityResult> CreateAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync(cancellationToken);

                var command = new SqlCommand(
                    "INSERT INTO AspNetUsers (Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash, SecurityStamp, ConcurrencyStamp, PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnd, LockoutEnabled, AccessFailedCount) " +
                    "VALUES (@Id, @UserName, @NormalizedUserName, @Email, @NormalizedEmail, @EmailConfirmed, @PasswordHash, @SecurityStamp, @ConcurrencyStamp, @PhoneNumber, @PhoneNumberConfirmed, @TwoFactorEnabled, @LockoutEnd, @LockoutEnabled, @AccessFailedCount)",
                    connection
                );

                command.Parameters.AddWithValue("@Id", user.Id);
                command.Parameters.AddWithValue("@UserName", user.UserName);
                command.Parameters.AddWithValue("@NormalizedUserName", user.NormalizedUserName ?? user.UserName);
                command.Parameters.AddWithValue("@Email", user.Email);
                command.Parameters.AddWithValue("@NormalizedEmail", user.NormalizedEmail);
                command.Parameters.AddWithValue("@EmailConfirmed", user.EmailConfirmed);
                command.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);
                command.Parameters.AddWithValue("@SecurityStamp", user.SecurityStamp);
                command.Parameters.AddWithValue("@ConcurrencyStamp", user.ConcurrencyStamp);
                command.Parameters.AddWithValue("@PhoneNumber", user.PhoneNumber);
                command.Parameters.AddWithValue("@PhoneNumberConfirmed", user.PhoneNumberConfirmed);
                command.Parameters.AddWithValue("@TwoFactorEnabled", user.TwoFactorEnabled);
                command.Parameters.AddWithValue("@LockoutEnd", user.LockoutEnd);
                command.Parameters.AddWithValue("@LockoutEnabled", user.LockoutEnabled);
                command.Parameters.AddWithValue("@AccessFailedCount", user.AccessFailedCount);

                await command.ExecuteNonQueryAsync(cancellationToken);
            }

            return IdentityResult.Success;
        }

        public Task<IdentityResult> UpdateAsync(IdentityUser user, CancellationToken cancellationToken)
        {
           throw new NotImplementedException();
        }

        public Task<string> GetUserIdAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string?> GetUserNameAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            return Task.FromResult(user.UserName);
        }

        public Task SetUserNameAsync(IdentityUser user, string? userName, CancellationToken cancellationToken)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            user.UserName = userName;
            return Task.FromResult(0);
        }

        public Task<string?> GetNormalizedUserNameAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetNormalizedUserNameAsync(IdentityUser user, string? normalizedName, CancellationToken cancellationToken)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            user.NormalizedUserName = normalizedName;
            return Task.FromResult(0);
        }

        public Task<IdentityResult> DeleteAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<IdentityUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync(cancellationToken);

                var commandText = "SELECT * FROM AspNetUsers WHERE NormalizedUserName = @NormalizedUserName";
                using (var command = new SqlCommand(commandText, connection))
                {
                    command.Parameters.AddWithValue("@NormalizedUserName", normalizedUserName);

                    using (var reader = await command.ExecuteReaderAsync(cancellationToken))
                    {
                        if (await reader.ReadAsync(cancellationToken))
                        {
                            return MapUserFromReader(reader);
                        }
                    }
                }
            }

            return null;
        }

        private IdentityUser MapUserFromReader(SqlDataReader reader)
        {
            return new IdentityUser
            {
                Id = reader["Id"].ToString(),
                UserName = reader["UserName"].ToString(),
                NormalizedUserName = reader["NormalizedUserName"].ToString(),
                Email = reader["Email"].ToString(),
                NormalizedEmail = reader["NormalizedEmail"].ToString(),
                EmailConfirmed = Convert.ToBoolean(reader["EmailConfirmed"]),
                PasswordHash = reader["PasswordHash"].ToString(),
                SecurityStamp = reader["SecurityStamp"].ToString(),
                ConcurrencyStamp = reader["ConcurrencyStamp"].ToString(),
                PhoneNumber = reader["PhoneNumber"].ToString(),
                PhoneNumberConfirmed = Convert.ToBoolean(reader["PhoneNumberConfirmed"]),
                TwoFactorEnabled = Convert.ToBoolean(reader["TwoFactorEnabled"]),
                LockoutEnd = reader["LockoutEnd"] != DBNull.Value ? (DateTimeOffset?)reader["LockoutEnd"] : null,
                LockoutEnabled = Convert.ToBoolean(reader["LockoutEnabled"]),
                AccessFailedCount = Convert.ToInt32(reader["AccessFailedCount"])
            };
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public Task<IdentityUser?> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IList<Claim>> GetClaimsAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task AddClaimsAsync(IdentityUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task ReplaceClaimAsync(IdentityUser user, Claim claim, Claim newClaim, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task RemoveClaimsAsync(IdentityUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IList<IdentityUser>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task AddLoginAsync(IdentityUser user, UserLoginInfo login, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task RemoveLoginAsync(IdentityUser user, string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IList<UserLoginInfo>> GetLoginsAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityUser?> FindByLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetSecurityStampAsync(IdentityUser user, string stamp, CancellationToken cancellationToken)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            user.SecurityStamp = stamp;
            return Task.FromResult(0);
        }

        public Task<string?> GetSecurityStampAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            return Task.FromResult(user.SecurityStamp);
        }

        public Task SetPasswordHashAsync(IdentityUser user, string? passwordHash, CancellationToken cancellationToken)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            user.PasswordHash = passwordHash;
            return Task.FromResult(0);
        }

        public Task<string?> GetPasswordHashAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            return Task.FromResult(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task AddToRoleAsync(IdentityUser user, string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task RemoveFromRoleAsync(IdentityUser user, string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IList<string>> GetRolesAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsInRoleAsync(IdentityUser user, string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IList<IdentityUser>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }

}
