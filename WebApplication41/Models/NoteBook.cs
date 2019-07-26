using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication41.Models
{
    public class NoteBook
    {
        public int NoteBookId { get; set; }
  /*  public int Sender { get; set; } = 0;
       public int Recipient { get; set; } = 0;*/
       public long Sender { get; set; } = 0;
        public long Recipient { get; set; } = 0;
        public int EventId { get; set; } = 0;//03.07.19
        public int permissionForAllInformation { get; set; } = 0;
    }
}
