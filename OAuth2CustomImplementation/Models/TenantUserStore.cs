using System;
using System.Data.Common;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace OAuth2CustomImplementation.Models
{
    /*
     * Basic user management APIs
     *
     * The main issue is that we should connect to the right database basing on tenant.
     */
    public class TenantUserStore : IUserStore<ApplicationUser>, IUserPasswordStore<ApplicationUser>
    {
        private bool _disposed;
        public DbConnection Connection { get; set; }

        /// <summary>Insert a new user</summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task CreateAsync(ApplicationUser user)
        {
            ThrowIfDisposed();
            throw new NotImplementedException();
        }

        /// <summary>Update a user</summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task UpdateAsync(ApplicationUser user)
        {
            ThrowIfDisposed();
            throw new NotImplementedException();
        }

        /// <summary>Delete a user</summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task DeleteAsync(ApplicationUser user)
        {
            ThrowIfDisposed();
            throw new NotImplementedException();
        }

        /// <summary>Finds a user</summary>
        /// <param name="userId"></param>
        /// <returns>The application user. null if the user does not exists</returns>
        public Task<ApplicationUser> FindByIdAsync(string userId)
        {
            ThrowIfDisposed();
            return InternalFindByNameAsync(userId);
        }

        /// <summary>Find a user by name</summary>
        /// <param name="fullUserName"></param>
        /// <returns>The application user. null if the user does not exists</returns>
        public Task<ApplicationUser> FindByNameAsync(string fullUserName)
        {
            ThrowIfDisposed();
            return InternalFindByNameAsync(fullUserName);
        }

        private Task<ApplicationUser> InternalFindByNameAsync(string fullUserName)
        {
            string tenantId = ApplicationUser.GetTenantId(fullUserName);
            string userName = ApplicationUser.GetUserName(fullUserName);

            if (string.IsNullOrWhiteSpace(tenantId))
                throw new InvalidOperationException("Invalid full user name. Tenant not specified. The syntax is <Tenant>\\<UserName>");

            if (string.IsNullOrWhiteSpace(userName))
                throw new InvalidOperationException("fullUserName not valid. UserName not specified. The syntax is <Tenant>\\<UserName>");

            return Task.FromResult(new ApplicationUser() { UserName = fullUserName });
        }


        public Task SetPasswordHashAsync(ApplicationUser user, string passwordHash)
        {
            ThrowIfDisposed();
            throw new NotImplementedException();
        }

        public Task<string> GetPasswordHashAsync(ApplicationUser user)
        {
            ThrowIfDisposed();
            // TODO: read password from database
            return Task.FromResult("Password");
        }

        public Task<bool> HasPasswordAsync(ApplicationUser user)
        {
            ThrowIfDisposed();
            return Task.FromResult(true);
        }

        /// <summary>Dispose the store</summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     If disposing, calls dispose on the Context.  Always nulls out the Context
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (this.Connection != null)
                this.Connection.Dispose();
            _disposed = true;
            Connection = null;
        }


        private void ThrowIfDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().Name);
        }



    }
}