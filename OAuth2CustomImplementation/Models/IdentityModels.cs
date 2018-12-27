using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace OAuth2CustomImplementation.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }

        public string Id
        {
            get { return UserName;}
        }

        /// <summary>
        /// Gets or sets the full user name (tenant and user name).
        /// </summary>
        /// <value>
        /// The full name of the user.
        /// </value>
        public string UserName { get; set; }

        public static string GetTenantId(string fullUserName)
        {
            string[] fullUserNameParts = fullUserName.Split(new [] {'\\'}, 2);
            if (fullUserNameParts.Length != 2)
                return null;
            return fullUserNameParts[0];
        }

        public static string GetUserName(string fullUserName)
        {
            string[] fullUserNameParts = fullUserName.Split(new[] { '\\' }, 2);
            if (fullUserNameParts.Length != 2)
                return fullUserName;
            return fullUserNameParts[1];
        }
    }

}