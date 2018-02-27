using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using ForumProject.Models.Data;

namespace ForumProject.Models
{
    public class Popular
    {
        //get 4 most popular records
        public static IEnumerable<Records> MostPopularRecords()
        {
            List<Records> records;
            using (ForumDBEntities entities = new ForumDBEntities())
            {
                records = entities.Records.Include(r => r.User).OrderByDescending(r => r.UsersWhoLike.Count).Take(4).ToList();
            }

            return records;
        }
    }
}