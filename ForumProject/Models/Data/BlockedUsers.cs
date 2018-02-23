namespace ForumProject.Models.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BlockedUsers")]
    public partial class BlockedUsers
    {
        public int id { get; set; }

        public int UserId { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime BlockTime { get; set; }

        public virtual Users User { get; set; }
    }
}
