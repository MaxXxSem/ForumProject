using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using ForumProject.Models.Data;


namespace ForumProject.Models
{
    public class Profile
    {
        //check and change user's level info
        public void CheckLevelInfo(ForumDBEntities entities, int userId)
        {
            if (entities == null)
            {
                return;
            }
            else
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
    }
}