using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ForumProject.Models
{
    public class PageInfo
    {
        public const int pageSize = 7;

        public enum PagedAction
        {
            Index,
            Search,
            SubtopicsRecords
        };
    }
}