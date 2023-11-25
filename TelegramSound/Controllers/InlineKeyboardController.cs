using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramSound.Controllers
{
    public class InlineKeyboardController
    {
        private readonly ITelegramBotClient _botClient;
        public InlineKeyboardController(ITelegramBotClient botClient)
        {
             _botClient = botClient;
        }
        public async Task Handle(CallbackQuery callbackQuery, CancellationToken ct)
        {
            await _botClient.SendTextMessageAsync(callbackQuery!.Message!.Chat.Id, text: $"{callbackQuery.Data}");
        }
    }
}
    