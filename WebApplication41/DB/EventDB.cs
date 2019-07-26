using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication41.Models;

namespace WebApplication41.DB
{
    public class EventDB
    {

        public EventDB()
        {
            Db = new DB().Connect();
        }
        public readonly MyContext Db;
        public bool CheckCode(string Code)
        {
            if (Db.Events.FirstOrDefault(n => n.CodeForUsers == Code) != null)
                return true;
            return false;
        }
        public bool CheckCodeForOrganisators(string Code)
        {
            if(Db.Events.FirstOrDefault(n=>n.CodeForOrganisators==Code)!=null)
            {
                return true;
            }
            return false;
        }
        public string GetName(string Code)
        {
            string name = Db.Events.FirstOrDefault(n => n.CodeForUsers == Code).NameOfEvent;
            return name;
        }
        public string GetInformationAboutEventWithName(string name)
        {
            List<Event> events = Db.Events.ToList();
            LogsDB log = new LogsDB();
            string temp = "";
            foreach (var item1 in events)
                    {
                        if (item1.NameOfEvent == name)
                        {
                            if (item1.Information != null)
                            {
                                log.AddLog(item1.Information);
                                temp = item1.Information;
                            }
                            if (item1.Reference != null)
                            {
                                log.AddLog(item1.Reference);
                                temp = temp + item1.Reference;
                            }
                            if (temp == "") temp = "Информации об этом мероприятии ещё нет";

                        }
                }
            
            return temp;
        }
            public string GetInformationAboutEvent(long chatId)
        {
            User user = Db.Users.FirstOrDefault(n => n.TelegramId == chatId);
            List<Event> events = Db.Events.ToList();
            List<CurrentEvent> currentEvents = Db.CurrentEvents.ToList();
            LogsDB log = new LogsDB();
            string temp = "";
            foreach(var item in currentEvents)
            {
                if(item.UserId==user.UserId)
                {
                    foreach(var item1 in events)
                    {
                        if (item1.EventId == item.EventId)
                        {
                            if (item1.Information != null)
                            {
                                log.AddLog(item1.Information);
                                temp = item1.Information;
                            }
                            if (item1.Reference != null)
                            {
                                log.AddLog(item1.Reference);
                                temp = temp + item1.Reference;
                            }
                            if(temp=="") temp = "Информации об этом мероприятии ещё нет";
                          
                        }

                    }
                }
            }
            return temp;
        }
    }
}
