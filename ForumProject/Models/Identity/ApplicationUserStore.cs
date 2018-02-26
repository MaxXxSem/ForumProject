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
    public class ApplicationUserStore : IUserStore<ApplicationUser>, IUserPasswordStore<ApplicationUser>, IUserSecurityStampStore<ApplicationUser>
    {
        private ForumDBEntities db;
        UserStore<IdentityUser> userStore = new UserStore<IdentityUser>(new ForumDBEntities());
        public ApplicationUserStore(ForumDBEntities entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException();
            }

            db = entities;
        }

        public Task CreateAsync(ApplicationUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException();
            }

            var db = userStore.Context as ForumDBEntities;
            db.UserData.Add(user);
            db.Configuration.ValidateOnSaveEnabled = false;
            return db.SaveChangesAsync();
        }

        public Task UpdateAsync(ApplicationUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException();
            }

            var db = userStore.Context as ForumDBEntities;
            db.UserData.Attach(user);
            db.Entry(user).State = EntityState.Modified;
            db.Configuration.ValidateOnSaveEnabled = false;
            return db.SaveChangesAsync();
        }

        public Task DeleteAsync(ApplicationUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException();
            }

            var db = userStore.Context as ForumDBEntities;
            db.UserData.Remove(user);
            db.Configuration.ValidateOnSaveEnabled = false;
            return db.SaveChangesAsync();
        }

        public Task<ApplicationUser> FindByIdAsync(string Id)
        {
            if (String.IsNullOrEmpty(Id))
            {
                throw new ArgumentNullException();
            }

            var db = userStore.Context as ForumDBEntities;
            return db.UserData.Where(u => u.Id.ToLower() == Id.ToLower()).FirstOrDefaultAsync();
        }

        public Task<ApplicationUser> FindByNameAsync(string name)
        {
            if (String.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException();
            }

            var db = userStore.Context as ForumDBEntities;
            return db.UserData.Where(u => u.UserName == name).FirstOrDefaultAsync();
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
    }
}