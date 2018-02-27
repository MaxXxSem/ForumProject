using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using ForumProject.Models.Data;


namespace ForumProject.Models
{
    public class ProfileInfo
    {
        //check and change user's level info
        public static void CheckLevelInfo(int userId)
        {
            using (ForumDBEntities entities = new ForumDBEntities())
            { 
                var user = entities.Users.Where(u => u.Id == userId).Include(u => u.Records).Include(u => u.LikedRecords).Include(u => u.Comments).FirstOrDefault();

                foreach (var level in entities.LevelInfo)
                {
                    if (user.Records.Count >= level.RecordsBarrier && user.LikedRecords.Count >= level.LikesBarrier && user.Comments.Count >= level.CommentsBarrier)
                    {
                        user.LevelInfo = level;
                    }
                }

                entities.SaveChanges();
            }
        }

        public static bool IsBlocked(string login)
        {
            bool result;
            using (ForumDBEntities entities = new ForumDBEntities())
            {
                var userRecord = entities.BlockedUsers.Where(u => u.UserId == entities.UserData.Where(us => us.UserName == login).FirstOrDefault().User.Id).First();

                if (userRecord == null || userRecord.BlockTime < DateTime.Now)
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

        public static void BlockUser(int id, DateTime date)
        {
            using (ForumDBEntities entities = new ForumDBEntities())
            {
                BlockedUsers user = new BlockedUsers()
                {
                    UserId = id,
                    BlockTime = date
                };

                entities.BlockedUsers.Add(user);
                entities.SaveChanges();
            }
        }
    }
}