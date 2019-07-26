using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication41.Models
{
    public class MeetBook
    {
        public int MeetBookId { get; set; }
       /* public int Sender { get; set; }
        public int Recipient { get; set; }*/
        public long Sender { get; set; }
        public long Recipient { get; set; }
        public string InformationAboutMeeting { get; set; }
    }
}
