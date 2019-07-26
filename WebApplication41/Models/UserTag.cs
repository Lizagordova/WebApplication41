using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication41.Models
{
    public class UserTag
    {
        public int UserId { get; set; }
        public User user { get; set; }
        public int TagId { get; set; }
        public Tag tag { get; set; }
    }

    }
