using System.Data.SqlClient;

namespace Ballastlane.TaskManagement.Infrastructure.Data
{
    public class DatabaseInitializerRepository
    {
        private readonly string _masterConnectionString;

        public DatabaseInitializerRepository(string masterConnectionString)
        {
            _masterConnectionString = masterConnectionString ?? throw new ArgumentNullException(nameof(masterConnectionString));
        }

        public void InitializeDatabase(string databaseName)
        {
            using (SqlConnection connection = new SqlConnection(_masterConnectionString))
            {
                connection.Open();

                using (SqlCommand createDbCommand = new SqlCommand($"IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = '{databaseName}')" +
                    $" BEGIN " +
                        $"CREATE DATABASE [{databaseName}]" +
                    $" END ", connection))
                {
                    createDbCommand.ExecuteNonQuery();
                }
            }

            // Actualizar la cadena de conexión con el nombre de la base de datos
            string connectionString = _masterConnectionString.Replace("master", databaseName);

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                
                using (SqlCommand command = new SqlCommand(@"
                    IF NOT EXISTS (SELECT 1 
                               FROM INFORMATION_SCHEMA.TABLES 
                               WHERE TABLE_NAME='TaskItems') 
                        BEGIN
                            CREATE TABLE TaskItems (
                                Id INT PRIMARY KEY IDENTITY(1,1),
                                Title NVARCHAR(255),
                                Description NVARCHAR(MAX),
                                DueDate DATETIME)
                        END", connection))
                {
                    command.ExecuteNonQuery();
                }
            }

            //users
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(@"
                    IF NOT EXISTS (SELECT 1 
                               FROM INFORMATION_SCHEMA.TABLES 
                               WHERE TABLE_NAME='AspNetUsers') 
                        BEGIN
                            CREATE TABLE [dbo].[AspNetUsers] (
                                [Id] NVARCHAR(450) NOT NULL,
                                [UserName] NVARCHAR(256) NULL,
                                [NormalizedUserName] NVARCHAR(256) NULL,
                                [Email] NVARCHAR(256) NULL,
                                [NormalizedEmail] NVARCHAR(256) NULL,
                                [EmailConfirmed] BIT NOT NULL,
                                [PasswordHash] NVARCHAR(MAX) NULL,
                                [SecurityStamp] NVARCHAR(MAX) NULL,
                                [ConcurrencyStamp] NVARCHAR(MAX) NULL,
                                [PhoneNumber] NVARCHAR(MAX) NULL,
                                [PhoneNumberConfirmed] BIT NOT NULL,
                                [TwoFactorEnabled] BIT NOT NULL,
                                [LockoutEnd] DATETIMEOFFSET(7) NULL,
                                [LockoutEnabled] BIT NOT NULL,
                                [AccessFailedCount] INT NOT NULL,
                                CONSTRAINT [PK_AspNetUsers] PRIMARY KEY ([Id])
                            );
                        END", connection))
                {
                    command.ExecuteNonQuery();
                }
            }

            //roles
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(@"
                    IF NOT EXISTS (SELECT 1 
                               FROM INFORMATION_SCHEMA.TABLES 
                               WHERE TABLE_NAME='AspNetRoles') 
                        BEGIN
                            CREATE TABLE [dbo].[AspNetRoles] (
                                [Id] NVARCHAR(450) NOT NULL,
                                [Name] NVARCHAR(256) NULL,
                                [NormalizedName] NVARCHAR(256) NULL,
                                [ConcurrencyStamp] NVARCHAR(MAX) NULL,
                                CONSTRAINT [PK_AspNetRoles] PRIMARY KEY ([Id])
                            );
                        END", connection))
                {
                    command.ExecuteNonQuery();
                }
            }

            //userRoles
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(@"
                    IF NOT EXISTS (SELECT 1 
                               FROM INFORMATION_SCHEMA.TABLES 
                               WHERE TABLE_NAME='AspNetUserRoles') 
                        BEGIN
                            CREATE TABLE [dbo].[AspNetUserRoles] (
                                [UserId] NVARCHAR(450) NOT NULL,
                                [RoleId] NVARCHAR(450) NOT NULL,
                                CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY ([UserId], [RoleId]),
                                CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[AspNetRoles] ([Id]) ON DELETE CASCADE,
                                CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE
                            );
                        END", connection))
                {
                    command.ExecuteNonQuery();
                }
            }

            //logins
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(@"
                    IF NOT EXISTS (SELECT 1 
                               FROM INFORMATION_SCHEMA.TABLES 
                               WHERE TABLE_NAME='AspNetUserLogins') 
                        BEGIN
                            CREATE TABLE [dbo].[AspNetUserLogins] (
                                [LoginProvider] NVARCHAR(128) NOT NULL,
                                [ProviderKey] NVARCHAR(128) NOT NULL,
                                [ProviderDisplayName] NVARCHAR(MAX) NULL,
                                [UserId] NVARCHAR(450) NOT NULL,
                                CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey]),
                                CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE
                            );
                        END", connection))
                {
                    command.ExecuteNonQuery();
                }
            }

            //tokens
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(@"
                    IF NOT EXISTS (SELECT 1 
                               FROM INFORMATION_SCHEMA.TABLES 
                               WHERE TABLE_NAME='AspNetUserLogins') 
                        BEGIN
                            CREATE TABLE [dbo].[AspNetUserTokens] (
                                [UserId] NVARCHAR(450) NOT NULL,
                                [LoginProvider] NVARCHAR(128) NOT NULL,
                                [Name] NVARCHAR(128) NOT NULL,
                                [Value] NVARCHAR(MAX) NULL,
                                CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name]),
                                CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE
                            );
                        END", connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
