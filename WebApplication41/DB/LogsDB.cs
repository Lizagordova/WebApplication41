using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication41.Models;

namespace WebApplication41.DB
{
    public class LogsDB
    {
        public LogsDB()
        {
            Db = new DB().Connect();
        }
        public readonly MyContext Db;

        public void AddLog(string log)
        {
            Logs logg = new Logs();
            logg.Logss = log;
            Db.Logs.Add(logg);
            Db.SaveChanges();
        }
    }
}
