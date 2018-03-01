using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using ForumProject.Models.Data;
using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ForumProject.Models.Identity
{
    public class ApplicationRoleStore : IQueryableRoleStore<ApplicationRole, Guid>
    {
        public IQueryable<ApplicationRole> Roles
        {
            get { return (roleStore.Context as ForumDBEntities).Roles; }
        }

        public ForumDBEntities DB
        {
            get { return roleStore.Context as ForumDBEntities; }
        }

        private RoleStore<IdentityRole> roleStore;

        public ApplicationRoleStore(ForumDBEntities context)
        {
            if (context == null)
            {
                throw new ArgumentNullException();
            }

            roleStore = new RoleStore<IdentityRole>(context);
        }

        public Task CreateAsync(ApplicationRole role)
        {
            if (role == null)
            {
                throw new ArgumentNullException();
            }

            DB.Roles.Add(role);
            DB.Configuration.ValidateOnSaveEnabled = false;
            return DB.SaveChangesAsync();
        }

        public Task DeleteAsync(ApplicationRole role)
        {
            if (role == null)
            {
                throw new ArgumentNullException();
            }

            DB.Roles.Remove(role);
            DB.Configuration.ValidateOnSaveEnabled = false;
            return DB.SaveChangesAsync();
        }

        public void Dispose()
        {
            roleStore.Context.Dispose();
        }

        public Task<ApplicationRole> FindByIdAsync(Guid roleId)
        {
            return DB.Roles.FindAsync(roleId);
        }

        public Task<ApplicationRole> FindByNameAsync(string roleName)
        {
            if (String.IsNullOrEmpty(roleName))
            {
                throw new ArgumentNullException();
            }

            return DB.Roles.Where(r => r.Name == roleName).FirstAsync();
        }

        public Task UpdateAsync(ApplicationRole role)
        {
            if (role == null)
            {
                throw new ArgumentNullException();
            }

            DB.Entry(role).State = EntityState.Added;
            return DB.SaveChangesAsync();
        }
    }
}