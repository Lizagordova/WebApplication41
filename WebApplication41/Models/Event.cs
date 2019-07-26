using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication41.Models
{
    public class Event
    {
        public int EventId { get; set; }
        public string NameOfEvent { get; set; }
        public string CodeForUsers { get; set; }
        public string CodeForOrganisators { get; set; }
        public string Information { get; set; }
        public string Reference { get; set; }
        public List<OldEvent> OldEventss { get; set; }
        public List<CurrentEvent> currentEvents { get; set; }
        public Event ()
        {
            currentEvents = new List<CurrentEvent>();
            OldEventss = new List<OldEvent>();
        }

    }
}
