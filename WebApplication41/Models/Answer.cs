using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication41.Models
{
    public class Answer
    {
        public int AnswerId { get; set; }
        public long TelegramId { get; set; }
        public string Answers { get; set; }
       public DateTime time { get; set; }
        public int QuestionId { get; set; }
    }
}
