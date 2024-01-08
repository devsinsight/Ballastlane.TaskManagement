using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ballastlane.TaskManagement.Security
{
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using static System.Formats.Asn1.AsnWriter;

    public class CustomSignInManager : SignInManager<IdentityUser>
    {

        private readonly IPasswordHasher<IdentityUser> _passwordHasher;
        public CustomSignInManager(
            UserManager<IdentityUser> userManager,
            IHttpContextAccessor contextAccessor,
            IUserClaimsPrincipalFactory<IdentityUser> claimsFactory,
            IOptions<IdentityOptions> optionsAccessor,
            ILogger<SignInManager<IdentityUser>> logger,
            IAuthenticationSchemeProvider schemes,
            IPasswordHasher<IdentityUser> passwordHasher)
            : base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes, null)
        {
            _passwordHasher = passwordHasher;
        }

        // Puedes añadir métodos personalizados o anular los métodos existentes según tus necesidades.

        // Ejemplo de un método personalizado para realizar acciones después de iniciar sesión
        public override async Task<SignInResult> PasswordSignInAsync(string userName, string password, bool isPersistent, bool lockoutOnFailure)
        {
            var user = await UserManager.FindByNameAsync(userName);
            if (user == null)
            {
                return SignInResult.Failed;
            }


            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);

            return result == PasswordVerificationResult.Success ? SignInResult.Success : SignInResult.Failed;

        }

    }

}
