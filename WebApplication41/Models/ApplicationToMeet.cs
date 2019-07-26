using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication41.Models
{
    public class ApplicationToMeet
    {
        public int ApplicationToMeetId { get; set; }
       /* public int ToSend { get; set; }
        public int ToRecipe { get; set; }*/
        public long ToSend { get; set; }
        public long ToRecipe { get; set; }
    }
}
