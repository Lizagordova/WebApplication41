using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication41.Models;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Net;

namespace WebApplication41.DB
{
    public class UserDB
    {
        public UserDB()
        {
            Db = new DB().Connect();
        }
        public readonly MyContext Db;
        public string GetUserTagsAboutOthers(long chatId)
        {
            string temp = "";
            User user = Db.Users.FirstOrDefault(n => n.TelegramId == chatId);
            List<CurrentTagsAboutOthers> currentTagsAboutOthers = Db.CurrentTagsAboutOthers.ToList();
            foreach(var item in currentTagsAboutOthers)
            {
                if(user.UserId==item.UserId)
                {
                    temp = temp + Db.Tags.FirstOrDefault(n => n.TagId == item.TagId).Name;
                }
            }
            return temp;
        }
        public string GetUserTags(long chatId)//ПОЛУЧЕНИЕ ТЕКУЩИХ ТЕГОВ ЮЗЕРА
        {
            LogsDB log = new LogsDB();
            string temp="";
            User user = Db.Users.FirstOrDefault(n => n.TelegramId == chatId);
            List<UserTag> userTags = Db.UserTag.ToList();
             foreach(var item in userTags)
            {
                if (user.UserId==item.UserId)
                {

                    log.AddLog("getusertags2");
                    temp = temp + Db.Tags.FirstOrDefault(n => n.TagId == item.TagId).Name+"\n";
                   // log.AddLog(temp);
                    log.AddLog("getusertags3");
                }
            }
            log.AddLog(temp);
            return temp;
        }
       public List<string> UsersChosenTags(long chatId)//03.07.19
        {
            User user = Db.Users.FirstOrDefault(n => n.TelegramId == chatId);
           List<UserTag> userTags= Db.UserTag.ToList();
            List<string> chosenTags = new List<string>();
            List<Tag> tags = Db.Tags.ToList();
            foreach(var item in userTags)
            {
                foreach (var item1 in tags)
                {
                    if (item.TagId==item1.TagId && item.UserId==user.UserId)
                    {
                        chosenTags.Add(item1.Name);
                    }
                }
                
            }
            return chosenTags;
        }
        public List<string> UsersChosenTagsAboutOthers(long chatId)
        {
            User user = Db.Users.FirstOrDefault(n => n.TelegramId == chatId);
            List<CurrentTagsAboutOthers> currentTagsAbouts = Db.CurrentTagsAboutOthers.ToList();
            List<string> ChosenTags = new List<string>();
            List<Tag> tags = Db.Tags.ToList();
            log.AddLog(user.UserId.ToString());
            foreach(var item in currentTagsAbouts)
            {   if (item.UserId == user.UserId)
                {
                    foreach (var item1 in tags)
                    {
                        if (item1.TagId == item.TagId)
                        {
                            ChosenTags.Add(item1.Name);
                            log.AddLog(item1.Name);
                        }
                    }
                }
            }
            return ChosenTags;
        }
        public string GetAllSubtags(List<string> tags)//ПОЛУЧИТЬ ВСЕ ПОДТЕГИ 03.07.19
        {

            string temp = "";
            log.AddLog("getallsubtags0");
            List<Subtag> subtags = Db.Subtags.ToList();
            List<Tag> tagss = Db.Tags.ToList();
            List<Tag> tagsss = new List<Tag>();
            foreach(var item in tags)
            {
                log.AddLog(item);
                foreach(var item1 in tagss)
                {
                    if(item==item1.Name)
                    {
                        tagsss.Add(item1);
                    }
                }
            }
            int count = 1;
                foreach(var item1 in tagsss)
                {
                foreach (var item in  subtags)
                {
                    if (item.TagId == item1.TagId)
                    {
                        temp = temp + count.ToString() + ")" + item.Name + "\n";
                        count++;
                    }
                }
            }
           return temp;
        }
        public string UsersChosenTag(long chatID)
        {
            string temp = "";
            User user = Db.Users.FirstOrDefault(n => n.TelegramId == chatID);
             List<UserTag> userTags = Db.UserTag.ToList();
            List<Tag> tags = Db.Tags.ToList();
            foreach(var item in userTags)
            {
                log.AddLog("chosentag2");
                if (user.UserId == item.UserId)
                {
                    foreach (var item1 in tags)
                    {
                        log.AddLog("chosentag3");
                        if (item1.TagId == item.TagId) temp = item1.Name;
                    }
                }
                
            }
            return temp;
        }
        public void DeleteSubtagFromCurrentSubtags(long chatId, string value)
        {
            User USER = Db.Users.FirstOrDefault(n => n.TelegramId == chatId);
            Subtag subtag = Db.Subtags.FirstOrDefault(n => n.Name == value);
            foreach (var item in Db.CurrentSubtags)
            {
                if (item.UserId == USER.UserId && item.SubtagId == subtag.SubtagId)
                    Db.CurrentSubtags.Remove(item);
            }
            Db.SaveChanges();
        }
        public string UsersChosenTagAboutOthers(long chatID)
        {
            string temp = "";
            User user = Db.Users.FirstOrDefault(n => n.TelegramId == chatID);
            List<CurrentTagsAboutOthers> currentTagsAboutOthers = Db.CurrentTagsAboutOthers.ToList();
            List<Tag> tags = Db.Tags.ToList();
            foreach (var item in currentTagsAboutOthers)
            {if (item.UserId == user.UserId)
                {
                    foreach (var item1 in tags)
                    {
                        if (item1.TagId == item.TagId) temp = item1.Name;
                    }
                }

            }
            return temp;
        }
        /*   public Dictionary<int,string>GetAllSubtagsList(string name)//ВСЕ ПОДТЕГИ ТЕГА
           {
               List<Subtag> subtags = Db.Subtags.ToList();
               log.AddLog("list0");
               List<Tag> tags = Db.Tags.ToList();
               Tag tag = new Tag();
               foreach(var item in tags)
               {
                   if (item.Name == name)
                       tag = item;
               }
              // Tag tag = Db.Tags.FirstOrDefault(n => n.Name == name);
               log.AddLog("list1");
               Dictionary<int,string> back = new Dictionary<int, string>();
               log.AddLog("list2");
               int count = 1;
               foreach (var item in subtags)
               {
                   log.AddLog(item.TagId.ToString());
                   log.AddLog(tag.TagId.ToString());
                   if (item.tag.TagId == tag.TagId)
                   {
                       back.Add(count, item.Name);
                       count++;
                   }
               }
               log.AddLog("list3");
               return back;
           }*/
        public Dictionary<int, string> GetAllSubtagsList(List<string> tagss)//ВСЕ ПОДТЕГИ ТЕГА
        {
            List<Subtag> subtags = Db.Subtags.ToList();
            List<Tag> tags = Db.Tags.ToList();
            Tag tag = new Tag();
            List<Tag> chosenTags= new List<Tag>();
            foreach (var item in tags)
            {
                foreach (var item1 in tagss)
                {
                    if (item.Name == item1)
                        chosenTags.Add(item);
                }
            }
            Dictionary<int, string> back = new Dictionary<int, string>();
            int count = 1;
           foreach (var item in subtags)
            {
                foreach(var item1 in chosenTags)
                {
                    if(item1.TagId==item.TagId)
                    {
                        log.AddLog(count.ToString());
                        log.AddLog(item.Name);
                        back.Add(count, item.Name);
                        count++;
                    }
                }                
            }

            return back;
        }
        
        public void RecreateUser(long chatId)
        {
            LogsDB log = new LogsDB();
            if (!CheckUser(chatId)) return;
            log.AddLog("recreate0");
            User user = Db.Users.FirstOrDefault(n => n.TelegramId == chatId);
            log.AddLog("recreate00");
            CurrentActions currentAction = Db.CurrentActions.FirstOrDefault(n => n.TelegramId == chatId);
            log.AddLog("recreate0000");
            int userId = user.UserId;
             bool eventExist = false;
            Event eventt = new Event();
           List<Event> events = Db.Events.ToList();
            List<CurrentEvent> currentEvents = Db.CurrentEvents.ToList();
            log.AddLog("recreate1");
            foreach (var item in currentEvents)
            {
                if(item.UserId==userId)
                {
                    eventt = item.eventt;
                    eventExist = true;
                }
            }

            log.AddLog("recreate2");
            string email = user.Email;
            string name = user.Name;
            string wishes = user.AboutTalkingWishes;
            string work = user.Work;
            string position = user.Position;
            string usefulness = user.Usufulness;
            UserTag userTag = new UserTag();
            log.AddLog("recreate4");
            List<CurrentTagsAboutOthers> currentTagsAboutOthers = new List<CurrentTagsAboutOthers>();
            List<CurrentSubtagsAboutOthers> currentSubtagsAboutOthers = new List<CurrentSubtagsAboutOthers>();
            List<CurrentSubtags> currentsubtags = new List<CurrentSubtags>();
            List<OldEvent> oldEvents = new List<OldEvent>();
            Tag tag=new Tag();
            bool temp = false;
            log.AddLog("recreate5");
            if (Db.UserTag.FirstOrDefault(n => n.UserId == userId) != null)
            {
                temp = true;
                userTag = Db.UserTag.FirstOrDefault(n => n.UserId == userId);
                tag = Db.Tags.FirstOrDefault(n => n.TagId == userTag.TagId);
            }
            bool tempSubtags = false;
          
            if(Db.CurrentSubtags.FirstOrDefault(n=>n.UserId==userId)!=null)
            {
                tempSubtags = true;
                List<CurrentSubtags> currentSubtags = Db.CurrentSubtags.ToList();
                foreach (var item in currentSubtags)
                {
                    if (item.UserId == userId)
                    {
                          CurrentSubtags subtag = Db.CurrentSubtags.FirstOrDefault(n => n.SubtagId == item.SubtagId);
                        currentsubtags.Add(subtag);
                    }
                }                
            }
            log.AddLog("recreate6");
             bool tempTagAboutOthers = false;

             if(Db.CurrentTagsAboutOthers.FirstOrDefault(n=>n.UserId==userId)!=null)
             {
                 tempTagAboutOthers = true;
                List<CurrentTagsAboutOthers> currentTagsAboutOther = Db.CurrentTagsAboutOthers.ToList();
                 foreach(var item in currentTagsAboutOther)
                 {
                     if(item.UserId==userId)
                     {
                         CurrentTagsAboutOthers currentTagAboutOthers = item;
                         currentTagsAboutOthers.Add(currentTagAboutOthers);
                     }
                 }
             }
            log.AddLog("recreate7");
            bool tempOldEvents = false;
            if(Db.OldEvents.FirstOrDefault(n=>n.UserId==userId)!=null)
            {
                tempOldEvents = true;
                List<OldEvent> oldEvent= Db.OldEvents.ToList();
                foreach(var item in oldEvent)
                {
                    log.AddLog(item.UserId.ToString());
                    log.AddLog(userId.ToString());
                    if(item.UserId==userId)
                    {

                        OldEvent oldEventt= item;
                        oldEvents.Add(item);
                    }
                }
            }
            log.AddLog("recreate18");
            bool notebook = false;
            bool meetbook = false;
            List<NoteBook> noteBooks = new List<NoteBook>();
            List<MeetBook> meetBooks = new List<MeetBook>();
            if(Db.NoteBooks.FirstOrDefault(n=>n.Sender==chatId)!=null)
            {
                notebook = true;
                List<NoteBook> noteBookss = Db.NoteBooks.ToList();
                foreach (var item in noteBookss)
                {
                  if(item.Sender==user.UserId)
                    {
                       
                        noteBooks.Add(item);
                        Db.NoteBooks.Remove(item);
                    }
                }
            }
            log.AddLog("recreate12");
            if (Db.MeetBooks.FirstOrDefault(n => n.Sender == chatId)!= null)
            {
                meetbook = true;
                List<MeetBook> meetBookss = Db.MeetBooks.ToList();
                foreach (var item in meetBookss)
                {
                    if (item.Sender == user.UserId)
                    {

                        meetBooks.Add(item);
                        Db.MeetBooks.Remove(item);
                    }
                }
            }
             bool tempSubtagsAboutOthers = false;
              if(Db.CurrentSubtagsAboutOthers.FirstOrDefault(n=>n.UserId==userId)!=null)
              {
                  tempSubtagsAboutOthers = true;
                List<CurrentSubtagsAboutOthers> currentSubtagsAboutOtherss = Db.CurrentSubtagsAboutOthers.ToList();
                  foreach(var item in currentSubtagsAboutOtherss)
                  {
                      if(item.UserId==userId)
                      {
                          CurrentSubtagsAboutOthers currentSubtagAboutOthers = item;
                          currentSubtagsAboutOthers.Add(item);
                      }
                  }
              }
            Db.CurrentActions.Remove(currentAction);
            Db.Users.Remove(user);
            Db.SaveChanges();
            CreateUser(chatId);
            log.AddLog("recreate3");
            User user1 = Db.Users.FirstOrDefault(u => u.TelegramId == chatId);
            user1.Email = email;
            user1.Name = name;
            user1.AboutTalkingWishes = wishes;
            user1.Work = work;
            user1.Position = position;
            user1.Usufulness = usefulness;

            if (temp)
            {
                user1.CurrentTagss.Add(new UserTag { UserId = user1.UserId, TagId = tag.TagId });
            }
            if (tempSubtags)
            {
                foreach (var item in currentsubtags)
                {
                    user1.CurrentSubtagss.Add(new CurrentSubtags { UserId = user1.UserId, SubtagId = item.SubtagId });
                }
            }
             if(tempTagAboutOthers)
             {
                 foreach(var item in currentTagsAboutOthers)
                 {
                     user1.CurrentTagsAboutOtherss.Add(new CurrentTagsAboutOthers { UserId = user1.UserId, TagId = item.TagId });
                 }
             }
             if(eventExist)
            {
                user1.CurrentEvent = eventt;
                user1.CurrentEventss.Add(new CurrentEvent { UserId = user.UserId, EventId = eventt.EventId });
            }
             if(tempOldEvents)
            {
                log.AddLog("tempOldEvents0");
                foreach(var item in oldEvents)
                {
                    log.AddLog("tempOldEvents1");
                    user1.OldEventss.Add(item);
                    log.AddLog("tempOldEvents2"); ;
                }

            }
             if(notebook)
            {
                log.AddLog("recreate notebook0");
                foreach(var item in noteBooks)
                {
                    log.AddLog("recreate notebook1");
                    NoteBook notebookNew = new NoteBook();
                    notebookNew.Recipient = item.Recipient;
                    notebookNew.Sender = item.Sender;
                    notebookNew.EventId = item.EventId;
                    Db.NoteBooks.Add(notebookNew);
                    log.AddLog("recreate notebook2");
                    log.AddLog("recreate notebook3");
                }
            }
            if (meetbook)
            {
                foreach (var item in meetBooks)
                {
                    Db.MeetBooks.Add(item);
                }
            }
             if(tempSubtagsAboutOthers)
             {
                 foreach(var item in currentSubtagsAboutOthers)
                 {
                     user1.CurrentSubtagsAboutOtherss.Add(new CurrentSubtagsAboutOthers { UserId = user1.UserId, SubtagId = item.SubtagId });
                 }
             }
            log.AddLog("recreate7");
            Db.SaveChanges();
            log.AddLog("recreate8");
        }
        public void CreateUser(long userId)
        {
            if (CheckUser(userId)) return;
            User user = new User { TelegramId = userId };
            CurrentActions currentAction = new CurrentActions { TelegramId =userId };
            Db.CurrentActions.Add(currentAction);
            Db.Users.Add(user);
            Db.SaveChanges();
        }
        public bool CheckUser(long userId)
        {
            if (Db.Users.FirstOrDefault(n => n.TelegramId == userId) != null)
                return true;
            return false;
         }
       
             public void AddElement(long chatId,string type,string parametr)
        {
            switch(type)
            {
               
                case "InformationAboutEvent":
                    {
                        User user = Db.Users.FirstOrDefault(n => n.TelegramId == chatId);
                        List<CurrentEvent> currentEvents = Db.CurrentEvents.ToList();
                        List<Event> events = Db.Events.ToList();
                        Event eventt = new Event();
                        foreach(var item in currentEvents)
                        {
                            if(item.UserId==user.UserId)
                            {
                                foreach(var item1 in events)
                                {
                                    if (item1.EventId == item.EventId)
                                        eventt = item1;
                                }
                            }
                        }
                        eventt.Information = parametr;
                        log.AddLog("aaddinformation1");
                        Db.Update(eventt);
                        log.AddLog("aaddinformation2");
                        log.AddLog("aaddinformation0");
                        Db.SaveChanges();
                        log.AddLog("aaddinformation");
                    }
                    break;
                case "Name":
                    {
                        User user = Db.Users.FirstOrDefault(n => n.TelegramId == chatId);
                        user.Name = parametr;
                        Db.Users.Update(user);
                        Db.SaveChanges();
                    }
                    break;
                case "Username":
                    {
                        User user = Db.Users.FirstOrDefault(n => n.TelegramId == chatId);
                        user.Username= parametr;
                        Db.Users.Update(user);
                        Db.SaveChanges();
                    }
                    break;
                case "Usefulness":
                    {
                        User user = Db.Users.FirstOrDefault(n => n.TelegramId == chatId);
                        user.Usufulness = parametr;
                        Db.Users.Update(user);
                        Db.SaveChanges();
                    }
                    break;
                case "Email":
                    {
                        User user = Db.Users.FirstOrDefault(n => n.TelegramId == chatId);
                        user.Email = parametr;
                        Db.Users.Update(user);
                        Db.SaveChanges();
                    }
                    break;
                case "Code":
                    {
                        User user = Db.Users.FirstOrDefault(n => n.TelegramId == chatId);
                        log.AddLog("code0");
                        Event even = new Event();
                        if (Db.Events.FirstOrDefault(n => n.CodeForUsers == parametr) != null)
                        {

                            even = Db.Events.FirstOrDefault(n => n.CodeForUsers == parametr);
                            log.AddLog("code1");
                        }
                        else if(Db.Events.FirstOrDefault(n => n.CodeForOrganisators == parametr) != null)
                        {
                            even = Db.Events.FirstOrDefault(n => n.CodeForOrganisators== parametr);
                            log.AddLog("code2");
                        }
                        user.CurrentEvent = even;
                        log.AddLog("code33");
                        Db.CurrentEvents.Add(new CurrentEvent { UserId = user.UserId, EventId = even.EventId });
                           log.AddLog("code3");
                       
                        log.AddLog("code4");
                        Db.SaveChanges();
                        log.AddLog("code5");
                    }
                    break;
                case "AllEvents":
                    {
                        log.AddLog("allevents0");
                        User user = Db.Users.FirstOrDefault(n => n.TelegramId == chatId);
                        log.AddLog("allevents1");
                        Event even = Db.Events.FirstOrDefault(n => n.CodeForUsers == parametr);
                        log.AddLog("allevents2");
                        
                        user.OldEventss.Add(new OldEvent { UserId = user.UserId, EventId = even.EventId });
                        log.AddLog("allevents3");
                        Db.Users.Update(user);
                        log.AddLog("allevents4");
                        Db.SaveChanges();
                        log.AddLog("allevents5");
                    }
                    break;
                case "Work":
                    {
                        User user = Db.Users.FirstOrDefault(n => n.TelegramId == chatId);
                        user.Work = parametr;
                        Db.Users.Update(user);
                        Db.SaveChanges();
                    }
                    break;
                case "Position":
                    {
                        User user = Db.Users.FirstOrDefault(n => n.TelegramId == chatId);
                        user.Position= parametr;
                        Db.Users.Update(user);
                        Db.SaveChanges();
                    }
                    break;
                case "AboutTalkingWishes":
                    {
                        User user = Db.Users.FirstOrDefault(n => n.TelegramId == chatId);
                        user.AboutTalkingWishes = parametr;
                        Db.Users.Update(user);
                        Db.SaveChanges();
                    }
                    break;
                case "CurrentTagss":
                    {
                       
                       User user = Db.Users.FirstOrDefault(n => n.TelegramId == chatId);
                        Tag tag = new Tag();
                        List<Tag> tags = Db.Tags.ToList();
                        foreach(var item in tags)
                        {
                            if (item.Name == parametr)
                                tag = item;
                        }
                        List<UserTag> currentTags = Db.UserTag.ToList();
                        UserTag userTag = new UserTag();
                        bool exist = false;
                        foreach(var item in currentTags)
                        {
                            if (item.UserId == user.UserId && item.TagId == tag.TagId)
                            {
                                exist = true;
                                userTag = item;
                            }
                                
                        }
                        if (exist == true)
                        {
                            Db.UserTag.Remove(userTag);
                        }
                         if (exist == false)
                            Db.UserTag.Add(new UserTag { UserId = user.UserId, TagId = tag.TagId });
                      // user.CurrentTagss.Add( });
                       //Db.Users.Update(user);
                       Db.SaveChanges();
                    }
                   break;
                case "CurrentTagAboutOtherss":
                    {
                        User user = Db.Users.FirstOrDefault(n => n.TelegramId == chatId);
                         Tag tag = Db.Tags.FirstOrDefault(N => N.Name == parametr);
                        bool exist = false;
                        List<CurrentTagsAboutOthers> currentTagsAboutOthers = Db.CurrentTagsAboutOthers.ToList();
                        CurrentTagsAboutOthers currentTagsAboutOther = new CurrentTagsAboutOthers();
                        foreach(var item in currentTagsAboutOthers)
                        {
                            if (item.UserId == user.UserId && item.TagId == tag.TagId)
                            {
                                exist = true;
                                currentTagsAboutOther=item;
                            }
                        }
                        if (exist == true)
                            Db.CurrentTagsAboutOthers.Remove(currentTagsAboutOther);
                        //if (exist == false)
                            Db.CurrentTagsAboutOthers.Add(new CurrentTagsAboutOthers { UserId = user.UserId, TagId = tag.TagId });
                      /* user.CurrentTagsAboutOtherss.Add(new CurrentTagsAboutOthers { UserId = user.UserId, TagId = tag.TagId });
                         Db.Users.Update(user);*/
                        Db.SaveChanges();
                     }
                    break;
                case "CurrentSubtagss":
                    {
                        User user = Db.Users.FirstOrDefault(n => n.TelegramId == chatId);
                         Subtag subtag = new Subtag();
                        List<Subtag> subtags = Db.Subtags.ToList();
                        List<CurrentSubtags> currentSubtags = Db.CurrentSubtags.ToList();
                        bool exist = false;
                        foreach(var item in subtags)
                        {
                            foreach(var item1 in currentSubtags)
                            {
                                if(item.Name==parametr)
                                {
                                    subtag = item;
                                    if (item1.UserId == user.UserId && item1.SubtagId == item.SubtagId) exist = true;
                                }
                            }
                            if (item.Name == parametr)
                                subtag = item;
                        }
                       // Subtag subtag = Db.Subtags.FirstOrDefault(n => n.Name == parametr);
                        if(exist==false)
                        Db.CurrentSubtags.Add(new CurrentSubtags { UserId = user.UserId, SubtagId = subtag.SubtagId });
                       // Db.Users.Update(user);
                        Db.SaveChanges();
                    }
                    break;
                case "CurrentSubtagsAboutOtherss":
                    {
                        User user = Db.Users.FirstOrDefault(N => N.TelegramId == chatId);
                        Subtag subtag = Db.Subtags.FirstOrDefault(N => N.Name == parametr);
                        List<Subtag> subtags = Db.Subtags.ToList();
                        List<CurrentSubtagsAboutOthers> currentSubtags = Db.CurrentSubtagsAboutOthers.ToList();
                        bool exist = false;
                        foreach(var item in subtags)
                        {
                            foreach(var item1 in currentSubtags)
                            {
                               
                                    if (item.Name == parametr)
                                    {
                                        subtag = item;
                                        if (item1.UserId == user.UserId && item1.SubtagId == item.SubtagId) exist = true;
                                    }
                                
                                if (item.Name == parametr)
                                    subtag = item;
                            }
                        }
                        if(exist==false)
                         
                        Db.CurrentSubtagsAboutOthers.Add(new CurrentSubtagsAboutOthers {UserId = user.UserId, SubtagId = subtag.SubtagId });
                        //Db.Users.Update(user);
                        Db.SaveChanges();
                    }
                    break;
                case "Survey":
                    {
                        Survey survey = new Survey();
                        survey.Name = parametr;
                        Db.Survey.Add(survey);
                       // Db.Survey.Update(survey);
                        Db.SaveChanges();
                    }
                    break;
                case "AddQuestionToSurvey":
                    {
                        Question question = new Question();
                        CurrentActions currentUser = Db.CurrentActions.FirstOrDefault(n => n.TelegramId == chatId);
                        question.SurveyId = currentUser.NumberOfSurvey;
                        question.Questions = parametr;
                        Db.Question.Add(question);
                        Db.SaveChanges();
                    }
                    break;
               }
        }
        LogsDB log = new LogsDB();
        public void ReplaceEvent(long chatId,string code)//03.07.19
        {
            log.AddLog("replace0");
            User user = Db.Users.FirstOrDefault(n => n.TelegramId == chatId);
            Event eventt=new Event();
            CurrentEvent eventToRemove = new CurrentEvent();
            List<Event> events = Db.Events.ToList();
            List<CurrentEvent> currentEvents = Db.CurrentEvents.ToList();
            foreach(var item in currentEvents)
            {
                if(item.UserId==user.UserId)
                {
                    foreach(var item1 in events)
                    {
                        if(item1.EventId==item.EventId)
                        {
                            log.AddLog("replace12");
                            log.AddLog(item.EventId.ToString());
                            eventToRemove = item;
                        }
                    }
                }
            }
            foreach (var item in events)
            {
                log.AddLog("replace13");
                if (item.CodeForUsers.ToLower() == code.ToLower())
                {
                    log.AddLog("replace14");
                    eventt = item;
                }
                if (item.CodeForOrganisators.ToLower() == code.ToLower())
                    eventt = item;
            }
            List<OldEvent> oldEvents = Db.OldEvents.ToList();
            bool oldeventExist = false;
            foreach(var item in oldEvents)
            {
                if (item.EventId == eventToRemove.EventId && item.UserId == user.UserId)
                    oldeventExist = true;                    
            }
            if (oldeventExist == false)
            {
                log.AddLog("oldevent false");
                Db.OldEvents.Add(new OldEvent { UserId = user.UserId, EventId = eventToRemove.EventId });
            }
            //user.OldEventss.Add(new OldEvent { UserId = user.UserId, EventId = eventToRemove.EventId });
            log.AddLog("replace16");
            Db.CurrentEvents.Remove(eventToRemove);
            Db.CurrentEvents.Add(new CurrentEvent { UserId = user.UserId, EventId = eventt.EventId });
         //  user.CurrentEventss.Add(new CurrentEvent { UserId = user.UserId, EventId = eventt.EventId });
            log.AddLog("replace17");
           
                      
            log.AddLog("replace19");
            Db.SaveChanges();
            log.AddLog("replace20");
        }
        public void ReplaceEventWithName(long chatId,string nameOfEvent)
        {
             Event eventt = Db.Events.FirstOrDefault(n => n.NameOfEvent == nameOfEvent);
            ReplaceEvent(chatId, eventt.CodeForUsers);
        }


        public bool CheckElements(long ChatId,string type)
        {
            switch(type)
            {
                case "Event":
                    if(Db.Users.FirstOrDefault(n=>n.TelegramId==ChatId)!=null)
                    {
                        User user = Db.Users.FirstOrDefault(n => n.TelegramId == ChatId);
                        if (Db.CurrentEvents.FirstOrDefault(n => n.UserId == user.UserId) != null) return true;
                    }
                    break;
                case "Usefulness":
                    if(Db.Users.FirstOrDefault(n=>n.TelegramId==ChatId)!=null)
                    {
                        if (Db.Users.FirstOrDefault(n => n.TelegramId == ChatId).Usufulness != null)
                            return true;
                    }
                    break;
                case "Name":
                    if (Db.Users.FirstOrDefault(n => n.TelegramId == ChatId) != null)
                    {
                        if (Db.Users.FirstOrDefault(n => n.TelegramId == ChatId).Name != null)
                            return true;
                    }
                    break;
                case "Email":
                    if (Db.Users.FirstOrDefault(n => n.TelegramId == ChatId) != null)
                    {
                        if (Db.Users.FirstOrDefault(n => n.TelegramId == ChatId).Email != null)
                            return true;
                    }
                    break;
                case "work":
                    if(Db.Users.FirstOrDefault(n=>n.TelegramId==ChatId)!=null)
                    {
                        if (Db.Users.FirstOrDefault(n => n.TelegramId == ChatId).Work != null)
                            return true;
                    }
                    break;
                case "AboutTalkingWishes":
                    if (Db.Users.FirstOrDefault(n => n.TelegramId == ChatId) != null)
                    {
                        if (Db.Users.FirstOrDefault(n => n.TelegramId == ChatId).AboutTalkingWishes != null)
                            return true;
                    }
                    break;
                case "Position":
                    if (Db.Users.FirstOrDefault(n => n.TelegramId == ChatId) != null)
                    {
                        if (Db.Users.FirstOrDefault(n => n.TelegramId == ChatId).Position != null)
                            return true;
                    }
                    break;
                case "CurrentTagss":
                    
                    int UserId = Db.Users.FirstOrDefault(n => n.TelegramId == ChatId).UserId;
                    if (Db.UserTag.FirstOrDefault(n => n.UserId == UserId) != null)
                    {
                         return true;
                    }
               break;
                case "CurrentTagAboutOtherss":
                    {
                       
                        int userId = Db.Users.FirstOrDefault(n => n.TelegramId == ChatId).UserId;
                       if (Db.CurrentTagsAboutOthers.FirstOrDefault(n => n.UserId == userId) != null)
                        {
                            return true;
                        }
                    }
                    break;
                case "CurrentSubtagss":
                    {
                        int userId = Db.Users.FirstOrDefault(n => n.TelegramId == ChatId).UserId;
                        if (Db.CurrentSubtags.FirstOrDefault(n => n.UserId == userId) != null)
                            return true;
                     }
                   break;
                case "CurrentSubtagsAboutOtherss":
                    {
                        int userId = Db.Users.FirstOrDefault(n => n.TelegramId == ChatId).UserId;
                        if (Db.CurrentSubtagsAboutOthers.FirstOrDefault(n => n.UserId == userId) != null) return true;
                    }
                    break;
               /* case "CurrentSubtagsAboutOthers":
                    {
                        int userId = Db.Users.FirstOrDefault(N => N.TelegramId == ChatId).UserId;
                        if (Db.CurrentSubtagsAboutOthers.FirstOrDefault(n => n.UserId == userId) != null)
                            return true;
                    }
                    break;*/
              
            }
            return false;
        }
        public List<string> GetTags()//ПОЛУЧЕНИЕ ВСЕХ СУЩЕСТВУЮЩИХ ТЕГОВ
        {
            List<string> result = new List<string>();
            List<Tag> source = Db.Tags.ToList();
            foreach(var item in source)
            {
                result.Add(item.Name);
            }
            return result;
        }
      
        public string GetUsersSubtags(long chatId)
        {
            string subtags = "";
            User user = Db.Users.FirstOrDefault(n => n.TelegramId == chatId);
            List<Subtag> subtagss= Db.Subtags.ToList();
            List<CurrentSubtags> currentSubtags = Db.CurrentSubtags.ToList();
            foreach(var item in currentSubtags)
            {
               if(item.UserId==user.UserId)
                {
                    subtags = subtags +"<code>"+ subtagss.FirstOrDefault(N => N.SubtagId == item.SubtagId).Name + ", </code> ";
                }
            }
            
            log.AddLog("getuserssubtags3");
            
            return subtags;
        }
        public List<string> UsersSubtags(long chatId,List<string> tags)
        {
            List<string> userSubtags = new List<string>();
            int userId = GetUserId(chatId);
            Tag tag = new Tag();
            List<Tag> tagss = Db.Tags.ToList();
            foreach(var item in tagss)
            {
                foreach(var item1 in tags)
                {
                    if(item.Name==item1)
                    {
                        tag = item;
                    }
                }
            }
            List<CurrentSubtags> currentSubtags = Db.CurrentSubtags.ToList();
            List<Subtag> subtags = Db.Subtags.ToList();
            foreach (var item in currentSubtags)
            {
                if (item.UserId == userId)
                {
                    foreach (var item1 in subtags)
                    {
                        if (item.SubtagId == item1.SubtagId)
                        {
                            if (item1.TagId == tag.TagId)
                                userSubtags.Add(item1.Name);
                        }
                    }
                }
              
            }
            log.AddLog("userssubtags4");
            return userSubtags;
        }
        public List<string> UsersSubtagsAboutOthers(long chatId, List<string> tags)
        {
            List<string> userSubtags = new List<string>();
            int userId = GetUserId(chatId);
            Tag tag = new Tag();
            List<Tag> tagss = Db.Tags.ToList();
            foreach (var item in tagss)
            {
                foreach (var item1 in tags)
                {
                    if (item.Name == item1)
                    {
                        tag = item;
                    }
                }
            }
            List<CurrentSubtagsAboutOthers> currentSubtagsAboutOthers = Db.CurrentSubtagsAboutOthers.ToList();
            List<Subtag> subtags = Db.Subtags.ToList();
            foreach (var item in currentSubtagsAboutOthers)
            {
                if (item.UserId == userId)
                {
                    foreach (var item1 in subtags)
                    {
                        if (item.SubtagId == item1.SubtagId)
                        {
                            if (item1.TagId == tag.TagId)
                                userSubtags.Add(item1.Name);
                        }
                    }
                }

            }
            log.AddLog("userssubtags4");
            return userSubtags;
        }
        public string GetUsersSubtagsAboutOthers(long chatId)//ПОЛУЧИТЬ ТЕКУЩИЕ ПОДТЕГИ ДРУГИХ ЛЮДЕЙ
        {
            string subtags = "";
            User user = Db.Users.FirstOrDefault(n => n.TelegramId == chatId);
            List<Subtag> subtagss = Db.Subtags.ToList();
            List<CurrentSubtagsAboutOthers> currentSubtagsAboutOthers = Db.CurrentSubtagsAboutOthers.ToList();
             foreach (var item in currentSubtagsAboutOthers)
            {
                if (item.UserId==user.UserId)
                {
                    subtags = subtags +"<code>"+subtagss.FirstOrDefault(n=>n.SubtagId==item.SubtagId).Name+", </code>";
                }
            }
            
            return subtags;
        }
        public bool CheckEmail(long chatId,string email)
        {
            User user = Db.Users.FirstOrDefault(n => n.TelegramId == chatId);
            if (user.Email == email) return true;
            return false;

        }
        public bool DoesEmailExist(string email)
        {
            List<User> users = Db.Users.ToList();
            foreach (var item in users)
            {
                if (item.Email == email) return true;
            }
            return false;
        }
        public List<int> GetAllUsersForTinder(List<string>subtags,long chatId)
        {
            List<int> usersForTinder = new List<int>();
            User user = Db.Users.FirstOrDefault(n => n.TelegramId == chatId);
            List<User> users = Db.Users.ToList();
            List<Subtag> subtagss = Db.Subtags.ToList();
            List<CurrentSubtags> currentSubtags = Db.CurrentSubtags.ToList();
            List<Subtag> currentSubtagss = new List<Subtag>();
            List<CurrentEvent> currentEvents = Db.CurrentEvents.ToList();
            int EventId = 0;
            foreach(var item in currentEvents)
            {
                if(item.UserId==user.UserId)
                {
                    EventId = item.EventId;
                }
            }
           foreach (var item in subtags)
           {
                log.AddLog(item);
               foreach(var item1 in subtagss)
                {
                    if(item1.Name==item)
                    {
                        currentSubtagss.Add(item1);
                    }
                }
            }
            foreach (var item in users)
            {
                foreach (var item1 in currentSubtags)
                {
                    foreach (var item2 in currentSubtagss)
                    {
                        foreach (var item3 in currentEvents)
                        {
                            if (item1.UserId == item.UserId && item1.SubtagId == item2.SubtagId && item3.UserId==item.UserId && item3.EventId==EventId)
                            {
                                if (!usersForTinder.Contains(item.UserId))
                                    usersForTinder.Add(item.UserId);

                            }
                        }
                    }
                }
            }
            foreach(var item in users)
            {
                if (!usersForTinder.Contains(item.UserId))
                    usersForTinder.Add(item.UserId);
            }
            log.AddLog(usersForTinder.Count.ToString());
            log.AddLog("retunr users");
            return usersForTinder;
        }
        public List<string> GetAllUsersSubtagsAboutOthersList(long chatId)
        {
            List<string> subtags = new List<string>();
            User user = Db.Users.FirstOrDefault(n => n.TelegramId == chatId);
             List<CurrentSubtagsAboutOthers> currentSubtags= Db.CurrentSubtagsAboutOthers.ToList();
           List<Subtag> subtagss = Db.Subtags.ToList();
            log.AddLog("ABOUTOTHERS1");
            foreach (var item in currentSubtags)
            {
               if (item.UserId == user.UserId)
                {
                    foreach(var item1 in subtagss)
                    {
                        if(item1.SubtagId==item.SubtagId)
                        {
                            subtags.Add(item1.Name);
                        }
                    }
                }
            }
            log.AddLog("ABOUTOTHERS2");
            log.AddLog(subtags.Count.ToString());
            subtags.Add(" ");
            return subtags;

        }
        public List<string> GetAllSubtagss(long chatId)
        {
            List<string> subtags = new List<string>();
            List<Subtag> subtagss = Db.Subtags.ToList();
            foreach(var ITEM in subtagss)
            {
                subtags.Add(ITEM.Name);
            }
            
            
            return subtags;

        }
        public string GetAdditionalInformationAboutPeople(long chatId)
        {
            User user = Db.Users.FirstOrDefault(n => n.TelegramId == chatId);
            string temp = "\n";
            temp = temp + user.Email+"\n";
            if(user.Username !=null)
            temp = temp + "@" + user.Username;
            return temp;
        }
        public string GetInformationAboutPeople(long chatId)
        {          
            User user = Db.Users.FirstOrDefault(n => n.TelegramId == chatId);
            string temp = "";
            if (user.Name != null)
            {
                temp = temp + "<i>Имя и фамилия:  </i>";
                temp = temp + "<b>";
                temp = temp + user.Name.ToString();
                temp = temp + "</b> \n";
            }
            if (user.Work != null)
            {
                temp = temp + "<i>Место работы:  </i>";
                temp = temp + user.Work.ToString();
                temp = temp + "\n";
            }
            if (user.Position != null)
            {
                temp = temp + "<i>Позиция в компании:  </i> ";
                temp = temp + user.Position.ToString();
                temp = temp + "\n \n";
            }
            if (user.Usufulness != null)
            {
                temp = temp + "<i>Чем полезен:  </i>";
                temp = temp + user.Usufulness.ToString();
                temp = temp + "\n";
            }
            if (user.AboutTalkingWishes != null)
            {
                temp = temp + "<i>О чём может пообщаться:  </i>";
                temp = temp + user.AboutTalkingWishes.ToString();
                temp = temp + "\n \n";
            }

           
                
                temp = temp + "<i>Личные теги:  </i>";
                List<CurrentSubtags> tags = Db.CurrentSubtags.ToList();
                List<Subtag> subtags = Db.Subtags.ToList();
                foreach (var item in tags)
                {
                if (item.UserId == user.UserId)
                {
                    foreach (var item1 in subtags)
                    {
                        if (item.SubtagId == item1.SubtagId)
                            temp = temp +"<code>"+item1.Name +",</code>  ";
                    }
                }
                
                return temp;
            }
            temp = temp + "\n"; 
                temp = temp + "<i>Выбранные теги(теги поиска):  </i>";
            List<CurrentSubtagsAboutOthers> currentSubtagsAboutOthers = Db.CurrentSubtagsAboutOthers.ToList();
                 foreach(var item in currentSubtagsAboutOthers)
                {
                    if(item.UserId==user.UserId)
                    {
                        foreach(var item1 in subtags)
                        {
                        if (item.SubtagId == item1.SubtagId)
                        {
                            temp = temp +"<code>"+item1.Name + ",</code>  ";
                        }
                        }
                    }
                }
         
            return temp;
           
        }
        public long GetChatId(int userId)
        {
            User user = Db.Users.FirstOrDefault(n => n.UserId == userId);
            return user.TelegramId;
        }
        public int GetUserId(long chatId)
        {
            User user = Db.Users.FirstOrDefault(n => n.TelegramId == chatId);
            return user.UserId;
        }
       public void AddToRequesting(long userToRequest,long chatId)//ЗАЯВКИ НА ДОБАВЛЕНИЕ В ЗАПИСНУЮ КНИЖКУ 03.07.19
        {
            User user = Db.Users.FirstOrDefault(n => n.TelegramId == chatId);
            Applications application = new Applications();
            List<Applications> applications = Db.Applications.ToList();
            bool exist = false;
            foreach(var item in applications)
            {
                if (item.ToRecipe == userToRequest && item.ToSend == chatId)
                    exist = true;
            }
            List<NoteBook> noteBooks = Db.NoteBooks.ToList();
            bool existInNotebook = false;
            log.AddLog("hereeee");
            
            log.AddLog("hereeee1");
            List<CurrentEvent> currentEvents = Db.CurrentEvents.ToList();
              foreach(var item in currentEvents)
              {
                  if (item.UserId == user.UserId)
                  {
                      application.EventId = item.EventId;
                  }
              }
            foreach (var item in noteBooks)
            {
                if (item.Recipient == userToRequest && item.Sender == chatId && item.EventId ==application.EventId)
                    existInNotebook = true;
            }
            log.AddLog("hereeee2");
            if (exist == false)
            {
                log.AddLog("hereeee3");
                //application.EventId = 1;
                application.ToRecipe = userToRequest;
               application.ToSend = chatId;
                log.AddLog("hereeee4");
                Db.Applications.Add(application);
               // Db.Applications.Update(application);
            }
            log.AddLog("hereeee4");
            if (existInNotebook == false)
            {
                       log.AddLog("hereeee5");
                NoteBook notebook = new NoteBook();
                notebook.Sender = chatId;
                notebook.Recipient = userToRequest;
                notebook.EventId = application.EventId;
                Db.NoteBooks.Add(notebook);
                log.AddLog("hereeee6");
            }
            log.AddLog("hereeee7");
            // Db.Update(Db.Applications);
            Db.SaveChanges();
            log.AddLog("adtorequestuon6");

        }
        public void AddToRequestToMeet(long toRecipe, long toSend)
        {
            List<ApplicationToMeet> applications = Db.ApplicationsToMeet.ToList();
            ApplicationToMeet application = new ApplicationToMeet();
            bool exist = false;
            foreach(var item in applications)
            {
                if(item.ToRecipe==toRecipe && item.ToSend==toSend)
                {
                    exist = true;
                }
            }
            if (exist == false)
            {
                application.ToRecipe = toRecipe;
                application.ToSend = toSend;
                applications.Add(application);
                Db.ApplicationsToMeet.Update(application);
            }
            Db.SaveChanges();
        }
      
        public int GetApplicationId(long userToRequest, long chatId)
        {
            List<Applications> applications = Db.Applications.ToList();
            int applicationId=0;
            foreach(var item in applications)
            {
                if (item.ToRecipe == userToRequest && item.ToSend == chatId)
                    applicationId = item.ApplicationsId;
            }
            return applicationId;
        }
        public int GetApplicationIdOfMeeting(long toRecipe,long toSend)
        {
            List<ApplicationToMeet> applications = Db.ApplicationsToMeet.ToList();
            int applicationId=0;
            foreach(var item in applications)
            {
                if (item.ToRecipe == toRecipe && item.ToSend == toSend)
                    applicationId = item.ApplicationToMeetId;
            }
            return applicationId;
        }
        public void AddElementToNoteBook(int idOfApplication)
        {
            Applications application = Db.Applications.FirstOrDefault(n => n.ApplicationsId == idOfApplication);
             NoteBook notebook = new NoteBook();
            bool exist = false;
            List<NoteBook> noteBooks = Db.NoteBooks.ToList();
            foreach(var item in noteBooks)
            {
                if (item.Recipient == application.ToRecipe && item.Sender == application.ToSend)
                {
                    exist = true;
                    notebook = item;
                }
            }
            notebook.permissionForAllInformation = 1;
            if (exist == false)
            {
                notebook.Recipient = application.ToRecipe;
                notebook.Sender = application.ToSend;
                notebook.EventId = application.EventId;
                Db.NoteBooks.Add(notebook);
                Db.Applications.Remove(application);
            }
            Db.SaveChanges();
        }
        public void AddElementToMeetBook(int idOfApplication)
        {
            ApplicationToMeet application = Db.ApplicationsToMeet.FirstOrDefault(n => n.ApplicationToMeetId == idOfApplication);
            MeetBook meetbook = new MeetBook();
            bool exist = false;
            List<MeetBook> meetBooks = Db.MeetBooks.ToList();
            foreach(var item in meetBooks)
            {
                if (item.Recipient == application.ToRecipe && item.Sender == application.ToSend)
                    exist = true;
            }
            if(exist==false)
            {
                meetbook.Recipient = application.ToRecipe;
                meetbook.Sender = application.ToSend;
                Db.MeetBooks.Add(meetbook);
                Db.ApplicationsToMeet.Remove(application);
            }
            Db.SaveChanges();
        }
        public long GetIdOfRecipient(int idOfApplication)
        {
            Applications application = Db.Applications.FirstOrDefault(n => n.ApplicationsId == idOfApplication);
            return application.ToRecipe;
        }
        public long GetIdOfRecipientOfMeeting(int idOfApplication)
        {
            ApplicationToMeet application = Db.ApplicationsToMeet.FirstOrDefault(n => n.ApplicationToMeetId == idOfApplication);
            return application.ToRecipe;
        }
        public long GetIdOfSender(int idOfApplication)
        {
            Applications application = Db.Applications.FirstOrDefault(n => n.ApplicationsId == idOfApplication);
            return application.ToSend;
        }
        public long GetIdOfSenderOfMeeting(int idOfApplication)
        {
            ApplicationToMeet application = Db.ApplicationsToMeet.FirstOrDefault(n => n.ApplicationToMeetId == idOfApplication);
            return application.ToSend;
        }
        public string GetNameOfUser(long chatId)
        {
            User user = Db.Users.FirstOrDefault(n => n.UserId == chatId);
            return user.Name;
        }
       public void RemoveApplication(int idOfApplication)
        {
            Applications application= Db.Applications.FirstOrDefault(n => n.ApplicationsId == idOfApplication);
            Db.Applications.Remove(application);
             Db.SaveChanges();
        }
        public void RemoveApplicationOfMeeting(int idOfApplication)
        {
            ApplicationToMeet application = Db.ApplicationsToMeet.FirstOrDefault(n => n.ApplicationToMeetId == idOfApplication);
            Db.ApplicationsToMeet.Remove(application);
           Db.SaveChanges();
        }
        public string AllListFromNotebook(long chatId,int count)//03.07.19
        {
            string temp = "";
            User user = Db.Users.FirstOrDefault(n => n.TelegramId == chatId);
            List<NoteBook> noteBooks = Db.NoteBooks.ToList();
            int saturation = 4;
            if (count < 1) count = 1;
            int idOfEvent = 0;
            List<CurrentEvent> currentEvents = Db.CurrentEvents.ToList();
            foreach(var item  in currentEvents)
            {
                if(item.UserId==user.UserId)
                {
                    idOfEvent = item.EventId;
                }
            }
            foreach (var item in noteBooks)
            {
                if (item.Sender == chatId && saturation>=0 && item.EventId==idOfEvent)
                {
                    User user1 = Db.Users.FirstOrDefault(n => n.TelegramId == item.Recipient);
                    temp = temp + count.ToString()+")  " + user1.Name + ",  " + user1.Position + "\n <i>Чем полезен: </i>" + user1.Usufulness + " \n";
                    saturation--;
                    count++;
                }
            }
            
            return temp;
        }
        public long GetUserFromNotebook(long chatId,int count)
        {
            List<NoteBook> noteBooks = Db.NoteBooks.ToList();
            User user = Db.Users.FirstOrDefault(n => n.TelegramId == chatId);
            return noteBooks[count--].Recipient;
        }
        public void RemoveCurrentTagsAndSubtags(long chatId)
        {
            User user = Db.Users.FirstOrDefault(n => n.TelegramId == chatId);
            List<UserTag> userTags = Db.UserTag.ToList();
                     foreach (var item in userTags)
            {
                if (item.UserId == user.UserId)
                {
                    Db.UserTag.Remove(item);
                 }
            }
            List<CurrentSubtags> currentSubtags = Db.CurrentSubtags.ToList();
            foreach(var item in currentSubtags)
            {
                if (item.UserId == user.UserId)
                {
                    Db.CurrentSubtags.Remove(item);
                }
            }
            Db.SaveChanges();
        }
        public void RemoveCurrentTagsAndSubtagsAboutOthers(long chatId)
        {
            User user = Db.Users.FirstOrDefault(n => n.TelegramId == chatId);
            List<CurrentTagsAboutOthers> currentTagsAboutOthers = Db.CurrentTagsAboutOthers.ToList();
            foreach (var item in currentTagsAboutOthers)
            {
                if (item.UserId == user.UserId)
                    Db.CurrentTagsAboutOthers.Remove(item);
            }
            List<CurrentSubtagsAboutOthers> currentSubtagsAboutOthers = Db.CurrentSubtagsAboutOthers.ToList();
            foreach (var item in currentSubtagsAboutOthers)
            {
                if (item.UserId == user.UserId)
                    Db.CurrentSubtagsAboutOthers.Remove(item);
            }

            Db.SaveChanges();
        }
        public string GetMyProfile(long chatId)
        {
            User user = Db.Users.FirstOrDefault(n => n.TelegramId == chatId);
            string temp = "";
            if (user.Name != null)
            {
                temp = temp + "<i>Имя и фамилия:  </i>";
                temp = temp + user.Name.ToString()+"\n";
                
            }
            if(user.Work!=null)
            {
                temp = temp + "<i>Компания:  </i>";
                temp = temp + user.Work.ToString()+"\n";
            }
            if (user.Position != null)
            {
                temp = temp + "<i>Позиция в компания:  </i>";
                temp = temp + user.Position.ToString();
                temp = temp + "\n \n";
            }
           
            
            if(user.Usufulness !=null)
            {
                temp = temp + "<i>Чем  полезен(-на):  </i> ";
                temp = temp + user.Usufulness.ToString();
                temp = temp + "\n";
            }
            if (user.AboutTalkingWishes != null)
            {
                temp = temp + "<i>О чём может пообщаться:  </i>";
                temp = temp + user.AboutTalkingWishes.ToString();
                temp = temp + "\n \n";
            }


            string s = "";
            if(user.CurrentSubtagss !=null)
            {
                temp = temp + "<i>Личные теги:  </i> ";
                List<Subtag> subtags = Db.Subtags.ToList();
                List<CurrentSubtags> currentSubtags = Db.CurrentSubtags.ToList();
                 foreach(var item in currentSubtags)
                {
                    foreach(var item1 in subtags)
                    {
                        if(item.SubtagId==item1.SubtagId && item.UserId==user.UserId)
                        {
                            log.AddLog(item.UserId.ToString());
                           temp = temp + "<code>"+item1.Name + ",  </code>";
                        }
                    }
                }
                
            }
            

            temp = temp + "\n";
         
            if (user.CurrentSubtagsAboutOtherss!= null)
            {
                temp = temp + "<i>Выбранныe теги: </i> ";
                List<Subtag> subtags = Db.Subtags.ToList();
                List<CurrentSubtagsAboutOthers> currentSubtagsAboutOthers = Db.CurrentSubtagsAboutOthers.ToList();
                foreach (var item in currentSubtagsAboutOthers)
                {
                    foreach (var item1 in subtags)
                    {
                        if (item.SubtagId == item1.SubtagId && item.UserId==user.UserId)
                        {
                          
                            temp = temp +"<code>"+item1.Name + ",  </code> ";
                        }
                    }
                }
            }
            
            return temp;
        }
        public void ChangeElement(long chatId, string type, string parametr)
        {
            switch (type)
            {
                case "Name":
                    {
                        User user = Db.Users.FirstOrDefault(n => n.TelegramId == chatId);
                        user.Name = parametr;
                        Db.Users.Update(user);
                        Db.SaveChanges();
                    }
                    break;
                case "Work":
                    {
                        User user = Db.Users.FirstOrDefault(n => n.TelegramId == chatId);
                        user.Work = parametr;
                        Db.Users.Update(user);
                        Db.SaveChanges();
                    }
                    break;
                case "Position":
                    {
                        User user = Db.Users.FirstOrDefault(n => n.TelegramId == chatId);
                        user.Position = parametr;
                        Db.Users.Update(user);
                        Db.SaveChanges();
                    }
                    break;
                case "Usefulness":
                    {
                        User user = Db.Users.FirstOrDefault(n => n.TelegramId == chatId);
                        user.Usufulness= parametr;
                        Db.Users.Update(user);
                        Db.SaveChanges();
                    }
                    break;
                case "AboutWishes":
                    {
                        User user = Db.Users.FirstOrDefault(n => n.TelegramId == chatId);
                        user.AboutTalkingWishes = parametr;
                        Db.Users.Update(user);
                        Db.SaveChanges();
                    }
                    break;
            }
        }
        public List<string> GetAllEvents(int userId)//03.07.19
        {
            List<OldEvent> oldEvents = Db.OldEvents.ToList();
            List<Event> events = Db.Events.ToList();
            List<string> oldEventss = new List<string>();
            foreach(var item in oldEvents)
            {
                if (item.UserId == userId)
                {
                    foreach(var item1 in events)
                    {
                        if(item.EventId==item1.EventId)
                        {
                            oldEventss.Add(item1.NameOfEvent);
                        }
                    }
                }
            }
            return oldEventss;
        }
        public string GetAmountOfUsers(long chatId)
        {
            string temp = "";
            User user = Db.Users.FirstOrDefault(n => n.TelegramId == chatId);
            List<Event> events = Db.Events.ToList();
            List<CurrentEvent> currentEvents = Db.CurrentEvents.ToList();
            int count = 0;
            int eventId = 0;
               foreach(var item1 in currentEvents)
                {
                    if(item1.UserId==user.UserId)
                    {
                    foreach (var item in events)
                    {
                        if (item.EventId == item1.EventId)
                            eventId = item.EventId;
                    }
                }
            }
               foreach(var item in currentEvents)
            {
                if(item.EventId==eventId)
                {
                    count++;
                }
            }
            temp = count.ToString();
            return temp;
        }
        public string GetAmountOfUsersAtAll(long chatId)
        {
            string temp = "";
            User user = Db.Users.FirstOrDefault(n => n.TelegramId == chatId);
            List<Event> events = Db.Events.ToList();
            List<OldEvent> oldEvents = Db.OldEvents.ToList();
            int count = 0;
            int eventId = 0;
            foreach (var item1 in oldEvents)
            {
                if (item1.UserId == user.UserId)
                {
                    foreach (var item in events)
                    {
                        if (item.EventId == item1.EventId)
                            eventId = item.EventId;
                    }
                }
            }
            foreach (var item in oldEvents)
            {
                if (item.EventId == eventId)
                {
                    count++;
                }
            }
            temp = count.ToString();
            return temp;
        }
        public List<long> PeopleForDistribution(long chatId)
        {
            User user = Db.Users.FirstOrDefault(n => n.TelegramId == chatId);
            List<CurrentEvent> currentEvents = Db.CurrentEvents.ToList();
            List<long> peopleForDistribution = new List<long>();
            int eventId = 0;
            foreach(var item in currentEvents)
            {
                if(item.UserId==user.UserId)
                {
                    eventId = item.EventId;
                }
            }
            foreach(var item in currentEvents)
            {
                if(item.EventId==eventId)
                {
                    long chatik = GetChatId(item.UserId);
                 peopleForDistribution.Add(chatik);
                    log.AddLog(chatik.ToString());
                }
            }
            return peopleForDistribution;
        }
   
    public void CurrentActionOn(long chatId, string parametr)
    {
        switch (parametr)
        {
                case "AddQuestionToSurvey":
                    {
                        CurrentActions currentAction = Db.CurrentActions.FirstOrDefault(n => n.TelegramId == chatId);
                        currentAction.AddQuestionToSurvey = 1;
                        Db.Update(currentAction);
                        Db.SaveChanges();
                    }
                    break;
                case "CreateSurvey":
                    {
                        CurrentActions currentAction = Db.CurrentActions.FirstOrDefault(n => n.TelegramId == chatId);
                        currentAction.CreateSurvey = 1;
                        Db.Update(currentAction);
                        Db.SaveChanges();
                    }
                    break;
                case "PrivateCabinet":
                    {
                        CurrentActions currentAction = Db.CurrentActions.FirstOrDefault(n => n.TelegramId == chatId);
                        currentAction.PrivateCabinet = 1;
                        Db.Update(currentAction);
                        Db.SaveChanges();
                    }
                    break;
                case "Usefulness":
                    {
                        CurrentActions currentAction = Db.CurrentActions.FirstOrDefault(n => n.TelegramId == chatId);
                        currentAction.Usefulness = 1;
                        Db.Update(currentAction);
                        Db.SaveChanges();
                    }
                    break;
                case "editUsefulness":
                    {
                        CurrentActions currentAction = Db.CurrentActions.FirstOrDefault(n => n.TelegramId == chatId);
                        currentAction.editUsefulness = 1;
                        Db.Update(currentAction);
                        Db.SaveChanges();
                    }
                    break;
                case "CheckEmail":
                    {
                        CurrentActions currentAction = Db.CurrentActions.FirstOrDefault(n => n.TelegramId == chatId);
                        currentAction.CheckEmail =1;
                        Db.Update(currentAction);
                        Db.SaveChanges();
                    }
                    break;
                case "NameAndLastName":
                    {
                        CurrentActions currentAction = Db.CurrentActions.FirstOrDefault(n => n.TelegramId == chatId);
                        currentAction.NameAndLastName = 1;
                        Db.Update(currentAction);
                        Db.SaveChanges();
                    }
                    break;
                case "ChoseTag":
                    {
                        CurrentActions currentAction = Db.CurrentActions.FirstOrDefault(n => n.TelegramId == chatId);
                        currentAction.ChoseTag = 1;
                        Db.Update(currentAction);
                        Db.SaveChanges();
                    }
                    break;
                case "EnterWithEventCode":
                    {
                        CurrentActions currentAction = Db.CurrentActions.FirstOrDefault(n => n.TelegramId == chatId);
                        currentAction.EnterWithEventCode = 1;
                        Db.Update(currentAction);
                        Db.SaveChanges();
                    }
                    break;
                case "EventCode":
                    {
                        CurrentActions currentAction = Db.CurrentActions.FirstOrDefault(n => n.TelegramId == chatId);
                        currentAction.EventCode = 1;
                        Db.Update(currentAction);
                        Db.SaveChanges();
                    }
                    break;
                case "Email":
                    {
                        CurrentActions currentAction = Db.CurrentActions.FirstOrDefault(n => n.TelegramId == chatId);
                        currentAction.Email = 1;
                        Db.Update(currentAction);
                        Db.SaveChanges();
                    }
                    break;
                case "AboutEvent":
                    {
                        CurrentActions currentAction = Db.CurrentActions.FirstOrDefault(n => n.TelegramId == chatId);
                        currentAction.AboutEvent = 1;
                        Db.Update(currentAction);
                        Db.SaveChanges();
                    }
                    break;
                case "Networking":
                    {
                        CurrentActions currentAction = Db.CurrentActions.FirstOrDefault(n => n.TelegramId == chatId);
                        currentAction.Networking = 1;
                        Db.Update(currentAction);
                        Db.SaveChanges();
                    }
                    break;
                case "Work":
                    {
                        CurrentActions currentAction = Db.CurrentActions.FirstOrDefault(n => n.TelegramId == chatId);
                        currentAction.Work = 1;
                        Db.Update(currentAction);
                        Db.SaveChanges();
                    }
                    break;
                case "Position":
                    {
                        CurrentActions currentAction = Db.CurrentActions.FirstOrDefault(n => n.TelegramId == chatId);
                        currentAction.Position = 1;
                        Db.Update(currentAction);
                        Db.SaveChanges();
                    }
                    break;
                case "AboutWishes":
                    {
                        CurrentActions currentAction = Db.CurrentActions.FirstOrDefault(n => n.TelegramId == chatId);
                        currentAction.AboutWishes = 1;
                        Db.Update(currentAction);
                        Db.SaveChanges();
                    }
                    break;
                case "ChoseSubtags":
                    {
                        CurrentActions currentAction = Db.CurrentActions.FirstOrDefault(n => n.TelegramId == chatId);
                        currentAction.ChoseSubtags = 1;
                        Db.Update(currentAction);
                        Db.SaveChanges();
                    }
                    break;
                case "ChoseSubtagsAboutOthers":
                    {
                        CurrentActions currentAction = Db.CurrentActions.FirstOrDefault(n => n.TelegramId == chatId);
                        currentAction.ChoseSubtagsAboutOthers = 1;
                        Db.Update(currentAction);
                        Db.SaveChanges();
                    }
                    break;
                case "ChoseTagAboutOthers":
                    {
                        CurrentActions currentAction = Db.CurrentActions.FirstOrDefault(n => n.TelegramId == chatId);
                        currentAction.ChoseTagAboutOthers = 1;
                        Db.Update(currentAction);
                        Db.SaveChanges();
                    }
                    break;
                case "NetworkingFull":
                    {
                        CurrentActions currentAction = Db.CurrentActions.FirstOrDefault(n => n.TelegramId == chatId);
                        currentAction.NetworkingFull = 1;
                        Db.Update(currentAction);
                        Db.SaveChanges();
                    }
                    break;
                case "MyProfile":
                    {
                        CurrentActions currentAction = Db.CurrentActions.FirstOrDefault(n => n.TelegramId == chatId);
                        currentAction.MyProfile = 1;
                        Db.Update(currentAction);
                        Db.SaveChanges();
                    }
                    break;
                case "editName":
                    {
                        CurrentActions currentAction = Db.CurrentActions.FirstOrDefault(n => n.TelegramId == chatId);
                        currentAction.editName = 1;
                        Db.Update(currentAction);
                        Db.SaveChanges();
                    }
                    break;
                case "editWork":
                    {
                        CurrentActions currentAction = Db.CurrentActions.FirstOrDefault(n => n.TelegramId == chatId);
                        currentAction.editWork = 1;
                        Db.Update(currentAction);
                        Db.SaveChanges();
                    }
                    break;
                case "editPosition":
                    {
                        CurrentActions currentAction = Db.CurrentActions.FirstOrDefault(n => n.TelegramId == chatId);
                        currentAction.editPosition = 1;
                        Db.Update(currentAction);
                        Db.SaveChanges();
                    }
                    break;
                case "editAboutWishes":
                    {
                        CurrentActions currentAction = Db.CurrentActions.FirstOrDefault(n => n.TelegramId == chatId);
                        currentAction.editAboutWishes = 1;
                        Db.Update(currentAction);
                        Db.SaveChanges();
                    }
                    break;
                case "editTags":
                    {
                        CurrentActions currentAction = Db.CurrentActions.FirstOrDefault(n => n.TelegramId == chatId);
                        currentAction.editTags = 1;
                        Db.Update(currentAction);
                        Db.SaveChanges();
                    }
                    break;
                case "EditTag":
                    {
                        CurrentActions currentAction = Db.CurrentActions.FirstOrDefault(n => n.TelegramId == chatId);
                        currentAction.EditTag = 1;
                        Db.Update(currentAction);
                        Db.SaveChanges();
                    }
                    break;
                case "EditSubtag":
                    {
                        CurrentActions currentAction = Db.CurrentActions.FirstOrDefault(n => n.TelegramId == chatId);
                        currentAction.EditSubtag = 1;
                        Db.Update(currentAction);
                        Db.SaveChanges();
                    }
                    break;
                case "EditTagAboutOthers":
                    {
                        CurrentActions currentAction = Db.CurrentActions.FirstOrDefault(n => n.TelegramId == chatId);
                        currentAction.EditTagAboutOthers = 1;
                        Db.Update(currentAction);
                        Db.SaveChanges();
                    }
                    break;
                case "EditSubtagAboutOthers":
                    {
                        CurrentActions currentAction = Db.CurrentActions.FirstOrDefault(n => n.TelegramId == chatId);
                        currentAction.EditSubtagAboutOthers =1;
                        Db.Update(currentAction);
                        Db.SaveChanges();
                    }
                    break;
                case "AddTag":
                    {
                        CurrentActions currentAction = Db.CurrentActions.FirstOrDefault(n => n.TelegramId == chatId);
                        currentAction.AddTag = 1;
                        Db.Update(currentAction);
                        Db.SaveChanges();
                    }
                    break;
                case "AddSubtag":
                    {
                        CurrentActions currentAction = Db.CurrentActions.FirstOrDefault(n => n.TelegramId == chatId);
                        currentAction.AddSubtag = 1;
                        Db.Update(currentAction);
                        Db.SaveChanges();
                    }
                    break;
                case "ChoseOldEvent":
                    {
                        CurrentActions currentAction = Db.CurrentActions.FirstOrDefault(n => n.TelegramId == chatId);
                        currentAction.ChoseOldEvent = 1;
                        Db.Update(currentAction);
                        Db.SaveChanges();
                    }
                    break;
                case "AddSubtagAboutOthers":
                    {
                        CurrentActions currentAction = Db.CurrentActions.FirstOrDefault(n => n.TelegramId == chatId);
                        currentAction.AddSubtagAboutOthers =1;
                        Db.Update(currentAction);
                        Db.SaveChanges();
                    }
                    break;
                case "AddTagAboutOthers":
                    {
                        CurrentActions currentAction = Db.CurrentActions.FirstOrDefault(n => n.TelegramId == chatId);
                        currentAction.AddTagAboutOthers = 1;
                        Db.Update(currentAction);
                        Db.SaveChanges();
                    }
                    break;
                case "AddInformationAboutEvent":
                    {
                        CurrentActions currentAction = Db.CurrentActions.FirstOrDefault(n => n.TelegramId == chatId);
                        currentAction.AddInformationAboutEvent = 1;
                        Db.Update(currentAction);
                        Db.SaveChanges();
                    }
                    break;
                case "EnteranceForOrganisators":
                    {
                        CurrentActions currentAction = Db.CurrentActions.FirstOrDefault(n => n.TelegramId == chatId);
                        currentAction.EnteranceForOrganisators = 1;
                        Db.Update(currentAction);
                        Db.SaveChanges();
                    }
                    break;
                case "CreateNotification":
                    {
                        CurrentActions currentAction = Db.CurrentActions.FirstOrDefault(n => n.TelegramId == chatId);
                        currentAction.CreateNotification = 1;
                        Db.Update(currentAction);
                        Db.SaveChanges();
                    }
                    break;
            }
    }
        public void CurrentActionOff(long chatId, string parametr)
        {
            switch (parametr)
            {
                case "CreateSurvey":
                    {
                        CurrentActions currentAction = Db.CurrentActions.FirstOrDefault(n => n.TelegramId == chatId);
                        currentAction.CreateSurvey = 0;
                        Db.CurrentActions.Update(currentAction);
                        Db.SaveChanges();
                    }
                    break;
                case "PrivateCabinet":
                    {
                        CurrentActions currentAction = Db.CurrentActions.FirstOrDefault(n => n.TelegramId == chatId);
                        currentAction.PrivateCabinet = 0;
                        Db.CurrentActions.Update(currentAction);
                        Db.SaveChanges();
                        }
                    break;
                case "CheckEmail":
                    {
                        CurrentActions currentAction = Db.CurrentActions.FirstOrDefault(n => n.TelegramId == chatId);
                        currentAction.CheckEmail = 0;
                        Db.Update(currentAction);
                        Db.SaveChanges();
                    }
                    break;
                case "Usefulness":
                    {
                        CurrentActions currentAction = Db.CurrentActions.FirstOrDefault(n => n.TelegramId == chatId);
                        currentAction.Usefulness = 0;
                        Db.Update(currentAction);
                        Db.SaveChanges();
                    }
                    break;
                case "Notebook":
                    {
                        CurrentActions currentAction = Db.CurrentActions.FirstOrDefault(n => n.TelegramId == chatId);
                        currentAction.NoteBook = 0;
                        Db.Update(currentAction);
                        Db.SaveChanges();
                    }
                    break;
                case "editUsefulness":
                    {
                        CurrentActions currentAction = Db.CurrentActions.FirstOrDefault(n => n.TelegramId == chatId);
                        currentAction.editUsefulness = 0;
                        Db.Update(currentAction);
                        Db.SaveChanges();
                    }
                    break;
                case "NameAndLastName":
                    {
                        CurrentActions currentAction = Db.CurrentActions.FirstOrDefault(n => n.TelegramId == chatId);
                        currentAction.NameAndLastName = 0;
                        Db.Update(currentAction);
                        Db.SaveChanges();
                    }
                    break;
               case "ChoseTag":
                    {
                        CurrentActions currentAction = Db.CurrentActions.FirstOrDefault(n => n.TelegramId == chatId);
                        currentAction.ChoseTag = 0;
                        Db.Update(currentAction);
                        Db.SaveChanges();
                    }
                    break;
                case "EnterWithEventCode":
                    {
                        CurrentActions currentAction = Db.CurrentActions.FirstOrDefault(n => n.TelegramId == chatId);
                        currentAction.EnterWithEventCode = 0;
                        Db.Update(currentAction);
                        Db.SaveChanges();
                    }
                    break;
                case "EventCode":
                    {
                        CurrentActions currentAction = Db.CurrentActions.FirstOrDefault(n => n.TelegramId == chatId);
                        currentAction.EventCode = 0;
                        Db.Update(currentAction);
                        Db.SaveChanges();
                    }
                    break;
                case "Email":
                    {
                        CurrentActions currentAction = Db.CurrentActions.FirstOrDefault(n => n.TelegramId == chatId);
                        currentAction.Email = 0;
                        Db.Update(currentAction);
                        Db.SaveChanges();
                    }
                    break;
                case "AboutEvent":
                    {
                        CurrentActions currentAction = Db.CurrentActions.FirstOrDefault(n => n.TelegramId == chatId);
                        currentAction.AboutEvent = 0;
                        Db.Update(currentAction);
                        Db.SaveChanges();
                    }
                    break;
                case "Networking":
                    {
                        CurrentActions currentAction = Db.CurrentActions.FirstOrDefault(n => n.TelegramId == chatId);
                        currentAction.Networking = 0;
                        Db.Update(currentAction);
                        Db.SaveChanges();
                    }
                    break;
                case "Work":
                    {
                        CurrentActions currentAction = Db.CurrentActions.FirstOrDefault(n => n.TelegramId == chatId);
                        currentAction.Work = 0;
                        Db.Update(currentAction);
                        Db.SaveChanges();
                    }
                    break;
                case "Position":
                    {
                        CurrentActions currentAction = Db.CurrentActions.FirstOrDefault(n => n.TelegramId == chatId);
                        currentAction.Position = 0;
                        Db.Update(currentAction);
                        Db.SaveChanges();
                    }
                    break;
                case "AboutWishes":
                    {
                        CurrentActions currentAction = Db.CurrentActions.FirstOrDefault(n => n.TelegramId == chatId);
                        currentAction.AboutWishes = 0;
                        Db.Update(currentAction);
                        Db.SaveChanges();
                    }
                    break;
                case "ChoseSubtags":
                    {
                        CurrentActions currentAction = Db.CurrentActions.FirstOrDefault(n => n.TelegramId == chatId);
                        currentAction.ChoseSubtags = 0;
                        Db.Update(currentAction);
                        Db.SaveChanges();
                    }
                    break;
                case "ChoseSubtagsAboutOthers":
                    {
                        CurrentActions currentAction = Db.CurrentActions.FirstOrDefault(n => n.TelegramId == chatId);
                        currentAction.ChoseSubtagsAboutOthers = 0;
                        Db.Update(currentAction);
                        Db.SaveChanges();
                    }
                    break;
                case "ChoseTagAboutOthers":
                    {
                        CurrentActions currentAction = Db.CurrentActions.FirstOrDefault(n => n.TelegramId == chatId);
                        currentAction.ChoseTagAboutOthers = 0;
                        Db.Update(currentAction);
                        Db.SaveChanges();
                    }
                    break;
                case "NetworkingFull":
                    {
                        CurrentActions currentAction = Db.CurrentActions.FirstOrDefault(n => n.TelegramId == chatId);
                        currentAction.NetworkingFull = 0;
                        Db.Update(currentAction);
                        Db.SaveChanges();
                    }
                    break;
                case "MyProfile":
                    {
                        CurrentActions currentAction = Db.CurrentActions.FirstOrDefault(n => n.TelegramId == chatId);
                        currentAction.MyProfile = 0;
                        Db.Update(currentAction);
                        Db.SaveChanges();
                    }
                    break;
                case "editName":
                    {
                        CurrentActions currentAction = Db.CurrentActions.FirstOrDefault(n => n.TelegramId == chatId);
                        currentAction.editName = 0;
                        Db.Update(currentAction);
                        Db.SaveChanges();
                    }
                    break;
                case "editWork":
                    {
                        CurrentActions currentAction = Db.CurrentActions.FirstOrDefault(n => n.TelegramId == chatId);
                        currentAction.editWork = 0;
                        Db.Update(currentAction);
                        Db.SaveChanges();
                    }
                    break;
                case "editPosition":
                    {
                        CurrentActions currentAction = Db.CurrentActions.FirstOrDefault(n => n.TelegramId == chatId);
                        currentAction.editPosition = 0;
                        Db.Update(currentAction);
                        Db.SaveChanges();
                    }
                    break;
                case "editAboutWishes":
                    {
                        CurrentActions currentAction = Db.CurrentActions.FirstOrDefault(n => n.TelegramId == chatId);
                        currentAction.editAboutWishes = 0;
                        Db.Update(currentAction);
                        Db.SaveChanges();
                    }
                    break;
                case "editTags":
                    {
                        CurrentActions currentAction = Db.CurrentActions.FirstOrDefault(n => n.TelegramId == chatId);
                        currentAction.editTags = 0;
                        Db.Update(currentAction);
                        Db.SaveChanges();
                    }
                    break;
                case "EditTag":
                    {
                        CurrentActions currentAction = Db.CurrentActions.FirstOrDefault(n => n.TelegramId == chatId);
                        currentAction.EditTag = 0;
                        Db.Update(currentAction);
                        Db.SaveChanges();
                    }
                    break;
                case "EditSubtag":
                    {
                        CurrentActions currentAction = Db.CurrentActions.FirstOrDefault(n => n.TelegramId == chatId);
                        currentAction.EditSubtag = 0;
                        Db.Update(currentAction);
                        Db.SaveChanges();
                    }
                    break;
                case "EditTagAboutOthers":
                    {
                        CurrentActions currentAction = Db.CurrentActions.FirstOrDefault(n => n.TelegramId == chatId);
                        currentAction.EditTagAboutOthers = 0;
                        Db.Update(currentAction);
                        Db.SaveChanges();
                    }
                    break;
                case "EditSubtagAboutOthers":
                    {
                        CurrentActions currentAction = Db.CurrentActions.FirstOrDefault(n => n.TelegramId == chatId);
                        currentAction.EditSubtagAboutOthers = 0;
                        Db.Update(currentAction);
                        Db.SaveChanges();
                    }
                    break;
                case "AddTag":
                    {
                        CurrentActions currentAction = Db.CurrentActions.FirstOrDefault(n => n.TelegramId == chatId);
                        currentAction.AddTag = 0;
                        Db.Update(currentAction);
                        Db.SaveChanges();
                    }
                    break;
                case "AddSubtag":
                    {
                        CurrentActions currentAction = Db.CurrentActions.FirstOrDefault(n => n.TelegramId == chatId);
                        currentAction.AddSubtag = 0;
                        Db.Update(currentAction);
                        Db.SaveChanges();
                    }
                    break;
                case "ChoseOldEvent":
                    {
                        CurrentActions currentAction = Db.CurrentActions.FirstOrDefault(n => n.TelegramId == chatId);
                        currentAction.ChoseOldEvent = 0;
                        Db.Update(currentAction);
                        Db.SaveChanges();
                    }
                    break;
                case "AddSubtagAboutOthers":
                    {
                        CurrentActions currentAction = Db.CurrentActions.FirstOrDefault(n => n.TelegramId == chatId);
                        currentAction.AddSubtagAboutOthers = 0;
                        Db.Update(currentAction);
                        Db.SaveChanges();
                    }
                    break;
                case "AddTagAboutOthers":
                    {
                        CurrentActions currentAction = Db.CurrentActions.FirstOrDefault(n => n.TelegramId == chatId);
                        currentAction.AddTagAboutOthers = 0;
                        Db.Update(currentAction);
                        Db.SaveChanges();
                    }
                    break;
                case "AddInformationAboutEvent":
                    {
                        CurrentActions currentAction = Db.CurrentActions.FirstOrDefault(n => n.TelegramId == chatId);
                        currentAction.AddInformationAboutEvent = 0;
                        Db.Update(currentAction);
                        Db.SaveChanges();
                    }
                    break;
                case "EnteranceForOrganisators":
                    {
                        CurrentActions currentAction = Db.CurrentActions.FirstOrDefault(n => n.TelegramId == chatId);
                        currentAction.EnteranceForOrganisators = 0;
                        Db.Update(currentAction);
                        Db.SaveChanges();
                    }
                    break;
                case "CreateNotification":
                    {
                        CurrentActions currentAction = Db.CurrentActions.FirstOrDefault(n => n.TelegramId == chatId);
                        currentAction.CreateNotification = 0;
                        Db.Update(currentAction);
                        Db.SaveChanges();
                    }
                    break;
                case "AddQuestionToSurvey":
                    {
                        CurrentActions currentAction = Db.CurrentActions.FirstOrDefault(n => n.TelegramId == chatId);
                        currentAction.AddQuestionToSurvey = 0;
                        Db.Update(currentAction);
                        Db.SaveChanges();
                    }
                    break;
            } }
        public int CheckCurrentAction(long chatId, string parametr)
        {
            if(parametr== "AddQuestionToSurvey")
            {
                CurrentActions currentAction = Db.CurrentActions.FirstOrDefault(n => n.TelegramId == chatId);
                return currentAction.AddQuestionToSurvey;
            }
            if(parametr=="CreateSurvey")
            {
                CurrentActions currentAction = Db.CurrentActions.FirstOrDefault(n => n.TelegramId == chatId);
                return currentAction.CreateSurvey;
            }
            if (parametr == "PrivateCabinet")
            {
                  CurrentActions currentAction = Db.CurrentActions.FirstOrDefault(n => n.TelegramId == chatId);
                return currentAction.PrivateCabinet;
            }
            if (parametr == "CheckEmail")
            {

                CurrentActions currentAction = Db.CurrentActions.FirstOrDefault(n => n.TelegramId == chatId);
                return currentAction.CheckEmail;
            }
            if (parametr == "editUsefulness")
            {

                CurrentActions currentAction = Db.CurrentActions.FirstOrDefault(n => n.TelegramId == chatId);
                return currentAction.editUsefulness;
            }
            if (parametr == "NameAndLastName")
            {

                CurrentActions currentAction = Db.CurrentActions.FirstOrDefault(n => n.TelegramId == chatId);
                return currentAction.NameAndLastName;
            }
            if (parametr == "Usefulness")
            {
                 CurrentActions currentAction = Db.CurrentActions.FirstOrDefault(n => n.TelegramId == chatId);
                return currentAction.Usefulness;
            }
            if (parametr == "ChoseTag")
            {

                CurrentActions currentAction = Db.CurrentActions.FirstOrDefault(n => n.TelegramId == chatId);
                return currentAction.ChoseTag;
            }
            if (parametr == "EnterWithEventCode")
            {

                CurrentActions currentAction = Db.CurrentActions.FirstOrDefault(n => n.TelegramId == chatId);
                return currentAction.EnterWithEventCode;
            }
            if (parametr == "EventCode")
            {

                CurrentActions currentAction = Db.CurrentActions.FirstOrDefault(n => n.TelegramId == chatId);
                return currentAction.EventCode;
            }
            if (parametr == "Email")
            {

                CurrentActions currentAction = Db.CurrentActions.FirstOrDefault(n => n.TelegramId == chatId);
                return currentAction.Email;
            }
            if (parametr == "AboutEvent")
            {

                CurrentActions currentAction = Db.CurrentActions.FirstOrDefault(n => n.TelegramId == chatId);
                return currentAction.AboutEvent;
            }
            if (parametr == "Networking")
            {

                CurrentActions currentAction = Db.CurrentActions.FirstOrDefault(n => n.TelegramId == chatId);
                return currentAction.Networking;
            }
            if (parametr == "Work")
            {

                CurrentActions currentAction = Db.CurrentActions.FirstOrDefault(n => n.TelegramId == chatId);
                return currentAction.Work;
            }
            if (parametr == "Position")
            {

                CurrentActions currentAction = Db.CurrentActions.FirstOrDefault(n => n.TelegramId == chatId);
                return currentAction.Position;
            }
            if (parametr == "AboutWishes")
            {

                CurrentActions currentAction = Db.CurrentActions.FirstOrDefault(n => n.TelegramId == chatId);
                return currentAction.AboutWishes;
            }
            if (parametr == "ChoseSubtags")
            {

                CurrentActions currentAction = Db.CurrentActions.FirstOrDefault(n => n.TelegramId == chatId);
                return currentAction.ChoseSubtags;
            }
            if (parametr == "ChoseSubtagsAboutOthers")
            {

                CurrentActions currentAction = Db.CurrentActions.FirstOrDefault(n => n.TelegramId == chatId);
                return currentAction.ChoseSubtagsAboutOthers;
            }

            if (parametr == "ChoseTagAboutOthers")
            {

                CurrentActions currentAction = Db.CurrentActions.FirstOrDefault(n => n.TelegramId == chatId);
                return currentAction.ChoseTagAboutOthers;
            }
            if (parametr == "NetworkingFull")
            {
                log.AddLog("networking full 0");
                CurrentActions currentAction = Db.CurrentActions.FirstOrDefault(n => n.TelegramId == chatId);
                log.AddLog("networking full 1");
                log.AddLog(chatId.ToString());
                return currentAction.NetworkingFull;
            }
            if (parametr == "MyProfile"){
                

                CurrentActions currentAction = Db.CurrentActions.FirstOrDefault(n => n.TelegramId == chatId);
                return currentAction.MyProfile;
            }
            if (parametr == "editName")
            {

                CurrentActions currentAction = Db.CurrentActions.FirstOrDefault(n => n.TelegramId == chatId);
                return currentAction.editName;
            }
            if (parametr == "editWork")
            {

                CurrentActions currentAction = Db.CurrentActions.FirstOrDefault(n => n.TelegramId == chatId);
                return currentAction.editWork;
            }
            if (parametr == "editPosition")
            {

                CurrentActions currentAction = Db.CurrentActions.FirstOrDefault(n => n.TelegramId == chatId);
                return currentAction.editPosition;
            }
            if (parametr == "editAboutWishes")
            {

                CurrentActions currentAction = Db.CurrentActions.FirstOrDefault(n => n.TelegramId == chatId);
                return currentAction.editAboutWishes;
            }
            if (parametr == "editTags")
            {

                CurrentActions currentAction = Db.CurrentActions.FirstOrDefault(n => n.TelegramId == chatId);
                return currentAction.editTags;
            }
            if (parametr == "EditSubtag")
            {

                CurrentActions currentAction = Db.CurrentActions.FirstOrDefault(n => n.TelegramId == chatId);
                return currentAction.EditSubtag;
            }
            if (parametr == "EditTag")
            {

                CurrentActions currentAction = Db.CurrentActions.FirstOrDefault(n => n.TelegramId == chatId);
                return currentAction.EditTag;
            }
            if (parametr == "EditTagAboutOthers")
            {

                CurrentActions currentAction = Db.CurrentActions.FirstOrDefault(n => n.TelegramId == chatId);
                return currentAction.EditTagAboutOthers;
            }
            
            if (parametr == "EditSubtagAboutOthers")
            {

                CurrentActions currentAction = Db.CurrentActions.FirstOrDefault(n => n.TelegramId == chatId);
                return currentAction.EditSubtagAboutOthers;
            }
            if (parametr == "AddTag")
            {

                CurrentActions currentAction = Db.CurrentActions.FirstOrDefault(n => n.TelegramId == chatId);
                return currentAction.AddTag;
            }
            if (parametr == "AddSubtag")
            {

                CurrentActions currentAction = Db.CurrentActions.FirstOrDefault(n => n.TelegramId == chatId);
                return currentAction.AddSubtag;
            }
            if (parametr == "ChoseOldEvent")
            {

                CurrentActions currentAction = Db.CurrentActions.FirstOrDefault(n => n.TelegramId == chatId);
                return currentAction.ChoseOldEvent;
            }
            if (parametr == "AddSubtagAboutOthers")
            {

                CurrentActions currentAction = Db.CurrentActions.FirstOrDefault(n => n.TelegramId == chatId);
                return currentAction.AddSubtagAboutOthers;
            }
            if (parametr == "AddTagAboutOthers")
            {

                CurrentActions currentAction = Db.CurrentActions.FirstOrDefault(n => n.TelegramId == chatId);
                return currentAction.AddTagAboutOthers;
            }
            if (parametr == "AddInformationAboutEvent")
            {

                CurrentActions currentAction = Db.CurrentActions.FirstOrDefault(n => n.TelegramId == chatId);
                return currentAction.AddInformationAboutEvent;
            }
            if (parametr == "EnteranceForOrganisators")
            {

                CurrentActions currentAction = Db.CurrentActions.FirstOrDefault(n => n.TelegramId == chatId);
                return currentAction.EnteranceForOrganisators;
            }
            if (parametr == "amountForTinder")
            {

                CurrentActions currentAction = Db.CurrentActions.FirstOrDefault(n => n.TelegramId == chatId);
                return currentAction.amountForTinder;
            }
            if (parametr == "lengthOfUsersBySubtags")
            {

                CurrentActions currentAction = Db.CurrentActions.FirstOrDefault(n => n.TelegramId == chatId);
                return currentAction.lengthOfUsersBySubtags;
            }
            if(parametr=="CreateNotification")
            {
                CurrentActions currentAction = Db.CurrentActions.FirstOrDefault(n => n.TelegramId == chatId);
                return currentAction.CreateNotification;
            }
            return 0;

        }
        public void AddlengthOfUsersBySubtags(long chatId,int count)
        {
            CurrentActions currentAction = Db.CurrentActions.FirstOrDefault(n => n.TelegramId == chatId);
            currentAction.lengthOfUsersBySubtags = count;
            Db.SaveChanges();
        }
        public void AddAmountForTinder(long chatId, int count)
        {
            CurrentActions currentAction = Db.CurrentActions.FirstOrDefault(n => n.TelegramId == chatId);
            currentAction.amountForTinder = count;
            Db.SaveChanges();
        }
        public bool CheckCurrentSubtag(long chatId,string value)
        {
            List<CurrentSubtags> currentSubtags = Db.CurrentSubtags.ToList();
            User user = Db.Users.FirstOrDefault(n => n.TelegramId == chatId);
            List<Subtag> subtags = Db.Subtags.ToList();
            foreach(var item in currentSubtags)
            {
                if(item.UserId==user.UserId)
                {
                    foreach(var item1 in subtags)
                    {
                        if (item1.Name == value && item.SubtagId==item1.SubtagId)
                            return true;
                    }
                }
            }
            return false;
        }
        public bool CheckCurrentSubtagAboutOthers(long chatId,string value)
        {
            List<CurrentSubtagsAboutOthers> currentSubtags = Db.CurrentSubtagsAboutOthers.ToList();
            User user = Db.Users.FirstOrDefault(n => n.TelegramId == chatId);
            List<Subtag> subtags = Db.Subtags.ToList();
            foreach (var item in currentSubtags)
            {
                if (item.UserId == user.UserId)
                {
                    foreach (var item1 in subtags)
                    {
                        if (item1.Name == value && item.SubtagId == item1.SubtagId)
                            return true;
                    }
                }
            }
            return false;
        }
        public void RemoveCurrentSubtag(long chatId,string value)
        {
            List<CurrentSubtags> currentSubtags = Db.CurrentSubtags.ToList();
            User user = Db.Users.FirstOrDefault(n => n.TelegramId == chatId);
            List<Subtag> subtags = Db.Subtags.ToList();
            foreach (var item in currentSubtags)
            {
                if (item.UserId == user.UserId)
                {
                    foreach (var item1 in subtags)
                    {
                        if (item1.Name == value && item.SubtagId==item1.SubtagId)
                            Db.CurrentSubtags.Remove(item);
                    }
                }
            }
            Db.SaveChanges();
        }
        public void RemoveCurrentSubtagAboutOthers(long chatId, string value)
        {
            List<CurrentSubtagsAboutOthers> currentSubtags = Db.CurrentSubtagsAboutOthers.ToList();
            User user = Db.Users.FirstOrDefault(n => n.TelegramId == chatId);
            List<Subtag> subtags = Db.Subtags.ToList();
            foreach (var item in currentSubtags)
            {
                if (item.UserId == user.UserId)
                {
                    foreach (var item1 in subtags)
                    {
                        if (item1.Name == value && item.SubtagId == item1.SubtagId)
                            Db.CurrentSubtagsAboutOthers.Remove(item);
                    }
                }
            }
            Db.SaveChanges();
        }
        public void AddCurrentNumberOfSurvey(long chatId,string parametr)
        {
            Survey survey = Db.Survey.FirstOrDefault(n => n.Name == parametr);
           CurrentActions user = Db.CurrentActions.FirstOrDefault(n => n.TelegramId == chatId);
            user.NumberOfSurvey =survey.SurveyId;
            Db.CurrentActions.Update(user);
            Db.SaveChanges();
        }
        public Dictionary<int,string> QuestionsForDistribution(long chatId)
        {
            List<Question> questions = Db.Question.ToList();
            Dictionary<int,string> questionss = new Dictionary<int, string>();
            CurrentActions currentAction = Db.CurrentActions.FirstOrDefault(n => n.TelegramId == chatId);
            int surveyId = currentAction.NumberOfSurvey;
            foreach(var item in questions)
            {
                if(item.SurveyId==surveyId)
                {
                    questionss.Add(item.QuestionId,item.Questions);
                }
            }
            return questionss;
        }
        public void AddAnswerToSurvey(long chatId,int numberOfQuestion,int answer)
        {
            Answer answerr = new Answer();
            List<Answer> answers = Db.Answer.ToList();
            foreach(var item in answers)
            {
                if (item.TelegramId == chatId && item.QuestionId == numberOfQuestion)
                    Db.Answer.Remove(item);
            }
            answerr.TelegramId = chatId;
            switch (answer)
            {
                
                case 1:
                    answerr.Answers = "🔥".ToString();
                    break;
                case 2:
                    answerr.Answers = "👍".ToString();
                    break;
                case 3:
                    answerr.Answers = "👌".ToString();
                    break;
                case 4:
                    answerr.Answers = "👎".ToString();
                    break;
                case 5:
                    answerr.Answers = "🤢".ToString();
                    break;
            }
            answerr.QuestionId = numberOfQuestion;
            answerr.time = DateTime.Now;
            Db.Answer.Add(answerr);
             Db.SaveChanges();
            log.AddLog("answer3");
        }
        public string GetQuestion(int numberOfQuestion)
        {
            Question question = Db.Question.FirstOrDefault(N => N.QuestionId == numberOfQuestion);
            return question.Questions;
        }
        public void RemoveAnswer(long chatId,int NumberOfQuestion)
        {
            List<Answer> answers = Db.Answer.ToList();
            foreach (var item in answers)
            {
                if (item.TelegramId == chatId && item.QuestionId == NumberOfQuestion)
                    Db.Answer.Remove(item);
            }
            Db.SaveChanges();
            
        }
        public long ChatidFromNotebook(long chatId,int count)
        {
             List<NoteBook> notebook = Db.NoteBooks.ToList();
            User user = Db.Users.FirstOrDefault(n => n.TelegramId == chatId);
            int amount = 0;
            long chat = 0;
            int EventId = 0;
            List<CurrentEvent> currentEvents = Db.CurrentEvents.ToList();
            foreach(var item in currentEvents)
            {
                if(item.UserId==user.UserId)
                {
                    EventId = item.EventId;

                }
            }
            foreach(var item in notebook)
            {
                if (item.Sender == chatId && item.EventId==EventId)
                {
                    amount++;
                    if (amount == count)
                    {
                        chat = item.Recipient;
                    }

                }
            }
            return chat;
        }
        public bool CheckPermission(long chatId,long chatIdOfPeopleFromNotebook)
        {
            bool forReturn = false;
            List<NoteBook> noteBooks = Db.NoteBooks.ToList();
            foreach(var item in noteBooks)
            {
                if (item.Sender == chatId && item.Recipient == chatIdOfPeopleFromNotebook)
                    forReturn = true;
            }
            return forReturn;
        }
        public string GetInformationAboutPeopleForCommunication(long chatId)
        {
            User user = Db.Users.FirstOrDefault(n => n.TelegramId == chatId);
            string temp = "";
            if (user.Name != null)
            {
                
                temp = temp + "<b>";
                temp = temp + user.Name.ToString();
                temp = temp + ", </b>";
            }
            if (user.Position != null)
            {
                temp = temp + user.Position.ToString();
                temp = temp + "\n";
            }
            if (user.Work != null)
            {
                temp = temp + "<i>Компания:  </i>";
                temp = temp + user.Work.ToString();
                temp = temp + "\n \n";
            }

            temp = temp + "<i>Теги:  </i>";
            List<CurrentSubtags> tags = Db.CurrentSubtags.ToList();
            List<Subtag> subtags = Db.Subtags.ToList();
            foreach (var item in tags)
            {
                if (item.UserId == user.UserId)
                {
                    foreach (var item1 in subtags)
                    {
                        if (item.SubtagId == item1.SubtagId)
                            temp = temp +"<code>"+ item1.Name +",</code> ";
                    }
                }
            }
            temp = temp + "\n \n";
            if (user.Usufulness != null)
            {
                temp = temp + "<i>Чем полезен:  </i>";
                temp = temp + user.Usufulness.ToString();
                temp = temp + "\n";
            }
            if (user.AboutTalkingWishes != null)
            {
                temp = temp + "<i>О чём может пообщаться:  </i>";
                temp = temp + user.AboutTalkingWishes.ToString();
                temp = temp + "\n \n";
            }



            temp = temp + "\n";

            return temp;
        }
        public void AddCurrentTag(long chatId,string tag)
        {
            User user = Db.Users.FirstOrDefault(n => n.TelegramId == chatId);
            List<Tag> tags = Db.Tags.ToList();
            int TagId = 0;
            foreach(var item in tags)
            {
                if (item.Name == tag)
                    TagId = item.TagId;
            }

        }
       
    }
}
