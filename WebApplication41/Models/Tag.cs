using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication41.Models
{
    public class Tag
    {
        public int TagId { get; set; }
        public string Name { get; set; }
        public List<UserTag> CurrentTagss { get; set; }
        public List<CurrentTagsAboutOthers> CurrentTagsAboutOtherss { get; set; }
        public ICollection<Subtag> Subtags { get; set; }
        public Tag()
        {
            CurrentTagss= new List<UserTag>();
            CurrentTagsAboutOtherss = new List<CurrentTagsAboutOthers>();
           // Subtags = new List<Subtag>();
        }


    }
}
