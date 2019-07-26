using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication41.Models;

namespace WebApplication41.DB
{
    public class DB
    {
        public MyContext Connect()
        {
            var optionsBuilder = new DbContextOptionsBuilder<MyContext>();
            // optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=eventbot;Trusted_Connection=True;MultipleActiveResultSets=true");
           optionsBuilder.UseSqlServer("Server=localhost;Database=u0641156_bot;User Id=u0641156_bot;Password=ReportBot123!");
            return new MyContext(optionsBuilder.Options);
        }
    }
}
