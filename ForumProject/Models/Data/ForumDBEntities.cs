namespace ForumProject.Models.Data
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using ForumProject.Models.Identity;

    public partial class ForumDBEntities : DbContext
    {
        public ForumDBEntities()
            : base("name=ForumDBEntities")
        {
        }

        public virtual DbSet<AccessLevel> AccessLevel { get; set; }
        public virtual DbSet<BlockedUsers> BlockedUsers { get; set; }
        public virtual DbSet<Comments> Comments { get; set; }
        public virtual DbSet<LevelInfo> LevelInfo { get; set; }
        public virtual DbSet<Records> Records { get; set; }
        public virtual DbSet<Subtopics> Subtopics { get; set; }
        public virtual DbSet<Topics> Topics { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<ApplicationUser> UserData { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AccessLevel>()
                .HasMany(e => e.Users)
                .WithRequired(e => e.AccessLevel)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Comments>()
                .HasMany(e => e.UsersWhoLike)
                .WithMany(e => e.LikedComments)
                .Map(m => m.ToTable("CommentsLikes").MapLeftKey("CommentId").MapRightKey("UserId"));

            modelBuilder.Entity<LevelInfo>()
                .HasMany(e => e.Users)
                .WithRequired(e => e.LevelInfo)
                .HasForeignKey(e => e.LevelId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Records>()
                .HasMany(e => e.UsersWhoLike)
                .WithMany(e => e.LikedRecords)
                .Map(m => m.ToTable("RecordsLikes").MapLeftKey("RecordId").MapRightKey("UserId"));

            modelBuilder.Entity<Subtopics>()
                .HasMany(e => e.Records)
                .WithRequired(e => e.Subtopic)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Topics>()
                .HasMany(e => e.Subtopics)
                .WithRequired(e => e.Topic)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Users>()
                .Property(e => e.Password)
                .IsFixedLength();

            modelBuilder.Entity<Users>()
                .HasMany(e => e.Comments)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.UserId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Users>()
                .HasMany(e => e.Records)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<Users>()
                .HasMany(e => e.Subscriptions)
                .WithMany(e => e.Subscribers)
                .Map(m => m.ToTable("Subscribes").MapLeftKey("SubscriberId").MapRightKey("PublisherId"));

            modelBuilder.Entity<ApplicationUser>()
                .HasRequired(u => u.User)
                .WithOptional()
                .Map(u => u.MapKey("UserId"));
        }

        public static ForumDBEntities Create()
        {
            return new ForumDBEntities();
        }
    }
}
