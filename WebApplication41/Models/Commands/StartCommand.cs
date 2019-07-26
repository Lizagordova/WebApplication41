using System;
using System.Collections.Generic;
using System.Linq;
using Telegram.Bot;
using Telegram.Bot.Types;
using System.Threading.Tasks;
using WebApplication41.Models;
using WebApplication41.DB;

namespace WebApplication41.Models.Commands
{
    public class StartCommand : Command
    {
        public override string Name => @"/start";

        public override bool Contains(Message message)
        {
            if (message.Type != Telegram.Bot.Types.Enums.MessageType.Text)
                return false;

            return message.Text.Contains(Name);
        }

        public override async Task Execute(Message message, TelegramBotClient botClient)
        {
            var chatId = message.Chat.Id;
            LogsDB log = new LogsDB();
            UserDB userDb = new UserDB();
            TelegramKeybord keyboard = new TelegramKeybord();
            if (!userDb.CheckUser(chatId))
            {
                log.AddLog("why");
                userDb.CreateUser(chatId);
                log.AddLog("why1");
            }
            else
            {
                log.AddLog("why2");
               userDb.RecreateUser(chatId);
                log.AddLog("why3");
            }
            string[][] actions = { new[] { "Войти по ивент-коду" }, new[] {"Личный кабинет"} };
            userDb.CurrentActionOn(chatId, "EventCode");
            await botClient.SendTextMessageAsync(chatId, "Чудненько "+"😇".ToString()+" Можем приступить", parseMode:Telegram.Bot.Types.Enums.ParseMode.Markdown);
            await botClient.SendTextMessageAsync(chatId, "У вас есть личный кабинет? Если нет, то войдите по <b>ивент-коду</b> \n P.S.<b>Ивент-код</b> отправлен в письме регистрации",Telegram.Bot.Types.Enums.ParseMode.Html,replyMarkup:keyboard.GetKeyboard(actions));

        }
    }
}
