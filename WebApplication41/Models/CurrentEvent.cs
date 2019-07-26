using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication41.Models
{
    public class CurrentEvent
    {
        public int UserId { get; set; }
        public User user { get; set; }
        public int EventId { get; set; }
        public Event eventt{ get; set; }
    }
}
