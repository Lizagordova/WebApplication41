using System;
using System.Collections.Generic;
using System.Net;
using MimeKit;
using MailKit;
using MailKit.Net.Smtp;
using System.Net.Mail;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;
using WebApplication41.DB;
using WebApplication41.Models;
using WebApplication41.Models.Telegramm;




namespace WebApplication41.Controllers
{
    [Route("api/message/update")]
    public class MessageController : Controller
    {
        UserDB userDb = new UserDB();
        EventDB eventDb = new EventDB(); private static bool EnterWithEventCode { get; set; } = false;
     

        // GET api/values 
        [HttpGet]
        public string Get()
        {
            return "Method GET unuvalable";
        }
        
        public  async Task SendEmailAsync(string email,string subject,string message)//эмейл получателя,тема письма,текст письма
        {
            LogsDB log = new LogsDB();
            log.AddLog(email);
           var emailMessage = new MimeMessage();//создет объект отправляемого сообщения
            emailMessage.From.Add(new MailboxAddress("Администрация бота", "info@diffind.com"));//определяется отправитель
            emailMessage.To.Add(new MailboxAddress(email));//коллекция получателей
             emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Plain)//тело сообщения
            {
                Text = message
            };
            using (var client = new MailKit.Net.Smtp.SmtpClient())//непосредственно само отправление сообщения
            {
                log.AddLog("send4");
                await client.ConnectAsync("wpl19.hosting.reg.ru", 587,false);//подключение к серверу
                log.AddLog("send5");
                await client.AuthenticateAsync("info@diffind.com", "Diffind123!");//аутенфикация
                log.AddLog("send6");
                await client.SendAsync(emailMessage);//отправка сообщения
                log.AddLog("send7");
                await client.DisconnectAsync(true);//отключение
                log.AddLog("send8");
            }
         }
        // POST api/values 
        [HttpPost]
        public async Task<OkResult> Post([FromBody]Update update)
        {
            if (update == null) return Ok();

            if (update.Type == UpdateType.Message)
            {
                var message = update.Message;
                string[][] tagsKeybord = { new[] { "ОК" }, new[] { "Добавить тег" }, new[] { "Выбрать заново" }, new[] { "На главную страницу" } };
                LogsDB log = new LogsDB();
                log.AddLog("best0");
                var botClient = await Bot.GetBotClientAsync();
                log.AddLog("best1");
                var chatId = message.Chat.Id;
                log.AddLog(chatId.ToString());
                log.AddLog("best2");
                TelegramKeybord keybord = new TelegramKeybord();
                var commands = Bot.Commands;
                log.AddLog("best3");
                string[][] actions = { new[] { "Войти по ивент-коду" }, new[] { "Личный кабинет" } };
                string[][] NetworkingMode = { new[] { "Об ивенте", "Все ивенты" }, new[] { "Мой профиль" }, new[] { "Общение" }, new[] { "Записная книжка" }, new[] { "Вернуться на главную" } };
                log.AddLog("best4");
                foreach (var command in commands)
                {
                    if (command.Contains(message))
                    {
                        await command.Execute(message, botClient);
                        return Ok();
                    }
                }
                log.AddLog("best5");
                if (!userDb.CheckUser(chatId))
                {
                    message.Text = @"/start";
                    foreach (var command in commands)
                    {
                        if (command.Contains(message))
                        {
                            await command.Execute(message, botClient);
                            return Ok();
                        }
                    }
                }
                log.AddLog("best6");
                if (message.Text == "/start" || message.Text == "start")
                {
                    log.AddLog("best7");
                    userDb.CurrentActionOff(chatId, "PrivateCabinet");
                    userDb.CurrentActionOff(chatId, "CheckEmail");
                    userDb.CurrentActionOff(chatId, "NameAndLastName");
                    userDb.CurrentActionOff(chatId, "ChoseTag");
                    userDb.CurrentActionOff(chatId, "EnterWithEventCode");
                    userDb.CurrentActionOff(chatId, "EventCode");
                    userDb.CurrentActionOff(chatId, "Email");
                    userDb.CurrentActionOff(chatId, "AboutEvent");
                    userDb.CurrentActionOff(chatId, "Nerworking");
                    userDb.CurrentActionOff(chatId, "Work");
                    userDb.CurrentActionOff(chatId, "Position");
                    userDb.CurrentActionOff(chatId, "AboutWishes");
                    userDb.CurrentActionOff(chatId, "ChoseSubtags");
                    userDb.CurrentActionOff(chatId, "ChoseSubtagsAboutOthers");
                    userDb.CurrentActionOff(chatId, "ChoseTagAboutOthers");
                    userDb.CurrentActionOff(chatId, "Notebook");
                    userDb.CurrentActionOff(chatId, "NetworkingFull");
                    userDb.CurrentActionOff(chatId, "MyProfile");
                    userDb.CurrentActionOff(chatId, "editName");
                    userDb.CurrentActionOff(chatId, "editWork");
                    userDb.CurrentActionOff(chatId, "editPosition");
                    userDb.CurrentActionOff(chatId, "editAboutWishes");
                    userDb.CurrentActionOff(chatId, "editTags");
                    userDb.CurrentActionOff(chatId, "EditTag");
                    userDb.CurrentActionOff(chatId, "EditSubtag");
                    userDb.CurrentActionOff(chatId, "EditTagAboutOthers");
                    userDb.CurrentActionOff(chatId, "EditSubtagAboutOthers");
                    userDb.CurrentActionOff(chatId, "AddTag");
                    userDb.CurrentActionOff(chatId, "AddSubtag");
                    userDb.CurrentActionOff(chatId, "ChoseOldEvent");
                    userDb.CurrentActionOff(chatId, "AddSubtagAboutOthers");
                    userDb.CurrentActionOff(chatId, "AddTagAboutOthers");
                    userDb.CurrentActionOff(chatId, "AddInformationAboutEvent");
                    userDb.CurrentActionOff(chatId, "EnteranceForOrganisators");
                    userDb.CurrentActionOff(chatId, "CreateNotification");
                    userDb.CurrentActionOff(chatId, "Usefulness");
                    userDb.CurrentActionOff(chatId, "editUsefulness");
                    userDb.CurrentActionOff(chatId, "CreateSurvey");
                    userDb.CurrentActionOff(chatId, "AddQuestionToSurvey");
                    log.AddLog("best8");
                }
                log.AddLog("best9");
                if (message.From.Username != null)
                {
                    var from = message.From.Username;
                    userDb.AddElement(chatId, "Username", from);
                }
                log.AddLog("best10");
                string[][] funcional = { new[] { "О мероприятии" }, new[] { "Режим общения" }, new[] { "Записная книжка" }, new[] { "Все мероприятия" } };
                string[][] organisatorMode = { new[] { "Об ивенте" }, new[] { "Информация о пользователях" }, new[] { "Создать опрос" }, new[] { "Создать оповещение" }, new[] { "Войти как обычный участник" } };

                //ВХОД ПО ИВЕНТ-КОДУ
                if (message.Text == "Войти по ивент-коду")
                {
                    userDb.CurrentActionOn(chatId, "EventCode");
                    userDb.CurrentActionOff(chatId, "CheckEmail");                   
                    if(userDb.CheckElements(chatId,"Name"))
                    await botClient.SendTextMessageAsync(chatId, "Введите ивент-код, пожалуйста", ParseMode.Html);
                    else await botClient.SendTextMessageAsync(chatId, "Мы с вами не знакомы, но обязательно это исправим. А пока введите ивент-код");
                    return Ok();
                }
                //ВХОД СРАЗУ В ЛИЧНЫЙ КАБИНЕТtagskey
                if (message.Text == "Личный кабинет")
                {
                    log.AddLog("enter to private cabinet");
                    userDb.CurrentActionOff(chatId, "NameAndLastName");
                    userDb.CurrentActionOff(chatId, "Email");
                    userDb.CurrentActionOff(chatId, "EventCode");
                    userDb.CurrentActionOn(chatId, "CheckEmail");
                    log.AddLog("enter to private cabinet1");
                    await botClient.SendTextMessageAsync(chatId, "Как хорошо, что мы уже знакомы"+ "😄".ToString()+" Введите почту, на которую регистрировались");
                    return Ok();
                }
              
                if (userDb.CheckCurrentAction(chatId, "Email") == 1)//ДОБАВЛЯЕМ ПОЧТУ
                {
                    userDb.CurrentActionOff(chatId, "EnterWithEventCode");
                    userDb.CurrentActionOff(chatId, "Email");
                    userDb.CurrentActionOn(chatId, "PrivateCabinet");
                    //userDb.CurrentActionOff(chatId,"")
                    userDb.AddElement(chatId, "Email", message.Text);
                    await botClient.SendTextMessageAsync(chatId, "Просто прекрасно" + "🧐".ToString() + " Сейчас вам доступен мой функционал", replyMarkup: keybord.GetKeyboard(funcional));
                    return Ok();
                }
                if (userDb.CheckCurrentAction(chatId, "NameAndLastName") == 1/* || userDb.CheckElements(chatId, "Email") == false*/)//ЕСЛИ ЕЩЁ НЕТ ИМЕНИ С ФАМИЛИЕЙ->ДОБАВЛЯЕМ И ПРЕДЛАГАЕМ ВВЕСТИ АДРЕС ЭЛЕКТРОННОЙ ПОЧТЫ
                {
                    userDb.CurrentActionOff(chatId, "NameAndLastName");
                    userDb.CurrentActionOn(chatId, "Email");
                    userDb.AddElement(chatId, "Name", message.Text);
                    await botClient.SendTextMessageAsync(chatId, "Вот мы и познакомились, а теперь введите адрес электронной почты", ParseMode.Markdown);
                    return Ok();
                }
                if (userDb.CheckCurrentAction(chatId, "EventCode") == 1)
                {
                    if (eventDb.CheckCode(message.Text))
                    {
                        userDb.CurrentActionOff(chatId, "EventCode");
                        if (userDb.CheckElements(chatId, "Event") == false)
                        {
                            userDb.AddElement(chatId, "Code", message.Text);
                        }
                        else
                        {
                            userDb.ReplaceEvent(chatId, message.Text);
                        }
                        // userDb.AddElement(chatId, "AllEvents", message.Text);
                        string nameOfEvent = eventDb.GetName(message.Text);
                        string temp;
                        if (userDb.CheckElements(chatId, "Name") == false)
                        {
                            userDb.CurrentActionOn(chatId, "NameAndLastName");
                            temp = "Вы подключились к " + eventDb.GetName(message.Text) + ". А теперь познакомимся. Как вас зовут? Имя и фамилию, пожалуйста";
                            await botClient.SendTextMessageAsync(chatId, temp, ParseMode.Markdown);
                            return Ok();
                        }
                        else if (userDb.CheckElements(chatId, "Email") == false)
                        {
                            userDb.CurrentActionOn(chatId, "Email");
                            temp = "Вы подключились к " + eventDb.GetName(message.Text) + ".Введите электронную почту, пожалуйста";
                            await botClient.SendTextMessageAsync(chatId, temp, ParseMode.Markdown);
                            return Ok();
                        }
                        else
                        {
                            userDb.CurrentActionOn(chatId, "PrivateCabinet");
                            temp = "Вы подключились к " + eventDb.GetName(message.Text);
                            await botClient.SendTextMessageAsync(chatId, temp, replyMarkup: keybord.GetKeyboard(funcional));
                            await botClient.SendTextMessageAsync(chatId, "Просто прекрасно" + "🧐".ToString() + " Сейчас вам доступен мой функционал");
                            return Ok();
                        }
                    }
                    else if (eventDb.CheckCodeForOrganisators(message.Text))
                    {
                        userDb.CurrentActionOff(chatId, "EventCode");
                         userDb.CurrentActionOn(chatId, "EnteranceForOrganisators");
                        if (userDb.CheckElements(chatId, "Event") == false)
                        {
                            userDb.AddElement(chatId, "Code", message.Text);
                        }
                        else
                        {
                            userDb.ReplaceEvent(chatId, message.Text);
                        }
                        await botClient.SendTextMessageAsync(chatId, "Включён режим организатора. Вам доступен расширенный функционал", replyMarkup: keybord.GetKeyboard(organisatorMode));
                        return Ok();
                    }
                    else
                    {
                        await botClient.SendTextMessageAsync(chatId, "Вы неправильно ввели код, введите ещё раз", ParseMode.Markdown);
                        return Ok();
                    }
                }
               
                if (message.Text == "На главную" && userDb.CheckCurrentAction(chatId, "NetworkingFull") == 1)//обработай лучше
                {
                    await botClient.SendTextMessageAsync(chatId, "Вы вернулись в главное меню", replyMarkup: keybord.GetKeyboard(NetworkingMode));
                    return Ok();
                }
                else if (message.Text == "На главную" && userDb.CheckCurrentAction(chatId, "NetworkingFull") == 0)
                {
                    await botClient.SendTextMessageAsync(chatId, "Вы вернулись в главное меню", replyMarkup: keybord.GetKeyboard(funcional));
                    return Ok();
                }
                string[][] myProfile = { new[] { "Редактировать профиль" }, new[] { "Изменить теги" }, new[] { "Выбрать заново теги" }, new[] { "Вернуться на главную" } };
                if (message.Text == "Мой профиль" && userDb.CheckCurrentAction(chatId, "PrivateCabinet") == 1)
                {
                    userDb.CurrentActionOn(chatId, "MyProfile");
                    string temp = userDb.GetMyProfile(chatId);
                    string[][] toMain = { new[] { "На главную" } };
                    await botClient.SendTextMessageAsync(chatId, temp,ParseMode.Html, replyMarkup: keybord.GetKeyboard(myProfile));
                    return Ok();

                }
                string[][] editProfile = { new[] { "Имя и фамилия" }, new[] { "Работа" }, new[] { "Полезность" }, new[] { "О чем пообщаться" }, new[] { "Вернуться в мой профиль" } };
                if (message.Text == "Добавить тег" && userDb.CheckCurrentAction(chatId, "Networking") == 1 &&  userDb.CheckCurrentAction(chatId,"ChoseSubtags")==1 && userDb.CheckCurrentAction(chatId, "editTags")==0)//03.07.19
                {
                    userDb.CurrentActionOn(chatId, "AddTag");
                    List<string> tags = userDb.GetTags();
                    string[][] tagss = new string[tags.Count + 1][];
                    string temp = "Ваши теги:    \n" + userDb.GetUserTags(chatId) + "Выберите ещё теги";
                    int count = 0;
                    foreach (var item in tags)
                    {
                        tagss[count] = new[] { item };
                        count++;
                    }
                    tagss[count] = new[] { "Вернуться в меню" };
                    await botClient.SendTextMessageAsync(chatId, temp, replyMarkup: keybord.GetKeyboard(tagss));
                    return Ok();
                }
                if (message.Text == "Добавить тег" && userDb.CheckCurrentAction(chatId, "Networking") == 1 &&  userDb.CheckCurrentAction(chatId, "ChoseSubtagsAboutOthers") == 1)//03.07.19
                {
                    userDb.CurrentActionOn(chatId, "AddTagAboutOthers");
                    List<string> tags = userDb.GetTags();
                    string[][] tagss = new string[tags.Count + 1][];
                    string temp = "Ваши теги о других людях:    \n" + userDb.GetUsersSubtagsAboutOthers(chatId) + "Выберите ещё теги";
                    int count = 0;
                    foreach (var item in tags)
                    {
                        tagss[count] = new[] { item };
                        count++;
                    }
                    tagss[count] = new[] { "Вернуться в меню" };
                    await botClient.SendTextMessageAsync(chatId, temp,ParseMode.Html, replyMarkup: keybord.GetKeyboard(tagss));
                    return Ok();
                }
                if (message.Text == "Добавить тег" && userDb.CheckCurrentAction(chatId, "Networking") == 1  && userDb.CheckCurrentAction(chatId, "EditSubtagAboutOthers") == 1)//03.07.19
                {
                    userDb.CurrentActionOn(chatId, "AddTagAboutOthers");
                    List<string> tags = userDb.GetTags();
                    string[][] tagss = new string[tags.Count + 1][];
                    string temp = "Ваши теги:    \n" + userDb.GetUserTags(chatId) + "Выберите ещё теги";
                    int count = 0;
                    foreach (var item in tags)
                    {
                        tagss[count] = new[] { item };
                        count++;
                    }
                    tagss[count] = new[] { "Вернуться назад" };
                    await botClient.SendTextMessageAsync(chatId, temp, replyMarkup: keybord.GetKeyboard(tagss));
                    return Ok();
                }
                if(message.Text=="Вернуться назад" && (userDb.CheckCurrentAction(chatId,"editTags")==1 || userDb.CheckCurrentAction(chatId, "EditSubtag") == 1 || userDb.CheckCurrentAction(chatId, "EditSubtagAboutOthers") == 1))
                {
                    userDb.CurrentActionOff(chatId, "ChoseTagAboutOthers");
                    userDb.CurrentActionOff(chatId, "ChoseSubtagsAboutOthers");
                    userDb.CurrentActionOff(chatId, "ChoseTag");
                    userDb.CurrentActionOff(chatId, "ChoseSubtags");
                    userDb.CurrentActionOff(chatId, "AddTag");
                    userDb.CurrentActionOff(chatId, "AddSubtag");
                    userDb.CurrentActionOff(chatId, "AddSubtagAboutOthers");
                    userDb.CurrentActionOff(chatId, "AddTagAboutOthers");
                    userDb.CurrentActionOff(chatId, "EditTag");
                    userDb.CurrentActionOff(chatId, "EditSubtag");
                    userDb.CurrentActionOff(chatId, "EditSubtagAboutOthers");
                    userDb.CurrentActionOff(chatId, "EditTagAboutOthers");
                    userDb.CurrentActionOff(chatId, "editTags");
                    await botClient.SendTextMessageAsync(chatId, "Я вернулся", replyMarkup: keybord.GetKeyboard(myProfile));
                    return Ok();
                }

                if (message.Text == "Добавить тег" && userDb.CheckCurrentAction(chatId, "Networking") == 1 && userDb.CheckCurrentAction(chatId, "EditSubtag") == 1)//03.07.19
                {
                    userDb.CurrentActionOn(chatId, "AddTag");
                    List<string> tags = userDb.GetTags();
                    string[][] tagss = new string[tags.Count + 1][];
                    string temp = "Ваши теги: " + userDb.GetUserTags(chatId) + "\n Выберите ещё теги";
                    int count = 0;
                    foreach (var item in tags)
                    {
                        tagss[count] = new[] { item };
                        count++;
                    }
                    tagss[count] = new[] { "Вернуться назад" };
                    await botClient.SendTextMessageAsync(chatId, temp, replyMarkup: keybord.GetKeyboard(tagss));
                    return Ok();
                }
                string[][] choseCurrentSubtags = { new[] { "1", "2", "3", "4","5"}, new[] {  "6", "7", "8", "9","10" }, new[] { "Показывать всех" } };
                string[][] choseCurrentSubtagsCallBack = { new[] { "01", "02", "03", "04", "05" }, new[] {  "06", "07", "08", "09","0-"}, new[] { "00" } };
                string[][] choseCurrentSubtagsAboutOthers = { new[] { "1", "2", "3", "4","5" }, new[] { "6", "7", "8", "9","10" }, new[] { "Показать всех" } };
                string[][] choseCurrentSubtagsCallBackAboutOthers = { new[] { "11", "12", "13", "14", "15" }, new[] {  "16", "17", "18", "19","0-" }, new[] { "10" } };
                if (userDb.CheckCurrentAction(chatId, "AddTag") == 1)
                {
                    userDb.CurrentActionOff(chatId, "AddTag");
                    userDb.CurrentActionOff(chatId, "EditTag");
                  //  userDb.CurrentActionOff(chatId, "EditSubtag");
                   // userDb.CurrentActionOff(chatId, "ChoseSubtag");
                    List<string> chosenTag = new List<string>();
                    chosenTag.Add(message.Text);
                    userDb.AddElement(chatId, "CurrentTagss", message.Text);
                    string temp;
                    temp = "Ваши теги:" ;
                    // +get userscurrentsubtags на каждом шаге
                    userDb.CurrentActionOn(chatId, "AddSubtag");
                    string temp1 = userDb.GetAllSubtags(chosenTag);
                    await botClient.SendTextMessageAsync(chatId, temp1, replyMarkup: keybord.GetKeyboard(tagsKeybord));
                    await botClient.SendTextMessageAsync(chatId, temp, replyMarkup: keybord.GetInlineKeyboard(choseCurrentSubtags, choseCurrentSubtagsCallBack));
                    return Ok();
                }
                if (userDb.CheckCurrentAction(chatId, "AddTagAboutOthers") == 1)
                {
                      List<string> chosenTagAboutOthers = new List<string>();
                    chosenTagAboutOthers.Add(message.Text);
                    userDb.CurrentActionOff(chatId, "AddTagAboutOthers");
                    userDb.CurrentActionOff(chatId, "EditTagAboutOthers");
                    //userDb.CurrentActionOff(chatId, "EditSubtagAboutOthers");
                  //  userDb.CurrentActionOff(chatId, "ChoseSubtagsAboutOthers");
                    List<string> chosenTag = new List<string>();
                    chosenTag.Add(message.Text);
                    userDb.AddElement(chatId, "CurrentTagAboutOtherss", message.Text);
                    string temp;
                    temp = "Теги для других людей: " + userDb.GetUsersSubtagsAboutOthers(chatId);
                    userDb.CurrentActionOn(chatId, "AddSubtagAboutOthers");
                    string temp1 = userDb.GetAllSubtags(chosenTagAboutOthers);
                    await botClient.SendTextMessageAsync(chatId, temp1, replyMarkup: keybord.GetKeyboard(tagsKeybord));
                    await botClient.SendTextMessageAsync(chatId, temp,ParseMode.Html, replyMarkup: keybord.GetInlineKeyboard(choseCurrentSubtagsAboutOthers, choseCurrentSubtagsCallBackAboutOthers));
                    return Ok();
                }
                if (message.Text == "ОК" && userDb.CheckCurrentAction(chatId, "AddSubtagAboutOthers") == 1)//03.07.19
                {
                    userDb.CurrentActionOff(chatId, "AddSubtagAboutOthers");
                    userDb.CurrentActionOn(chatId, "NetworkingFull");
                    userDb.CurrentActionOff(chatId, "ChoseSubtagsAboutOthers");
                    userDb.CurrentActionOff(chatId, "EditSubtagAboutOthers");
                    string temp = "Теги нужных людей: " + "\n" + userDb.GetUsersSubtagsAboutOthers(chatId);
                    await botClient.SendTextMessageAsync(chatId, temp,ParseMode.Html);
                    await botClient.SendTextMessageAsync(chatId, "Поздравляю! Настройка режима общения завершена", replyMarkup: keybord.GetKeyboard(NetworkingMode));
                    return Ok();
                }
                if (message.Text == "ОК" && userDb.CheckCurrentAction(chatId, "AddSubtag") == 1)//03.07.19
                {
                    userDb.CurrentActionOff(chatId, "AddSubtag");
                    userDb.CurrentActionOff(chatId, "ChoseSubtags");
                    userDb.CurrentActionOff(chatId, "EditSubtag");
                    if (userDb.CheckCurrentAction(chatId, "MyProfile") == 1)
                    {
                        userDb.CurrentActionOn(chatId, "AddTagAboutOthers");
                        await botClient.SendTextMessageAsync(chatId, "Все сохранено", replyMarkup: keybord.GetKeyboard(myProfile));
                        return Ok();
                    }
                    List<string> choseTags = userDb.GetTags();
                    string[][] choseTagss = new string[choseTags.Count + 1][];
                    int count = 0;
                    foreach (var item in choseTags)
                    {
                        choseTagss[count] = new[] { item };
                        count++;
                    }
                    choseTagss[count] = new[] { "На главную страницу" };
                    await botClient.SendTextMessageAsync(chatId, "Почти всё. Осталось выбрать теги людей, которые нужны ВАМ", replyMarkup: keybord.GetKeyboard(choseTagss));
                    return Ok();
                }
                log.AddLog("to back0");
                log.AddLog(chatId.ToString());
                if (message.Text == "Вернуться в меню" && userDb.CheckCurrentAction(chatId,"NetworkingFull")==0)//03.07.19 ПОСМОТРИ ОТКУДА ОН ВОЗВРАЩАЕТСЯ
                {
                  
                    userDb.CurrentActionOff(chatId, "ChoseOldEvent");
                    await botClient.SendTextMessageAsync(chatId, "Вы в главном меню", replyMarkup: keybord.GetKeyboard(funcional));
                    return Ok();
                }
                log.AddLog("to back1");
                if (message.Text == "Вернуться в меню" && userDb.CheckCurrentAction(chatId, "NetworkingFull") == 1)
                {
                    log.AddLog("to back2");
                    userDb.CurrentActionOff(chatId, "ChoseOldEvent");
                    log.AddLog("to back3");
                    await botClient.SendTextMessageAsync(chatId, "Вы вернулись в меню", replyMarkup: keybord.GetKeyboard(NetworkingMode));
                    return Ok();

                }
                if (message.Text=="Вернуться назад" && userDb.CheckCurrentAction(chatId, "ChoseOldEvent") == 1)
                {
                    userDb.CurrentActionOff(chatId, "ChoseOldEvent");
                    await botClient.SendTextMessageAsync(chatId, "Я вернулся", replyMarkup: keybord.GetKeyboard(funcional));
                    return Ok();
                }
                if (message.Text == "Все ивенты" && userDb.CheckCurrentAction(chatId, "NetworkingFull") == 1)
                {
                    userDb.CurrentActionOn(chatId, "ChoseOldEvent");
                    int chatik = userDb.GetUserId(chatId);
                    List<string> allEvents = userDb.GetAllEvents(chatik);
                    string[][] allEventss = new string[allEvents.Count + 1][];
                    int count = 0;
                    foreach(var item in allEvents)
                    {
                        allEventss[count] = new[] { item };
                        count++;
                    }
                    allEventss[count] = new[] { "Вернуться в меню" };
                    await botClient.SendTextMessageAsync(chatId, "Вы принимали участие в этих мероприятиях"+ " 👇".ToString(),replyMarkup: keybord.GetKeyboard(allEventss));
                    return Ok();
                }
                string[][] backFromEvents = { new[] { "Вернуться назад" } };
                if (userDb.CheckCurrentAction(chatId, "ChoseOldEvent") == 1)//03.07.19
                {

                    string temp = eventDb.GetInformationAboutEventWithName(message.Text);
                    await botClient.SendTextMessageAsync(chatId, temp, replyMarkup: keybord.GetKeyboard(backFromEvents));
                    /* userDb.ReplaceEventWithName(chatId, message.Text);
                     string temp;
                     userDb.CurrentActionOn(chatId, "PrivateCabinet");
                     temp = "Вы подключились к " + eventDb.GetName(message.Text);
                     await botClient.SendTextMessageAsync(chatId, temp, replyMarkup: keybord.GetKeyboard(funcional));
                     await botClient.SendTextMessageAsync(chatId, "Просто прекрасно. Сейчас вам доступен мой функционал");*/
                    return Ok();

                }
                
                if (message.Text == "Все мероприятия" && userDb.CheckCurrentAction(chatId, "PrivateCabinet") == 1)//03.07.19
                {
                    userDb.CurrentActionOn(chatId, "ChoseOldEvent");
                    int chatik = userDb.GetUserId(chatId);
                    List<string> allEvents = userDb.GetAllEvents(chatik);
                    string[][] allEventss = new string[allEvents.Count + 1][];
                    int count = 0;
                    foreach (var item in allEvents)
                    {
                        allEventss[count] = new[] { item };
                        count++;
                    }
                    allEventss[count] = new[] { "Вернуться в меню" };
                    await botClient.SendTextMessageAsync(chatId, "Вы принимали участие в этих мероприятиях", replyMarkup: keybord.GetKeyboard(allEventss));//ЗДЕСЬ ФРАЗУ ЗАМЕНИТЬ
                    return Ok();//
                }
                string[][] toBackFromEdition = { new[] { "Отменить" } };
                if (message.Text == "Редактировать профиль" && userDb.CheckCurrentAction(chatId, "MyProfile") == 1)
                {
                    await botClient.SendTextMessageAsync(chatId, "Выберите пункты для редактирования \n P.S. Теги поиска - теги людей, которые нужны ВАМ \n Личные теги - это ВАШИ теги", replyMarkup: keybord.GetKeyboard(editProfile));
                    return Ok();
                }
                if (message.Text == "Имя и фамилия" && userDb.CheckCurrentAction(chatId, "MyProfile") == 1)
                {
                    userDb.CurrentActionOn(chatId, "editName");
                   await botClient.SendTextMessageAsync(chatId, "Введите имя и фамилию", replyMarkup: keybord.GetKeyboard(toBackFromEdition));
                    return Ok();
                }
                if (userDb.CheckCurrentAction(chatId, "editName") == 1)
                {
                    userDb.CurrentActionOff(chatId, "editName");
                    userDb.ChangeElement(chatId, "Name", message.Text);
                    await botClient.SendTextMessageAsync(chatId, "Отлично, данные сохранены", replyMarkup: keybord.GetKeyboard(editProfile));
                    return Ok();
                }
                if (message.Text == "Работа" && userDb.CheckCurrentAction(chatId, "MyProfile") == 1)
                {
                    userDb.CurrentActionOn(chatId, "editWork");
                   
                    await botClient.SendTextMessageAsync(chatId, "Введите место работы", replyMarkup: keybord.GetKeyboard(toBackFromEdition));
                    return Ok();
                }
                if (userDb.CheckCurrentAction(chatId, "editWork") == 1)
                {
                    userDb.CurrentActionOff(chatId, "editWork");
                    userDb.CurrentActionOn(chatId, "editPosition");
                    userDb.ChangeElement(chatId, "Work", message.Text);
                    await botClient.SendTextMessageAsync(chatId, "Введите должность", replyMarkup: keybord.GetKeyboard(editProfile));
                    return Ok();
                }
                if (message.Text == "Полезность" && userDb.CheckCurrentAction(chatId, "MyProfile") == 1)
                {
                    userDb.CurrentActionOn(chatId, "editUsefulness");
                    await botClient.SendTextMessageAsync(chatId, "Чем вы можете быть полезны?", replyMarkup: keybord.GetKeyboard(toBackFromEdition));
                    return Ok();
                }
                if (userDb.CheckCurrentAction(chatId, "editUsefulness") == 1)
                {
                    userDb.CurrentActionOff(chatId, "editUsefulness");
                    userDb.ChangeElement(chatId, "Usefulness", message.Text);
                    await botClient.SendTextMessageAsync(chatId, "Отлично, данные сохранены", replyMarkup: keybord.GetKeyboard(editProfile));
                    return Ok();
                }
                if (userDb.CheckCurrentAction(chatId, "editPosition") == 1)
                {
                    userDb.CurrentActionOff(chatId, "editPosition");
                    userDb.ChangeElement(chatId, "Position", message.Text);
                    await botClient.SendTextMessageAsync(chatId, "Отлично, данные сохранены", replyMarkup: keybord.GetKeyboard(editProfile));
                    return Ok();
                }
                if (message.Text == "О чём пообщаться" && userDb.CheckCurrentAction(chatId, "MyProfile") == 1)
                {
                  
                    userDb.CurrentActionOn(chatId, "editAboutWishes");
                    await botClient.SendTextMessageAsync(chatId, "О чём бы вы хотели пообщаться?", replyMarkup: keybord.GetKeyboard(toBackFromEdition));
                    return Ok();
                }
                if(message.Text=="Отменить" && userDb.CheckCurrentAction(chatId,"MyProfile")==1)
                {
                    userDb.CurrentActionOff(chatId, "editPosition");
                    userDb.CurrentActionOff(chatId, "editAboutWishes");
                    userDb.CurrentActionOff(chatId, "editUsefulness");
                    userDb.CurrentActionOff(chatId, "editWork");
                    userDb.CurrentActionOff(chatId, "editName");
                    await botClient.SendTextMessageAsync(chatId, "", replyMarkup: keybord.GetKeyboard(editProfile));
                    return Ok();
                }
                if (userDb.CheckCurrentAction(chatId, "editAboutWishes") == 1)
                {
                    userDb.CurrentActionOff(chatId, "editAboutWishes");
                    userDb.ChangeElement(chatId, "AboutWishes", message.Text);
                    await botClient.SendTextMessageAsync(chatId, "Отлично, данные сохранены", replyMarkup: keybord.GetKeyboard(editProfile));
                    return Ok();
                }
                if (message.Text == "Вернуться в мой профиль" && userDb.CheckCurrentAction(chatId, "MyProfile") == 1)
                {
                    userDb.CurrentActionOff(chatId, "editName");
                    userDb.CurrentActionOff(chatId, "editAboutWishes");
                    userDb.CurrentActionOff(chatId, "editPosition");
                    userDb.CurrentActionOff(chatId, "editWork");
                    userDb.CurrentActionOff(chatId, "editTags");
                    userDb.CurrentActionOff(chatId, "editTag");
                    userDb.CurrentActionOff(chatId, "EditSubtag");
                    userDb.CurrentActionOff(chatId, "EditSubtagAboutOthers");
                    userDb.CurrentActionOff(chatId, " EditTagAboutOthers");
                    userDb.CurrentActionOff(chatId, "editName");
                    userDb.CurrentActionOff(chatId, "ChoseTagAboutOthers");
                    userDb.CurrentActionOff(chatId, "ChoseSubtagsAboutOthers");
                    userDb.CurrentActionOff(chatId, "ChoseTag");
                    userDb.CurrentActionOff(chatId, "ChoseSubtags");
                    userDb.CurrentActionOff(chatId, "AddTag");
                    userDb.CurrentActionOff(chatId, "AddSubtag");
                    userDb.CurrentActionOff(chatId, "AddSubtagAboutOthers");
                    userDb.CurrentActionOff(chatId, "AddTagAboutOthers");
                    userDb.CurrentActionOff(chatId, "EditTag");
                    userDb.CurrentActionOff(chatId, "EditSubtag");
                    userDb.CurrentActionOff(chatId, "EditTagAboutOthers");
                    userDb.CurrentActionOff(chatId, "editTags");
                    string temp = userDb.GetMyProfile(chatId);
                    string[][] toMain = { new[] { "На главную" } };
                    await botClient.SendTextMessageAsync(chatId, temp,ParseMode.Html, replyMarkup: keybord.GetKeyboard(myProfile));
                    return Ok();
                }

                string[][] editTagsAndSubtags = { new[] { "Теги поиска" }, new[] { "Личные теги" }, new[] { "Вернуться в мой профиль" } };
                string[][] tagsKeybordEdit = { new[] { "ОК" }, new[] { "Добавить тег" }, new[] { "Вернуться в мой профиль" } };

                if (message.Text == "Изменить теги" && userDb.CheckCurrentAction(chatId, "MyProfile") == 1)
                {
                    //userDb.CurrentActionOn(chatId,"Tags"); ЗДЕСЬ ДОЛЖЕН БЫТЬ ДРУГОЙ ФОЛЗ      
                    userDb.CurrentActionOn(chatId, "editTags");
                    await botClient.SendTextMessageAsync(chatId, "Выберите пункты для редактирования", replyMarkup: keybord.GetKeyboard(editTagsAndSubtags));
                    return Ok();
                }
               
                if (message.Text == "Теги поиска" && userDb.CheckCurrentAction(chatId, "editTags") == 1)
                {
                    userDb.CurrentActionOff(chatId, "editTags");
                    userDb.CurrentActionOn(chatId, "EditTagAboutOthers");
                    userDb.RemoveCurrentTagsAndSubtagsAboutOthers(chatId);
                    List<string> tags = userDb.GetTags();
                    string[][] choseTags = new string[tags.Count + 1][];
                    int count = 0;
                    foreach (var item in tags)
                    {
                        choseTags[count] = new[] { item };
                        count++;
                    }
                    choseTags[count] = new[] { "Вернуться в мой профиль" };
                    await botClient.SendTextMessageAsync(chatId, "Выберите теги", replyMarkup: keybord.GetKeyboard(choseTags));
                    return Ok();
                }
                if (userDb.CheckCurrentAction(chatId, "EditTagAboutOthers") == 1)
                {
                    List<string> chosenTagAboutOthers = new List<string>();
                    chosenTagAboutOthers.Add(message.Text);
                    userDb.CurrentActionOff(chatId, "EditTagAboutOthers");
                    userDb.CurrentActionOn(chatId, "EditSubtagAboutOthers");
                    userDb.AddElement(chatId, "CurrentTagAboutOtherss", message.Text);
                    string temp = "Теги нужных людей:";
                    temp = temp + userDb.GetUserTagsAboutOthers(chatId);
                    string temp1 = userDb.GetAllSubtags(chosenTagAboutOthers);
                    await botClient.SendTextMessageAsync(chatId, temp1, replyMarkup: keybord.GetKeyboard(tagsKeybord));
                    await botClient.SendTextMessageAsync(chatId, temp, replyMarkup: keybord.GetInlineKeyboard(choseCurrentSubtagsAboutOthers, choseCurrentSubtagsCallBackAboutOthers));
                    return Ok();

                }
                if (message.Text == "ОК" && userDb.CheckCurrentAction(chatId, "EditSubtagAboutOthers") == 1)
                {
                    userDb.CurrentActionOff(chatId, "editTags");
                    userDb.CurrentActionOff(chatId, "EditTagAboutOthers");
                    userDb.CurrentActionOff(chatId, "EditSubtagAboutOthers");
                    await botClient.SendTextMessageAsync(chatId, "Данные сохранены", replyMarkup: keybord.GetKeyboard(myProfile));
                    return Ok();

                }
                if (message.Text == "Личные теги" && userDb.CheckCurrentAction(chatId, "editTags") == 1)
                {
                    userDb.CurrentActionOn(chatId, "EditTag");
                    userDb.CurrentActionOff(chatId, "editTags");
                    userDb.RemoveCurrentTagsAndSubtags(chatId);
                    List<string> tags = userDb.GetTags();
                    string[][] choseTags = new string[tags.Count + 1][];
                    int count = 0;
                    foreach (var item in tags)
                    {
                        choseTags[count] = new[] { item };
                        count++;
                    }
                    choseTags[count] = new[] { "Вернуться в мой профиль" };
                    await botClient.SendTextMessageAsync(chatId, "Выберите теги", replyMarkup: keybord.GetKeyboard(choseTags));
                    return Ok();
                }
                if (userDb.CheckCurrentAction(chatId, "EditTag") == 1)//РЕДАКТИРОВАНИЕ ТЕКУЩИХ ТЕГОВ
                {
                    List<string> chosenTag = new List<string>();
                    chosenTag.Add(message.Text);
                    userDb.CurrentActionOff(chatId, "EditTag");
                    userDb.AddElement(chatId, "CurrentTagss", message.Text);
                    string temp;
                    temp = "Ваши теги:";
                    // +get userscurrentsubtags на каждом шаге
                    userDb.CurrentActionOn(chatId, "EditSubtag");
                    string temp1 = userDb.GetAllSubtags(chosenTag);
                    await botClient.SendTextMessageAsync(chatId, temp1,ParseMode.Html, replyMarkup: keybord.GetKeyboard(tagsKeybordEdit));
                    await botClient.SendTextMessageAsync(chatId, temp, replyMarkup: keybord.GetInlineKeyboard(choseCurrentSubtags, choseCurrentSubtagsCallBack));
                    return Ok();
                }
                if (message.Text == "ОК" && userDb.CheckCurrentAction(chatId, "EditSubtag") == 1)
                {
                    userDb.CurrentActionOff(chatId, "editTags");
                    userDb.CurrentActionOff(chatId, "EditTag");
                    userDb.CurrentActionOff(chatId, "EditSubtag");

                    await botClient.SendTextMessageAsync(chatId, "Данные сохранены", replyMarkup: keybord.GetKeyboard(myProfile));
                    return Ok();

                }
                if (message.Text == "Записная книжка" && userDb.CheckCurrentAction(chatId, "PrivateCabinet") == 1)//ЗАПИСНАЯ КНИЖКА
                {
                    string[][] ToMain = { new[] { "На главную" } };
                    log.AddLog("notebook1");
                    string ListOfSubtags = "Ваши теги: \n" + userDb.GetUsersSubtagsAboutOthers(chatId) +"\n";
                    log.AddLog("notebook2");
                    string[][] ActionsInNoteBook = { new[] { "⬅️".ToString(), "➡️".ToString() }, new[] { "1", "2", "3", "4" } };
                    int count = 1;
                    log.AddLog("notebook3");
                    string temp = "Ваши контакты: \n" + userDb.AllListFromNotebook(chatId, count);
                    log.AddLog("notebook4");
                    string ToBack = "7-" + count.ToString();
                    string ToAhead = "8-" + count.ToString();
                    string[][] ActionsInNoteBookCallBack = { new[] { ToBack, ToAhead }, new[] { "91-"+count.ToString(), "91-" + (count + 1).ToString(), "91-" + (count + 2).ToString(), "91-" + (count + 3).ToString() } };
                    log.AddLog("notebook5");
                    await botClient.SendTextMessageAsync(chatId,ListOfSubtags+temp,ParseMode.Html, replyMarkup: keybord.GetInlineKeyboard(ActionsInNoteBook, ActionsInNoteBookCallBack));
                    await botClient.SendTextMessageAsync(chatId, "Здесь я храню ваши выбранные контакты ", replyMarkup: keybord.GetKeyboard(ToMain));
                    return Ok();
                }
                if (message.Text == "Вернуться на главную" && userDb.CheckCurrentAction(chatId, "NetworkingFull") == 1 )
                {
                                    
                   
                    userDb.CurrentActionOff(chatId, "ChoseTagAboutOthers");
                    userDb.CurrentActionOff(chatId, "ChoseSubtagsAboutOthers");
                    userDb.CurrentActionOff(chatId, "ChoseTag");
                    userDb.CurrentActionOff(chatId, "ChoseSubtags");
                    userDb.CurrentActionOff(chatId, "AddTag");
                    userDb.CurrentActionOff(chatId, "AddSubtag");
                    userDb.CurrentActionOff(chatId, "AddSubtagAboutOthers");
                    userDb.CurrentActionOff(chatId, "AddTagAboutOthers");
                    userDb.CurrentActionOff(chatId, "EditTag");
                    userDb.CurrentActionOff(chatId, "EditSubtag");
                    userDb.CurrentActionOff(chatId, "EditSubtagAboutOthers");
                    userDb.CurrentActionOff(chatId, "EditTagAboutOthers");
                    userDb.CurrentActionOff(chatId, "editTags");
                    if (userDb.CheckCurrentAction(chatId, "MyProfile") == 1)
                    {
                        userDb.CurrentActionOff(chatId, "MyProfile");
                        await botClient.SendTextMessageAsync(chatId, "Я вернулся на главную страницу", replyMarkup: keybord.GetKeyboard(NetworkingMode));
                        return Ok();
                    }
                    userDb.CurrentActionOff(chatId, "MyProfile");
                    userDb.CurrentActionOff(chatId, "NetworkingFull");
                    userDb.CurrentActionOff(chatId, "Networking");
                    await botClient.SendTextMessageAsync(chatId, "Я вернулся на главную страницу", replyMarkup: keybord.GetKeyboard(funcional));
                    return Ok();
                }
                if (message.Text == "Выбрать заново" && userDb.CheckCurrentAction(chatId, "PrivateCabinet") == 1)//ВЫБРАТЬ ЗАНОВО ТЕГИ
                {
                    userDb.RemoveCurrentTagsAndSubtags(chatId);
                    userDb.RemoveCurrentTagsAndSubtagsAboutOthers(chatId);//может быть и не надо

                    userDb.CurrentActionOn(chatId, "ChoseTag");
                    userDb.CurrentActionOff(chatId, "ChoseSubtags");
                    userDb.CurrentActionOff(chatId, "ChoseTagAboutOthers");
                    userDb.CurrentActionOff(chatId, "ChoseSubtagsAboutOthers");
                    List<string> tags = userDb.GetTags();
                    string[][] choseTags = new string[tags.Count + 1][];
                    int count = 0;
                    foreach (var item in tags)
                    {
                        choseTags[count] = new[] { item };
                        count++;
                    }
                    choseTags[count] = new[] { "На главную страницу" };
                    await botClient.SendTextMessageAsync(chatId, "Выберите теги, которые подходят ВАМ", replyMarkup: keybord.GetKeyboard(choseTags));
                    return Ok();

                }
                  if (message.Text == "Выбрать заново теги" && userDb.CheckCurrentAction(chatId, "MyProfile") == 1)
             {
                    userDb.RemoveCurrentTagsAndSubtags(chatId);
                    userDb.RemoveCurrentTagsAndSubtagsAboutOthers(chatId);//может быть и не надо
                    userDb.CurrentActionOn(chatId, "ChoseTag");
                    userDb.CurrentActionOff(chatId, "ChoseSubtags");
                    userDb.CurrentActionOff(chatId, "ChoseTagAboutOthers");
                    userDb.CurrentActionOff(chatId, "ChoseSubtagsAboutOthers");
                    List<string> tags = userDb.GetTags();
                    string[][] choseTags = new string[tags.Count + 1][];
                    int count = 0;
                    foreach (var item in tags)
                    {
                        choseTags[count] = new[] { item };
                        count++;
                    }
                    choseTags[count] = new[] { "На главную страницу" };
                    await botClient.SendTextMessageAsync(chatId, "Выберите теги, которые подходят ВАМ", replyMarkup: keybord.GetKeyboard(choseTags));
                    //await botClient.SendTextMessageAsync(chatId, "Выберите пункты для редактирования", replyMarkup: keybord.GetKeyboard(editTagsAndSubtags));
                 return Ok();
             }

                if (message.Text == "На главную страницу" && userDb.CheckCurrentAction(chatId, "PrivateCabinet") == 1 && userDb.CheckCurrentAction(chatId,"NetworkingFull")==1)
                {
                    userDb.CurrentActionOff(chatId, "ChoseTagAboutOthers");
                    userDb.CurrentActionOff(chatId, "ChoseSubtagsAboutOthers");
                    userDb.CurrentActionOff(chatId, "ChoseTag");
                    userDb.CurrentActionOff(chatId, "ChoseSubtags");
                    userDb.CurrentActionOff(chatId, "AddTag");
                    userDb.CurrentActionOff(chatId, "AddSubtag");
                    userDb.CurrentActionOff(chatId, "AddSubtagAboutOthers");
                    userDb.CurrentActionOff(chatId, "AddTagAboutOthers");
                    userDb.CurrentActionOff(chatId, "EditTag");
                    userDb.CurrentActionOff(chatId, "EditSubtag");
                    userDb.CurrentActionOff(chatId, "EditSubtagAboutOthers");
                    userDb.CurrentActionOff(chatId, "EditTagAboutOthers");
                    userDb.CurrentActionOff(chatId, "editTags");

                    await botClient.SendTextMessageAsync(chatId, "Вы вернулись на главную страницу", replyMarkup: keybord.GetKeyboard(NetworkingMode));
                    return Ok();
                }
                if (message.Text == "На главную страницу" && userDb.CheckCurrentAction(chatId, "PrivateCabinet") == 1 && userDb.CheckCurrentAction(chatId, "NetworkingFull") == 0)
                {
                    userDb.CurrentActionOff(chatId, "Networking");
                    userDb.CurrentActionOff(chatId, "ChoseTagAboutOthers");
                    userDb.CurrentActionOff(chatId, "ChoseSubtagsAboutOthers");
                    userDb.CurrentActionOff(chatId, "ChoseTag");
                    userDb.CurrentActionOff(chatId, "ChoseSubtags");
                    userDb.CurrentActionOff(chatId, "AddTag");
                    userDb.CurrentActionOff(chatId, "AddSubtag");
                    userDb.CurrentActionOff(chatId, "AddSubtagAboutOthers");
                    userDb.CurrentActionOff(chatId, "AddTagAboutOthers");
                    userDb.CurrentActionOff(chatId, "EditTag");
                    userDb.CurrentActionOff(chatId, "EditSubtag");
                    userDb.CurrentActionOff(chatId, "EditSubtagAboutOthers");
                    userDb.CurrentActionOff(chatId, "EditTagAboutOthers");
                    userDb.CurrentActionOff(chatId, "editTags");
                    await botClient.SendTextMessageAsync(chatId, "Вы вернулись на главную страницу", replyMarkup: keybord.GetKeyboard(funcional));
                    return Ok();
                }
               
                if (userDb.CheckCurrentAction(chatId, "AddInformationAboutEvent") == 1)
                {
                    if (message.Text == "Сохранить")
                    {
                       // userDb.AddElement(chatId, "InformationAboutEvent", message.Text);
                        userDb.CurrentActionOff(chatId, "AddInformationAboutEvent");
                        await botClient.SendTextMessageAsync(chatId, "Данные о мероприятии успешно сохранены", replyMarkup: keybord.GetKeyboard(organisatorMode));
                        return Ok();
                    }
                    userDb.AddElement(chatId, "InformationAboutEvent", message.Text);
                    return Ok();

                }
                string[][] save = { new[] { "Сохранить" } };
                if (message.Text == "Об ивенте" && userDb.CheckCurrentAction(chatId, "EnteranceForOrganisators") == 1)
                {
                    userDb.CurrentActionOn(chatId, "AddInformationAboutEvent");
                    string temp = eventDb.GetInformationAboutEvent(chatId);
                    await botClient.SendTextMessageAsync(chatId,temp+ "\n \nДобавьте всю необходимую информацию в виде статьи на телеграф и отправьте ссылку", replyMarkup: keybord.GetKeyboard(save));
                    return Ok();
                }
                string[][] informationAboutUsers = new[] { new[]{ "Количество пользователей" }, new[] { "Количество активаций нетворкинга" },new[] { "Выйти" } };
                if (message.Text == "Информация о пользователях" && userDb.CheckCurrentAction(chatId, "EnteranceForOrganisators") == 1)
                {
                   
                    await botClient.SendTextMessageAsync(chatId,"Выберите действие", replyMarkup: keybord.GetKeyboard(informationAboutUsers));
                    return Ok();
                }

                if (message.Text == "Войти как обычный участник" && userDb.CheckCurrentAction(chatId, "EnteranceForOrganisators") == 1)//08.07.19
                {
                    userDb.CurrentActionOff(chatId, "CreateNotification");
                    userDb.CurrentActionOff(chatId, "EnteranceForOrganisators");
                    userDb.CurrentActionOff(chatId, "CreateSurvey");
                    userDb.CurrentActionOff(chatId, "AddQuestionToSurvey");
                    await botClient.SendTextMessageAsync(chatId, "Теперь вы можете войти как обычный участник", replyMarkup: keybord.GetKeyboard(actions));
                    return Ok();
                }
                if (message.Text == "Количество пользователей" && userDb.CheckCurrentAction(chatId, "EnteranceForOrganisators") == 1)
                {
                    string temp = userDb.GetAmountOfUsersAtAll(chatId);
                    await botClient.SendTextMessageAsync(chatId, temp, replyMarkup: keybord.GetKeyboard(organisatorMode));
                    return Ok();
                }
                if(message.Text=="Выйти" && userDb.CheckCurrentAction(chatId,"EnteranceForOrganisators")==1)
                {
                    await botClient.SendTextMessageAsync(chatId, "Вы вернулись в главное меню", replyMarkup: keybord.GetKeyboard(organisatorMode));
                    return Ok();
                }
                if (message.Text == "Количество активаций нетворкинга" && userDb.CheckCurrentAction(chatId, "EnteranceForOrganisators") == 1)
                {
                    string temp = userDb.GetAmountOfUsers(chatId);
                    await botClient.SendTextMessageAsync(chatId, temp, replyMarkup: keybord.GetKeyboard(organisatorMode));
                    return Ok();
                }
                if (userDb.CheckCurrentAction(chatId, "CreateNotification") == 1)
                {
                    userDb.CurrentActionOff(chatId, "CreateNotification");
                    List<long> peopleForDistribution = userDb.PeopleForDistribution(chatId);
                    foreach (var item in peopleForDistribution)
                    {
                        await botClient.SendTextMessageAsync(item, message.Text);
                    }
                    await botClient.SendTextMessageAsync(chatId, "Ваше сообщение успешно разослано");
                    return Ok();
                }
                if (message.Text == "Создать оповещение" && userDb.CheckCurrentAction(chatId, "EnteranceForOrganisators") == 1)
                {
                    userDb.CurrentActionOn(chatId, "CreateNotification");
                    await botClient.SendTextMessageAsync(chatId, "Можете отправить любое сообщение, оно будет отправлено всем");
                    return Ok();
                }
                
                if (userDb.CheckElements(chatId, "Name") == false && userDb.CheckCurrentAction(chatId, "NameAndLastName") == 0)//ЕСЛИ ПОЛЬЗОВАТЕЛЬ ЕЩЁ НЕ ВВОДИЛ ИМЯ И ФАМИЛИЮ
                {
                    userDb.CurrentActionOn(chatId, "NameAndLastName");
                    await botClient.SendTextMessageAsync(chatId, "Введите имя и фамилию");
                    return Ok();
                }
                
                if (userDb.CheckCurrentAction(chatId, "CheckEmail") == 1)//ПРОВЕРКА ПОЧТЫ, ЧТОБЫ ВОЙТИ В ЛИЧНЫЙ КАБИНЕТ
                {
                    userDb.CurrentActionOff(chatId, "CheckEmail");
                    userDb.CurrentActionOff(chatId, "EventCode");
                    if (userDb.CheckEmail(chatId, message.Text) == true)//ЕСЛИ СОВПАЛО
                    {
                        userDb.CurrentActionOn(chatId, "PrivateCabinet");
                        await botClient.SendTextMessageAsync(chatId, "Сейчас вам доступен мой функционал", replyMarkup: keybord.GetKeyboard(funcional));
                        return Ok();
                    }
                    else if (userDb.DoesEmailExist(message.Text))//ЕСЛИ ПОЧТА СУЩЕСТВУЕТ,НО...
                    {
                        log.AddLog("COOL2");
                         await SendEmailAsync(message.Text, "Lizochka", "is super");
                        log.AddLog("COOL1");
                        await botClient.SendTextMessageAsync(chatId, "Пользователь с этой почтой ранее использовал другой аккаунт телеграм. На эту почту отправлен код идентификации. Пожалуйста, введите код");
                        return Ok();
                    }
                    else//ЕСЛИ НЕТ ТАКОЙ ПОЧТЫ
                    {
                        await botClient.SendTextMessageAsync(chatId, "Пользователь с этой почтой ранее зарегистрирован не был, выберите, пожалуйста, вход по ивент коду", replyMarkup: keybord.GetKeyboard(actions));
                        return Ok();

                    }
                }
                string[][] back = { new[] { "Назад" } };
                string[][] createSurvey = { new[] { "Добавить вопрос" }, new[] { "Сохранить и отправить опрос" }, new[] { "Вернуться" } };
                if(message.Text=="Вернуться" && (userDb.CheckCurrentAction(chatId,"CreateSurvey")==1 || userDb.CheckCurrentAction(chatId, "AddQuestionToSurvey") == 1))
                {
                    userDb.CurrentActionOff(chatId, "CreateSurvey");
                    userDb.CurrentActionOff(chatId, "AddQuestionToSurvey");
                    await botClient.SendTextMessageAsync(chatId, "Я вернулся", replyMarkup: keybord.GetKeyboard(organisatorMode));
                    return Ok();
                }
                if (message.Text == "Создать опрос" && userDb.CheckCurrentAction(chatId, "EnteranceForOrganisators")==1)
                {
                    userDb.CurrentActionOn(chatId, "CreateSurvey");
                    await botClient.SendTextMessageAsync(chatId, "Напишите тему опроса", replyMarkup: keybord.GetKeyboard(createSurvey));
                    return Ok();
                }
               
                if (message.Text == "Добавить вопрос" && userDb.CheckCurrentAction(chatId, "CreateSurvey") == 1)
                {
                    userDb.AddElement(chatId, "AddQuestionToSurvey", message.Text);
                    await botClient.SendTextMessageAsync(chatId, "Выберите,что дальше делать");
                    return Ok();
                }
                if (message.Text == "Сохранить и отправить опрос" && userDb.CheckCurrentAction(chatId, "CreateSurvey") == 1)
                {
                    userDb.CurrentActionOff(chatId, "CreateSurvey");
                    userDb.CurrentActionOff(chatId, "AddQuestionToSurvey");
                    log.AddLog("save0");
                    List<long> users = userDb.PeopleForDistribution(chatId);
                    log.AddLog("save1");
                    Dictionary<int, string> Questions = userDb.QuestionsForDistribution(chatId);
                    log.AddLog("save2");
                    //ПРОДУМАЙ ЭТОТ МОМЕНТ
                    foreach (var item in users)
                    {
                        foreach (var item1 in Questions)
                        {
                            log.AddLog(item1.Key.ToString());
                            string[][] question = { new[] { "🔥".ToString(), "👍".ToString(),"👌".ToString(),"👎".ToString(),"🤢".ToString()} };
                            string temp1 = "991-" + item1.Key.ToString();
                            string temp2 = "992-" + item1.Key.ToString();
                            string temp3 = "993-" + item1.Key.ToString();
                            string temp4 = "994-" + item1.Key.ToString();
                            string temp5 = "995-" + item1.Key.ToString();
                            string[][] question1 = { new[] { temp1,temp2,temp3,temp4,temp5 } };
                            await botClient.SendTextMessageAsync(item, item1.Value, replyMarkup: keybord.GetInlineKeyboard(question, question1));
                        }
                    }
                    log.AddLog("save15");
                    await botClient.SendTextMessageAsync(chatId, "Ваш опрос успешно отправлен участникам", replyMarkup: keybord.GetKeyboard(organisatorMode));
                    return Ok();
                }
                if (userDb.CheckCurrentAction(chatId, "AddQuestionToSurvey") == 1)
                {
                    userDb.AddElement(chatId, "AddQuestionToSurvey", message.Text);
                    await botClient.SendTextMessageAsync(chatId, "Выберите,что дальше делать");
                    return Ok();
                }
                if (userDb.CheckCurrentAction(chatId, "CreateSurvey") == 1)
                {
                    userDb.AddElement(chatId, "Survey", message.Text);//добавляет тему опроса
                    userDb.AddCurrentNumberOfSurvey(chatId, message.Text);//фиксирует номер опроса 
                     userDb.CurrentActionOn(chatId, "AddQuestionToSurvey");
                    await botClient.SendTextMessageAsync(chatId, "Напишите вопрос", replyMarkup: keybord.GetKeyboard(createSurvey));
                    return Ok();
                }
               
              
                    if (message.Text == "О мероприятии" && userDb.CheckCurrentAction(chatId, "PrivateCabinet") == 1)//ЕСЛИ ПОЛЬЗОВАТЕЛЬ ВХОДИТ В МЕРОПРИЯТИЕ,И ОН В ЛИЧНОМ КАБИНЕТЕ
                {
                    //здесь инфа из мероприятия
                    string temp=eventDb.GetInformationAboutEvent(chatId);                
                    await botClient.SendTextMessageAsync(chatId, temp, replyMarkup: keybord.GetKeyboard(back));
                    return Ok();
                }
                if (message.Text == "Назад" && userDb.CheckCurrentAction(chatId, "PrivateCabinet") == 1)
                {
                    //ПРОПИСАТЬ ВСЕ ВОЗМОЖНЫЕ ФОЛЗЫ!
                    userDb.CurrentActionOff(chatId, "Networking");
                    userDb.CurrentActionOff(chatId, "Work");
                    userDb.CurrentActionOff(chatId, "Email");
                    userDb.CurrentActionOff(chatId, "Position");
                    userDb.CurrentActionOff(chatId, "AboutWishes");
                    userDb.CurrentActionOff(chatId, "NameAndLastName");
                    await botClient.SendTextMessageAsync(chatId, "Вы в главном меню", replyMarkup: keybord.GetKeyboard(funcional));
                    return Ok();
                }
                if (userDb.CheckCurrentAction(chatId, "Work") == 1)//РЕЖИМ ДОБАВЛЕНИЯ РАБОТЫ
                {
                    userDb.CurrentActionOff(chatId, "Work");
                    userDb.CurrentActionOn(chatId, "Position");
                    userDb.AddElement(chatId, "Work", message.Text);
                    await botClient.SendTextMessageAsync(chatId, "Отлично, какая у вас позиция (должность) в компании?", ParseMode.Markdown);
                    return Ok();
                }
                if (userDb.CheckCurrentAction(chatId, "Position") == 1)//РЕЖИМ ДОБАВЛЕНИЯ ДОЛЖНОСТИ
                {
                    userDb.CurrentActionOff(chatId, "Position");
                    userDb.CurrentActionOn(chatId, "Usefulness");
                    userDb.AddElement(chatId, "Position", message.Text);
                    await botClient.SendTextMessageAsync(chatId, "Теперь более интересные вопросы"+ "😜".ToString()+" Чем вы можете быть полезны?");
                    return Ok();
                }
                log.AddLog("15");
                if (userDb.CheckCurrentAction(chatId, "Usefulness") == 1)
                {
                    log.AddLog("16");
                    userDb.CurrentActionOff(chatId, "Usefulness");
                    userDb.CurrentActionOn(chatId, "AboutWishes");
                    log.AddLog("17");
                    userDb.AddElement(chatId, "Usefulness", message.Text);
                    log.AddLog("18");
                    await botClient.SendTextMessageAsync(chatId, "Более отвлечённый вопрос"+ "🤗".ToString()+" О чем бы вы хотели пообщаться? Темы рабочие и не очень", ParseMode.Markdown);
                    return Ok();
                }
                if (userDb.CheckCurrentAction(chatId, "AboutWishes") == 1)//РЕЖИМ ДОБАВЛЕНИЯ ЖЕЛАНИЙ
                {
                    userDb.CurrentActionOff(chatId, "AboutWishes");
                    userDb.CurrentActionOn(chatId, "ChoseTag");
                    userDb.AddElement(chatId, "AboutTalkingWishes", message.Text);
                    List<string> tags = userDb.GetTags();
                    string[][] choseTags = new string[tags.Count + 1][];
                    int count = 0;
                    foreach (var item in tags)
                    {
                        choseTags[count] = new[] { item };
                        count++;
                    }
                    choseTags[count] = new[] { "На главную страницу" };// 03.07.19 возможно другой текст
                    await botClient.SendTextMessageAsync(chatId, "Хорошо, перейдём к тегам. Для начала выберите ВАШИ теги (подходят лично ВАМ)", replyMarkup: keybord.GetKeyboard(choseTags));
                    return Ok();
                }

                if (userDb.CheckCurrentAction(chatId, "ChoseTag") == 1)//ДОБАВЛЕНИЕ ТЕКУЩИХ ТЕГОВ
                {
                    log.AddLog("CHOSE TAG 1");
                    List<string> chosenTag = new List<string>();
                    chosenTag.Add(message.Text);
                    userDb.CurrentActionOff(chatId, "ChoseTag");
                    log.AddLog("BEFORE CHOSE TAG2");
                    userDb.AddElement(chatId, "CurrentTagss", message.Text);
                    log.AddLog("BEFORE CHOSE TAG3");
                    string temp;
                    temp = "Ваши теги:" ;
                    log.AddLog("BEFORE CHOSE TAG4");
                    userDb.CurrentActionOn(chatId, "ChoseSubtags");
                    // +get userscurrentsubtags на каждом шаге
                    string temp1 = userDb.GetAllSubtags(chosenTag);
                    log.AddLog("BEFORE CHOSE TAG5");
                    await botClient.SendTextMessageAsync(chatId, temp1,ParseMode.Html,replyMarkup: keybord.GetKeyboard(tagsKeybord));
                    await botClient.SendTextMessageAsync(chatId, temp, replyMarkup: keybord.GetInlineKeyboard(choseCurrentSubtags, choseCurrentSubtagsCallBack));
                    return Ok();
                }
                if (userDb.CheckCurrentAction(chatId, "ChoseTagAboutOthers") == 1)//ДОБАВЛЕНИЕ ТЕГОВ О ДРУГИХ ЛЮДЯХ
                {
                    List<string> chosenTagsAboutOthers = new List<string>();
                    chosenTagsAboutOthers.Add(message.Text);
                    string ChosenTagAboutOthers = message.Text;
                    // ChosenTagAboutOthers = message.Text;
                    userDb.CurrentActionOn(chatId, "ChoseSubtagsAboutOthers");
                    userDb.CurrentActionOff(chatId, "ChoseTagAboutOthers");
                    userDb.AddElement(chatId, "CurrentTagAboutOtherss", message.Text);
                    log.AddLog("here");
                    string temp = "Теги нужных людей: ";
                    log.AddLog("here1");
                    string temp1 = userDb.GetAllSubtags(chosenTagsAboutOthers);
                    log.AddLog("here2");
                    await botClient.SendTextMessageAsync(chatId, temp1,ParseMode.Html, replyMarkup: keybord.GetKeyboard(tagsKeybord));
                    await botClient.SendTextMessageAsync(chatId, temp,
                        replyMarkup: keybord.GetInlineKeyboard(choseCurrentSubtagsAboutOthers, choseCurrentSubtagsCallBackAboutOthers));
                    return Ok();
                }
                string[][] Tinder = { new[] { "⬅️".ToString(), "В книжку", "Встреча", "➡️".ToString() } };
                string[][] TinderCallback = { new[] { "21", "22", "23", "24" } };
                if (userDb.CheckCurrentAction(chatId, "NetworkingFull") == 1 && message.Text == "Общение")//ТИНДЕР
                {
                    // List<string> subtags = userDb.GetAllUsersSubtagsAboutOthersList(chatId);
                   /* List<string> tags = new List<string>();
                    tags.Add("БИЗНЕС");
                    tags.Add("IT");*/
                    List<string> subtags = userDb.GetAllSubtagss(chatId);
                    if(subtags.Count==0)
                    {
                        await botClient.SendTextMessageAsync(chatId, "Вы не выбрали теги для поиска других людей", replyMarkup: keybord.GetInlineKeyboard(Tinder, TinderCallback));
                        return Ok();
                    }
                    List<int> usersForTinder = userDb.GetAllUsersForTinder(subtags,chatId);
                    if(usersForTinder.Count==0)
                    {
                        await botClient.SendTextMessageAsync(chatId, "Людей с выбранными тегами нет", replyMarkup: keybord.GetInlineKeyboard(Tinder, TinderCallback));
                        return Ok();
                    }
                   userDb.AddlengthOfUsersBySubtags(chatId, usersForTinder.Count);
                     userDb.AddAmountForTinder(chatId, 0);
                    long userId = userDb.GetChatId(usersForTinder[0]);
                    string temp = userDb.GetInformationAboutPeopleForCommunication(userId);
                    log.AddLog("COMMUNICATION4");
                    await botClient.SendTextMessageAsync(chatId, temp,ParseMode.Html, replyMarkup: keybord.GetInlineKeyboard(Tinder, TinderCallback));
                    return Ok();
                }
                if (message.Text == "ОК" && userDb.CheckCurrentAction(chatId, "ChoseSubtagsAboutOthers") == 1 && userDb.CheckCurrentAction(chatId, "MyProfile") == 1)
                {
                    userDb.CurrentActionOff(chatId, "ChoseSubtagsAboutOthers");
                    //userDb.CurrentActionOn(chatId, "NetworkingFull");
                    string temp = "Теги нужных людей:" + "\n" + userDb.GetUsersSubtagsAboutOthers(chatId);
                    await botClient.SendTextMessageAsync(chatId, temp,ParseMode.Html);
                    await botClient.SendTextMessageAsync(chatId, "Данные изменены", replyMarkup: keybord.GetKeyboard(myProfile));
                    return Ok();
                }
                if (message.Text == "ОК" && userDb.CheckCurrentAction(chatId, "ChoseSubtagsAboutOthers") == 1)
                {
                    userDb.CurrentActionOff(chatId, "ChoseSubtagsAboutOthers");
                    userDb.CurrentActionOn(chatId, "NetworkingFull");
                    string temp = "Теги нужных людей:" + "\n" + userDb.GetUsersSubtagsAboutOthers(chatId);
                    await botClient.SendTextMessageAsync(chatId, temp,ParseMode.Html);
                    await botClient.SendTextMessageAsync(chatId, "Поздравляю!"+ "🥳".ToString()+" Настройка режима общения завершена", replyMarkup: keybord.GetKeyboard(NetworkingMode));
                    return Ok();
                }
                if (message.Text == "ОК" && userDb.CheckCurrentAction(chatId, "ChoseSubtags") == 1)
                {
                    userDb.CurrentActionOff(chatId, "ChoseSubtags");
                    userDb.CurrentActionOn(chatId, "ChoseTagAboutOthers");
                    List<string> choseTags = userDb.GetTags();
                    string[][] choseTagss = new string[choseTags.Count + 1][];
                    int count = 0;
                    foreach (var item in choseTags)
                    {
                        choseTagss[count] = new[] { item };
                        count++;
                    }
                    choseTagss[count] = new[] { "На главную страницу" };
                    await botClient.SendTextMessageAsync(chatId, "Почти всё. Осталось выбрать теги людей, которые нужны ВАМ", replyMarkup: keybord.GetKeyboard(choseTagss));
                    return Ok();
                }
                //NETWORKING
                log.AddLog("before networking");
                if (message.Text == "Режим общения" && userDb.CheckCurrentAction(chatId, "PrivateCabinet") == 1)//ЕСЛИ РЕЖИМ НЕТВОРКИНГА В ЛИЧНОМ КАБИНЕТЕ
                {
                    if (userDb.CheckElements(chatId, "Event") == false)
                    {
                        log.AddLog("EVENT");
                        userDb.CurrentActionOn(chatId, "EventCode");
                        await botClient.SendTextMessageAsync(chatId, "Вы не вошли ни в какое мероприятие. Введите ивент-код, пожалуйста");
                        return Ok();
                    }
                    userDb.CurrentActionOn(chatId, "Networking");
                    if (userDb.CheckElements(chatId, "work") == false)//ЕСЛИ ЕЩЕ НЕ ДОБАВИЛ РАБОТУ
                    {
                        userDb.CurrentActionOn(chatId, "Work");
                        await botClient.SendTextMessageAsync(chatId, "Для режима общения жизненно необходимо ввести дополнительные сведения, давайте начнём", replyMarkup: keybord.GetKeyboard(back)); ;
                        await botClient.SendTextMessageAsync(chatId, "Где Вы работаете? Это поможет людям понять, чем вы можете быть интересны", ParseMode.Markdown);
                        return Ok();
                    }

                    else if (userDb.CheckElements(chatId, "Position") == false)//ЕСЛИ НЕ ДОБАВИЛ ЕЩЕ ДОЛЖНОСТЬ
                    {
                        userDb.CurrentActionOn(chatId, "Position");
                        await botClient.SendTextMessageAsync(chatId, "Какую позицию (должность) вы занимаете?", replyMarkup: keybord.GetKeyboard(back));
                        return Ok();
                    }
                    else if (userDb.CheckElements(chatId, "Usefulness") == false)
                    {
                        userDb.CurrentActionOn(chatId, "Usefulness");
                        await botClient.SendTextMessageAsync(chatId, "Чем вы можете быть полезны другим людям?", ParseMode.Markdown);
                        return Ok();
                    }
                    else if (userDb.CheckElements(chatId, "AboutTalkingWishes") == false)//ЕСЛИ ЕЩЕ НЕ ДОБАВИЛ ЖЕЛАНИЯ
                    {
                        userDb.CurrentActionOn(chatId, "AboutWishes");
                        await botClient.SendTextMessageAsync(chatId, "Более отвлечённый вопрос" + "🤗".ToString() + " О чем бы вы хотели пообщаться? Темы рабочие и не очень", ParseMode.Markdown);
                        return Ok();
                    }
                    else if (userDb.CheckElements(chatId, "CurrentTagss") == false)//ЕСЛИ ЕЩЕ ТЕКУЩИЕ ТЕГИ НЕ ВЫБИРАЛ
                    {
                        userDb.CurrentActionOn(chatId, "ChoseTag");
                        List<string> tags = userDb.GetTags();
                        string[][] choseTags = new string[tags.Count + 1][];
                        int count = 0;
                        foreach (var item in tags)
                        {
                            choseTags[count] = new[] { item };
                            count++;
                        }
                        
                        choseTags[count] = new[] { "На главную страницу" };
                        await botClient.SendTextMessageAsync(chatId, "Для начала выберите ВАШИ теги (подходят лично ВАМ)", replyMarkup: keybord.GetKeyboard(choseTags));
                        return Ok();
                    }

                    else if (userDb.CheckElements(chatId, "CurrentSubtagss") == false)//ЕСЛИ ЕЩЁ НЕ ВЫБРАЛ ПОДТЕГИ СВОИ
                    {
                        userDb.CurrentActionOff(chatId, "ChoseTag");

                        List<string> chosenTags = userDb.UsersChosenTags(chatId);
                        string temp;
                        temp = "Ваши теги:";
                        string temp1 = userDb.GetAllSubtags(chosenTags);
                        userDb.CurrentActionOn(chatId, "ChoseSubtags");
                        await botClient.SendTextMessageAsync(chatId, temp1,ParseMode.Html, replyMarkup: keybord.GetKeyboard(tagsKeybord));
                        await botClient.SendTextMessageAsync(chatId, temp, replyMarkup: keybord.GetInlineKeyboard(choseCurrentSubtags, choseCurrentSubtagsCallBack));
                        return Ok();
                    }
                    else if (userDb.CheckElements(chatId, "CurrentTagAboutOtherss") == false)
                    {
                        userDb.CurrentActionOn(chatId, "ChoseTagAboutOthers");
                        List<string> tags = userDb.GetTags();
                        string[][] choseTags = new string[tags.Count + 1][];
                        int count = 0;
                        foreach (var item in tags)
                        {
                            choseTags[count] = new[] { item };
                            count++;
                        }
                        choseTags[count] = new[] { "На главную страницу" };
                        string temp = "Ваши теги:    \n" + userDb.GetUsersSubtags(chatId) + "\n Осталось выбрать теги людей, которые нужны ВАМ";
                        await botClient.SendTextMessageAsync(chatId, temp,ParseMode.Html, replyMarkup: keybord.GetKeyboard(choseTags));
                        return Ok();
                    }
                    else if (userDb.CheckElements(chatId, "CurrentSubtagsAboutOtherss") == false)//03.07.19
                    {
                        userDb.CurrentActionOn(chatId, "ChoseSubtagsAboutOthers");
                        string temp = "Теги нужных людей:    \n";
                        temp = temp + userDb.GetUserTagsAboutOthers(chatId);
                        List<string> ChosenTagAboutOthers = userDb.UsersChosenTagsAboutOthers(chatId);
                        string temp1 = userDb.GetAllSubtags(ChosenTagAboutOthers);
                        await botClient.SendTextMessageAsync(chatId, temp1,ParseMode.Html, replyMarkup: keybord.GetKeyboard(tagsKeybord));
                        await botClient.SendTextMessageAsync(chatId, temp,ParseMode.Html, replyMarkup: keybord.GetInlineKeyboard(choseCurrentSubtagsAboutOthers, choseCurrentSubtagsCallBackAboutOthers));
                        return Ok();
                    }
                    else
                    {
                        userDb.CurrentActionOn(chatId, "NetworkingFull");
                        await botClient.SendTextMessageAsync(chatId, "Режим общения включен", replyMarkup: keybord.GetKeyboard(NetworkingMode));
                        return Ok();
                    }
                }

                if (message.Text == "Об ивенте" && userDb.CheckCurrentAction(chatId, "NetworkingFull") == 1)
                {
                   string temp= eventDb.GetInformationAboutEvent(chatId);
                    await botClient.SendTextMessageAsync(chatId, temp, replyMarkup: keybord.GetKeyboard(back));
                    return Ok();
                }              


                await botClient.SendTextMessageAsync(chatId, "Бот такое ещё не понимает😅 Используйте клавиатуру бота", ParseMode.Markdown);
                return Ok();
            }
        
            else if (update.Type == UpdateType.CallbackQuery)
            {  LogsDB log = new LogsDB();
                long chatId = update.CallbackQuery.Message.Chat.Id;
                var botClient = await Bot.GetBotClientAsync();
                TelegramKeybord keybord = new TelegramKeybord();
                int a = Convert.ToInt32(Char.GetNumericValue(update.CallbackQuery.Data[0]));
                int b = 0;
                if (Convert.ToString(update.CallbackQuery.Data[1]) != "-")
                    b = Convert.ToInt32(Char.GetNumericValue(update.CallbackQuery.Data[1]));
                else b = 10;
                string[][] choseCurrentSubtags = { new[] { "1", "2", "3", "4", "5" }, new[] { "6", "7", "8", "9" ,"10"}, new[] { "Показать всех" } };
                string[][] choseCurrentSubtagsCallBack = { new[] { "01", "02", "03", "04", "05" }, new[] {  "06", "07", "08", "09","0-" }, new[] { "00" } };
                string[][] choseCurrentSubtagsAboutOthers = { new[] { "1", "2", "3", "4" , "5" }, new[] {  "6", "7", "8", "9","10"}, new[] { "Показывать всех" } };
                string[][] choseCurrentSubtagsCallBackAboutOthers = { new[] { "11", "12", "13", "14", "15" }, new[] { "16", "17", "18", "19","1-"}, new[] { "10" } };
                string[][] Tinder = { new[] { "⬅️".ToString(), "В книжку", "Встреча", "➡️".ToString() } };
                string[][] TinderCallback = { new[] { "21", "22", "23", "24" } };
                log.AddLog(chatId.ToString());
                //ПОСМОТРИ В КАКУЮ ВЕТВЬ ИДЁТ ДОБАВЛЕHИЕ ПОДТЕГОВ !!!
                if (a == 0)
                {
                    string temp2 = "";
                    List<string> chosenTags = new List<string>();
                    string chosenTag = userDb.UsersChosenTag(chatId);
                    log.AddLog(chosenTag);
                    chosenTags.Add(chosenTag);
                    Dictionary<int, string> subtags = userDb.GetAllSubtagsList(chosenTags);
                    foreach (var item in subtags)
                    {
                        if (b == 0 )
                        {
                            userDb.AddElement(chatId, "CurrentSubtagss", item.Value);
                            temp2 = "Ваши теги:    \n" + userDb.GetUsersSubtags(chatId);

                            for (int i = 0; i < 2; i++)
                            {
                                for (int j = 0; j < choseCurrentSubtags[i].Length; j++)
                                {
                                    choseCurrentSubtags[i][j] = "✅".ToString();

                                }
                            }
                            await botClient.EditMessageTextAsync(chatId, update.CallbackQuery.Message.MessageId, temp2,ParseMode.Html, replyMarkup: keybord.GetInlineKeyboard(choseCurrentSubtags, choseCurrentSubtagsCallBack));
                            await botClient.AnswerCallbackQueryAsync(update.CallbackQuery.Id);
                        }
                        if (item.Key == b && userDb.CheckCurrentSubtag(chatId, item.Value) )//это если при повторном нажатии должно вернуться обратно
                        {
                            userDb.RemoveCurrentSubtag(chatId, item.Value);
                            temp2 = "Ваши теги:    \n" + userDb.GetUsersSubtags(chatId);
                            for (int i = 0; i < 2; i++)
                            {
                                for (int j = 0; j < choseCurrentSubtags[i].Length; j++)
                                {
                                    if (Convert.ToInt32(choseCurrentSubtags[i][j]) == b)
                                    {
                                        choseCurrentSubtags[i][j] = b.ToString();
                                    }
                                }
                            }
                        }
                        else if (item.Key == b)//если первый раз нажал на кнопку
                        {

                            userDb.AddElement(chatId, "CurrentSubtagss", item.Value);
                            temp2 = "Ваши теги:    \n" + userDb.GetUsersSubtags(chatId);
                            for (int i = 0; i < 2; i++)
                            {
                                for (int j = 0; j < choseCurrentSubtags[i].Length; j++)
                                {
                                    if (Convert.ToInt32(choseCurrentSubtags[i][j]) == b)
                                    {
                                        choseCurrentSubtags[i][j] = "✅".ToString();
                                    }
                                }
                            }
                        }
                    }
                    List<string> currentSubtagsWithChosenTag = userDb.UsersSubtags(chatId, chosenTags);
                    string[][] choseCurrentSubtags1 = { new[] { "1", "2", "3", "4","5"}, new[] {  "6", "7", "8", "9","10"}, new[] { "Показать всех" } };
                    foreach (var item in subtags)
                    {
                        foreach (var item1 in currentSubtagsWithChosenTag)
                        {
                            if (item.Value == item1)
                            {
                                for (int i = 0; i < 2; i++)
                                {
                                    for (int j = 0; j < choseCurrentSubtags[i].Length; j++)
                                    {
                                        if (choseCurrentSubtags1[i][j] == item.Key.ToString())
                                            choseCurrentSubtags[i][j] = "✅".ToString();
                                    }
                                }
                            }
                        }

                    }
                    await botClient.EditMessageTextAsync(chatId, update.CallbackQuery.Message.MessageId, temp2,ParseMode.Html, replyMarkup: keybord.GetInlineKeyboard(choseCurrentSubtags, choseCurrentSubtagsCallBack));
                    await botClient.AnswerCallbackQueryAsync(update.CallbackQuery.Id);
                    return Ok();
                }
                else if (a == 1)//ВЫБРАТЬ ПОДТЕГИ ДЛЯ СЕБЯ
                {
                    
                     string temp2 = "";
                    List<string> chosenTagsAboutOthers = new List<string>();
                    string chosenTagAboutOthers = userDb.UsersChosenTagAboutOthers(chatId);
                    chosenTagsAboutOthers.Add(chosenTagAboutOthers);

                    Dictionary<int, string> subtagsAboutOthers = userDb.GetAllSubtagsList(chosenTagsAboutOthers);
                    foreach (var item in subtagsAboutOthers)
                    {
                        if (b == 0)
                        {
                            userDb.AddElement(chatId, "CurrentSubtagsAboutOtherss", item.Value);
                            temp2 = "Теги нужных людей:   " + " \n" + userDb.GetUsersSubtagsAboutOthers(chatId);
                            for (int i = 0; i < 2; i++)
                            {
                                for (int j = 0; j < choseCurrentSubtagsAboutOthers[i].Length; j++)
                                {
                                    choseCurrentSubtagsAboutOthers[i][j] = "✅".ToString();

                                }
                            }
                           /* await botClient.EditMessageTextAsync(chatId, update.CallbackQuery.Message.MessageId, temp2, replyMarkup: keybord.GetInlineKeyboard(choseCurrentSubtagsAboutOthers, choseCurrentSubtagsCallBackAboutOthers));
                            await botClient.AnswerCallbackQueryAsync(update.CallbackQuery.Id);*/
                        }
                        if (item.Key == b && userDb.CheckCurrentSubtagAboutOthers(chatId, item.Value))
                        {
                            userDb.RemoveCurrentSubtagAboutOthers(chatId, item.Value);
                            temp2 = "Теги нужных людей:    " + " \n" + userDb.GetUsersSubtagsAboutOthers(chatId);
                            for (int i = 0; i < 2; i++)
                            {
                                for (int j = 0; j < choseCurrentSubtagsAboutOthers[i].Length; j++)
                                {
                                    if (Convert.ToInt32(choseCurrentSubtagsAboutOthers[i][j]) == b)
                                    {
                                        choseCurrentSubtagsAboutOthers[i][j] = b.ToString();
                                    }
                                }
                            }

                        }
                        else if (item.Key == b)
                        {
                            userDb.AddElement(chatId, "CurrentSubtagsAboutOtherss", item.Value);
                            temp2 = "Теги нужных людей:    " + " \n" + userDb.GetUsersSubtagsAboutOthers(chatId);
                            for (int i = 0; i < 2; i++)
                            {
                                for (int j = 0; j < choseCurrentSubtagsAboutOthers[i].Length; j++)
                                {
                                    if (Convert.ToInt32(choseCurrentSubtagsAboutOthers[i][j]) == b)
                                    {
                                        choseCurrentSubtagsAboutOthers[i][j] = "✅".ToString();
                                    }
                                }
                            }
                           /* await botClient.EditMessageTextAsync(chatId, update.CallbackQuery.Message.MessageId, temp2, replyMarkup: keybord.GetInlineKeyboard(choseCurrentSubtagsAboutOthers, choseCurrentSubtagsCallBackAboutOthers));
                            await botClient.AnswerCallbackQueryAsync(update.CallbackQuery.Id);*/
                           
                        }
                    }
                        List<string> currentSubtagsAboutOthersWithChosenTag = userDb.UsersSubtagsAboutOthers(chatId, chosenTagsAboutOthers);
                        string[][] choseCurrentSubtags1 = { new[] { "1", "2", "3", "4","5" }, new[] {  "6", "7", "8", "9","10" }, new[] { "Показать всех" } };
                   foreach (var item in subtagsAboutOthers)
                        {

                        foreach (var item1 in currentSubtagsAboutOthersWithChosenTag)
                            {
                            if (item.Value == item1)
                                {                             
                                    for (int i = 0; i < 2; i++)
                                    {
                                        for (int j = 0; j < choseCurrentSubtagsAboutOthers[i].Length; j++)
                                        {
                                            if (choseCurrentSubtags1[i][j] == item.Key.ToString())
                                        
                                                choseCurrentSubtagsAboutOthers[i][j] = "✅".ToString();
                                        }
                                    }
                                }
                            }

                        }
                    await botClient.EditMessageTextAsync(chatId, update.CallbackQuery.Message.MessageId, temp2,ParseMode.Html, replyMarkup: keybord.GetInlineKeyboard(choseCurrentSubtagsAboutOthers, choseCurrentSubtagsCallBackAboutOthers));
                        await botClient.AnswerCallbackQueryAsync(update.CallbackQuery.Id);
                        return Ok();
                    
                }
                else if (a == 2)//РЕЖИМ ТИНДЕРА
                {
                    if (b == 1)//кнопка назад
                    {
                      
                        if (userDb.CheckCurrentAction(chatId,"amountForTinder") == 0)
                        {
                            int lengthOfUsersBySubtags = userDb.CheckCurrentAction(chatId, "lengthOfUsersBySubtags");
                            int  amountForTinder = --lengthOfUsersBySubtags;
                            userDb.AddAmountForTinder(chatId, amountForTinder);
                            //List<string> subtags = userDb.GetAllUsersSubtagsAboutOthersList(chatId);
                            List<string> subtags = userDb.GetAllSubtagss(chatId);
                            List<int> usersForTinder = userDb.GetAllUsersForTinder(subtags,chatId);
                            long userId1 = userDb.GetChatId(usersForTinder[amountForTinder]);
                            string temp = userDb.GetInformationAboutPeopleForCommunication(userId1);
                            await botClient.EditMessageTextAsync(chatId, update.CallbackQuery.Message.MessageId, temp,ParseMode.Html, replyMarkup: keybord.GetInlineKeyboard(Tinder, TinderCallback));
                            await botClient.AnswerCallbackQueryAsync(update.CallbackQuery.Id);

                        }
                        else
                        {
                            List<string> subtags = userDb.GetAllSubtagss(chatId);
                           // List<string> subtags = userDb.GetAllUsersSubtagsAboutOthersList(chatId);
                            List<int> usersForTinder = userDb.GetAllUsersForTinder(subtags,chatId);
                            int amountForTinder = userDb.CheckCurrentAction(chatId, "amountForTinder");
                            --amountForTinder;
                            userDb.AddAmountForTinder(chatId, amountForTinder);
                            long userId1 = userDb.GetChatId(usersForTinder[amountForTinder]);
                            string temp = userDb.GetInformationAboutPeopleForCommunication(userId1);
                            await botClient.EditMessageTextAsync(chatId, update.CallbackQuery.Message.MessageId, temp,ParseMode.Html, replyMarkup: keybord.GetInlineKeyboard(Tinder, TinderCallback));
                            await botClient.AnswerCallbackQueryAsync(update.CallbackQuery.Id);
                        }
                    }
                    else if (b == 2)//КНОПКА В КНИЖКУ
                    {
                        //сначала сокращенный вариант 
                      //  List<string> subtags = userDb.GetAllUsersSubtagsAboutOthersList(chatId);
                        List<string> subtags = userDb.GetAllSubtagss(chatId);
                        List<int> usersForTinder = userDb.GetAllUsersForTinder(subtags,chatId);
                         int amountForTinder = userDb.CheckCurrentAction(chatId, "amountForTinder");
                         long userId1 = userDb.GetChatId(usersForTinder[amountForTinder]);
                        string temp = userDb.GetInformationAboutPeople(userId1);
                        //where is an adding?
                        long toSend = userDb.GetChatId(usersForTinder[amountForTinder]);
                         int chatik = userDb.GetUserId(chatId);
                        log.AddLog("added to req0");
                        userDb.AddToRequesting(userId1, chatId);//ДОБАВЛЕНИЕ В СПИСОК ЗАПРОСИВШИХ
                        log.AddLog("added to req");
                         long idOfApplication = userDb.GetApplicationId(userId1, chatId);
                        string accept = "3-" + idOfApplication.ToString();
                        string cancel = "4-" + idOfApplication.ToString();
                        string[][] querryForBook = { new[] { "Принять", "Отклонить" } };
                        string[][] querryForBookCallBack = { new[] { accept, cancel } };
                         string temporary = "Вам поступило приглашение на обмен контактами от \n" + userDb.GetInformationAboutPeople(chatId);
                        await botClient.SendTextMessageAsync(chatId, "Человек добавлен в записную книжку, НО его контакты станут доступны по его разрешению");
                        await botClient.SendTextMessageAsync(toSend, temporary,ParseMode.Html, replyMarkup: keybord.GetInlineKeyboard(querryForBook, querryForBookCallBack));
                       // await botClient.SendTextMessageAsync(chatId, temp, replyMarkup: keybord.GetInlineKeyboard(Tinder, TinderCallback));
                        return Ok();
                        //await botClient.EditMessageTextAsync(chatId, update.CallbackQuery.Message.MessageId, temp, replyMarkup: keybord.GetInlineKeyboard(Tinder, TinderCallback));
                        await botClient.AnswerCallbackQueryAsync(update.CallbackQuery.Id);
                        
                    }
                    else if (b == 3)//КНОПКА ВСТРЕЧА
                    {
                        //List<string> subtags = userDb.GetAllUsersSubtagsAboutOthersList(chatId);
                        List<string> subtags = userDb.GetAllSubtagss(chatId);
                        List<int> usersForTinder = userDb.GetAllUsersForTinder(subtags,chatId);
                        int amountForTinder = userDb.CheckCurrentAction(chatId, "amountForTinder");
                      long userId1 = userDb.GetChatId(usersForTinder[amountForTinder]);
                        string temp = userDb.GetInformationAboutPeople(userId1);
                        long toSend = userDb.GetChatId(usersForTinder[amountForTinder]);
                       // int chatik = userDb.GetUserId(chatId);
                         userDb.AddToRequestToMeet(userId1, chatId);
                        long idOfApplication = userDb.GetApplicationIdOfMeeting(userId1, chatId);
                        string accept = "5-" + idOfApplication.ToString();
                        string cancel = "6-" + idOfApplication.ToString();
                        string[][] querryToMeet = { new[] { "Принять", "Отклонить" } };
                        string[][] querryToMeetCallBack = { new[] { accept, cancel } };
                        //int userId = userDb.GetUserId(chatId);
                        string temporary = "Вам поступило приглашение на встречу от " + userDb.GetInformationAboutPeople(chatId);
                        await botClient.SendTextMessageAsync(toSend,temporary,ParseMode.Html, replyMarkup:keybord.GetInlineKeyboard(querryToMeet,querryToMeetCallBack));
                        //await botClient.AnswerCallbackQueryAsync(update.CallbackQuery.Id);
                        //await botClient.EditMessageTextAsync(chatId, update.CallbackQuery.Message.MessageId, temp, replyMarkup: keybord.GetInlineKeyboard(Tinder, TinderCallback));
                        await botClient.SendTextMessageAsync(chatId, "Запрос на встречу отправлен!");
                        return Ok();
                         }
                    else if (b == 4)//КНОПКА ВПЕРЕД
                    {
                        if ((userDb.CheckCurrentAction(chatId,"lengthOfUsersBySubtags")-1) ==userDb.CheckCurrentAction(chatId,"amountForTinder"))
                        {
                            int  amountForTinder = 0;
                            userDb.AddAmountForTinder(chatId, amountForTinder);
                            //List<string> subtags = userDb.GetAllUsersSubtagsAboutOthersList(chatId);
                            List<string> subtags = userDb.GetAllSubtagss(chatId);
                            List<int> usersForTinder = userDb.GetAllUsersForTinder(subtags,chatId);
                            long userId1 = userDb.GetChatId(usersForTinder[amountForTinder]);
                            string temp = userDb.GetInformationAboutPeopleForCommunication(userId1);
                            await botClient.EditMessageTextAsync(chatId, update.CallbackQuery.Message.MessageId, temp,ParseMode.Html, replyMarkup: keybord.GetInlineKeyboard(Tinder, TinderCallback));
                            await botClient.AnswerCallbackQueryAsync(update.CallbackQuery.Id);
                        }
                        else
                        {
                          // List<string> subtags = userDb.GetAllUsersSubtagsAboutOthersList(chatId);
                            List<string> subtags = userDb.GetAllSubtagss(chatId);
                            List<int> usersForTinder = userDb.GetAllUsersForTinder(subtags,chatId);
                            int amountForTinder = userDb.CheckCurrentAction(chatId, "amountForTinder");
                            amountForTinder++;
                            userDb.AddAmountForTinder(chatId, amountForTinder);
                            long userId1 = userDb.GetChatId(usersForTinder[amountForTinder]);
                            string temp = userDb.GetInformationAboutPeopleForCommunication(userId1);
                            await botClient.EditMessageTextAsync(chatId, update.CallbackQuery.Message.MessageId, temp,ParseMode.Html, replyMarkup: keybord.GetInlineKeyboard(Tinder, TinderCallback));
                            await botClient.AnswerCallbackQueryAsync(update.CallbackQuery.Id);
                        }
                    }
                    return Ok();
                }
               else if (a ==3)//ПРИНЯТИЕ НА ЗАПРОС НА ДОБАВЛЕНИЕ В КНИЖКУ
                {
                   string[] words = update.CallbackQuery.Data.Split("-");
                   int idOfApplication = Convert.ToInt32(words[1]);
                     long sender=userDb.GetIdOfRecipient(idOfApplication);//кто отсылает ответ
                    long recepient = userDb.GetIdOfSender(idOfApplication);//кому отсылаем ответ
                     userDb.AddElementToNoteBook(idOfApplication);
                    string temp = "";string temp1 = "";
                         temp = "Вы успешно обменялись контактами с \n" + userDb.GetInformationAboutPeople(sender) + "\n" ;
                        temp = temp + userDb.GetAdditionalInformationAboutPeople(sender);//+добавить ссылки на телеграм аккаунт
                        temp1 = "Вы успешно обменялись контактами с \n" + userDb.GetInformationAboutPeople(recepient) + "\n";
                        temp1 = temp1 + userDb.GetAdditionalInformationAboutPeople(recepient);//+добавить ссылки на телеграм аккаунт
                    await botClient.SendTextMessageAsync(recepient, temp,ParseMode.Html);
                    await botClient.SendTextMessageAsync(sender, temp1,ParseMode.Html);
                    return Ok();
                }
                else if (a == 5)//ПРИНЯТИЕ НА ЗАПРОС О ВСТРЕЧЕ
                {
                    string[] words = update.CallbackQuery.Data.Split("-");
                    int idOfApplication = Convert.ToInt32(words[1]);
                    long sender = userDb.GetIdOfRecipientOfMeeting(idOfApplication);//кто отсылает ответ
                    long recepient = userDb.GetIdOfSenderOfMeeting(idOfApplication);//кому отсылаем ответ
                    userDb.AddElementToMeetBook(idOfApplication);
                     string name = userDb.GetNameOfUser(sender);
                    string temp = "Назначение встречи согласовано!" + userDb.GetInformationAboutPeople(sender);//СЮДА НАДО ССЫЛКУ НА ТЕЛЕГУ
                    await botClient.SendTextMessageAsync(recepient, temp,ParseMode.Html);
                    await botClient.SendTextMessageAsync(sender, temp);
                    return Ok();
                }
                else if(a==4)//ОТКЛОНЕНИЕ НА ЗАПРОС НА ДОБАВЛЕНИЕ В КНИЖКУ
                {
                    string[] words = update.CallbackQuery.Data.Split("-");
                    int idOfApplication = Convert.ToInt32(words[1]);
                    long sender = userDb.GetIdOfRecipient(idOfApplication);//кто отсылает ответ
                    long recepient = userDb.GetIdOfSender(idOfApplication);//кому отсылаем ответ
                    string name = userDb.GetNameOfUser(sender);
                    userDb.RemoveApplication(idOfApplication);
                    string temp = "К сожалению "+name+" отклонил(-a) вашу заявку на добавление в записную книжку";
                    await botClient.SendTextMessageAsync(recepient, temp,ParseMode.Html);

                    return Ok();
                }
                
                else if(a==6)//ОТКЛОНЕНИЕ ЗАПРОСА О ВСТРЕЧЕ
                {
                    string[] words = update.CallbackQuery.Data.Split("-");
                    int idOfApplication = Convert.ToInt32(words[1]);
                    long sender = userDb.GetIdOfRecipientOfMeeting(idOfApplication);//кто отсылает ответ
                    long recepient = userDb.GetIdOfSenderOfMeeting(idOfApplication);//кому отсылаем ответ
                    string name = userDb.GetNameOfUser(sender);
                    userDb.RemoveApplicationOfMeeting(idOfApplication);
                   string temp = "К сожалению " + name + "отклонил(-a) вашу заявку на встречу";
                    await botClient.SendTextMessageAsync(recepient, temp,ParseMode.Html);
                    return Ok();
                }
                else if(a==7)//ЛИСТАЕМ ВПЕРЁД ПО КНИЖКЕ
                {
                    string[] words = update.CallbackQuery.Data.Split("-");
                    int count = Convert.ToInt32(words[1]);
                    count = count + 4;
                    //+рассмотри вариант,когда каунт выходит за пределы все людей из этой книжки
                    string contacts = userDb.AllListFromNotebook(chatId, count);
                    string ToBack = "7-" + count.ToString();
                    string ToAhead = "8-" + count.ToString();
                    string[][] ActionsInNoteBook = { new[] { "⬅️".ToString(), "➡️".ToString() }, new[] { "1", "2", "3", "4" } };
                    string[][] ActionsInNoteBookCallBack = { new[] { ToBack, ToAhead }, new[] { "9-"+count.ToString(), "9-" + (count + 1).ToString(), "9-" + (count + 2).ToString(), "9-" + (count + 3).ToString() } };
                    await botClient.EditMessageTextAsync(chatId, update.CallbackQuery.Message.MessageId, contacts,ParseMode.Html,replyMarkup: keybord.GetInlineKeyboard(ActionsInNoteBook,ActionsInNoteBookCallBack));
                    await botClient.AnswerCallbackQueryAsync(update.CallbackQuery.Id);
                 }
                else if(a==8)//ЛИСТАЕМ НАЗАД ПО КНИЖКЕ
                {
                    string[] words = update.CallbackQuery.Data.Split("-");
                    int count = Convert.ToInt32(words[1]);
                    count = count - 4;
                    string contacts = userDb.AllListFromNotebook(chatId, count);
                    string ToBack = "7-" + count.ToString();
                    string ToAhead = "8-" + count.ToString();
                    string[][] ActionsInNoteBook = { new[] { "⬅️".ToString(), "➡️".ToString() },new[] { "1", "2", "3", "4" } };
                    string[][] ActionsInNoteBookCallBack = { new[] { ToBack, ToAhead }, new[] { "9-" + count.ToString(), "9-" + (count + 1).ToString(), "9-" + (count + 2).ToString(), "9-" + (count + 3).ToString() } };
                    await botClient.EditMessageTextAsync(chatId, update.CallbackQuery.Message.MessageId, contacts, replyMarkup: keybord.GetInlineKeyboard(ActionsInNoteBook, ActionsInNoteBookCallBack));
                    await botClient.AnswerCallbackQueryAsync(update.CallbackQuery.Id);
                 }
                else if(a==9)
                {
                     string[] words = update.CallbackQuery.Data.Split("-");
                    int splitten = Convert.ToInt32(words[1]);
                   
                    if (b==1)//если нажали на человека в записной книжке
                    {
                        int amount = Convert.ToInt32(words[1]);
                        string[][] Tinder1 = { new[] { "⬅️".ToString(), "В книжку", "Встреча", "➡️".ToString() } };//
                        string[][] TinderCallback1 = { new[] { "92" + amount.ToString(), "22", "23", "93" + amount.ToString() } };
                        long chatOfPeopleFromNotembook = userDb.ChatidFromNotebook(chatId, amount);
                        string tempp = userDb.GetInformationAboutPeople(chatOfPeopleFromNotembook);
                        if (userDb.CheckPermission(chatId, chatOfPeopleFromNotembook))
                            tempp = tempp + userDb.GetAdditionalInformationAboutPeople(chatOfPeopleFromNotembook);
                        await botClient.EditMessageTextAsync(chatId, update.CallbackQuery.Message.MessageId, tempp, ParseMode.Html, replyMarkup: keybord.GetInlineKeyboard(Tinder1, TinderCallback1));
                       // await botClient.SendTextMessageAsync(chatId, tempp, ParseMode.Html,replyMarkup: keybord.GetInlineKeyboard(Tinder1, TinderCallback1));
                        return Ok();
                        /*List<string> subtags = userDb.GetAllUsersSubtagsAboutOthersList(chatId);
                    List<int> usersForTinder = userDb.GetAllUsersForTinder(subtags);
                    userDb.AddlengthOfUsersBySubtags(chatId, usersForTinder.Count);
                    
                    int countt = userDb.CheckCurrentAction(chatId, "amountForTinder");
                        userDb.AddAmountForTinder(chatId, 0);
                        long userIdd = userDb.GetChatId(usersForTinder[countt]);
                   
                    log.AddLog("COMMUNICATION4");
                   
                    return Ok();*/

                }
                else if (b==2)//назад по книжке
                    {
                        int amount = Convert.ToInt32(words[1]);
                        amount--;
                        string[][] Tinder1 = { new[] { "⬅️".ToString(), "В книжку", "Встреча", "➡️".ToString() } };//
                        string[][] TinderCallback1 = { new[] { "92" + amount.ToString(), "22", "23", "93" + amount.ToString() } };
                        long chatOfPeopleFromNotembook = userDb.ChatidFromNotebook(chatId, amount);
                        string tempp = userDb.GetInformationAboutPeople(chatOfPeopleFromNotembook);
                        await botClient.SendTextMessageAsync(chatId, tempp, ParseMode.Html, replyMarkup: keybord.GetInlineKeyboard(Tinder1, TinderCallback1));
                        return Ok();
                    }
                else if(b==3)//вперёд по книжке
                    {
                        int amount = Convert.ToInt32(words[1]);
                        amount++;
                        string[][] Tinder1 = { new[] { "⬅️".ToString(), "В книжку", "Встреча", "➡️".ToString() } };//
                        string[][] TinderCallback1 = { new[] { "92" + amount.ToString(), "22", "23", "93" + amount.ToString() } };
                        long chatOfPeopleFromNotembook = userDb.ChatidFromNotebook(chatId, amount);
                        string tempp = userDb.GetInformationAboutPeople(chatOfPeopleFromNotembook);
                        await botClient.SendTextMessageAsync(chatId, tempp, ParseMode.Html, replyMarkup: keybord.GetInlineKeyboard(Tinder1, TinderCallback1));
                        return Ok();

                    }
                     if(b==9)//если ответ на вопрос опроса
                    {
                        log.AddLog("b=9");
                        int c= Convert.ToInt32(Char.GetNumericValue(update.CallbackQuery.Data[2]));
                        log.AddLog(c.ToString());
                        int question = splitten;
                        if (c==0)
                        {
                            userDb.RemoveAnswer(chatId, question);
                            string[][] questionnn = { new[] { "🔥".ToString(), "👍".ToString(), "👌".ToString(), "👎".ToString(), "🤢".ToString() } };
                            string tempp1 = "991-" + question.ToString();
                            string tempp2 = "992-" + question.ToString();
                            string tempp3 = "993-" + question.ToString();
                            string tempp4 = "994-" + question.ToString();
                            string tempp5 = "954-" + question.ToString();
                            string[][] questionnn1 = { new[] { tempp1, tempp2, tempp3, tempp4,tempp5 } };
                            string temporary1 = userDb.GetQuestion(question);
                            await botClient.EditMessageTextAsync(chatId, update.CallbackQuery.Message.MessageId, temporary1, replyMarkup: keybord.GetInlineKeyboard(questionnn, questionnn1));
                            return Ok();
                        }
                       
                        userDb.AddAnswerToSurvey(chatId, question, c);
                        string[][] questionn = { new[] { "🔥".ToString(), "👍".ToString(), "👌".ToString(), "👎".ToString(), "🤢".ToString() } };
                        c--;
                        questionn[0][c]= "✅".ToString();
                         string temp1 = "991-" + question.ToString();
                        string temp2 = "992-" + question.ToString();
                        string temp3 = "993-" + question.ToString();
                        string temp4 = "994-" + question.ToString();
                        string temp5= "995-" + question.ToString();
                        string[][] questionn1 = { new[] { temp1, temp2, temp3, temp4,temp5 } };
                        questionn1[0][c] = "990-" + question.ToString();
                       string temporary=userDb.GetQuestion(question);
                        await botClient.EditMessageTextAsync(chatId, update.CallbackQuery.Message.MessageId,temporary, replyMarkup: keybord.GetInlineKeyboard(questionn,questionn1));
                        return Ok();
                    }
                    if (splitten==000)//ЗАПРОС О ВСТРЕЧЕ 
                    {
                        int amount = Convert.ToInt32(words[2]);
                        long IdOfRecepient = userDb.GetUserFromNotebook(chatId, amount);
                        int chatikOfSender = userDb.GetUserId(chatId);
                        userDb.AddToRequestToMeet(IdOfRecepient, chatikOfSender);
                        int idOfApplication = userDb.GetApplicationIdOfMeeting(IdOfRecepient, chatikOfSender);
                        string accept = "5-" + idOfApplication.ToString();
                        string cancel = "6-" + idOfApplication.ToString();
                        string[][] querryToMeet = { new[] { "Принять", "Отклонить" } };
                        string[][] querryToMeetCallBack = { new[] { accept, cancel } };
                        string temporary = "Вам поступило приглашение на встречу от " + userDb.GetInformationAboutPeople(chatikOfSender);
                        await botClient.SendTextMessageAsync(IdOfRecepient, temporary,ParseMode.Html, replyMarkup: keybord.GetInlineKeyboard(querryToMeet, querryToMeetCallBack));
                        await botClient.AnswerCallbackQueryAsync(update.CallbackQuery.Id);
                        string[][] actionsInContacts = { new[] { "➡️".ToString(), "Встреча", "⬅️".ToString() }  };
                        string[][] actionsInContactsCallbackQuerry = { new[] { "9-" + (amount - 1).ToString(), "9-000" + amount.ToString(), "9-" + (amount + 1).ToString() } };
                        await botClient.EditMessageTextAsync(chatId, update.CallbackQuery.Message.MessageId, userDb.GetAdditionalInformationAboutPeople(IdOfRecepient), replyMarkup: keybord.GetInlineKeyboard(actionsInContacts,actionsInContactsCallbackQuerry));
                        await botClient.SendTextMessageAsync(chatId, "Запрос на встречу отправлен!");
                        return Ok();
                    }
                    int count = Convert.ToInt32(words[1]); 
                    long userId = userDb.GetUserFromNotebook(chatId, count);
                    string temp = userDb.GetAdditionalInformationAboutPeople(userId);
                    string[][] actionsInContacts1= { new[] { "➡️".ToString(), "Встреча", "⬅️".ToString() }};
                    string[][] actionsInContactsCallbackQuerry1= { new[] { "9-" + (count - 1).ToString(), "9-000" + count.ToString(), "9-" + (count + 1).ToString() } };
                    await botClient.EditMessageTextAsync(chatId, update.CallbackQuery.Message.MessageId, temp, replyMarkup: keybord.GetInlineKeyboard(actionsInContacts1, actionsInContactsCallbackQuerry1));
                    await botClient.AnswerCallbackQueryAsync(update.CallbackQuery.Id);

                }
                return Ok();

            }
            return Ok();
        }
    }
}
