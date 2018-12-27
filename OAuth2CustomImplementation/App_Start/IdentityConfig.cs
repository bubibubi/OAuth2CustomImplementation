using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using OAuth2CustomImplementation.Models;

namespace OAuth2CustomImplementation
{
    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.

    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store)
            : base(store)
        {
        }

        protected override async Task<bool> VerifyPasswordAsync(IUserPasswordStore<ApplicationUser, string> store, ApplicationUser user, string password)
        {
            var hash = await store.GetPasswordHashAsync(user);
            return hash == password;
        }

        protected override async Task<IdentityResult> UpdatePassword(IUserPasswordStore<ApplicationUser, string> passwordStore, ApplicationUser user, string newPassword)
        {
            var result = await PasswordValidator.ValidateAsync(newPassword);
            if (!result.Succeeded)
            {
                return result;
            }
            
            // TODO: use the proper password hasher
            await passwordStore.SetPasswordHashAsync(user, PasswordHasher.HashPassword(newPassword));
            return IdentityResult.Success;
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            var manager = new ApplicationUserManager(new TenantUserStore());
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<ApplicationUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };
            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }
    }
}
