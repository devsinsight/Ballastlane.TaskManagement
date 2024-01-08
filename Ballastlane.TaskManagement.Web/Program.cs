using AspNetCore.Authentication.Basic;
using Ballastlane.TaskManagement.Core.Mappers;
using Ballastlane.TaskManagement.Core.Repositories;
using Ballastlane.TaskManagement.Core.Services;
using Ballastlane.TaskManagement.Infrastructure.Data;
using Ballastlane.TaskManagement.Security;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();


builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Task Management API", Version = "v1" });

    var basicSecuritySchema = new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Name = HeaderNames.Authorization,
        Scheme = BasicDefaults.AuthenticationScheme,
        In = ParameterLocation.Header,
        Reference = new OpenApiReference
        {
            Id = BasicDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        },
        Flows = new OpenApiOAuthFlows
        {
            ClientCredentials = new OpenApiOAuthFlow
            {
                AuthorizationUrl = new Uri("/api/authentication/login")
            }
        }
    };

    c.AddSecurityDefinition(basicSecuritySchema.Reference.Id, basicSecuritySchema); ;

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { basicSecuritySchema, Array.Empty<string>() }
    });

});

builder.Services.AddAuthentication("Basic")
                .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("Basic", null);

builder.Services.AddAuthorization(b =>
{
    b.AddPolicy("Basic", pb =>
    {
        pb.AddAuthenticationSchemes("Basic")
        .RequireAuthenticatedUser();
    });
});

var connectionString = builder.Configuration.GetConnectionString("TaskManagementDbConnection") ?? throw new ArgumentNullException("TaskManagementDbConnection");

builder.Services.AddScoped<ITaskManagementRepository>(_ => new TaskManagementRepository(connectionString));
builder.Services.AddScoped<ITaskItemMapper, TaskItemMapper>();
builder.Services.AddScoped<TaskManagementService>();
builder.Services.AddScoped<SecurityDatabaseInitializer>();
//builder.Services.AddScoped<SwaggerBasicAuthMiddleware>();

builder.Services.AddScoped<IUserStore<IdentityUser>>(provider => new CustomUserStore(connectionString));
builder.Services.AddScoped<IRoleStore<IdentityRole>>(provider => new CustomRoleStore(connectionString));
builder.Services.AddScoped<IUserPasswordStore<IdentityUser>>(provider => new CustomUserPasswordStore(connectionString));
builder.Services.AddScoped<IUserLoginStore<IdentityUser>>(provider => new CustomUserLoginStore(connectionString));
builder.Services.AddScoped<IPasswordHasher<IdentityUser>>(provider => new CustomPasswordHasher(connectionString));


builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
})
    .AddSignInManager<CustomSignInManager>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var masterConnectionString = builder.Configuration.GetConnectionString("MasterDbConnection") ?? throw new ArgumentNullException("MasterDbConnection");

    var databaseInitializer = new DatabaseInitializerRepository(masterConnectionString);
    databaseInitializer.InitializeDatabase("TaskManagementDb");
}

using (var scope = app.Services.CreateScope())
{
    var secutiytDatabaseInitializer = scope.ServiceProvider.GetService<SecurityDatabaseInitializer>();
    secutiytDatabaseInitializer.Initialize().Wait();
}

//app.UseMiddleware<SwaggerBasicAuthMiddleware>();


app.UseSwagger();
app.UseSwaggerUI(c =>
{
    
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Authentication API V1");

    c.EnableFilter();
    c.EnableValidator();
});


app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
