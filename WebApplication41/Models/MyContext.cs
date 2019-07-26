using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace WebApplication41.Models
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions<MyContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<CurrentSubtags>().HasKey(t => new { t.UserId, t.SubtagId });
            modelBuilder.Entity<CurrentSubtags>().HasOne(sc => sc.user).WithMany(t => t.CurrentSubtagss).HasForeignKey(m => m.UserId);
            modelBuilder.Entity<CurrentSubtags>().HasOne(sc => sc.subtag).WithMany(t => t.CurrentSubtagss).HasForeignKey(m => m.SubtagId);
            modelBuilder.Entity<UserTag>()
                .HasKey(t => new { t.UserId, t.TagId });

            modelBuilder.Entity<UserTag>()
                .HasOne(sc => sc.user)
                .WithMany(s => s.CurrentTagss)
                .HasForeignKey(sc => sc.UserId);

            modelBuilder.Entity<UserTag>()
                .HasOne(sc => sc.tag)
                .WithMany(c => c.CurrentTagss)
                .HasForeignKey(sc => sc.TagId);



            modelBuilder.Entity<CurrentTagsAboutOthers>().
                HasKey(t => new { t.UserId, t.TagId });
            modelBuilder.Entity<CurrentTagsAboutOthers>()
              .HasOne(sc => sc.user)
              .WithMany(s => s.CurrentTagsAboutOtherss)
              .HasForeignKey(sc => sc.UserId);

            modelBuilder.Entity<CurrentTagsAboutOthers>()
                .HasOne(sc => sc.tag)
                .WithMany(c => c.CurrentTagsAboutOtherss)
                .HasForeignKey(sc => sc.TagId);
            modelBuilder.Entity<CurrentSubtagsAboutOthers>().HasKey(t => new { t.UserId, t.SubtagId });
            modelBuilder.Entity<CurrentSubtagsAboutOthers>().HasOne(sc => sc.user).WithMany(s => s.CurrentSubtagsAboutOtherss).HasForeignKey(sc => sc.UserId);
            modelBuilder.Entity<CurrentSubtagsAboutOthers>().HasOne(sc => sc.subtag).WithMany(s => s.CurrentSubtagsAboutOtherss).HasForeignKey(sc => sc.SubtagId);

            modelBuilder.Entity<CurrentEvent>().HasKey(t => new { t.UserId, t.EventId });
            modelBuilder.Entity<CurrentEvent>().HasOne(sc => sc.user).WithMany(s => s.CurrentEventss).HasForeignKey(sc => sc.UserId);
            modelBuilder.Entity<CurrentEvent>().HasOne(sc => sc.eventt).WithMany(s => s.currentEvents).HasForeignKey(sc => sc.EventId);

            modelBuilder.Entity<OldEvent>().HasKey(t => new { t.UserId, t.EventId });
            modelBuilder.Entity<OldEvent>().HasOne(sc => sc.user).WithMany(s => s.OldEventss).HasForeignKey(sc => sc.UserId);
            modelBuilder.Entity<OldEvent>().HasOne(sc => sc.eventt).WithMany(s => s.OldEventss).HasForeignKey(sc => sc.EventId);

            modelBuilder.Entity<Subtag>()
                 .HasOne(t => t.tag)
                 .WithMany(w => w.Subtags)
                 .HasForeignKey(t => t.TagId);
            modelBuilder.Entity<CurrentActions>().Property(b => b.AboutEvent).HasDefaultValue(0);
            modelBuilder.Entity<CurrentActions>().Property(b => b.CheckEmail).HasDefaultValue(0);
            modelBuilder.Entity<CurrentActions>().Property(b => b.Email).HasDefaultValue(0);
            modelBuilder.Entity<CurrentActions>().Property(b => b.EnterWithEventCode).HasDefaultValue(0);
            modelBuilder.Entity<CurrentActions>().Property(b => b.EventCode).HasDefaultValue(0);
            modelBuilder.Entity<CurrentActions>().Property(b => b.NameAndLastName).HasDefaultValue(0);
            modelBuilder.Entity<CurrentActions>().Property(b => b.Networking).HasDefaultValue(0);
            modelBuilder.Entity<CurrentActions>().Property(b => b.NetworkingFull).HasDefaultValue(0);
            modelBuilder.Entity<CurrentActions>().Property(b => b.Position).HasDefaultValue(0);
            modelBuilder.Entity<CurrentActions>().Property(b => b.PrivateCabinet).HasDefaultValue(0);
            modelBuilder.Entity<CurrentActions>().Property(b => b.Work).HasDefaultValue(0);
            base.OnModelCreating(modelBuilder);
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Subtag> Subtags { get; set; }
        public DbSet<CurrentEvent> CurrentEvents { get; set; }
        public DbSet<Logs> Logs { get; set; }
        public DbSet<UserTag> UserTag { get; set; }
        public DbSet<CurrentTagsAboutOthers> CurrentTagsAboutOthers { get; set; }
        public DbSet<CurrentSubtags> CurrentSubtags { get; set; }
        public DbSet<CurrentSubtagsAboutOthers> CurrentSubtagsAboutOthers { get; set; }
        public DbSet<NoteBook> NoteBooks { get; set; }
        public DbSet<Applications> Applications{ get; set; }
        public DbSet<ApplicationToMeet> ApplicationsToMeet { get; set; }
        public DbSet<MeetBook> MeetBooks { get; set; }
        public DbSet<OldEvent> OldEvents { get; set; }
        public DbSet<CurrentActions> CurrentActions { get; set; }
        public DbSet <Survey> Survey { get; set; }
        public DbSet<Question> Question { get; set; }
        public DbSet<Answer> Answer { get; set; } 

    }
    
}
