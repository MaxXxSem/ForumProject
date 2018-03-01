using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using ForumProject.Models.Data;
using System.Threading.Tasks;
using System.Data.Entity;

namespace ForumProject.Models.Identity
{
    public class ApplicationUserStore : IUserStore<ApplicationUser>, IUserPasswordStore<ApplicationUser>, IUserSecurityStampStore<ApplicationUser>, IUserRoleStore<ApplicationUser, string>
    {
        private UserStore<IdentityUser> userStore;
        private ForumDBEntities DB
        {
            get { return userStore.Context as ForumDBEntities; }
        }


        public ApplicationUserStore(ForumDBEntities entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException();
            }
            
            userStore = new UserStore<IdentityUser>(entities);
        }

        public Task CreateAsync(ApplicationUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException();
            }

            DB.UserData.Add(user);
            DB.Configuration.ValidateOnSaveEnabled = false;
            return DB.SaveChangesAsync();
        }

        public Task UpdateAsync(ApplicationUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException();
            }

            DB.UserData.Attach(user);
            DB.Entry(user).State = EntityState.Modified;
            DB.Configuration.ValidateOnSaveEnabled = false;
            return DB.SaveChangesAsync();
        }

        public Task DeleteAsync(ApplicationUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException();
            }

            DB.UserData.Remove(user);
            DB.Configuration.ValidateOnSaveEnabled = false;
            return DB.SaveChangesAsync();
        }

        public Task<ApplicationUser> FindByIdAsync(string Id)
        {
            if (String.IsNullOrEmpty(Id))
            {
                throw new ArgumentNullException();
            }

            return DB.UserData.Where(u => u.Id.ToLower() == Id.ToLower()).FirstOrDefaultAsync();
        }

        public Task<ApplicationUser> FindByNameAsync(string name)
        {
            if (String.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException();
            }

            return DB.UserData.Where(u => u.UserName == name).FirstOrDefaultAsync();
        }

        public void Dispose()
        {
            userStore.Dispose();
        }

        public Task SetPasswordHashAsync(ApplicationUser user, string passwordHash)
        {
            if (user == null || String.IsNullOrEmpty(passwordHash))
            {
                throw new ArgumentNullException();
            }

            var identityUser = ToIdentityUser(user);
            var task = userStore.SetPasswordHashAsync(identityUser, passwordHash);
            SetApplicationUser(user, identityUser);
            return task;
        }

        public Task<string> GetPasswordHashAsync(ApplicationUser user)
        {
            string pass = user?.PasswordHash;
            if (pass == null)
            {
                throw new ArgumentNullException();
            }
            
            var identityUser = ToIdentityUser(user);
            var task = userStore.GetPasswordHashAsync(identityUser);
            SetApplicationUser(user, identityUser);
            return task;
        }

        public Task<bool> HasPasswordAsync(ApplicationUser user)
        {
            string pass = user?.PasswordHash;

            var identityUser = ToIdentityUser(user);
            var task = userStore.HasPasswordAsync(identityUser);
            SetApplicationUser(user, identityUser);
            return task;

        }

        public Task SetSecurityStampAsync(ApplicationUser user, string stamp)
        {
            if (user == null || String.IsNullOrEmpty(stamp))
            {
                throw new ArgumentNullException();
            }
            
            var identityUser = ToIdentityUser(user);
            var task = userStore.SetSecurityStampAsync(identityUser, stamp);
            SetApplicationUser(user, identityUser);
            return task;
        }

        public Task<string> GetSecurityStampAsync(ApplicationUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException();
            }

            var identityUser = ToIdentityUser(user);
            var task = userStore.GetSecurityStampAsync(identityUser);
            SetApplicationUser(user, identityUser);
            return task;
        }

        private static void SetApplicationUser(ApplicationUser user, IdentityUser identityUser)
        {
            user.PasswordHash = identityUser.PasswordHash;
            user.SecurityStamp = identityUser.SecurityStamp;
            user.Id = identityUser.Id;
            user.UserName = identityUser.UserName;
        }

        private IdentityUser ToIdentityUser(ApplicationUser user)
        {
            return new IdentityUser
            {
                Id = user.Id,
                PasswordHash = user.PasswordHash,
                SecurityStamp = user.SecurityStamp,
                UserName = user.UserName
            };
        }

        public Task AddToRoleAsync(ApplicationUser user, string roleName)
        {
            if (user == null || string.IsNullOrEmpty(roleName))
            {
                throw new ArgumentNullException();
            }

            var role = DB.Roles.Where(r => r.Name == roleName).FirstOrDefault();
            if (role != null && !user.Roles.Any(r => r.Name == roleName))
            {
                user.Roles.Add(role);
                DB.Entry(user).State = EntityState.Modified;
                return DB.SaveChangesAsync();
            }

            return Task.FromResult(0);
        }

        public Task RemoveFromRoleAsync(ApplicationUser user, string roleName)
        {
            if (user == null || string.IsNullOrEmpty(roleName))
            {
                throw new ArgumentNullException();
            }

            var role = DB.Roles.Where(r => r.Name == roleName).FirstOrDefault();
            if (role != null && user.Roles.Any(r => r.Name == roleName))
            {
                user.Roles.Remove(role);
                return DB.SaveChangesAsync();
            }

            return Task.FromResult(0);
        }

        public Task<IList<string>> GetRolesAsync(ApplicationUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException();
            }

            IList<string> roles = user.Roles.Select(r => r.Name).ToList();
            return Task.FromResult(roles);
        }

        public Task<bool> IsInRoleAsync(ApplicationUser user, string roleName)
        {
            if (user == null || string.IsNullOrEmpty(roleName))
            {
                throw new ArgumentNullException();
            }

            bool result = user.Roles.Any(r => r.Name == roleName);
            return Task.FromResult(result);
        }
    }
}