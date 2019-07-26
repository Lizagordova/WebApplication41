using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication41.Models
{
    public class Subtag
    {
        public int SubtagId { get; set; }
        public string Name { get; set; }
        public int TagId { get; set; }
        public  Tag tag { get; set; }
        public List<CurrentSubtags> CurrentSubtagss { get; set; }
        public List<CurrentSubtagsAboutOthers> CurrentSubtagsAboutOtherss { get; set; }
        public Subtag()
        {
            CurrentSubtagss = new List<CurrentSubtags>();
            CurrentSubtagsAboutOtherss = new List<CurrentSubtagsAboutOthers>();
        }
        //  public List<User> Users { get; set; }
    }
}
