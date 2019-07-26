using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication41.Models
{
    public class CurrentSubtagsAboutOthers
    {
        public int UserId { get; set; }
        public User user { get; set; }
       
        public int SubtagId { get; set; }
        public Subtag subtag { get; set; }
    }
}
