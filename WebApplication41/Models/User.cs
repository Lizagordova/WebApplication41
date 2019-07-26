using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication41.Models;
namespace WebApplication41.Models
{
    public class User
    {
        public int UserId { get; set; }
        public long TelegramId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }//здесь может быть другой тип данных?
        public string Work { get; set; }
        public string Position { get; set; }
        public string Usufulness { get; set; }
        public long Phone { get; set; }
        public Event CurrentEvent { get; set; }
        public string AboutTalkingWishes { get; set; }//о чём бы хотели пообщаться
        public string Username { get; set; }
        public List<UserTag> CurrentTagss { get; set; }
        public List<CurrentTagsAboutOthers> CurrentTagsAboutOtherss { get; set; }
        public List<Subtag> Subtags { get; set; }
        public List<CurrentSubtags> CurrentSubtagss { get; set; }
        public List<CurrentSubtagsAboutOthers> CurrentSubtagsAboutOtherss { get; set; }
        public List<OldEvent> OldEventss { get; set; }
        public List<CurrentEvent> CurrentEventss { get; set; }
        public List<Tag> CurrentTags { get; set; }
         public List<Subtag> CurrentSubtags { get; set; }
          public List<Tag> CurrentTagsAboutOthers { get; set; }
          public List<Subtag> CurrentSubtagsAboutOthers{ get; set; }
          public List<Event> OldEvents { get; set; }
        public User()
        {
            CurrentEventss = new List<CurrentEvent>();
            OldEventss = new List<OldEvent>();
            CurrentSubtagss = new List<CurrentSubtags>();
            CurrentTagss = new List<UserTag>();
            CurrentTagsAboutOtherss = new List<CurrentTagsAboutOthers>();
            CurrentSubtagsAboutOtherss = new List<CurrentSubtagsAboutOthers>();
        }
    }
}
