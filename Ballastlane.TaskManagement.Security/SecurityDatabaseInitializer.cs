using Microsoft.AspNetCore.Identity;

namespace Ballastlane.TaskManagement.Security
{
    public class SecurityDatabaseInitializer
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IPasswordHasher<IdentityUser> _passwordHasher;

        public SecurityDatabaseInitializer(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IPasswordHasher<IdentityUser> passwordHasher)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _passwordHasher = passwordHasher;
        }

        public async Task Initialize()
        {
            await CreateRoles();
            await CreateDummyUser();
        }

        private async Task CreateRoles()
        {
            // Lógica para crear roles si es necesario
            // Puedes usar _roleManager para crear roles
        }

        private async Task CreateDummyUser()
        {
            // Verifica si el usuario ya existe
            var existingUser = await _userManager.FindByNameAsync("jolivares");

            if (existingUser == null)
            {
                // Crea el usuario
                var newUser = new IdentityUser
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = "jolivares",
                    NormalizedUserName = "jolivares",
                    Email = "jolivares@example.com",
                    NormalizedEmail = "jolivares@example.com",
                    PhoneNumber = "1234567890",
                    AccessFailedCount = 0,
                    EmailConfirmed = false,
                    TwoFactorEnabled = false,
                    PhoneNumberConfirmed = false,
                    LockoutEnabled = false,
                    LockoutEnd = DateTime.Today.AddDays(-1),
                };

                newUser.PasswordHash = _passwordHasher.HashPassword(newUser, "Pass@w0rd1");

                var result = await _userManager.CreateAsync(newUser);

                if (result.Succeeded)
                {
                    // Asigna roles al usuario si es necesario
                    // Puedes usar _userManager.AddToRoleAsync para asignar roles
                }
                else
                {
                    // Manejo de errores si la creación del usuario no es exitosa
                }
            }
        }
    }

}
