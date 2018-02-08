using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ForumProject.Models
{
    public class SignInUp
    {
        public bool TrySignIn(string login, string password)
        {
            bool result;
            using (ForumDBEntities entities = new ForumDBEntities())
            {
                if (entities.Users.Any(u => u.Login == login && u.Password == password) && !entities.Users.Where(u => u.Login == login).First().BlockedUsers.Any(u => u.Users.Login == login))
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
            }

            return result;
        }

        public bool TrySignUp(string login)
        {
            bool result = true;
            using (ForumDBEntities entities = new ForumDBEntities())
            {
                if (entities.Users.Any(u => u.Login == login))
                {
                    result = false;
                }
                else
                {
                    result = true;
                }
            }

            return result;
        }
    }
}