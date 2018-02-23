using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ForumProject.Models.Data;

namespace ForumProject.Models
{
    //Provides methods for login and registration
    public class SignInUp
    {
        //check user login and password
        public bool TrySignIn(string login, string password)
        {
            bool result;
            using (ForumDBEntities entities = new ForumDBEntities())
            {
                if (entities.Users.Any(u => u.Login == login && u.Password == password) && !entities.Users.Where(u => u.Login == login).First().BlockedUsers.Any(u => u.User.Login == login))
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

        //tries to sign up user
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