using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication41.Models
{
    public class Applications
    {
        public int ApplicationsId { get; set; }
       /* public int ToSend { get; set; } = 0;
        public int ToRecipe { get; set; } = 0;*/
       public long ToSend { get; set; } = 0;
        public long ToRecipe { get; set; } = 0;
        public int EventId { get; set; } = 0;
    }
}
